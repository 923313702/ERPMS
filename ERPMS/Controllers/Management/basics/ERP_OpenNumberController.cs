
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
    /// 开数编码表
    /// </summary>
    public class ERP_OpenNumberController : Controller
    {
        IOpenNumberRepository oRepository = new OpenNumberRepository();
        // GET: ERP_OpenNumber
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取开数编码表信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult GetOpenNumber(int page = 1, int rows = 10)
        {
            var total =30;
            var list = oRepository.QueryByPage(page, rows, out total, p=>true,p=>true,true).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdate(T_OMS_开数编码表 data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                oRepository.Add(data);

            }
            else if (flag == "edit")
            {
                oRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                oRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.开数编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet); ;
            }
            return jr;
        }

        public ActionResult DeletOpenNumber(List<T_OMS_开数编码表> data)
        {
            List<T_OMS_开数编码表> list2 = new List<T_OMS_开数编码表>();
            JsonResult jr = null;
            if (data == null) return Json(new { success = -1, msg = "参数为空" }, JsonRequestBehavior.AllowGet);
            foreach (var item in data)
            {
                oRepository.Delete(item, true);
            }
            try
            {
                oRepository.SaverChanges();
           
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(int page, int rows)
        {
            var query = oRepository.Query().OrderBy(p => p.开数编码).Select(p => new { p.开数编码, p.开数名称, p.所属开式 }).Skip((page - 1) * rows).Take(rows);
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}