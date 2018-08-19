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
    public class ERP_JiaFangLuController : Controller
    {
        /// <summary>
        /// 基础_加放率表接口
        /// </summary>
        IJiaFangLuRepository jiaRepository = new JiaFangLuRepository();
        // GET: ERP_JiaFangLu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetJiaFangLuList(int page = 0, int rows = 10)
        {
            int total = 0;
            var query = jiaRepository.QueryByPage(page, rows, out total, p => true, p => p.序号, true).
                Select (p=>new { p.序号,p.最低印数,p.最高印数,p.印刷业务放数,p.印刷业务加放率,p.印刷生产加放率,p.印刷生产放数,p.装订业务加放率,p.装订业务放数,p.装订生产加放率,p.装订生产放数,p.备注})
                .ToList();
            return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveorUpdateJiaFangLu(T_OMS_基础_加放率表 data, string flag)
        {
            // process.统计编码 = process.统计编码.Trim();
            JsonResult jr = null;
            if (flag == "add")
            {
                jiaRepository.Add(data);
            }
            else if (flag == "edit")
            {
                jiaRepository.Edit(data);
            }
            else
            {
                jr = Json(new { success = -1, msg = "没有获取到正确的操作标志" }, JsonRequestBehavior.AllowGet);
                return jr;
            }
            try
            {
                jiaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteJiaFangLu(List<T_OMS_基础_加放率表> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                jiaRepository.Delete(item, true);
            }
            try
            {
                jiaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page = 0, int rows = 10)
        {
            var query = jiaRepository.Query().OrderBy(p => p.序号).Skip((page - 1) * rows).Take(rows).
                Select(p => new { p.序号, p.最低印数, p.最高印数, p.印刷业务放数, p.印刷业务加放率,
                    p.印刷生产加放率, p.印刷生产放数, p.装订业务加放率, p.装订业务放数, p.装订生产加放率,
                    p.装订生产放数, p.备注 }) .ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult PrintPage(int page = 0, int rows = 10)
        {
            TempData["JiaFangLu_page"] = page;
            TempData["JiaFangLu_rows"] = rows;
            return View();
        }
        public ActionResult GetPrintData()
        {
            int page = (int)TempData["JiaFangLu_page"];
            int rows = (int)TempData["JiaFangLu_rows"];
            var query = jiaRepository.Query().OrderBy(p => p.序号).Skip((page - 1) * rows).Take(rows).
               Select(p => new { p.最低印数, p.最高印数, p.印刷业务放数, p.印刷业务加放率, p.印刷生产加放率,
                   p.印刷生产放数, p.装订业务加放率, p.装订业务放数, p.装订生产加放率, p.装订生产放数, p.备注 })
                .ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}