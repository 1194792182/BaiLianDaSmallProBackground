﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Infrastructure;

namespace Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private bool _doNotValidate = false;

        public AdminAuthorizeAttribute(bool doNotValidate)
        {
            this._doNotValidate = doNotValidate;
        }

        public AdminAuthorizeAttribute()
        {

        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (this._doNotValidate)
            {
                return;
            }

            var sysContext = new CurrentWebContext();

            if (!sysContext.IsAdminUserLogined)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult { Data = new { IsSuccess = false, ReturnMsg = "未登录" } };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/admin/login");
                }
            }
        }
    }
}