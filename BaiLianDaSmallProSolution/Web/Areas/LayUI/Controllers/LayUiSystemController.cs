using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.LayUI.Models.Systems;

namespace Web.Areas.LayUI.Controllers
{
    public class LayUiSystemController : Controller
    {
        public ActionResult ChangePwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOldPwd(string oldPwd)
        {
            var model = new CheckOldPwdModel() {
                IsSuccess = false,
                ReturnMsg = "请输入正确的旧密码"
            };


            return Json(model);
        }
    }
}