using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 计价单报价
    /// </summary>
    public class ERP_OrderPriceOfferController : Controller
    {
        /// <summary>
        /// T_OMS_业务报价单_Master 接口
        /// </summary>
        IOfferRepository offerRepository = new OfferRepository();
        /// <summary>
        /// T_OMS_业务报价单_Detail 接口
        /// </summary>
        IOfferDetailRepository odRepository = new OfferDetailRepository();
        /// <summary>
        /// T_OMS_业务报价单_Detail_计价 接口
        /// </summary>
        IOfferPriceRepository opRepository = new OfferPriceRepository();
        /// <summary>
        /// 业务承接单 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        // GET: ERP_OrderPriceOffer
        public ActionResult Index(string orderNo)
        {
            TempData["orderPriceOffer_orderNo"] = orderNo;
            return View();
        }
        public ActionResult InitPage()
        {
            var orderNo = TempData["orderPriceOffer_orderNo"].ToString();
            T_OMS_业务报价单_Master master = null;
            ObjectParameter par = new ObjectParameter("primaryKey", "");

            master = offerRepository.QueryWhere(p => p.订单号 == orderNo).FirstOrDefault();
            if (master == null)
            {
                var order = oRepository.QueryWhere(p => p.订单号 == orderNo).FirstOrDefault();
                if (order != null)
                {
                    master = new T_OMS_业务报价单_Master(order.订单号, order.订单名称, order.客户编码, order.业务员编码, order.合同编号, null, order.成品数量, order.计量单位, order.成品尺寸, null, null, DateTime.Now);
                    var result = offerRepository.ExecProc2(par, master);
                    if (result > 0)
                        master.报价单号 = par.Value.ToString();
                }
            }
            var response = JsonConvert.SerializeObject(master);
            return Content(response);
        }

        public ActionResult Update(T_OMS_业务报价单_Master master)
        {
            JsonResult jr = null;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == master.报价单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            offerRepository.Edit(master);
            try
            {
                offerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;

        }

        public ActionResult Delete(T_OMS_业务报价单_Master master)
        {
            JsonResult jr = null;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == master.报价单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            offerRepository.Delete(master, true);
            try
            {
                offerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult Auditor(T_OMS_业务报价单_Master master)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码" };
            master.审核人编码 = User.Identity.Name;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == master.报价单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            offerRepository.Edit(master, strArr);
            try
            {
                offerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = master.审核人编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult UnAuditor(T_OMS_业务报价单_Master master)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码" };
            master.审核人编码 = null;
            offerRepository.Edit(master, strArr);
            try
            {
                offerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult GetOfferDetail(string paramNo)
        {
            SqlParameter par = new SqlParameter("@paramNo", paramNo);
            string sql = "select * from V_orderPriceoffer where 报价单号=@paramNo";
            var query = odRepository.SqlQuery2<业务报价单_Detail>(sql, par).ToList();
            // var query = odRepository.QueryWhere(p => p.报价单号 == paramNo).Select(p => new { p.行号, p.报价单号, p.印品部件, p.纸张客户编码, p.纸张编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOfferPrice(string paramNo)
        {
            var query = opRepository.QueryOrderBy(p => p.报价单号 == paramNo, p => p.印品部件, true).Select(p => new { flag = p.印品部件, p.ID, p.报价单号, p.印品部件, p.项目编码, p.计量单位, p.单价, p.数量, p.备注, p.金额, }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加修改业务报价单_Detail 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>

        public ActionResult SaveOrUpdateOfferDetail(T_OMS_业务报价单_Detail data, string flag)
        {
            JsonResult jr = null;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == data.报价单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            if (flag == "add")
            {
                odRepository.Add(data);

            }
            else if (flag == "edit")
            {
                odRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                odRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", No = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除业务报价单_Detail 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteDetail(List<T_OMS_业务报价单_Detail> data)
        {
            JsonResult jr = null;
            var No = data[0].报价单号;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == No).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            foreach (var i in data)
            {
                odRepository.Delete(i, true);
            }
            try
            {
                odRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult SaveOrUpdateOfferPrice(T_OMS_业务报价单_Detail_计价 data, string flag)
        {
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == data.报价单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            JsonResult jr = null;
            if (flag == "add")
            {
                opRepository.Add(data);

            }
            else if (flag == "edit")
            {
                opRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                opRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", No = data.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;

        }

        public ActionResult DeleteOfferPrice(List<T_OMS_业务报价单_Detail_计价> data)
        {
            JsonResult jr = null;
            var No = data[0].报价单号;
            var auditor = offerRepository.QueryWhere(p => p.报价单号 == No).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor))
                return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            foreach (var i in data)
            {
                opRepository.Delete(i, true);
            }
            try
            {
                opRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

    }
}