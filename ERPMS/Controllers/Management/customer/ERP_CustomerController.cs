using ERPMS.IRepository.Basics;
using ERPMS.IRepository.customer;
using ERPMS.IRepository.order;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Repository.customer;
using ERPMS.Repository.order;
using ERPMS.Util;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.customer
{
    public class ERP_CustomerController : Controller
    {
        /// <summary>
        /// 客户编码表接口
        /// </summary>
        ICustomerRepository customerRepository = new CustomerRepository();
        /// <summary>
        /// 客户对账单接口
        /// </summary>
        ICustomerAccountListRepository accountRepository = new CustomerAccountListRepository();
        /// <summary>
        /// T_OMS_客户日志记录表 接口
        /// </summary>

        ICustomerLogRepository logRepository = new CustomerLogRepository();
        /// <summary>
        /// 业务承接单接口
        /// </summary>
        IOrderRepository orderRepository = new OrderRepository();

       

        //-----------------------------客户档案------------------------
        // GET: ERP_Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCustomer(int page = 0, int rows = 10, string saleMan = "")
        {
            int total = 0;
            Expression<Func<T_OMS_客户编码表, bool>> ex = PredicateBuilder.New<T_OMS_客户编码表>(true);
            if (!string.IsNullOrEmpty(saleMan))
            {
                ex = ex.And(p => p.业务员编码==saleMan);
            }
            total = customerRepository.QueryWhere(ex).Count();
            var list = customerRepository.AsExpandable(ex, p => p.客户编码, true).Skip((page - 1) * rows).Take(rows).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveCustomer(T_OMS_客户编码表 data,string flag)
        {
            JsonResult jr = null;
            customerRepository.Add(data);
            try
            {
                customerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "添加成功"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { sucess = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult UpdateCustomer(T_OMS_客户编码表 data)
        {
            JsonResult jr = null;
            customerRepository.Edit(data);
            try
            {
                customerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { sucess = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteCustomer(List<T_OMS_客户编码表> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                customerRepository.Delete(item, true);
            }
            try
            {
                customerRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(SearchParams data)
        {
            Expression<Func<T_OMS_客户编码表, bool>> ex = PredicateBuilder.New<T_OMS_客户编码表>(true);
            if (!string.IsNullOrEmpty(data.orderName))
            {
                ex = ex.And(p => p.客户名称.Contains(data.orderName));
            }
            if (!string.IsNullOrEmpty(data.saleMan)) {
                ex = ex.And(p => p.业务员编码==data.saleMan);
            }
            var list = customerRepository.AsExpandable(ex, p => p.客户编码, true).Select(p=>new { p.客户编码,p.客户名称,p.客户简称,p.使用状态,p.客户级别,p.客户类别,p.建档日期}).Skip((data.page - 1) * data.rows).Take(data.rows).ToList();
            DataTable tb = list.ToDataTable(p => new object[] { list });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult CustomerPrintPage(SearchParams data)
        {
            TempData["customerPrintData"] = data;
            return View();
        }

        /// <summary>
        /// 报价清单打印
        /// </summary>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult GetPrintData()
        {
            SearchParams data = TempData["customerPrintData"] as SearchParams;
            Expression<Func<T_OMS_客户编码表, bool>> ex = PredicateBuilder.New<T_OMS_客户编码表>(true);
            if (!string.IsNullOrEmpty(data.orderName))
            {
                ex = ex.And(p => p.客户名称.Contains(data.orderName));
            }
            if (!string.IsNullOrEmpty(data.saleMan))
            {
                ex = ex.And(p => p.业务员编码 == data.saleMan);
            }
            var list = customerRepository.AsExpandable(ex, p => p.客户编码, true).Select(p => new { p.客户编码, p.客户名称, p.客户简称,使用状态= p.使用状态==1? "√": "×", p.客户级别, p.客户类别, p.建档日期 }).Skip((data.page - 1) * data.rows).Take(data.rows).ToList();
            return Json(list);
            //var response = JsonConvert.SerializeObject(list);
            //return Content(response);
        }
        //------------------客户对账单----------------------------
        /// <summary>
        /// 客户对账清单
        /// </summary>
        /// <returns></returns>

        public ActionResult AccountList()
        {
            return View();
        }
        public ActionResult ImportAccount(SearchParams data)
        {
            var newNum = "";
            List<T_OMS_客户_客户对账单> accountList = new List<T_OMS_客户_客户对账单>();
            List<T_OMS_客户_客户对账单> accountList2 = new List<T_OMS_客户_客户对账单>();
            var query = (from p in orderRepository.Query()
                         where p.客户编码 == data.客户编码 && p.制单日期 >= data.开始日期 && p.制单日期 <= data.结束日期 && !string.IsNullOrEmpty(p.审核人编码)
                         select new 
                         { 订单号 = p.订单号, 订单名称 = p.订单名称,客户编码=p.客户编码, 制单日期 = p.制单日期, 成品数量 = p.成品数量, 金额 = p.金额, 计量单位 = p.计量单位}).ToList()
                         .Select(c=> new T_OMS_客户_客户对账单 { 订单号 = c.订单号, 订单名称 = c.订单名称, 客户编码 = c.客户编码, 制单日期 = c.制单日期, 成品数量 = c.成品数量, 金额 = c.金额, 计量单位 = c.计量单位, 日期 = DateTime.Now, 开始日期 = data.开始日期, 结束日期 = data.结束日期 }).ToList();
            if (query.Count <= 0) { return Json(new { success = -1, msg = "该时间段内没有任何订单" }, JsonRequestBehavior.AllowGet); }
            var query2 = accountRepository.QueryWhere(p => p.客户编码 == data.客户编码 && p.开始日期 >= data.开始日期 && p.结束日期 <= data.结束日期).ToList();
            if (query2 .Count<=0)
            {
               accountList.AddRange(query);
            }
            else
            {
                newNum = query2[0].对账单号;
                var list = query.Except(query2, new AccountListEquality()).ToList();
                if (list .Count>0) { accountList2.AddRange(list); }
                string sql = "select a.订单号, a.订单名称, a.制单日期, a.成品数量, a.金额, a.计量单位, a.客户编码, (a.金额-b.金额)as 金额 from  T_OMS_订单_Master a inner join(select 订单号,sum(金额) as 金额 from [T_OMS_客户_客户对账单] where 客户编码=@customerNo  and  开始日期>= @startDate and 结束日期<=@endDate  group by 订单号) b  on a.订单号=b.订单号 and (a.金额-b.金额>0)";
                List<SqlParameter> pars = new List<SqlParameter>() {
                    new SqlParameter("@customerNo",data.客户编码),
                    new SqlParameter("@startDate",data.开始日期),
                    new SqlParameter("@endDate",data.结束日期)
                };
                var query3 = accountRepository.SqlQuery2<客户_客户对账单>(sql, pars.ToArray()).Select(p=>new T_OMS_客户_客户对账单 { 订单号 = p.订单号, 订单名称 = p.订单名称,客户编码=p.客户编码, 制单日期 = p.制单日期, 成品数量 = p.成品数量, 金额 = p.金额, 计量单位 = p.计量单位, 日期 = DateTime.Now, 开始日期 = data.开始日期, 结束日期 = data.结束日期}).ToList();
                if (query3.Count>0) { accountList2.AddRange(query3); }

            }
            if (accountList.Count > 0) {
                string accountNo = accountRepository.Query().Max(p => p.对账单号);
                string str = DateTime.Now.ToString("yyyyMM");
                if (string.IsNullOrEmpty(accountNo))
                {
                    newNum = newNum = "KH-" + str + "-0000";
                }
                else
                {
                    string[] strArr = accountNo.Split('-');
                    if (str == strArr[1])
                    {
                        int num = Convert.ToInt32(strArr[2]);
                        if (num + 1 < 10)
                        {
                            newNum = "KH-" + str + "-000" + num + 1;
                        }
                        else if (num + 1 >= 10)
                        {
                            newNum = "KH-" + str + "-00" + num + 1;
                        }
                        else if (num + 1 >= 100)
                        {
                            newNum = "KH-" + str + "-0" + num + 1;
                        }
                        else
                        {
                            newNum = "KH-" + str + "-" + num + 1;
                        }
                    }
                    else
                    {
                        newNum = "KH-" + str + "-0000";
                    }
                }
                foreach (var i in accountList)
                {
                    i.对账单号 = newNum;
                    i.制单人编码 = User.Identity.Name;
                    accountRepository.Add(i);
                }
               
            }
            if (accountList2.Count > 0) {
                foreach (var i in accountList2)
                {
                    i.对账单号 = newNum;
                    i.制单人编码 = User.Identity.Name;
                    accountRepository.Add(i);
                }
            }
            try
            {
                accountRepository.SaverChanges();
                return Json(new { success = 0, msg = "导入成功", id = newNum }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { success = -1, msg = "导入失败", id = newNum }, JsonRequestBehavior.AllowGet);
            }
           
        }

        public ActionResult SaveOrUpdateAccountList(T_OMS_客户_客户对账单 data,string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                data.制单人编码 = User.Identity.Name;
                accountRepository.Add(data);
            }
            else if (flag == "edit")
            {
                accountRepository.Edit(data);
            }
            else {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                accountRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteAccountList(List<T_OMS_客户_客户对账单> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {

                accountRepository.Delete(item, true);
            }
            try
            {
                accountRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult AuditorAccountList(string accountNo)
        {
            JsonResult jr = null;
            var list = accountRepository.QueryWhere(p => p.对账单号 == accountNo).ToList();
            foreach (var item in list)
            {
                item.审核人编码 = User.Identity.Name;
                item.审核日期 = DateTime.Now;
                accountRepository.Edit(item);
            }
            try
            {
                accountRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", auditorNo = User.Identity .Name}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult UnAuditorAccountList(string accountNo)
        {

            JsonResult jr = null;
            var list = accountRepository.QueryWhere(p => p.对账单号 == accountNo).ToList();
            foreach (var item in list)
            {
                item.审核人编码 = null;
                item.审核日期 =null;
                accountRepository.Edit(item);
            }
            try
            {
                accountRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤申成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤申失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        public ActionResult GetAccountList(string accountNo)
        {
            var query = accountRepository.QueryWhere(p => p.对账单号 == accountNo).Select(p => new
            {
                p.行号,
                p.对账单号,
                p.订单号,
                p.订单名称,
                p.制单日期,
                p.成品数量,
                p.金额,
                p.计量单位,
                p.日期,
                p.开始日期,
                p.结束日期
            }).ToList();
            string result = JsonConvert.SerializeObject(query);
            return Content(result);
        }

        public ActionResult RunPage(int page=0)
        {
            List<T_OMS_客户_客户对账单> list = null;
            var total = accountRepository.Query().GroupBy(p => p.对账单号).Count();
            var ss=accountRepository.Query().GroupBy(p => p.对账单号).OrderByDescending(p=>p.Key).Skip(page).Take(1).ToList();
            foreach (IGrouping<String, T_OMS_客户_客户对账单> info in ss) {
                list = info.ToList();
            }
            var response = JsonConvert.SerializeObject(new { total = total, rows = list });
            return Content(response);
        
        }

        public ActionResult AccountListExportExcel(SearchParams data)
        {
            Expression<Func<T_OMS_客户_客户对账单, bool>> ex = PredicateBuilder.New<T_OMS_客户_客户对账单>(true);

            if (!string.IsNullOrEmpty(data.客户编码)) {
                ex = ex.And(p => p.客户编码==data.客户编码);
            }
            if (data.开始日期 != null && data.结束日期 != null)
            {
                ex = ex.And(p => p.开始日期 >= data.开始日期 && p.结束日期 <= data.结束日期);
            }
            var query = accountRepository.AsExpandable(ex, p => p.行号, true).Skip((data.page - 1) * data.rows).Take(data.rows).Select(p=>new { p.订单号,p.制单日期,p.订单名称,p.计量单位,p.成品数量,p.金额,p.备注}).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult AccountListPrintPage(string accountNo)
        {
            TempData["accountList_accountNo"] = accountNo;
            return View();
        }
        public ActionResult printData()
        {
            string accountNo = TempData["accountList_accountNo"].ToString();
            var query = accountRepository.QueryWhere(p => p.对账单号 == accountNo).Select(p => new
            {
                p.客户编码,
                p.对账单号,
                p.订单号,
                p.订单名称,
                p.制单日期,
                p.成品数量,
                p.金额,
                p.计量单位,
                p.日期,
                p.开始日期,
                p.结束日期
            }).ToList();
            string result = JsonConvert.SerializeObject(query);
            return Content(result);
        }

        //-------------------------------客户对账单清单----------------

        public ActionResult AccountDetailList()
        {
            return View();
        }

        public ActionResult GetAccountNoList()
        {
            var query = accountRepository.Query().GroupBy(p=>p.对账单号).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountDetailList(SearchParams data, int page = 1, int rows = 10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = GetSql("V_accountDetailList", data, true, ref pars);
            var strSql = "select count(*) from " + sql;
            var total = accountRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 对账单号, CONVERT(varchar(12) , 日期, 111 ) as 对账日期 ,客户名称, 订单号, CONVERT(varchar(12) , 制单日期, 111 ) as 制单日期,  订单名称, 计量单位, 成品数量,金额,备注,制单人,审核人 from (select * , row_number() over( order  by 对账单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = accountRepository.SqlQuery2<V_accountDetailList>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
        }

        public ActionResult ExportAccountDetailListExcel(SearchParams data)
        {
            List<SqlParameter>  pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql =GetSql("V_accountDetailList", data,true, ref pars);
            sql = " select 对账单号,  CONVERT(varchar(12) , 日期, 111 ) as 对账日期,客户名称, 订单号, CONVERT(varchar(12) , 制单日期, 111 ) as 制单日期 ,  订单名称, 计量单位, 成品数量,金额,备注,制单人,审核人 from (select * , row_number() over( order  by 对账单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.ToArray();
            var query = accountRepository.SqlQuery2<V_accountDetailList>(sql, par).ToList();
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult AccountListDetailPrintPage(SearchParams data)
        {
            TempData["accountDetailListPrintData"] = data;
            return View();
        }
        public ActionResult GetAccountDetailListPrintData()
        {
            SearchParams data = TempData["accountDetailListPrintData"] as SearchParams;
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql = GetSql("V_accountDetailList", data,true, ref pars);
            sql = " select 对账单号,  CONVERT(varchar(12) , 日期, 111 ) as 对账日期,客户名称, 订单号, CONVERT(varchar(12) , 制单日期, 111 ) as 制单日期 ,  订单名称, 计量单位, 成品数量,金额,备注,制单人,审核人 from (select * , row_number() over( order  by 对账单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.ToArray();
            var query = accountRepository.SqlQuery2<V_accountDetailList>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }
        //----------------------------客户日志记录表-------------------------
        public ActionResult CustomerLog()
        {
            return View();
        }
        public ActionResult GetCustomerLog(int page =1,int rows=10)
        {
            SqlParameter[] pars = new SqlParameter[2] {
                new SqlParameter("@page",(page-1)*rows+1),
                new SqlParameter("rows",rows*page)
            };
            string sql = "select ID,订单日期,制单日期,手工单号,确认标识,客户编码,订单说明,发货说明,结余说明,办款说明, 单据类型,制单人编码,客户名称 from  (select ROW_NUMBER() over(order by  a.id desc) AS rowId, " +
                "a.ID,a.订单日期,a.制单日期,a.手工单号,a.确认标识,a.客户编码,a.订单说明,a.发货说明,a.结余说明,a.办款说明, a.单据类型,a.制单人编码,b.客户名称 from  T_OMS_客户日志记录表 a left join T_OMS_客户编码表 b on  a.客户编码=b.客户编码 where   a.确认标识 is null or a.确认标识<>1  ) s where  rowid between  @page and @rows ";
            var query = logRepository.SqlQuery2<客户日志记录表>(sql, pars).ToList();
            sql = "select count(*) from T_OMS_客户日志记录表 a where  a.确认标识 is null or a.确认标识<>1";
            int total = logRepository.SqlQuery2<int>(sql).FirstOrDefault();
            string response = JsonConvert.SerializeObject(new { total=total,rows=query});

            return Content(response);
        }
        public ActionResult SaveOrUpdateLog(T_OMS_客户日志记录表 data,string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                data.制单人编码 = User.Identity.Name;
                data.制单日期 = DateTime.Now;
                data.单据类型 = "业务员";
                logRepository.Add(data);
            }
            else if (flag == "edit")
            {
                if (data.制单人编码 != User.Identity.Name) {
                    return Json(new { success = -1, msg = "无权修改他人添加的数据" }, JsonRequestBehavior.AllowGet);
                }
                string[] strArr = { "订单日期", "手工单号", "客户编码", "订单说明", "发货说明", "结余说明", "办款说明" };
                logRepository.Edit(data, strArr);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标知" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                logRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" });
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "操作失败" });
            }
            return jr;
        }


        public ActionResult DeleteLog(List<T_OMS_客户日志记录表> data)
        {
            JsonResult jr = null;
            foreach (var i in data)
            {
                if (i.制单人编码 != User.Identity.Name)
                {
                    return Json(new { success = -1, msg = "不予许删除他人添加的数据" }, JsonRequestBehavior.AllowGet);
                }
                if (i.确认标识 == 1)
                {
                    return Json(new { success = -1, msg = "选中的数据有(已确认) 请认真选择" }, JsonRequestBehavior.AllowGet);
                }
                logRepository.Delete(i, true);
            }
            try
            {
                logRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult ConfirmLog(List<T_OMS_客户日志记录表> data)
        {
            JsonResult jr = null;
            int num = 0;
            foreach (var i in data)
            {
                if (i.单据类型 == "业务员") {
                    num++;
                    continue;
                }
                i.确认人编码 = User.Identity.Name;
                i.确认日期 = DateTime.Now;
                i.确认说明 = "收到";
                i.确认标识 = 1;
                string[] strArr = { "确认人编码", "确认日期", "确认说明", "确认标识" };
                logRepository.Edit(i,strArr);
            }
            try
            {
                logRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "确认成功", count = num }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "确认失败", count = num }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult LogExportExcel(int page = 1, int rows = 10)
        {
            SqlParameter[] pars = new SqlParameter[2] {
                new SqlParameter("@page",(page-1)*rows+1),
                new SqlParameter("rows",rows)
            };
            string sql = "select 订单日期,手工单号,确认标识,订单说明,发货说明,结余说明,办款说明 ,客户名称 from  (select ROW_NUMBER() over(order by  a.id desc) AS rowId, " +
                "a.ID,a.订单日期,a.制单日期,a.手工单号,a.确认标识,a.客户编码,a.订单说明,a.发货说明,a.结余说明,a.办款说明 ,b.客户名称 from  T_OMS_客户日志记录表 a left join T_OMS_客户编码表 b on  a.客户编码=b.客户编码 where   a.确认标识 is null or a.确认标识<>1  ) s where  rowid between  @page and @rows ";
            var query = logRepository.SqlQuery2<客户日志记录表>(sql, pars).Select(p=>new { p.订单日期,订单号= p.手工单号,p. 确认标识,p. 订单说明, p.发货说明,p. 结余说明, p.办款说明, p.客户名称 });
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult LogPrintPage(int page = 1, int rows = 10)
        {
            TempData["log_print_page"] = page;
            TempData["log_print_rows"] = rows;
            return View();
        }
        public ActionResult GetLogPrintData()
        {
            int page = (int)TempData["log_print_page"];
            int rows = (int)TempData["log_print_rows"];
            SqlParameter[] pars = new SqlParameter[2] {
                new SqlParameter("@page",(page-1)*rows+1),
                new SqlParameter("rows",rows)
            };
            string sql = "select 客户名称,手工单号,订单说明,发货说明,结余说明,办款说明,确认标识  from  (select ROW_NUMBER() over(order by  a.id desc) AS rowId, " +
                  "a.ID,a.订单日期,a.制单日期,a.手工单号,a.确认标识,a.客户编码,a.订单说明,a.发货说明,a.结余说明,a.办款说明 ,b.客户名称 from  T_OMS_客户日志记录表 a left join T_OMS_客户编码表 b on  a.客户编码=b.客户编码 where   a.确认标识 is null or a.确认标识<>1  ) s where  rowid between  @page and @rows ";
            var query = logRepository.SqlQuery2<客户日志记录表>(sql, pars).Select(p=>new { p.客户名称, 订单号=p.手工单号,p.订单说明,p.发货说明,p.结余说明,p.办款说明,p.确认标识,确认人=p.确认人编码,p.确认人编码,p.确认说明});
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }

        //----------------------------客户日志记录表清单-----------------------
        /// <summary>
        /// 客户日志记录表清单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LogDetailList()
        {
            return View();
        }


        public ActionResult GetLogListDetail(SearchParams data, int page = 1, int rows = 10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            var sql = GetSql("V_customer_logDetail", data,false, ref pars);
            var strSql = "select count(*) from " + sql;
            int total = logRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 订单日期, 手工单号,订单说明,发货说明,结余说明,制单日期,办款说明, 确认日期,确认说明,制单人,确认人,客户名称 from (select * , row_number() over( order  by ID desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = logRepository.SqlQuery2<V_customer_logDetail>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);

        }
        public ActionResult CustomerLogListExport(SearchParams data)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            var sql = GetSql("V_customer_logDetail", data, false, ref pars);
            sql = " select 订单日期, 手工单号,订单说明,发货说明,结余说明,制单日期,办款说明, 确认日期,确认说明,制单人,确认人,客户名称 from (select * , row_number() over( order  by ID desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";

            var query = logRepository.SqlQuery2<V_customer_logDetail>(sql, pars.ToArray())
                .Select(p => new {订单日期=p.订单日期.Value.ToString("yyyy-MM-dd"), p.客户名称, p.手工单号, p.订单说明, p.发货说明, p.结余说明, p.办款说明, p.制单人,制单日期=p.制单日期.Value.ToString ("yyyy-MM-dd"), p.确认人,确认日期=p.确认日期.Value.ToString("yyyy-MM-dd"), p.确认说明 });
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
        public ActionResult PrintLogDetailPage(SearchParams data)
        {
            TempData["logDetailSearch"] = data;
            return View();
        }
        public ActionResult GetPrintLogDetailData()
        {
            SearchParams data = TempData["logDetailSearch"] as SearchParams;
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            var sql = GetSql("V_customer_logDetail", data, false, ref pars);
            sql = " select 订单日期, 手工单号,订单说明,发货说明,结余说明,制单日期,办款说明, 确认日期,确认说明,制单人,确认人,客户名称 from (select * , row_number() over( order  by ID desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";

            var query = logRepository.SqlQuery2<V_customer_logDetail>(sql, pars.ToArray())
                .Select(p => new { 订单日期 = p.订单日期 == null ? null: p.订单日期.Value.ToString("yyyy-MM-dd"), p.客户名称, p.手工单号, p.订单说明, p.发货说明, p.结余说明, p.办款说明, p.制单人, 制单日期 = p.制单日期==null?null: p.制单日期.Value.ToString("yyyy-MM-dd"), p.确认人, 确认日期 = p.确认日期==null?null: p.确认日期.Value.ToString("yyyy-MM-dd"), p.确认说明 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        ///todu:最好抽出来公用(以后再抽)
        /// </summary>
        /// <param name="flag">制单日期</param>
        /// <param name="viewName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string GetSql(string viewName, SearchParams data,bool flag, ref List<SqlParameter> pars)
        {
           
            string sql = viewName+" where 1=1";

            if (!string.IsNullOrEmpty(data.orderNo))
            {
                sql += " and 对账单号 =@orderNo";
                pars.Add(new SqlParameter("@orderNo", data.orderNo));
            }

            if (!string.IsNullOrEmpty(data.customerNo))
            {
                sql += " and 客户编码 =@customerNo";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (data.zhidanStart != null && data.zhidanEnd != null)
            {
                sql += flag ? " and 日期 between @zhidanStart and @zhidanEnd" : " and 制单日期 between @zhidanStart and @zhidanEnd";
                //sql += " and 日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            return sql;
        }
    }

}