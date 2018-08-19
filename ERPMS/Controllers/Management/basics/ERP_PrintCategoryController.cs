using ERPMS.IRepository.Basics;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.basics
{
    public class ERP_PrintCategoryController : Controller
    {
        /// <summary>
        /// 印品类别编码表
        /// </summary>
        IPrintCategoryRepository printRepository = new PrintCategoryRepository(); //接口调用数据库
        // GET: ERP_PrintCategory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPrintCategoryList(int page=0,int rows=10)
        {
            int total = 0;
            var query = printRepository.QueryByPage(page, rows, out total, p => true, p => p.印品类别编码, true).ToList();
            return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveorUpdatePrintCategory(T_OMS_印品类别编码表 data, string flag)
        {
            // process.统计编码 = process.统计编码.Trim();
            JsonResult jr = null;
            if (flag == "add")
            {
                printRepository.Add(data);
            }
            else if (flag == "edit")
            {
                printRepository.Edit(data);
            }
            else
            {
                jr = Json(new { success = -1, msg = "没有获取到正确的操作标志" }, JsonRequestBehavior.AllowGet);
                return jr;
            }
            try
            {
                printRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeletePrintCategory(List<T_OMS_印品类别编码表> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                printRepository.Delete(item, true);
            }
            try
            {
                printRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page=0, int rows=10)
        {
            var query = printRepository.Query().OrderBy(p=>p.印品类别编码).Skip((page-1)* rows).Take(rows).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult PrintPage(int page = 0, int rows = 10)
        {
            TempData["printCategory_page"] = page;
            TempData["printCategory_rows"] = rows;
            return View();
        }
        public ActionResult GetPrintData()
        {
            int page =(int) TempData["printCategory_page"];
            int rows =(int)TempData["printCategory_rows"];
            var query = printRepository.Query().OrderBy(p => p.印品类别编码).Skip((page - 1) * rows).Take(rows).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}