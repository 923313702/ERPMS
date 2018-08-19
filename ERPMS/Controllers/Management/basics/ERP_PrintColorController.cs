
using ERPMS.IRepository.Basics;
using ERPMS.Models;
using ERPMS.Repository.Basics;
using ERPMS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.basics
{
    /// <summary>
    /// 印色编码表
    /// </summary>
    public class ERP_PrintColorController : Controller
    {
        IPrintColorRepository pRepository = new PrintColorRepository();
        // GET: ERP_PrintColor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPrintColor(int page = 1, int rows = 10)
        {
            int total = 0;
            var colorData = pRepository.QueryByPage(page, rows, out total, p => true, p => p.印色编码, true).ToList();
            return Json(new { total = total, rows = colorData }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查添加印色记录主键是否重复
        /// </summary>
        /// <param name="colorNo"></param>
        /// <returns></returns>
        public ActionResult CheckPrintColorNo(int colorNo)
        {
            var flag = pRepository.QueryWhere(p => p.印色编码 == colorNo).Count() > 0;
            return Content(flag.ToString());
        }

        public ActionResult SaveOrUpdate(T_OMS_基础_印色编码表 data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                pRepository.Add(data);
            }
            else if (flag == "edit")
            {
                pRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到状态" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;

        }

        public ActionResult DeletePrintColor(List<T_OMS_基础_印色编码表> data)
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
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page, int rows)
        {
            var list = pRepository.QueryOrderBy(p => true, p => p.印色编码, true).Skip((page - 1) * rows).Take(rows);
            //var list = (from p in db.T_OMS_基础_印色编码表 orderby p.印色编码 select p).Skip((pageNum - 1) * pageSize).Take(pageSize);
            DataTable tb = list.ToDataTable(p => new object[] { list });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}