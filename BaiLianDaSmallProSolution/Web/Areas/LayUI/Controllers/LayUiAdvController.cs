﻿using BaseDatabase.Entities.Admins.AdvertisingSpaces;
using BaseDatabase.Services.Admins.AdvertisingSpaces;
using MyUntil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.LayUI.Models;
using Web.Infrastructure;

namespace Web.Areas.LayUI.Controllers
{
    public class LayUiAdvController : BaseAdminWebController
    {
        private readonly IAdvertisingSpaceService _advertisingSpaceService;

        public LayUiAdvController()
        {
            _advertisingSpaceService = new AdvertisingSpaceService();
        }

        #region AdvertisingSpace

        public ActionResult AdvertisingSpaceIndex()
        {
            return View();
        }

        public ActionResult AdvertisingSpaceList(ListSearch search)
        {
            var model = new ListResult();
            try
            {
                var list = _advertisingSpaceService.GetPageList(search.pageIndex, search.limit);

                model.count = list.TotalRecords;
                model.data = list.Datas.Select(q => new
                {
                    Id = q.Id,
                    Sign = q.Sign,
                    Title = q.Title,
                    Type = ((AdvertisingSpaceType)q.TypeId).GetEnumDescription(),
                    Width = q.Width,
                    Height = q.Height,
                    Intro = q.Intro,
                    CreateOn = q.CreateOn
                });
            }
            catch (Exception ex)
            {
                model.code = -1;
                model.msg = ex.Message;
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private void CreateTypeList()
        {
            var typeList = new List<SelectListItem>();
            foreach (var item in AdvertisingSpaceType.Fit.ToDic<AdvertisingSpaceType>())
            {
                typeList.Add(new SelectListItem()
                {
                    Text = item.Key,
                    Value = item.Value.ToString()
                });
            }
            ViewBag.TypeList = typeList;
        }

        public ActionResult AddAdvertisingSpace()
        {
            CreateTypeList();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddAdvertisingSpace(AdvertisingSpaceInfoModel paraModel)
        {
            var model = new BaseReturnModel() { IsSuccess = false, ReturnMsg = "操作失败" };
            try
            {
                _advertisingSpaceService.Insert(new AdvertisingSpaceInfo()
                {
                    Height = paraModel.Height,
                    Width = paraModel.Width,
                    Sign = Guid.NewGuid().ToString("N"),
                    Intro = paraModel.Intro,
                    Title = paraModel.Title,
                    TypeId = paraModel.TypeId,
                    CreateOn = DateTime.Now
                });
                model.IsSuccess = true;
                model.ReturnMsg = "添加完成";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.ReturnMsg = ex.Message;
            }
            return Json(model);
        }

        public ActionResult EditAdvertisingSpace(int id)
        {
            CreateTypeList();
            var model = _advertisingSpaceService.GetById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAdvertisingSpace(AdvertisingSpaceInfoModel paraModel)
        {
            var model = new BaseReturnModel() { IsSuccess = false, ReturnMsg = "操作失败" };

            try
            {
                _advertisingSpaceService.Update(paraModel);
                model.IsSuccess = true;
                model.ReturnMsg = "编辑完成";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.ReturnMsg = ex.Message;
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult DeleteAdvertisingSpace(int id)
        {
            var model = new BaseReturnModel() { IsSuccess = false, ReturnMsg = "操作失败" };
            try
            {
                _advertisingSpaceService.Delete(id);
                model.IsSuccess = true;
                model.ReturnMsg = "删除完成";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.ReturnMsg = ex.Message;
            }

            return Json(model);
        }

        #endregion
    }
}