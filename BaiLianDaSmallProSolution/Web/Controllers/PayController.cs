using BaseDatabase.Services.PaySettings;
using BaseDatabase.Services.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Web.Models;
using Web.Models.Pays;
using MyUntil;
using BaseDatabase.Services.BaseSettings;
using BaseDatabase.Entities.BaseSettings;
using System.Xml;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using BaseDatabase.Entities.PayInfos;
using BaseDatabase.Services.PayInfos;

namespace Web.Controllers
{
    public class PayController : ApiController
    {
        private readonly IPaySettingService _PaySettingService;
        private readonly IBaseSettingService _baseSettingService;
        private readonly IUserInfoService _userInfoService;
        private readonly IPayInfoService _payInfoService;
        private readonly PayHelper _payHelper;
        

        private static readonly object PayLock = new object();
        private static readonly object PayNotifyLock = new object();
        public PayController()
        {
            _PaySettingService = new PaySettingService();
            _baseSettingService = new BaseSettingService();
            _userInfoService = new UserInfoService();
            _payInfoService = new PayInfoService();
            _payHelper = new PayHelper();
        }

        [HttpPost]
        public IHttpActionResult Pay(PayParamModel paramModel)
        {
            var model = new PayModel();

            try
            {
                lock (PayLock)
                {
                    var baseSetting = _baseSettingService.GetLast();
                    if (baseSetting == null)
                    {
                        throw new MyProException("无效的基础设置");
                    }

                    var paySetting = _PaySettingService.GetLast();
                    if (paySetting == null)
                    {
                        throw new MyProException("无效的支付设置");
                    }

                    if (string.IsNullOrEmpty(paramModel.OpenId))
                    {
                        throw new MyProException("openId不能为空");
                    }
                    var userInfo = _userInfoService.GetByOpenId(paramModel.OpenId);
                    if (userInfo == null)
                    {
                        throw new MyProException("openId 无效");
                    }
                    decimal totalFee;
                    if (!decimal.TryParse(paramModel.Amount, out totalFee))
                    {
                        throw new MyProException("支付金额有误");
                    }
                    var logStr = new StringBuilder();
                    var body = _payHelper.GetUtf8Str(paySetting.ShortName + "-美发/美容/美甲店");
                    var random = new Random();
                    Dictionary<string, string> resultDic = null;
                    var serialNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff") + random.Next(999);

                    var dic = new Dictionary<string, string>
                    {
                        {"appid", baseSetting.AppId},
                        {"mch_id", paySetting.MchId},
                        {"nonce_str", _payHelper.GetRandomString(20)},
                        {"body", body},
                        {"out_trade_no", serialNumber},
                        {"total_fee", paramModel.Amount},
                        {"spbill_create_ip", paySetting.IpAddress},
                        {"notify_url", paySetting.PayResulturl},
                        {"trade_type", "JSAPI"},
                        {"openid", paramModel.OpenId}
                    };
                    dic.Add("sign", _payHelper.GetSignString(dic, paySetting.PayKey));
                    var strB = new StringBuilder();
                    strB.Append("<xml>");
                    foreach (var d in dic)
                    {
                        strB.AppendFormat("<{0}>{1}</{0}>", d.Key, d.Value);
                    }
                    strB.Append("</xml>");

                    logStr.AppendLine("支付请求的xml" + strB);
                    var xml = new XmlDocument();
                    var en = Encoding.GetEncoding("UTF-8");
                    var response = _payHelper.CreatePostHttpResponse("https://api.mch.weixin.qq.com/pay/unifiedorder",
                        strB.ToString(), en);
                    if (response != null)
                    {
                        //打印返回值  
                        var stream = response.GetResponseStream(); //获取响应的字符串流
                        if (stream != null)
                        {
                            var sr = new StreamReader(stream); //创建一个stream读取流  
                            var html = sr.ReadToEnd(); //从头读到尾，放到字符串html
                            logStr.AppendLine("请求返回的结果:" + html);
                            xml.LoadXml(html);
                            //对请求返回值 进行处理  
                            var root = xml.DocumentElement;
                            var ds = new DataSet();
                            var stram = new StringReader(html);
                            var reader = new XmlTextReader(stram);
                            ds.ReadXml(reader);
                            var returnCode = ds.Tables[0].Rows[0]["return_code"].ToString();
                            if (returnCode.ToUpper() == "SUCCESS")
                            {
                                resultDic = new Dictionary<string, string>
                                {
                                    {"appId", baseSetting.AppId},
                                    {"timeStamp", _payHelper.GetTimeStamp()},
                                    {"nonceStr", dic["nonce_str"]},
                                    {"package", "prepay_id=" + ds.Tables[0].Rows[0]["prepay_id"]},
                                    {"signType", "MD5"}
                                };
                                //在服务器上签名
                                resultDic.Add("paySign", _payHelper.GetSignString(resultDic, paySetting.PayKey));
                                model.TimeStamp = resultDic["timeStamp"];
                                model.NonceStr = resultDic["nonceStr"];
                                model.Package = resultDic["package"];
                                model.SignType = resultDic["signType"];
                                model.PaySign = resultDic["paySign"];
                                var requestStr = JsonConvert.SerializeObject(resultDic);
                                logStr.AppendLine("返回的结果:" + requestStr);
                                var payInfo = new PayInfo()
                                {
                                    UserInfoId = userInfo.Id,
                                    Amount = decimal.Parse(paramModel.Amount) / 100,
                                    IsPay = false,
                                    TradeNo = serialNumber,
                                    Body = body,
                                    CreateOn = DateTime.Now
                                };
                                _payInfoService.Insert(payInfo);
                            }
                            else
                            {
                                var returnMsg = ds.Tables[0].Rows[0]["return_msg"].ToString();
                                model.IsSuccess = false;
                                model.ReturnMsg = returnMsg;
                                logStr.AppendLine("返回的结果:" + JsonConvert.SerializeObject(model));
                            }
                        }
                        else
                        {
                            model.IsSuccess = false;
                            model.ReturnMsg = "response.GetResponseStream() is null";
                            logStr.AppendLine("返回的结果:" + JsonConvert.SerializeObject(model));
                        }
                    }
                    else
                    {
                        model.IsSuccess = false;
                        model.ReturnMsg = "CreatePostHttpResponse is null";
                        logStr.AppendLine("返回的结果:" + JsonConvert.SerializeObject(model));
                    }
                    WebLogHelper.WebLog(logStr.ToString());
                    model.IsSuccess = true;
                    model.ReturnMsg = "调用成功";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.ReturnMsg = ex.Message;
            }


            return Json(model);
        }

        [HttpGet]
        public IHttpActionResult PayNotify()
        {

            return Ok("");
        }
    }
}
