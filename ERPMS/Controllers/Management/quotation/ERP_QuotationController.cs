using ERPMS.IRepository.Basics;
using ERPMS.IRepository.order;
using ERPMS.IRepository.quotation;
using ERPMS.Models;
using ERPMS.Repository.basics;
using ERPMS.Repository.order;
using ERPMS.Repository.quotation;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.quotaion
{
    /// <summary>
    /// 报价管理
    /// </summary>
    public class ERP_QuotationController : Controller
    {
        /// <summary>
        /// T_OMS_订单_Master 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        /// <summary>
        /// 订单detail 接口
        /// </summary>
        IOrderDetailRepository odRepository = new OrderDetailRepository();
        /// <summary>
        /// 订单detail 工艺接口
        /// </summary>
        IOrderDetailProcessRepository odpRepository = new OrderDetailProcessRepository();
        /// <summary>
        /// 订单detail 材料 接口
        /// </summary>
        IOrderDetailMaterialRepository odmRepository = new OrderDetailMaterialRepository();

        /// <summary>
        /// T_OMS_报价单_Master 接口
        /// </summary>
        IQuotationMasterRepository qmRepository = new QuotationMasterRepository();
        /// <summary>
        /// T_OMS_报价单_Detail 接口
        /// </summary>
        IQuotationDetailRepository detailRepository = new QuotationDetailRepository();
        /// <summary>
        /// T_OMS_报价单_Detail_工艺 接口
        /// </summary>
        IQuotationProcessRepository processRepository = new QuotationProcessRepository();
        /// <summary>
        /// T_OMS_报价单_Detail_材料 接口
        /// </summary>
        IQuotationDetailMaterialRepository mRepository = new QuotationDetailMaterialRepository();
        /// <summary>
        /// T_OMS_报价单_Detail_计价 接口
        /// </summary>
        IQuotationDetailPriceRepository priceRepository = new QuotationDetailPriceRepository();

        /// <summary>
        /// 工艺编码表
        /// </summary>
        IProcessRepository pRepository = new ProcessRepository();
        // GET: ERP_Quotation
        public ActionResult Index()
        {
            return View();
        }
        #region  报价单 增/删/改/刷新
        public ActionResult AddQuotation(T_OMS_报价单_Master data)
        {
            ObjectParameter par = new ObjectParameter("num", "");
            //order.制单人编码 = User.Identity.Name;
            data.制单人编码 = User.Identity.Name;

            var result = qmRepository.ExecProc2(par, data);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "添加成功", id = par.Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditQuotation(T_OMS_报价单_Master data)
        {
            JsonResult jr = null;
            qmRepository.Edit(data);
            try
            {
                qmRepository.SaverChanges();
                jr= Json(new { success = 0, msg = "修改成功"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
               jr= Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult RemoveQuotation(T_OMS_报价单_Master data)
        {
            JsonResult jr = null;
            qmRepository.Delete(data,true);
            try
            {
                qmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult Refresh(string paramNo)
        {
           var query = qmRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.是否开票,
                p.客户编码,
                p.联系方式,
                p.成品数量,
                p.计量单位,
                p.业务员编码,
                p.印品类别编码,
                p.成品尺寸,
                p.纸张放率,
                p.合同编号,
                p.生产单号,
                p.包装费率,
                p.运输费率,
                p.仓贮费率,
                p.开票税率,
                p.内部说明,
                p.报价要求,
                p.制单人编码,
                p.制单日期,
                p.烫金尺寸,
                p.烫金起步单价,
                p.UV尺寸,
                p.UV起步单价,
                p.审核人编码,
                p.审核日期

            }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult RunPage(int page)
        {
            var total = qmRepository.Query().Count();
            var query = qmRepository.Query().OrderByDescending(p => p.订单号).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.客户编码,
                p.联系方式,
                p.是否开票,
                p.成品数量,
                p.计量单位,
                p.业务员编码,
                p.印品类别编码,
                p.成品尺寸,
                p.纸张放率,
                p.合同编号,
                p.生产单号,
                p.包装费率,
                p.运输费率,
                p.仓贮费率,
                p.开票税率,
                p.内部说明,
                p.报价要求,
                p.烫金尺寸,
                p.烫金起步单价,
                p.UV尺寸,
                p.UV起步单价,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期

            }).Skip(page).Take(1).FirstOrDefault();
            //return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
        }

        #region 根据订单号显示 detail/材料/计价

        /// <summary>
        /// 此案是报价单detail
        /// </summary>
        /// <param name="paramNo"></param>
        /// <returns></returns>

        public ActionResult ShowDetail(string paramNo)
        {
            //string sql = "select * from (select ROW_NUMBER() over(order by 订单号) as rowId ,a.订单号,a.行号, a.印品部件, a.纸张编码,a.正面色数,a.背面色数, " +
            //    "a.版数,a.拼数, a.贴数,a.开料尺寸,a.上版方式, a.上机开数, a.正用数量, a.后道加放,a.印刷加放,a.合计张,b.纸张名称 " +
            //     "from T_OMS_报价单_Detail a left join T_MIMS_纸张编码表 b on a.纸张编码 = b.纸张编码 where a.订单号 = @orderNo ) as s where rowId between @page and @rows";
            //SqlParameter[] pars = new SqlParameter[] {
            //    new SqlParameter("@paramNo",paramNo),
            //     new SqlParameter("@page",(page -1)*rows +1),
            //     new SqlParameter("@rows",rows*page)

            //};
            string sql = "select a.订单号,a.行号, a.印品部件, a.纸张编码,a.正面色数,a.背面色数, " +
                "a.版数,a.拼数, a.贴数,a.开料尺寸,a.上版方式, a.上机开数, a.正用数量, a.后道加放,a.印刷加放,a.合计张,b.纸张名称 " +
                 "from T_OMS_报价单_Detail a left join T_MIMS_纸张编码表 b on a.纸张编码 = b.纸张编码 where a.订单号 = @paramNo ";
            SqlParameter par = new SqlParameter("@paramNo", paramNo);
            var query = detailRepository.SqlQuery2<报价单_Detail>(sql, par).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 显示报价单材料
        /// </summary>
        /// <param name="paramNo"></param>
        /// <returns></returns>

        public ActionResult ShowMaterial(string paramNo)
        {
            string sql = "select a.订单号,a.行号,a.数量,a.报价detail号 ,a.材料编码,a.材料来源,a.匹配系数,b.材料名称,b.规格型号 ,a.计量单位,a.单价 from dbo.T_OMS_报价单_Detail_材料 a inner join dbo.T_MIMS_材料编码表 b on a.材料编码 = b.材料编码 and a.订单号 =@paramNo ";
            SqlParameter par = new SqlParameter("@paramNo", paramNo);
            var query = mRepository.SqlQuery2<报价单_Detail_材料>(sql, par).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 显示报价单计价
        /// </summary>
        /// <param name="paramNo"></param>
        /// <returns></returns>

        public ActionResult ShowPrice(string paramNo)
        {
            string sql = " SELECT b.项目名称, a.ID, a.订单号, a.项目编码, a.印品部件, a.计量单位, a.数量, a.单价, a.系数, a.金额, a.标准单价, a.统计编码, a.报价detail号, a.报价工艺号, a.报价材料号 FROM T_OMS_报价单_Detail_计价 a  left JOIN T_OMS_工艺项目编码表 b  ON a.项目编码 = b.项目编码 where Flag = 0 and a.订单号=@paramNo order by a.统计编码";
            SqlParameter par = new SqlParameter("@paramNo", paramNo);
            var query = priceRepository.SqlQuery2<报价单_Detail_计价>(sql, par);
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        #endregion 
        public ActionResult GetUnit()
        {
            var list = mRepository.Query().GroupBy(p => p.计量单位).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
  

        /// <summary>
        /// 报价单审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult Auditing(T_OMS_报价单_Master data)
        {
            JsonResult jr = null;
            // var sum = odpRepository.QueryWhere(p => p.订单号 == order.订单号&& p.Flag==0).Sum(p => p.金额);
            string[] strArr = { "审核人编码", "审核日期"};
            data.审核人编码 = User.Identity.Name; ;
            data.审核日期 = DateTime.Now;
            //  order.金额 = sum;
            qmRepository.Edit(data, strArr);

            try
            {
                qmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "审核成功", AuditorNo = data.审核人编码 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "审核失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 报价单撤审
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult UnAuditing(T_OMS_报价单_Master data)
        {
            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            data.审核人编码 = null;
            data.审核日期 = null;
            qmRepository.Edit(data, strArr);
            try
            {
                qmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        /// <summary>
        /// 导入工艺或材料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="orderNo"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportProcess(List<T_OMS_报价单_Detail_工艺> data)
        {
            ///try cache 可以用过滤器代替
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            //select @unitPrice = 销售单价, @tongji = rtrim(统计编码), @unit = 计量单位, @pCategory = 工艺类别 from T_OMS_工艺项目编码表 where 项目编码 = @processNum;
            foreach (var i in data)
            {
                //var query = pRepository.QueryWhere(p => p.项目编码 == i.项目编码).Select(p => new { p.销售单价, p.统计编码, p.计量单位 }).FirstOrDefault();
                //if (query != null) {
                //    i.统计编码 = query.统计编码;
                //    i.计量单位 = query.计量单位;
                //    i.标准单价 = query.销售单价;
                //}
                // priceRepository.Add(i);
                processRepository.Add(i);
            }
            try
            {
                processRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        [HttpPost]
        public ActionResult ImportMaterial(List<T_OMS_报价单_Detail_材料> data)
        {
            JsonResult jr = null;
            //材料默认为本场
            var factory = System.Web.Configuration.WebConfigurationManager.AppSettings["factory"];
            var orderNo = data[0].订单号;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var i in data)
            {
                i.材料来源 = factory;
                mRepository.Add(i);
            }
            try
            {
                mRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }



        /// <summary>
        /// 导入计价
        /// </summary>
        /// <param name="paramNo"></param>
        /// <returns></returns>
        public ActionResult ImportJijia(string paramNo)
        {
            JsonResult jr = null;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核,不予许操作" }, JsonRequestBehavior.AllowGet); }
            var query = priceRepository.QueryWhere(p => p.订单号 == paramNo && p.Flag == 1).ToList(); ;
            if (query.Count > 0)
            {
                foreach (var i in query)
                {
                    i.Flag = 0;
                    priceRepository.Edit(i);
                }
                try
                {
                    priceRepository.SaverChanges();
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
        /// 添加修改报价detail
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>

        public ActionResult SaveOrUpdateQuotationDetail(T_OMS_报价单_Detail data, string flag)
        {
            JsonResult jr = null;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            if (flag == "add")
            {
                var count = detailRepository.QueryWhere(p => p.纸张编码 == data.纸张编码 && p.订单号 == data.订单号).Count();
                if (count > 0)
                    return Json(new { success = -1, msg = "该纸张已添加过,可以修改数量达到您的要求" }, JsonRequestBehavior.AllowGet);
                detailRepository.Add(data);
            }
            else if (flag == "edit")
            {
                detailRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                detailRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除报价detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteQuotationDetail(List<T_OMS_报价单_Detail> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            List<int?> intList = new List<int?>();
            foreach (var item in data)
            {

                intList.Add(item.行号);
                detailRepository.Delete(item, true);
            }
            var plist = processRepository.QueryWhere(p => intList.Contains(p.报价detail号) && p.订单号 == orderNo).ToList();
            var material = mRepository.QueryWhere(p => intList.Contains(p.报价detail号) && p.订单号 == orderNo).ToList();
            foreach (var item in plist) { processRepository.Delete(item, true); }
            foreach (var item in material) { mRepository.Delete(item, true); }
            try
            {
                detailRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 添加修改T_OMS_报价单_Detail_材料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateQuotationMaterial(T_OMS_报价单_Detail_材料 data, string flag)
        {
            JsonResult jr = null;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            if (flag == "add")
            {
                mRepository.Add(data);
            }
            else if (flag == "edit")
            {
                string[] arr = { "数量", "计量单位","材料编码","单价" };
                mRepository.Edit(data, arr);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                mRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// s删除T_OMS_报价单_Detail_材料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteQuotationMaterial(List<T_OMS_报价单_Detail_材料> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                mRepository.Delete(item, true);
            }
            try
            {
                mRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        /// <summary>
        /// 添加修改T_OMS_报价单_Detail_计价
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateQuotationPrice(T_OMS_报价单_Detail_计价 data, string flag)
        {
            var auditor = qmRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            JsonResult jr = null;
            if (flag == "add")
            {
                priceRepository.Add(data);
            }
            else if (flag == "edit")
            {
                string[] strArr = { "印品部件", "项目编码", "计量单位", "单价", "数量", "系数", "金额" };
                priceRepository.Edit(data,strArr);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                priceRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除T_OMS_报价单_Detail_工艺
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteQuotationPrice(List<T_OMS_报价单_Detail_计价> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                string[] strArr = { "Flag" };
                item.Flag = 1;
                priceRepository.Edit(item, strArr);
            }
            try
            {
                priceRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 制单页面
        /// </summary>
        /// <param name="paramNo"></param>
        /// <returns></returns>
        public ActionResult MackingBill(string paramNo)
        {
            TempData["quotation_paramNo"] = paramNo;
            return View();
        }

        public ActionResult InitPage()
        {
            var paramNo = TempData["quotation_paramNo"].ToString();
            var query = qmRepository.QueryWhere(p=>p.订单号==paramNo).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.客户编码,
                p.联系方式,
                p.是否开票,
                p.成品数量,
                p.计量单位,
                p.业务员编码,
                p.印品类别编码,
                p.成品尺寸,
                p.报价要求,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
        }


      /// <summary>
      /// 没有找到好的打印方式
      /// </summary>
      /// <param name="paramNo">订单号</param>
      /// <returns></returns>

        public ActionResult PrintPage(string paramNo)
        {
            TempData["quotation_paramNo2"] = paramNo;
            return View();
        }
        public ActionResult PrintData()
        {
            if (TempData["quotation_paramNo2"] == null) return Content ("");
            var orderNo = TempData["quotation_paramNo2"].ToString();
            var formData =  qmRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.客户编码,
                p.联系方式,
                p.是否开票,
                p.成品数量,
                p.计量单位,
                p.业务员编码,
                p.印品类别编码,
                p.成品尺寸,
                p.报价要求,
                p.制单人编码,
                p.制单日期,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            string sql = "select a.印品部件," +
                " b.纸张名称 ,a.开料尺寸  " +
                 "from T_OMS_报价单_Detail a left join T_MIMS_纸张编码表 b on a.纸张编码 = b.纸张编码 where a.订单号 = @paramNo ";
            SqlParameter par = new SqlParameter("@paramNo", orderNo);
            var query = detailRepository.SqlQuery2<报价单_Detail2>(sql, par).ToList();
             sql = " SELECT b.项目名称,  a.印品部件, a.计量单位, a.数量, a.单价, a.金额,  a.统计编码 FROM T_OMS_报价单_Detail_计价 a  left JOIN T_OMS_工艺项目编码表 b  ON a.项目编码 = b.项目编码 where Flag = 0 and a.订单号=@paramNo order by a.统计编码";
            SqlParameter par2 = new SqlParameter("@paramNo", orderNo);
            var query2 = priceRepository.SqlQuery2<报价单_Detail_计价2>(sql, par2);
            var response = JsonConvert.SerializeObject(new { formData = formData, detail = query,price=query2 });
            return Content(response);
        }

        public ActionResult QuotationList()
        {
            return View();
        }
        public ActionResult GetQuotation()
        {
            var query = qmRepository.Query().Select(p => new { Id=p.订单号,Key= p.订单名称 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetQuotationList(SearchParams data, int page = 1, int rows = 10)
        {
            System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = " V_quotationList where 1=1";
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
            var total = qmRepository.SqlQuery2<int>(strSql, pars.ToArray()).SingleOrDefault();
            sql = " select 订单号, 业务单号, 订单名称, 审核日期, 制单日期, 开票税率,运输费率,包装费率,仓贮费率,业务员,客户名称,审核人 ,total as 金额 from (select * , row_number() over( order  by 订单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = qmRepository.SqlQuery2<V_QuotationList>(sql, par).ToList();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            stopwatch.Stop(); //  停止监视
            TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            double seconds = timespan.TotalSeconds;  //  总秒数
            return Content(response);
        }

        public ActionResult AcceptanceList(string  orderNo)
        {
            //Select(p=>new { p.订单名称, p.业务员编码, p.客户编码, p.联系方式, p.成品数量, p.成品尺寸, p.印品类别编码, p.报价要求, p.是否开票,p.审核人编码 })
            var data = qmRepository.QueryWhere(p => p.订单号 == orderNo).FirstOrDefault();
            if (string.IsNullOrEmpty(data.审核人编码)) {
                return Json(new { success = -1, msg = "报价单:" + orderNo + "未审核" }, JsonRequestBehavior.AllowGet);
            }
            var details = detailRepository.QueryWhere(p =>p.订单号== orderNo).Select(p=>new {p.行号,p.订单号, p.印品部件, p.纸张客户编码, p.纸张编码, p.正面色数, p.背面色数, p.版数, p.上版方式, p.拼数, p.贴数, p.上机开数, p.正用数量, p.印刷加放, p.后道加放, p.合计张, p.开料尺寸 }).ToList();
            ObjectParameter par = new ObjectParameter("num", "");
            T_OMS_订单_Master master = new T_OMS_订单_Master(data.订单名称,data.业务员编码,data.客户编码,data.联系方式,data.成品数量,data.成品尺寸,data.印品类别编码,data.报价要求,data.是否开票,User.Identity.Name,DateTime.Now);
            var result = oRepository.ExecProc2(par, master);
            T_OMS_报价单_Detail_计价  price= null;
            if (result > 0) {
                foreach (var i in details) {
                    T_OMS_订单_Detail detail = new T_OMS_订单_Detail(par.Value.ToString (),i.印品部件,i.纸张客户编码,i.纸张编码,i.正面色数,i.背面色数,i.版数,i.上版方式,i.拼数,i.贴数,i.上机开数,i.正用数量,i.印刷加放,i.后道加放,(int)i.合计张,i.开料尺寸);
                    price = priceRepository.QueryWhere(p => p.报价detail号 == i.行号 && p.订单号 == i.订单号).FirstOrDefault();
                    if (price != null) {
                        detail.T_OMS_订单_Detail_计价.Add(new T_OMS_订单_Detail_计价 { 订单号 = par.Value.ToString(), 订单Detail号 = detail.行号, 印品部件 = price.印品部件, 工艺类别 = price.工艺类别, 项目编码 = price.项目编码, 计量单位 = price.计量单位, 数量 = price.数量, 系数 = price.系数, 标准单价 = price.标准单价,金额=price.金额,单价=price.单价, 统计编码 = price.统计编码, Flag = price.Flag });
                    }
                    odRepository.Add(detail);
                    var materials = mRepository.QueryWhere(p => p.订单号 == orderNo&&p.报价detail号==i.行号).Select(p => new {p.行号, p.印品部件, p.材料来源, p.材料编码, p.数量,p.单价 }).ToList();
                    foreach (var j in materials)
                    {
                        T_OMS_订单_Detail_材料 material = new T_OMS_订单_Detail_材料();
                        material.订单号 = par.Value.ToString();
                        material.印品部件 = j.印品部件;
                        material.材料来源 = j.材料来源;
                        material.材料编码 = j.材料编码;
                        material.数量 = j.数量;
                        material.订单detail号 = detail.行号;
                        price = priceRepository.QueryWhere(p => p.报价材料号 == j.行号 && p.订单号 ==i.订单号).FirstOrDefault();
                        if (price != null)
                        {
                            material.T_OMS_订单_Detail_计价.Add(new T_OMS_订单_Detail_计价 { 订单号 = par.Value.ToString(), 订单材料号 = material.行号, 印品部件 = price.印品部件, 工艺类别 = price.工艺类别, 项目编码 = price.项目编码, 计量单位 = price.计量单位, 数量 = price.数量, 系数 = price.系数, 单价=price.单价,标准单价 = price.标准单价, 统计编码 = price.统计编码, Flag = price.Flag });
                        }
                        odmRepository.Add(material);

                    }

                    var processs = processRepository.QueryWhere(p => p.订单号 == orderNo && p.报价detail号 == i.行号).Select(p => new { p.行号,p.印品部件, p.项目编码, p.数量 });
                    foreach (var f in processs) {
                        T_OMS_订单_Detail_工艺 process = new T_OMS_订单_Detail_工艺() { 订单号 = par.Value.ToString(), 订单detail号 = detail.行号, 印品部件 = f.印品部件, 项目编码 = f.项目编码, 数量 = f.数量 };
                        price = priceRepository.QueryWhere(p => p.报价工艺号 == f.行号 && p.订单号 == i.订单号).FirstOrDefault();
                        if (price != null)
                        {
                            process.T_OMS_订单_Detail_计价.Add(new T_OMS_订单_Detail_计价 { 订单号 = par.Value.ToString(), 订单工艺号 = process.行号, 印品部件 = price.印品部件, 工艺类别 = price.工艺类别, 项目编码 = price.项目编码, 计量单位 = price.计量单位, 数量 = price.数量, 系数 = price.系数, 标准单价 = price.标准单价, 单价=price.单价,金额=price.金额,统计编码 = price.统计编码, Flag = price.Flag });
                        }
                        odpRepository.Add(process);
                    }
                }
                data.业务单号 = par.Value.ToString();

                try
                {
                    odRepository.SaverChanges();
                    return Json(new { success = 0, msg = "生成业务单成功" , id=par.Value .ToString()}, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    master.订单号 = par.Value.ToString();
                    oRepository.Delete(master, true);
                    oRepository.SaverChanges();
                    return Json(new { success = -1, msg = "生成业务单失败"}, JsonRequestBehavior.AllowGet);
                }
        
            }

            return Json(new { success = -1, msg = "生成业务单失败", JsonRequestBehavior.AllowGet });
        }

        public ActionResult ExportExcel(SearchParams data)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql = " V_quotationList where 1=1";
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
            sql = " select 订单号, 业务单号, 订单名称, 审核日期, 制单日期, 开票税率,运输费率,包装费率,仓贮费率,业务员,客户名称,审核人 ,total as 金额 from (select * , row_number() over( order  by 订单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = qmRepository.SqlQuery2<V_QuotationList>(sql, pars.ToArray()).ToList().Select(p=>new { p.订单号,p.订单名称,p.业务单号, 制单日期=p.制单日期==null?"":p.制单日期.Value.ToString("yyyy-MM-dd hh:mm:ss"),p.客户名称,p.业务员,p.审核人,p.金额});
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }
        public ActionResult QuotationListPrintPage(SearchParams data)
        {
            TempData["quotationPrintData"] = data;
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
            SearchParams data = TempData["quotationPrintData"] as SearchParams;
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(data.page -1)*data.rows +1),
                 new SqlParameter("@rows",data.rows*data.page)
            };
            string sql = " V_quotationList where 1=1";
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
            sql = " select 订单号, 业务单号, 订单名称, 审核日期, 制单日期, 开票税率,运输费率,包装费率,仓贮费率,业务员,客户名称,审核人 ,total as 金额 from (select * , row_number() over( order  by 订单号 desc )as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = qmRepository.SqlQuery2<V_QuotationList>(sql, pars.ToArray()).ToList().Select(p => new { p.订单号, p.订单名称, p.业务单号, 制单日期 = p.制单日期 == null ? "" : p.制单日期.Value.ToString("yyyy-MM-dd"), p.客户名称, p.业务员, p.审核人, p.金额 });
            var response = JsonConvert.SerializeObject(query);
            return Content(response);

        }



    }
    public class 报价单_Detail2
    {
        public string 印品部件 { get; set; }
        public string 纸张名称 { get; set; }
        public string 开料尺寸 { get; set; }
        //public Nullable<decimal> 分切数 { get; set; }
    }

    public class 报价单_Detail_计价2
    {
        public string 统计编码 { get; set; }
        public string 印品部件 { get; set; }
        public string 项目名称 { get; set; }
    
        public string 计量单位 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        //public Nullable<decimal> 合计 { get; set; }

    }
}