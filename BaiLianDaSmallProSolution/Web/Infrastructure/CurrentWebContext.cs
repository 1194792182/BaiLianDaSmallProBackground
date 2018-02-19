using BaseDatabase.Entities.Admins.AdminUserInfos;
using BaseDatabase.Services.Admins.AdminUserInfos;
using MyUntil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Web.Infrastructure
{
    public class CurrentWebContext
    {
        private readonly IAdminUserInfoService _adminUserInfoService;
        private readonly string _cookieIdStr = FormsAuthentication.FormsCookieName + ".Admin";
        private readonly TimeSpan _timeOut;

        public CurrentWebContext()
        {
            _adminUserInfoService = new AdminUserInfoService();
            _timeOut = FormsAuthentication.Timeout;
        }

        private static AdminUserInfoModel _loginAdminUser;

        private AdminUserInfoModel GetAdminUserInfo()
        {
            if (_loginAdminUser != null)
            {
                return _loginAdminUser;
            }

            if (HttpContext.Current == null)
            {
                return null;
            }

            var cookie = HttpContext.Current.Request.Cookies[_cookieIdStr];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value) || cookie.Expires <= DateTime.Now)
            {
                return null;
            }
            FormsAuthenticationTicket ticket = null;
            try
            {
                ticket = FormsAuthentication.Decrypt(cookie.Value);
            }
            catch
            {
                return null;
            }

            if (ticket == null)
            {
                return null;
            }

            var userName = ticket.UserData;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            return _adminUserInfoService.GetByUserName(userName);
        }

        public bool IsAdminUserLogined
        {
            get
            {
                return _loginAdminUser != null;
            }
        }

        public AdminUserInfoModel LoginAdminUser
        {
            get
            {
                if (_loginAdminUser != null)
                {
                    return _loginAdminUser;
                }
                else
                {
                    _loginAdminUser = GetAdminUserInfo();
                }

                return _loginAdminUser;
            }

            set
            {
                _loginAdminUser = value;
            }
        }

        public void SetLogin(AdminUserInfoModel adminUserInfo, bool isPersistent = false, int days = 365)
        {
            var now = DateTime.Now;
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: Guid.NewGuid().ToString("N"),
                issueDate: now,
                expiration: now.Add(_timeOut),
                isPersistent: isPersistent,
                userData: adminUserInfo.UserName,
                cookiePath: FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(_cookieIdStr, FormsAuthentication.Encrypt(ticket));
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = DateTime.Now.AddDays(days);
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
            _loginAdminUser = adminUserInfo;
            try
            {
                _adminUserInfoService.SetLoginInfo(adminUserInfo.UserName, IpAddressHelper.GetIp());
            }
            catch
            {

            }
        }

        public void SetLoginOut()
        {
            _loginAdminUser = null;
            var cookie = new HttpCookie(_cookieIdStr);
            cookie.Expires = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}