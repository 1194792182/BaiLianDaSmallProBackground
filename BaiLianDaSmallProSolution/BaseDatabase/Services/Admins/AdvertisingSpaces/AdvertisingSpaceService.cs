﻿using BaseDatabase.AutoMaps.Admins;
using BaseDatabase.Entities.Admins.AdvertisingSpaces;
using BaseDatabase.Pages;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDatabase.Services.Admins.AdvertisingSpaces
{
    public class AdvertisingSpaceService : IAdvertisingSpaceService
    {
        public void Delete(int id)
        {
            using (var db = new BaseDatabaseContext())
            {
                var entity = db.AdvertisingSpaceInfos.Find(id);
                if (entity == null)
                {
                    throw new Exception("entity is null");
                }
                db.AdvertisingSpaceInfos.Remove(entity);
                db.SaveChanges();
            }
        }

        public AdvertisingSpaceInfoModel GetById(int id)
        {
            using (var db = new BaseDatabaseContext())
            {
                var entity = db.AdvertisingSpaceInfos.Find(id);
                if (entity == null)
                {
                    return null;
                }
                return entity.ToModel();
            }
        }

        public void Insert(AdvertisingSpaceInfo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            using (var db = new BaseDatabaseContext())
            {
                db.AdvertisingSpaceInfos.Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(AdvertisingSpaceInfoModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            using (var db = new BaseDatabaseContext())
            {
                var oldEntity = db.AdvertisingSpaceInfos.Find(model.Id);

                if (oldEntity == null)
                {
                    throw new Exception("oldEntity is null");
                }
                oldEntity = model.ToEntity(oldEntity);
                db.SaveChanges();
            }
        }

        public void RemoveAll()
        {
            using (var db = new BaseDatabaseContext())
            {
                var query = db.AdvertisingSpaceInfos.AsNoTracking();
                query.Delete();
            }
        }

        public AdvertisingSpaceInfoModel GetLast()
        {
            using (var db = new BaseDatabaseContext())
            {
                var entity = db.AdvertisingSpaceInfos.OrderByDescending(q => q.Id).FirstOrDefault();
                return entity.ToModel();
            }
        }

        public IPageList<AdvertisingSpaceInfo> GetPageList(int page, int size)
        {
            using (var db = new BaseDatabaseContext())
            {
                var query = db.AdvertisingSpaceInfos.OrderByDescending(q => q.Id);
                return new PageList<AdvertisingSpaceInfo>(query, page, size);
            }
        }
    }
}