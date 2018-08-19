using ERPMS.IRepository.Basics;
using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Repository.order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 发货
    /// </summary>
    public class ERP_OrderInvoiceController : Controller
    {
        /// <summary>
        /// T_OMS_订单_Master 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();

        /// <summary>
        ///    客户编码表 接口
        /// </summary>
        ICustomerRepository cRepository = new CustomerRepository();

        /// <summary>
        /// 人员编码表接口
        /// </summary>
        IStaffRepository sRepository = new StaffRepository();

        /// <summary>
        ///  T_OMS_订单_发货申请单
        /// </summary>
        IOrderInvoiceRepository oiRepository = new OrderInvoiceRepository();
        // GET: ERP_OrderInvoice
        public ActionResult Index(string orderNo)
        {
            TempData["orderInvoice_orderNo"] = orderNo;
            T_OMS_订单_发货申请单 single = null;
            if (!string.IsNullOrEmpty(orderNo))
            {
                int count = oiRepository.QueryWhere(p => p.订单号 == orderNo).Count();
                if (count <= 0)
                {
                    var query = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => new { p.业务员编码, p.成品数量, p.客户编码 }).SingleOrDefault();
                    if (query != null)
                    {
                        single = new T_OMS_订单_发货申请单() { 订单号 = orderNo, 申请人编码 = query.业务员编码, 制单日期 = DateTime.Now, 日期 = DateTime.Now, 数量 = query.成品数量, 客户编码 = query.客户编码 };
                    }

                    ObjectParameter par = new ObjectParameter("no", "");
                    int result = oiRepository.ExecProc2(par, single);
                    if (result < 0)
                    {
                        return Content("发货申请单申成错误");
                    }

                }
            }

            return View();
        }

        public ActionResult InitPage()
        {
            var orderNo = TempData["orderInvoice_orderNo"].ToString();
            var query = oiRepository.QueryWhere(p => p.订单号 == orderNo).FirstOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);

        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrder()
        {
            var query = oRepository.Query().Select(p => new { Id = p.订单号, Key = p.订单名称, Quantity = p.成品数量 ,CustomerNo=p.客户编码}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 客户编码表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustomer()
        {
            var list = cRepository.Query().Select(p => new { Key = p.客户名称, Id = p.客户编码 }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 业务员
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSalesman()
        {
            var list = sRepository.Query().Select(p => new { Key = p.姓名, Id = p.人员编码 }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddInvoice(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            ObjectParameter par = new ObjectParameter("no", "");
            int result = oiRepository.ExecProc2(par, data);
            if (result > 0)
            {
                jr = Json(new { success = 0, msg = "保存成功", applyNo = par.Value }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                jr = Json(new { success = -1, msg = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        public ActionResult UpdateInvoice(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            oiRepository.Edit(data);
            try
            {
                oiRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        public ActionResult DeleteInvoice(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            oiRepository.Delete(data, true);
            try
            {
                oiRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult Auditor(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            data.审核人编码 = User.Identity.Name;
            data.审核日期 = DateTime.Now;
            string[] strArr = { "审核人编码", "审核日期" };
            oiRepository.Edit(data, strArr);
            try
            {
                oiRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核通过", AuditorNo = data.审核人编码, Date = data.审核日期 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核未通过" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 撤审
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult UnAuditor(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            data.审核人编码 = null;
            data.审核日期 = null;
            string[] strArr = { "审核人编码", "审核日期" };
            oiRepository.Edit(data, strArr);
            try
            {
                oiRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功", AuditorNo = data.审核人编码, Date = data.审核日期 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name = "page" ></ param >
        /// < returns ></ returns >
        public ActionResult RunPage(int page = 0)
        {
            var total = oiRepository.Query().Count();
            var query = oiRepository.Query().OrderByDescending(p => p.申请单号).Select(p => new
            {
                p.申请单号,
                p.日期,
                p.订单号,
                p.申请人编码,
                p.客户编码,
                p.发货地址,
                p.发货地址二,
                p.发货地址三,
                p.联系方式,
                p.联系方式二,
                p.印品名称,
                p.数量,
                p.发货数量,
                p.备注,

                p.审核人编码,
                p.审核日期,
                p.发货标识,
                p.发货人编码,
                p.结款标识,
                p.标识日期,
                p.制单人编码,
                p.制单日期
            }).Skip(page).Take(1).FirstOrDefault();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
            /*return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet)*/;
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult Settlement(T_OMS_订单_发货申请单 data)
        {
            JsonResult jr = null;
            data.结款标识 = 1;
            data.标识日期 = DateTime.Now;
            string[] strArr = { "结款标识", "标识日期" };
            var single = oiRepository.QueryWhere(p => p.申请单号 == data.申请单号).Select(p => p.审核人编码).SingleOrDefault();
            if (string.IsNullOrEmpty(single)) { return Json(new { success = -1, msg = "该单号没有审核" }, JsonRequestBehavior.AllowGet); }
            oiRepository.Edit(data, strArr);
            try
            {
                oiRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "结款成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "结款失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
    }

}