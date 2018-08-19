using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.order;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 预收款凭单
    /// </summary>
    public class ERP_OrderAdvanceController : Controller
    {
        IOrderAdvanceRepository oaRepository = new OrderAdvanceRepository();
        // GET: ERP_OrderAdvance
        public ActionResult AdvanceIndex()
        {
            return View();
        }

        public ActionResult AddAdvance(T_OMS_订单_预收款凭单 data)
        {
            ObjectParameter par = new ObjectParameter("no", "");
            data.制单人编码 = User.Identity.Name;
             int result =oaRepository.ExecProc2(par, data);
            if (result > 0)
                return Json(new { success = 0, msg = "添加成功", id = par.Value.ToString() },JsonRequestBehavior.AllowGet);
            return Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAdvance(T_OMS_订单_预收款凭单 data)
        {
            JsonResult jr = null;
            oaRepository.Edit(data);
            try
            {
                oaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;

        }

        public ActionResult DeleteAdvance(T_OMS_订单_预收款凭单 data)
        {
            JsonResult jr = null;
            oaRepository.Delete(data, true);
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

        public ActionResult RunPage(int page = 0)
        {

            int total = oaRepository.Query().Count();
            var query = oaRepository.Query().OrderByDescending(p => p.单号).Skip(page).Take(1).Select(p => new
            {
                p.单号,
                p.客户编码,
                p.订单号,
                p.订单名称,
                p.送货联系方式,
                p.业务员编码,
                p.成品数量,
                p.计量单位,
                p.成品尺寸,
                p.交货日期,
                p.结款方式,
                p.预收金额,
                p.金额,
                p.应收金额,
                p.提货说明,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(new { total=total,rows=query});
            return Content(response);
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult Auditoring(T_OMS_订单_预收款凭单 data)
        {

            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            data.审核人编码 = User.Identity.Name;
            data.审核日期 = DateTime.Now;
            oaRepository.Edit(data, strArr);
            try
            {
                oaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = data.审核人编码 }, JsonRequestBehavior.AllowGet);
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
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult UnAuditor(T_OMS_订单_预收款凭单 data)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            data.审核人编码 = null;
            data.审核日期 = null;
            oaRepository.Edit(data, strArr);
            try
            {
                oaRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        public ActionResult AdvanceList()
        {
            return View();
        }
        public ActionResult ShowAdvanceList(SearchParams data, int page = 1, int rows = 10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = " V_AdvanceList where 1=1";
            if (!string.IsNullOrEmpty(data.orderName))
            {
                sql += " and 订单名称 like @orderName";
                pars.Add(new SqlParameter("@orderName", "%" + data.orderName + "%"));
            }
            if (!string.IsNullOrEmpty(data.orderNo))
            {
                sql += " and 订单号 =@orderNo";
                pars.Add(new SqlParameter("@orderNo", data.orderNo));
            }
           
            if (!string.IsNullOrEmpty(data.customerNo))
            {
                sql += " and 客户编码 =@customerNo";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (!string.IsNullOrEmpty(data.saleMan))
            {
                sql += " and 业务员编码=@saleMan";
                pars.Add(new SqlParameter("@saleMan", data.saleMan));
            }
            if (data.zhidanStart != null && data.zhidanEnd != null)
            {
                sql += " and 制单日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            var strSql = "select count(*) from " + sql;
            var total = oaRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 单号,订单号,订单名称,制单日期,金额,预收金额,应收金额,制单人,业务员,客户名称,审核人,审核日期 from (select * , row_number() over( order  by 单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = oaRepository.SqlQuery2<V_AdvanceList>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
            // return Json(new { total=total,rows=rows},JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcel(SearchParams data)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql = " V_AdvanceList where 1=1";
            if (!string.IsNullOrEmpty(data.orderName))
            {
                sql += " and 订单名称 like @orderName";
                pars.Add(new SqlParameter("@orderName", "%" + data.orderName + "%"));
            }
            if (!string.IsNullOrEmpty(data.orderNo))
            {
                sql += " and 订单号 =@orderNo";
                pars.Add(new SqlParameter("@orderNo", data.orderNo));
            }
            if (!string.IsNullOrEmpty(data.customerNo))
            {
                sql += " and 客户编码 =@customerNo";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (!string.IsNullOrEmpty(data.saleMan))
            {
                sql += " and 业务员编码=@saleMan";
                pars.Add(new SqlParameter("@saleMan", data.saleMan));
            }
            if (data.zhidanStart != null && data.zhidanEnd != null)
            {
                sql += " and 制单日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            sql = " select 单号,订单号,订单名称,制单日期,金额,预收金额,应收金额,制单人,业务员,客户名称,审核人,审核日期 from (select * , row_number() over( order  by 单号 desc)as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = oaRepository.SqlQuery2<V_AdvanceList>(sql, pars.ToArray()).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}