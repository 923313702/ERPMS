using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.order;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 设计生产流程单
    /// </summary>
    public class ERP_OrderDesignController : Controller
    {
        /// <summary>
        /// T_OMS_订单_Master 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        /// <summary>
        ///  T_MCMS_设计订单_Master 接口
        /// </summary>
        IDesignMasterRepository dmRepository = new DesignMasterRepository();
        /// <summary>
        /// T_MCMS_设计订单_Detail_派工工艺 
        /// </summary>
        IDesignDetailProcessRepository ddRepository = new DesignDetailProcessRepository();
        // GET: ERP_OrderDesign
        public ActionResult Index(string orderNo)
        {
            TempData["design_orderNo"] = orderNo;
            return View();
        }
        public ActionResult IsOrder(string orderNo)
        {
            var count = dmRepository.QueryWhere(p => p.业务单号 == orderNo).Count();
            if (count > 0)
            {
                return Json(new { success = 0, msg = "设计订单已存在" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "申成设计订单" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InitPageData(string orderNo)
        {
            if (string.IsNullOrEmpty(orderNo)) return Json(new { success = -1, msg = "设计单号申请失败" }, JsonRequestBehavior.AllowGet);
            var query = oRepository.QueryWhere(p => p.订单号 == orderNo)
                   .Select(p => new 
                   {
                       业务单号 = p.订单号,
                       订单名称 = p.订单名称,
                       客户编码 = p.客户编码,
                       业务员编码 = p.业务员编码,
                       联系方式 = p.联系方式,
                       交稿日期 = p.交货日期,
                       成品数量 = p.成品数量,
                       成品尺寸 = p.成品尺寸,
                       印品类别编码 = p.印品类别编码,
                       制单人编码 = User.Identity.Name,
                       制单日期 = DateTime.Now
                   }).FirstOrDefault();
            if (query == null) { return Json(new { success = -1, msg = "设计单号申请失败" }, JsonRequestBehavior.AllowGet); }
            ObjectParameter par = new ObjectParameter("number", "");
            T_MCMS_设计订单_Master master = new T_MCMS_设计订单_Master()
            {
                业务单号 = query.业务单号,
                订单名称 = query.订单名称,
                客户编码 = query.客户编码,
                业务员编码 = query.业务员编码,
                联系方式 = query.联系方式,
                交稿日期 = query.交稿日期,
                成品数量 = query.成品数量,
                成品尺寸 = query.成品尺寸,
                印品类别编码 = query.印品类别编码,
                制单人编码 = query.制单人编码,
                制单日期 = query.制单日期
            };
            var result = dmRepository.ExecProc2(par, master);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "设计单号申请成功" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "设计单号申请失败" }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetOrder()
        {
            var query = oRepository.Query().Select(p => new { Key = p.订单名称, Id = p.订单号 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDesignMaster()
        {
            var no = TempData["design_orderNo"].ToString();
            var query = dmRepository.QueryWhere(p => p.业务单号 == no).Select(p => new
            {
                p.订单号,
                p.业务单号,
                p.印品类别编码,
                p.订单名称,
                p.客户编码,
                p.业务员编码,
                p.联系方式,
                p.页数,
                p.交稿日期,
                p.定稿日期,
                p.成品数量,
                p.展开尺寸,
                p.成品尺寸,
                p.加工要求,
                p.备注,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }

        public ActionResult ShowDesignDetail(string paramNo)
        {
            var query = ddRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => new
            {
                p.行号,
                p.订单号,
                p.印品部件,
                p.项目编码,
                p.数量,
                p.单价,
                p.金额,
                p.人员编码,
                p.完工时间,
                p.完成数量
            }).ToList();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }

        public ActionResult RunPage(int page = 0)
        {
            var total = dmRepository.Query().Count();
            var query = dmRepository.Query().OrderByDescending(p => p.订单号).Skip(page).Take(1).Select(p => new {
                p.订单号,
                p.业务单号,
                p.印品类别编码,
                p.订单名称,
                p.客户编码,
                p.业务员编码,
                p.联系方式,
                p.页数,
                p.交稿日期,
                p.定稿日期,
                p.成品数量,
                p.展开尺寸,
                p.成品尺寸,
                p.加工要求,
                p.备注,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
           // return Json(new { total = total, query = query }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrUpdateDesignDetail(T_MCMS_设计订单_Detail_派工工艺 data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                ddRepository.Add(data);
            }
            else if (flag == "edit")
            {
                ddRepository.Edit(data);
            }
            else { return Json(new { success = -1, msg = "没获取到操作标识" }); }
            try
            {
                ddRepository.SaverChanges();
                jr = Json(new { success = 0,id=data.行号, msg = "操作成功" });
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" });
            }
            return jr;
        }
        public ActionResult DeleteDesignDetail(List<T_MCMS_设计订单_Detail_派工工艺> data)
        {
            JsonResult jr = null;
            foreach (var i in data)
            {
                ddRepository.Delete(i, true);
            }
            try
            {
                ddRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        public ActionResult AuditorDesign(T_MCMS_设计订单_Master master)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            master.审核人编码 = User.Identity.Name;
            master.审核日期 = DateTime.Now;
            dmRepository.Edit(master, strArr);
            try
            {
                dmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = master.审核人编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        public ActionResult UnAuditorDesign(T_MCMS_设计订单_Master master)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            master.审核人编码 = null;
            master.审核日期 = null;
            dmRepository.Edit(master, strArr);
            try
            {
                dmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult AddDesignMaster(T_MCMS_设计订单_Master master)
        {
            var query = oRepository.QueryWhere(p => p.订单号 == master.业务单号).FirstOrDefault();
            if (query == null) return Json(new { success = -1, msg = "没有该订单号" }, JsonRequestBehavior.AllowGet);
            ObjectParameter par = new ObjectParameter("number", "");
            var result = dmRepository.ExecProc2(par, master);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "新增设计单号成功", id = par.Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "新增设计单号失败" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDesignMaster(T_MCMS_设计订单_Master master)
        {
            JsonResult jr = null;
            var code = dmRepository.QueryWhere(p => p.订单号 == master.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) return Json(new { success = 0, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            dmRepository.Edit(master);
            try
            {
                dmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = 0, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteDesignMaster(T_MCMS_设计订单_Master master)
        {
            JsonResult jr = null;
            var code = dmRepository.QueryWhere(p => p.订单号 == master.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) return Json(new { success = 0, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet);
            dmRepository.Delete(master, true);
            try
            {
                dmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = 0, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        public ActionResult DesignSheet()
        {
            return View();
        }
        public ActionResult DesignSheetList()
        {
            return View();
        }

        public ActionResult ShowDesignSheetList(SearchParams data, int page = 1, int rows = 10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = " V_DesignSheetList where 1=1";
            if (!string.IsNullOrEmpty(data.orderName))
            {
                sql += " and 订单名称 like @orderName";
                pars.Add(new SqlParameter("@orderName", "%" + data.orderName + "%"));
            }
            if (!string.IsNullOrEmpty(data.orderNo))
            {
                sql += " and 业务单号 =@orderNo";
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
            var total = dmRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 订单号,订单名称,制单日期,定稿日期,制单人,业务员,客户名称,审核人 from (select * , row_number() over( order  by 订单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = dmRepository.SqlQuery2<V_DesignSheetList>(sql, par).ToList();
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
            string sql = " V_DesignSheetList where 1=1";
            if (!string.IsNullOrEmpty(data.orderName))
            {
                sql += " and 订单名称 like @orderName";
                pars.Add(new SqlParameter("@orderName", "%" + data.orderName + "%"));
            }
            if (!string.IsNullOrEmpty(data.orderNo))
            {
                sql += " and 业务单号 =@orderNo";
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
            sql = " select 订单号,订单名称,制单日期,交稿日期,制单人,业务员,客户名称,审核人 from (select * , row_number() over( order  by 订单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = dmRepository.SqlQuery2<V_DesignSheetList>(sql, pars.ToArray()).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
    }
}