using AutoMapper;
using BaseDatabase.Entities.BaseSettings;
using BaseDatabase.Entities.PayInfos;
using BaseDatabase.Entities.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.AutoMaps
{
    public class AutoMapConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                #region UserInfo

                cfg.CreateMap<UserInfo, UserInfoModel>();
                cfg.CreateMap<UserInfoModel, UserInfo>();

                #endregion

                #region PayInfo

                cfg.CreateMap<PayInfo, PayInfoModel>();
                cfg.CreateMap<PayInfoModel, PayInfo>()
                .ForMember(q => q.UserInfo, q => q.Ignore());

                #endregion

                #region BaseSetting

                cfg.CreateMap<BaseSetting, BaseSettingModel>();
                cfg.CreateMap<BaseSettingModel, BaseSetting>();

                #endregion

                #region PaySetting

                cfg.CreateMap<PaySetting, PaySettingModel>();
                cfg.CreateMap<PaySettingModel, PaySetting>();

                #endregion
            });
        }
    }
}
