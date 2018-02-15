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
    public static class MappingExtensions
    {
        private static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        private static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }

        #region UserInfo

        public static UserInfo ToEntity(this UserInfoModel model)
        {
            return Mapper.Map<UserInfoModel, UserInfo>(model);
        }

        public static UserInfo ToEntity(this UserInfoModel model, UserInfo destination)
        {
            return Mapper.Map(model, destination);
        }

        public static UserInfoModel ToModel(this UserInfo entity)
        {
            var model = Mapper.Map<UserInfo, UserInfoModel>(entity);
            return model;
        }

        #endregion

        #region PayInfo

        public static PayInfo ToEntity(this PayInfoModel model)
        {
            return Mapper.Map<PayInfoModel, PayInfo>(model);
        }

        public static PayInfo ToEntity(this PayInfoModel model, PayInfo destination)
        {
            return Mapper.Map(model, destination);
        }

        public static PayInfoModel ToModel(this PayInfo entity)
        {
            var model = Mapper.Map<PayInfo, PayInfoModel>(entity);
            return model;
        }

        #endregion

        #region BaseSetting

        public static BaseSetting ToEntity(this BaseSettingModel model)
        {
            return Mapper.Map<BaseSettingModel, BaseSetting>(model);
        }

        public static BaseSetting ToEntity(this BaseSettingModel model, BaseSetting destination)
        {
            return Mapper.Map(model, destination);
        }

        public static BaseSettingModel ToModel(this BaseSetting entity)
        {
            var model = Mapper.Map<BaseSetting, BaseSettingModel>(entity);
            return model;
        }

        #endregion

        #region PaySetting

        public static PaySetting ToEntity(this PaySettingModel model)
        {
            return Mapper.Map<PaySettingModel, PaySetting>(model);
        }

        public static PaySetting ToEntity(this PaySettingModel model, PaySetting destination)
        {
            return Mapper.Map(model, destination);
        }

        public static PaySettingModel ToModel(this PaySetting entity)
        {
            var model = Mapper.Map<PaySetting, PaySettingModel>(entity);
            return model;
        }

        #endregion
    }
}
