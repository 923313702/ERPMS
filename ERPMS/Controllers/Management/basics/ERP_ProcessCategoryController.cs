using ERPMS.IRepository.Basics;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Util;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.basics
{
    /// <summary>
    /// T_OMS_工艺类别系数表 controller
    /// </summary>
    public class ERP_ProcessCategoryController : Controller
    {
        IProcessCategoryRepository pcRepository = new ProcessCategoryRepository();
        // GET: ERP_ProcessCategory
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult GetCategory()
        {
            var query = pcRepository.Query().GroupBy(p => p.工艺类别).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProcessCategory(int page = 1, int rows = 10, string pcategory = "")
        {
            int total = 0;
            Expression<Func<T_OMS_工艺类别系数表, bool>> ex = PredicateBuilder.New<T_OMS_工艺类别系数表>(true);
            if (!string.IsNullOrEmpty(pcategory))
            {
                ex = ex.And(p => p.工艺类别.Contains(pcategory));
            }
            total = pcRepository.QueryWhere(ex).Count();
            var list = pcRepository.AsExpandable(ex, p => p.ID, true).Skip((page - 1) * rows).Take(rows).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate(T_OMS_工艺类别系数表 data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                pcRepository.Add(data);
            }
            else if (flag == "edit")
            {
                pcRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标志" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                pcRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteCategory(List<T_OMS_工艺类别系数表> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                pcRepository.Delete(item, true);
            }
            try
            {
                pcRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page = 1, int rows = 10, string pcategory = "")
        {
            Expression<Func<T_OMS_工艺类别系数表, bool>> ex = PredicateBuilder.New<T_OMS_工艺类别系数表>(true);
            if (!string.IsNullOrEmpty(pcategory))
            {
                ex = ex.And(p => p.工艺类别.Contains(pcategory));
            }
            var query = pcRepository.AsExpandable(ex, p => p.ID, true).Skip((page - 1) * rows).Take(rows).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}