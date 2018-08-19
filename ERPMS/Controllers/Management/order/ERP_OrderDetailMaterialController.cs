using ERPMS.Models;
using ERPMS.IRepository.order;
using ERPMS.Repository.order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    public class ERP_OrderDetailMaterialController : Controller
    {


        /// <summary>
        /// T_OMS_订单_Master 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        /// <summary>
        /// T_OMS_订单_Detail_材料 接口
        /// </summary>
        IOrderDetailMaterialRepository odmRepository = new OrderDetailMaterialRepository();
        /// <summary>
        /// 订单detail_材料
        /// </summary>
        /// <returns></returns>
        // GET: ERP_OrderDetailMaterial
        public ActionResult Index(string orderNo)
        {
            TempData["orderMaterial_orderNo"] = orderNo;
            return View();
        }
        public ActionResult InitPage()
        {
            var orderNo = TempData["orderMaterial_orderNo"].ToString();
            var query = oRepository.QueryWhere(p=>p.订单号==orderNo).Select(p => new { p.订单号, p.审核人编码, p.客户编码, p.成品数量, p.订单名称, p.交货日期, p.业务员编码, p.包装材料 }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }
        public ActionResult RunPage(int page = 0)
        {
            var total = oRepository.Query().Count();
            var query = oRepository.Query().OrderByDescending(p => p.订单号).Skip(page).Take(1).Select(p => new { p.订单号, p.审核人编码, p.客户编码, p.成品数量, p.订单名称, p.交货日期, p.业务员编码, p.包装材料 }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
            //return Json(new { total = total, query = query }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateOrderMaterial(T_OMS_订单_Detail_材料 data, string flag)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            string[] arr = { "数量", "计量单位", "材料编码" };
            odmRepository.Edit(data, arr);
            try
            {
                odmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteOrderMaterial(List<T_OMS_订单_Detail_材料> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                odmRepository.Delete(item, true);
            }
            try
            {
                odmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult UpdateMasterRemark(T_OMS_订单_Master data)
        {
            JsonResult jr = null;
            string[] strArr = { "包装材料 " };
            oRepository.Edit(data, strArr);
            try
            {
                oRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "备注成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "备注失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult GetMaterial(string paramNo)
        {
            var sql = "SELECT a.行号, a.订单号,a.印品部件, a.材料编码,  a.数量, a.计量单位, b.规格型号,b.材料名称 FROM T_OMS_订单_Detail_材料 a left JOIN T_MIMS_材料编码表 b ON a.材料编码 = b.材料编码 where a.订单号=@orderNo";
            SqlParameter par = new SqlParameter("@orderNo", paramNo);
            var query = odmRepository.SqlQuery2<订单_Detail_材料>(sql, par);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}