using BaseDatabase.BaseDbInstanceMangers;
using BaseDatabase.Entities.ShareLogs;
using BaseDatabase.Services.ShareLogs;
using MyUntil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models.Shares;

namespace Web.Controllers
{
    public class ShareController : ApiController
    {
        private readonly IShareLogInfoService shareLogInfoService;

        public ShareController()
        {
            this.shareLogInfoService = BaseDbInstanceManger.GetShareLogInfoService();
        }

        private readonly static object AddShareLogInfoLock = new object();

        [Route("api/Share/AddShareLogInfo")]
        [HttpPost]
        public IHttpActionResult AddShareLogInfo(dynamic obj)
        {
            var model = new AddShareLogInfoModel();
            try
            {
                lock (AddShareLogInfoLock)
                {
                    int shareUserInfoId = obj.ShareUserInfoId;
                    int targetUserInfoId = obj.TargetUserInfoId;
                    int shareType = obj.ShareType;
                    var isExists = shareLogInfoService.IsExists(shareUserInfoId, targetUserInfoId, shareType);
                    if (!isExists)
                    {
                        shareLogInfoService.Insert(new ShareLogInfo
                        {
                            ShareUserInfoId = obj.ShareUserInfoId,
                            TargetUserInfoId = obj.TargetUserInfoId,
                            ShareType = obj.ShareType,
                            ShareName = obj.ShareName
                        });
                        model.IsSuccess = true;
                        model.ReturnMsg = "添加成功";
                    }
                    else
                    {
                        model.IsSuccess = false;
                        model.ReturnMsg = "已添加过";
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.ReturnMsg = "操作失败，详情请查看日志";
                WebLogHelper.WebErrorLog("AddShareLogInfo", ex);
            }
            return Json(model);
        }
    }
}
