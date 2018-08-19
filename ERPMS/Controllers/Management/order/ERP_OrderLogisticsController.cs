using ERPMS.IRepository.Basics;
using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.basics;
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
    /// 物流 页面
    /// </summary>
    public class ERP_OrderLogisticsController : Controller
    {
        /// <summary>
        /// T_PDMS_物流申请单_Master 接口
        /// </summary>
        IOrderlogisticsRepository olRepository = new OrderlogisticsRepository();
        /// <summary>
        /// T_PDMS_物流申请单_Detail 接口
        /// </summary>
        IOrderlogisticsDetailRepository oldRepository = new OrderlogisticsDetailRepository();
        /// <summary>
        ///  T_PDMS_物流公司编码表 接口
        /// </summary>
        ILcompanyRepository lcRepository = new LcompanyRepository();

        /// <summary>
        /// 订单master 接口
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
        // GET: ERP_OrderLogistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Addlogistics(T_PDMS_物流申请单_Master master)
        {
            var identity = User.Identity.Name;
            //todo 要改回来
            //if (string.IsNullOrEmpty(identity))
            //{
            //    return Json(new { success = -1, msg = "没有获取到登录账号,请重新登录" }, JsonRequestBehavior.AllowGet);
            //}
            master.制单人编码 = /*identity*/ "demo";
           
            ObjectParameter par = new ObjectParameter("number", "");
            var result = olRepository.ExecProc2(par, master);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "物流单申请成功", id = par.Value }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = -1, msg = "物流单申请失败" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Updatelogistics(T_PDMS_物流申请单_Master master)
        {
            JsonResult jr = null;
            olRepository.Edit(master);
            try
            {
                olRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult Deletlogistics(T_PDMS_物流申请单_Master data)
        {
            JsonResult jr = null;
            olRepository.Delete(data, true);
            try
            {
                olRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        /// <summary>
        /// 物流公司
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCompany()
        {
            var query = lcRepository.Query().Select(p => new { Key = p.名称, Id = p.物流公司编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrder()
        {
            var query = oRepository.Query().Select(p => new { Id = p.订单号, Key = p.订单名称, Cno = p.客户编码, Pno = p.业务员编码 }).ToList();
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

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public ActionResult Auditing(T_PDMS_物流申请单_Master master)
        {
            JsonResult jr = null;
            master.审核人编码 = User.Identity.Name;
            master.审核日期 = DateTime.Now;
            string[] strArr = { "审核人编码", "审核日期", "运费金额", "进仓费", "金额" };
            olRepository.Edit(master, strArr);
            try
            {
                olRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核通过", AuditorNo = master.审核人编码 }, JsonRequestBehavior.AllowGet);
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
        /// <param name="master"></param>
        /// <returns></returns>
        public ActionResult Withdrawal(T_PDMS_物流申请单_Master master)
        {
            JsonResult jr = null;
            master.审核人编码 = null;
            master.审核日期 = null;
            master.金额 = null;
            master.进仓费 = null;
            master.运费金额 = null;
            string[] strArr = { "审核人编码", "审核日期", "运费金额", "进仓费", "金额" };
            olRepository.Edit(master, strArr);
            try
            {
                olRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
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
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult RunPage(int page = 0)
        {
            int total = 0;
             total = olRepository.Query().Count();
            var query = olRepository.Query().OrderByDescending(p => p.申请单号).Select(p => new { p.申请单号, p.日期, p.摘要, p.审核人编码, p.物流公司编码, p.物流经办人, p.到货地址, p.到货地址1, p.到货地址2, p.联系方式, p.联系方式1, p.联系方式2, p.运费金额, p.进仓费, p.金额, p.结账标志, p.发货人, p.制单日期 }).Skip(page).Take(1).FirstOrDefault();
            var ss = JsonConvert.SerializeObject(new { total=total,rows=query});
            return Content(ss);
        }

        /// <summary>
        /// 物流详情
        /// </summary>
        /// <param name="applyNum"></param>
        /// <returns></returns>
        public ActionResult ShowLogisticsDetail(string paramNo)
        {
            string sql = " select 行号, 订单号, 客户编码, 订单名称, 进仓费, 金额, 项目分类, 申请单号, 业务员编码,客户名称,姓名 from V_logistics_detail where 申请单号=@applyNum";
            SqlParameter par = new SqlParameter("@applyNum", paramNo);
            var query = oldRepository.SqlQuery2<物流申请单_Detail>(sql, par).ToList();
            //  var query = oldRepository.QueryWhere(p => p.申请单号 == applyNum).Select(p => new { p.行号, p.订单号, p.客户编码, p.订单名称, p.进仓费, p.金额, p.项目分类, p.申请单号, p.业务员编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加修改物流详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult SaveOrUpdateLoginsticsDetail(T_PDMS_物流申请单_Detail data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                oldRepository.Add(data);
            }
            else if (flag == "edit")
            {
                oldRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                oldRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功",id=data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除物流详情
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult DeleteLoginsticsDetail(List<T_PDMS_物流申请单_Detail> data)
        {
            JsonResult jr = null;
            foreach (var i in data)
            {
                oldRepository.Delete(i, true);
            }
            try
            {
                oldRepository.SaverChanges();
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