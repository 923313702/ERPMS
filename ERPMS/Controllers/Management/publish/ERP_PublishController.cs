using ERPMS.IRepository.publish;
using ERPMS.Models;
using ERPMS.Repository.publish;
using ERPMS.Util;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.publish
{
    /// <summary>
    /// 出版管理
    /// </summary>
    public class ERP_PublishController : Controller
    {
        /// <summary>
        /// T_OMS_出版_项目编码表 接口
        /// </summary>
        IPublishRepository pRepository = new PublishRepository();
        // GET: ERP_Publish
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPublish(string category="",int page=0,int rows=10)
        {

            int total = 0;
            Expression<Func<T_OMS_出版_项目编码表, bool>> ex = PredicateBuilder.New<T_OMS_出版_项目编码表>(true);
            if (!string.IsNullOrEmpty(category))
            {
                ex = ex.And(p => p.工艺类别.Contains(category));
            }
            total = pRepository.QueryWhere(ex).Count();
            var list = pRepository.AsExpandable(ex, p => p.项目编码, true).Skip((page - 1) * rows).Take(rows).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveOpUpdatePublish(T_OMS_出版_项目编码表 data,string flag)
        {
            JsonResult jr = null;
            if (flag == "addd")
            {
                pRepository.Add(data);
            } else if (flag == "edit")
            {
                pRepository.Edit(data);
            }
            try
            {
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeletePublish(List<T_OMS_出版_项目编码表> data)
        {
            JsonResult jr = null;

            foreach (var item in data)
            {
                pRepository.Delete(item, true);
            }
            try
            {
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page = 1, int rows = 10, string pcategory = "")
        {
            if (!string.IsNullOrEmpty(pcategory))
            {
            }
            Expression<Func<T_OMS_出版_项目编码表, bool>> ex = PredicateBuilder.New<T_OMS_出版_项目编码表>(true);
            if (!string.IsNullOrEmpty(pcategory))
            {
                ex = ex.And(p => p.工艺类别.Contains(pcategory));
            }
            var query = pRepository.AsExpandable(ex, p => p.项目编码, true).Skip((page - 1) * rows).Take(rows).Select(p=>new { p.项目编码,p.项目名称,p.工艺类别,p.计量单位}).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}