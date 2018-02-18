using AutoMapper;
using BaseDatabase.Entities.Admins.AdminUserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.AutoMaps.Admins
{
    public class SetAdminMap
    {
        public static void Config(IMapperConfigurationExpression cfg)
        {
            #region AdminUserInfo

            cfg.CreateMap<AdminUserInfo, AdminUserInfoModel>();
            cfg.CreateMap<AdminUserInfoModel, AdminUserInfo>();

            #endregion
        }
    }
}
