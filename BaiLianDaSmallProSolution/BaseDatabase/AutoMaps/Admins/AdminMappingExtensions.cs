using AutoMapper;
using BaseDatabase.Entities.Admins.AdminUserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.AutoMaps.Admins
{
    public static class AdminMappingExtensions
    {
        #region AdminAdminUserInfo

        public static AdminUserInfo ToEntity(this AdminUserInfoModel model)
        {
            return Mapper.Map<AdminUserInfoModel, AdminUserInfo>(model);
        }

        public static AdminUserInfo ToEntity(this AdminUserInfoModel model, AdminUserInfo destination)
        {
            return Mapper.Map(model, destination);
        }

        public static AdminUserInfoModel ToModel(this AdminUserInfo entity)
        {
            var model = Mapper.Map<AdminUserInfo, AdminUserInfoModel>(entity);
            return model;
        }

        #endregion
    }
}
