using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 计价单明细
    /// </summary>
    public class ERP_OrderAccountController : Controller
    {
        /// <summary>
        /// T_OMS_订单_Detail_附件 接口
        /// </summary>
        IOrderAccessoryResponse oaRepository = new OrderAccessoryResponse();
        /// <summary>
        /// order jiekou
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        // GET: ERP_OrderAccount
        public ActionResult Index(string orderNo)
        {
            TempData["orderNo"] = orderNo;
            return View();
        }
        public ActionResult ShowOrder()
        {
            var orderNo = TempData["orderNo"].ToString();
            if (string.IsNullOrEmpty(orderNo)) return null;
            var query = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => new { p.订单号, p.订单名称, p.制单人编码, p.审核人编码, p.制单日期, p.审核日期 }).SingleOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
            
        }
        public ActionResult SaveOrUpdateAccessory(T_OMS_订单_Detail_附件 data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                oaRepository.Add(data);
            }
            else if (flag == "edit")
            {
                oaRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                oaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        public ActionResult DeleteAccessory(List<T_OMS_订单_Detail_附件> data)
        {
            JsonResult jr = null;
            foreach (var i in data)
            {
                oaRepository.Delete(i, true);
            }
            try
            {
                oaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult GetAccessoryData(string paramNo)
        {
            var query = oaRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => new { p.拼数, p.金额, p.成品数, p.名称, p.用料尺寸, p.版号, p.订单号, p.单价, p.行号 });
            return Json(query);
        }

    }
}