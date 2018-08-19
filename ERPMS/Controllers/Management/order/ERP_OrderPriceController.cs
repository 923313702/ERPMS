using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{

    //业务计价单
    public class ERP_OrderPriceController : Controller
    {
        /// <summary>
        /// 订单_Master
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        /// <summary>
        /// 订单_Detail_计价
        /// </summary>
        IOrderDetailPriceRepository odpRepository = new OrderDetailPriceRepository();
        // GET: OrderPrice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2(string orderNo) {
            TempData["orderPrice_orderNo"] = orderNo;
            return View();
        }
        public  ActionResult InitPage()
        {
            var orderNo = TempData["orderPrice_orderNo"].ToString();
            var query = oRepository.QueryWhere(p=>p.订单号==orderNo).Select(p => new { p.客户类型, p.订单名称, p.订单号, p.成品数量, p.计量单位, p.成品尺寸, p.合同编号, p.调节项, p.帐期, p.结算方式, p.印品类别编码, p.税率, p.业务员编码, p.客户编码, p.开票金额, p.预付金额, p.产品级别, p.单据类型, p.计价审核人编码 }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }
        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ShowOrderMaster(int page = 0)
        {
            var total = oRepository.Query().Count();
            var query = oRepository.Query().OrderByDescending(p => p.订单号).Skip(page).Take(1).Select(p => new { p.客户类型, p.订单名称, p.订单号, p.成品数量, p.计量单位, p.成品尺寸, p.合同编号, p.调节项, p.帐期, p.结算方式, p.印品类别编码, p.税率, p.业务员编码, p.客户编码, p.开票金额, p.预付金额, p.产品级别, p.单据类型, p.计价审核人编码 }).FirstOrDefault();
            // return Json(new { rows = query, total = total }, JsonRequestBehavior.AllowGet);
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
        }
        public ActionResult RefreshOrderMaster(string paramNo)
        {
            var query = oRepository.QueryWhere(p => p.订单号==paramNo).Select(p => new { p.客户类型, p.订单名称, p.订单号, p.成品数量, p.计量单位, p.成品尺寸, p.合同编号, p.调节项, p.帐期, p.结算方式, p.印品类别编码, p.税率, p.业务员编码, p.客户编码, p.开票金额, p.预付金额, p.产品级别, p.单据类型, p.计价审核人编码 }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 显示T_OMS_订单_Detail_计价 数据
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult ShowOrderJiJia(string paramNo)
        {
            string sql = "SELECT   b.项目名称, a.ID, a.订单工艺号,  a.订单材料号, a.订单Detail号, a.订单号,a.项目编码, a.印品部件, " +
                " a.计量单位, a.数量, a.单价,  a.系数, a.金额, a.标准单价,a.统计编码 FROM T_OMS_订单_Detail_计价 a  left JOIN T_OMS_工艺项目编码表 b " +
                " ON a.项目编码 = b.项目编码 where a.订单号=@orderNo and Flag=0 order by a.统计编码";
            SqlParameter par = new SqlParameter("@orderNo", paramNo);
            List<订单_Detail_计价> query = null;
            try
            {
                query = odpRepository.SqlQuery2<订单_Detail_计价>(sql, par).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateOrder(T_OMS_订单_Master order)
        {
            var auditor = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            string[] strArr = { "订单号" ,"订单名称" ,"客户编码","业务员编码","合同编号" ,"印品类别编码","客户类型","预付金额","产品级别" ,
                "成品数量" ,"计量单位" ,"成品尺寸","单据类型" ,"帐期" ,"结算方式","税率", "开票金额","调节项" };
            oRepository.Edit(order, strArr);
            try
            {
                oRepository.SaverChanges();
                return Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteOrder(T_OMS_订单_Master order)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            oRepository.Delete(order, true);
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
        /// 计价单审核
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult AuditorJijia(T_OMS_订单_Master order)
        {
            
            JsonResult jr = null;
           // var sum = odpRepository.QueryWhere(p => p.订单号 == order.订单号&& p.Flag==0).Sum(p => p.金额);
            string[] strArr = { "计价审核人编码", "计价审核日期", "材料费", "设计费", "CTP版费", "印制费", "加工费", "手工费", "覆膜费", "返款金额", "其他金额", "税费", "金额" };
            order.计价审核人编码 = User.Identity.Name; ;
            order.计价审核日期 = DateTime.Now;
          //  order.金额 = sum;
            oRepository.Edit(order, strArr);
         
            try
            {
                oRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = order.计价审核人编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 撤审
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>

        public ActionResult UnAuditorJijia(T_OMS_订单_Master order)
        {
            var auditor = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            JsonResult jr = null;
            string[] strArr = { "计价审核人编码", "计价审核日期" };
            order.计价审核人编码 = null;
            order.计价审核日期 = null;
            oRepository.Edit(order, strArr);
            try
            {
                oRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 导入计价
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult ImportJijia(string orderNo)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.计价审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核,不予许操作" }, JsonRequestBehavior.AllowGet); }
            var query = odpRepository.QueryWhere(p => p.订单号 == orderNo && p.Flag == 1).ToList(); ;
            if (query.Count > 0)
            {
                foreach (var i in query)
                {
                    i.Flag = 0;
                    odpRepository.Edit(i);
                }
                try
                {
                    odpRepository.SaverChanges();
                    jr = Json(new { success = 0, msg = "导入成功" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    jr = Json(new { success = -1, msg = "导入失败" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                jr = Json(new { success = -1, msg = "没有要导入的数据" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 修改计价
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult UpdateJijia(T_OMS_订单_Detail_计价 data)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.计价审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核,不予许操作" }, JsonRequestBehavior.AllowGet); }
            try
            {
                odpRepository.Edit(data);
                odpRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除计价
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult DeleteJijia(List<T_OMS_订单_Detail_计价> data)
        {
            JsonResult jr = null;
            string orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.计价审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核,不予许操作" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                string[] strArr = { "Flag" };
                item.Flag = 1;
                odpRepository.Edit(item, strArr);
            }
            try
            {
                odpRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
    }
}