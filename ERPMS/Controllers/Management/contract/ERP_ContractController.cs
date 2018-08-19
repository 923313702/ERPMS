using ERPMS.IRepository.Basics;
using ERPMS.IRepository.contract;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Repository.contract;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.contract
{
    public class ERP_ContractController : Controller
    {
        /// <summary>
        ///  T_OMS_合同_Master 接口
        /// </summary>
        IContractRepository cRepository = new ContractRepository();
        /// <summary>
        /// T_OMS_合同_Detail_工艺 接口
        /// </summary>
        IContractProcessRepository cpRepository = new ContractProcessRepository();
        /// <summary>
        /// T_OMS_合同_Detail_印品 接口
        /// </summary>
        IContractPrintRepository cprintRepository = new ContractPrintRepository();
        // GET: ERP_Contract
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddContract(T_OMS_合同_Master data)
        {
            data.制单日期 = DateTime.Now;
            data.制单人编码 = User.Identity.Name;
            ObjectParameter par = new ObjectParameter("no", "");
            int result = cRepository.ExecProc2(par, data);
            if (result > 0) {
               return  Json(new { success = 0, msg = "添加成功", id = par.Value.ToString() },JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "添加失败" },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateContract(T_OMS_合同_Master data)
        {
            JsonResult jr = null;
            int count = cpRepository.QueryWhere(p => p.合同号 == data.合同号).Count();
            string[] strArr = { "工艺要求", "备注" };
            data.T_OMS_合同_Detail_工艺.合同号 = data.合同号;
            if (count > 0)
            {
                cpRepository.Edit(data.T_OMS_合同_Detail_工艺, strArr);
            }
            else
            {
                cpRepository.Add(data.T_OMS_合同_Detail_工艺);
            }
           string [] strArr2 = {"客户编码","业务员编码","金额","合同内容","provider","buyer" };
            cRepository.Edit(data, strArr2);
            try
            {
                cRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteContract(T_OMS_合同_Master data)
        {
            JsonResult jr = null;
            cRepository.Delete(data, true);
            try
            {
                cRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult SaveOrUpdatePrint(T_OMS_合同_Detail_印品 data,string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                cprintRepository.Add(data);
            }
            else if (flag == "edit")
            {
                cprintRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                cprintRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 });
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" });
            }
            return jr;
        }

        public ActionResult DeletePrint(List<T_OMS_合同_Detail_印品> data)
        {
            JsonResult jr = null;
            foreach (var i in data) {
                cprintRepository.Delete(i, true);
            }
            try
            {
                cprintRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" });
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" });
            }
            return jr;
        }

        public ActionResult RunPage(int page =0)
        {
            int total = 0;
            total = cRepository.Query().Count();
            var query = cRepository.Query().OrderByDescending(p => p.合同号).Select(p => new { p.合同号, p.客户编码, p.业务员编码, p.审核人编码, p.金额, p.合同内容, p.provider, p.buyer ,remark=p.T_OMS_合同_Detail_工艺.备注,ask=p.T_OMS_合同_Detail_工艺.工艺要求}).Skip(page).Take(1).FirstOrDefault();
         
            var ss = JsonConvert.SerializeObject(new { total=total,rows=query});
            return Content(ss);
        }
        public ActionResult ShowContractPrint(string paramNo)
        {
            var query = cprintRepository.QueryWhere(p => p.合同号 == paramNo).Select(p => new { p.合同号, p.单价, p.成品数量, p.行号, p.印品名称, p.交货日期, p.不含税金额, p.计量单位, p.合同金额 }).ToList();
            var ss = JsonConvert.SerializeObject(query);
            return Content(ss);
        }

        public ActionResult Auditor(T_OMS_合同_Master data) {

            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            data.审核人编码 = User.Identity.Name;
            data.审核日期 = DateTime.Now;
            cRepository.Edit(data, strArr);
            try
            {
                cRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = data.审核人编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult UnAuditor(T_OMS_合同_Master data)
        {
           
            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            data.审核人编码 = null;
            data.审核日期 = null;
            cRepository.Edit(data, strArr);
            try
            {
                cRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        public ActionResult PrintPage(string contractNo,string customerName,string saleMan)
        {
            TempData["contractNo"] = contractNo;
            TempData["contract_customerName"] = customerName;
            TempData["constract_saleMan"] = saleMan;
            return View();
        }
        /// <summary>
        /// 合同打印
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintData()
        {
            string contractNo = TempData["contractNo"].ToString();
            string customerName =TempData["contract_customerName"].ToString();
            string saleMane =TempData["constract_saleMan"] .ToString();
            var query = cRepository.QueryWhere(p => p.合同号 == contractNo).Select(p => new Contract{ ContractNo= p.合同号, CustomerName=customerName, SaleMan=saleMane,Money= p.金额, Content= p.合同内容, Provider=p.provider, Buyer=p.buyer, Remark = p.T_OMS_合同_Detail_工艺.备注, Ask = p.T_OMS_合同_Detail_工艺.工艺要求 }).FirstOrDefault();
            var rows = cprintRepository.QueryWhere(p => p.合同号 == contractNo).Select(p => new { 产品名称=p.印品名称, 规格=p.成品尺寸,  计量单位=p.计量单位, 数量 = p.成品数量, 单价 = p.单价, 金额 =p.合同金额, 不含税金额 = p.不含税金额, 交货日期 = p.交货日期 }).ToList();
            var ss = JsonConvert.SerializeObject(new { query=query,rows=rows});
            return Content(ss);
        }


        public ActionResult ContractList()
        {
            return View();
        }

        public ActionResult GetContractNo()
        {
            var query = cRepository .Query ().Select(p => new { Key = p.合同号, Id = p.合同号 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowContractList(SearchParams data, int page = 0, int rows = 10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = "V_ContractList where 1=1";
            if (!string.IsNullOrEmpty(data.contractNo)) {
                sql += " and 合同号 like @contractNo ";
                pars.Add(new SqlParameter("@contractNo","%"+ data.contractNo+"%"));
            }
            if (!string.IsNullOrEmpty(data.saleMan)) {
                sql += " and 业务员编码= @saleMan ";
                pars.Add(new SqlParameter("@saleMan", data.saleMan));
            }
            if (!string.IsNullOrEmpty(data.customerNo)) {
                sql += " and 客户编码= @customerNo ";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (data.zhidanStart != null && data.zhidanStart != null) {
                sql += " and 制单日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            var strSql = "select count(*) from " + sql;
            var total = cRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 合同号, 交货日期, 金额, 审核日期, 制单日期,评审单号,客户法定人,审核人,客户名称 from (select * , row_number() over( order  by 合同号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = cRepository.SqlQuery2<V_ContractList>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);

        }

        public ActionResult ContractListPrintPage(SearchParams data)
        {
            TempData["contractPrintData"] = data;
            return View();
        }
        /// <summary>
        /// 合同清单打印
        /// </summary>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult GetPrintData()
        {
            SearchParams data = TempData["contractPrintData"] as SearchParams;
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql = "V_ContractList where 1=1";
            if (!string.IsNullOrEmpty(data.contractNo))
            {
                sql += " and 合同号 like @contractNo ";
                pars.Add(new SqlParameter("@contractNo", "%" + data.contractNo + "%"));
            }
            if (!string.IsNullOrEmpty(data.saleMan))
            {
                sql += " and 业务员编码= @saleMan ";
                pars.Add(new SqlParameter("@saleMan", data.saleMan));
            }
            if (!string.IsNullOrEmpty(data.customerNo))
            {
                sql += " and 客户编码= @customerNo ";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (data.zhidanStart != null && data.zhidanStart != null)
            {
                sql += " and 制单日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            sql = " select 合同号 , 制单日期 as 签订日期,客户名称,客户法定人, 交货日期,评审单号,金额 as 合同金额,审核人, 审核日期 from (select * , row_number() over( order  by 合同号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = cRepository.SqlQuery2<V_ContractList2>(sql, pars.ToArray()).ToList();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);

        }
    }
}