using ERPMS.Models;
using ERPMS.IRepository.Basics;
using ERPMS.IRepository.order;
using ERPMS.Repository.basics;
using ERPMS.Repository.Basics;
using ERPMS.Repository.order;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.order
{
    /// <summary>
    /// 业务承接单
    /// </summary>
    public class ERP_OrderController : Controller
    {
        /// <summary>
        /// T_OMS_订单_Master 接口
        /// </summary>
        IOrderRepository oRepository = new OrderRepository();
        /// <summary>
        /// T_PHMS_采购申请单_Master 接口
        /// </summary>
        IPurchaseApplyRepository paRepository = new PurchaseApplyRepository();
        /// <summary>
        /// T_PHMS_采购申请单_Detail 接口
        /// </summary>
        IPurchaseApplyDetailRepository padRepository = new PurchaseApplyDetailRepository();
        
        /// <summary>
        /// 客户编码表
        /// </summary>
        ICustomerRepository cRepository = new CustomerRepository();
        /// <summary>
        /// 工艺编码表
        /// </summary>
        IProcessRepository processRepository = new ProcessRepository();
        /// <summary>
        /// 印品类别编码表
        /// </summary>
        IPrintCategoryRepository pCategoryRepository = new PrintCategoryRepository();
        /// <summary>
        /// 人员编码表接口
        /// </summary>
        IStaffRepository sRepository = new StaffRepository();

        /// <summary>
        /// 订单detail 接口
        /// </summary>
        IOrderDetailRepository odRepository = new OrderDetailRepository();
        /// <summary>
        /// 订单detail 工艺接口
        /// </summary>
        IOrderDetailProcessRepository odpRepository = new OrderDetailProcessRepository();
        /// <summary>
        /// 订单_Detail_计价
        /// </summary>
        IOrderDetailPriceRepository odpriceRepository = new OrderDetailPriceRepository();
        /// <summary>
        /// 订单detail 材料 接口
        /// </summary>
        IOrderDetailMaterialRepository odmRepository = new OrderDetailMaterialRepository();

        /// <summary>
        /// 开数编码表接口
        /// </summary>
        IOpenNumberRepository openRepository = new OpenNumberRepository();

        /// <summary>
        /// 纸张编码表接口
        /// </summary>
        IPaperRepository paperRepository = new PaperRepository();

        /// <summary>
        /// 材料编码表接口
        /// </summary>
        IMaterialRepository mRepository = new MaterialRepository();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrder(T_OMS_订单_Master order)
        {
            ObjectParameter par = new ObjectParameter("num", "");
            //order.制单人编码 = User.Identity.Name;
            order.制单人编码 = User.Identity.Name;
            var result = oRepository.ExecProc2(par, order);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "添加成功", id = par.Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult EditOrder(T_OMS_订单_Master order)
        {
            var auditor = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            string[] strArr = {
                "订单名称","制单日期","业务员编码","客户编码" , "联系方式","成品数量","客户单号","成品尺寸","印品类别编码","计量单位","专色色别",
                "交货日期","总P数" ,"样稿类型" ,"样稿随货带走","留版标识","送货联系方式","制单人编码","客户盯色","加工要求","变更说明","包装材料",
                "客户地址"
            }; oRepository.Edit(order, strArr);
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

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult DeleteOrder(T_OMS_订单_Master order)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            var details = odRepository.QueryWhere(p => p.订单号 == order.订单号).ToList();
            var process = odpRepository.QueryWhere(p =>  p.订单号 == order.订单号).ToList();
            var material = odmRepository.QueryWhere(p =>  p.订单号 == order.订单号).ToList();
            foreach (var item in process) { odpRepository.Delete(item, true); }
            foreach (var item in material) { odmRepository.Delete(item, true); }
            foreach (var item in details) { odRepository.Delete(item, true); }
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
        /// 审核
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult AuditorOrder(T_OMS_订单_Master order)
        {
            JsonResult jr = null;
            T_PHMS_采购申请单_Master apply = null;
            ObjectParameter par = null;
            int result = -1;
            string[] strArr = { "审核人编码", "审核日期" };
            order.审核人编码 = User.Identity.Name;
            order.审核日期 = DateTime.Now;
            var person = oRepository.QueryWhere(p => p.订单号 == order.订单号).Select(p => new { auditor = p.计价审核人编码, customer = p.客户编码 }).FirstOrDefault();
            if (string.IsNullOrEmpty(person.auditor))
                return Json(new { success = -1, msg = "该单号计价还没审核" }, JsonRequestBehavior.AllowGet);
            //是否存在申购单
            var query = paRepository.QueryWhere(p => p.订单号 == order.订单号).FirstOrDefault();
            if (query == null)
            {
                apply = new T_PHMS_采购申请单_Master();
                apply.订单号 = order.订单号;
                apply.日期 = DateTime.Now;
                apply.制单人编码 = order.审核人编码;
                apply.摘要 = order.订单名称;
                par = new ObjectParameter("number", "");
                result = paRepository.ExecProc2(par, apply);
            }
            oRepository.Edit(order, strArr);
            try
            {
                oRepository.SaverChanges();

                List<string> paper = null;
                if (par !=null)
                {
                    var no = par.Value.ToString();
                    paper = padRepository.QueryWhere(p => p.申请单号 == no).Select(p => p.材料名称).ToList();
                }
                jr = Json(new { success = 0, msg = "审核成功", auditorNo = order.审核人编码, plist = paper }, JsonRequestBehavior.AllowGet);
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
        public ActionResult UnAuditorOrder(T_OMS_订单_Master order)
        {

            JsonResult jr = null;
            string[] strArr = { "审核人编码", "审核日期" };
            order.审核人编码 = null;
            order.审核日期 = null;
            var count = paRepository.QueryWhere(p => p.订单号 == order.订单号).Count();
            if (count > 0)
                return jr = Json(new { success = -1, msg = "已申成采购申请单,请删除采购申请单,再撤审" }, JsonRequestBehavior.AllowGet);
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
        #region combobox 所要列表
        [HttpPost]
        public ActionResult GetFactory(string orderNo)
        {
            var factory = System.Web.Configuration.WebConfigurationManager.AppSettings["factory"];
            //var customerNo = oRepository.Query().Select(p => p.客户编码).FirstOrDefault();
            //List<string> cNo = new List<string>() { factory, customerNo };
            //var list = cRepository.QueryWhere(p => cNo.Contains(p.客户编码.ToString())).Select(p => new Factory { FactoryNo = p.客户编码.ToString(), FactoryName = p.客户名称 }).ToList();
            var list = cRepository.Query().OrderBy(p=>p.客户编码).Select(p => new Factory { FactoryNo = p.客户编码.ToString(), FactoryName = p.客户名称 }).ToList();
            foreach (var i in list)
            {
                if (i.FactoryNo == factory)
                {
                    i.Alias = "本厂";
                }
                else
                {
                    i.Alias = "外厂";
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCustormer()
        {
            var list = cRepository.Query().Select(p => new { Key = p.客户名称, Id = p.客户编码, Phone = p.电话, Fax=p.传真 }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustormer2()
        {
            var list = cRepository.Query().Select(p => new { Key = p.客户名称, Id = p.客户编码, Phone = p.电话 }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderByCustomerNo(string customerNo)
        {
            var query = oRepository.QueryWhere(p => p.客户编码 == customerNo).Select(p => new { Key = p.订单号, Id = p.订单号 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrder()
        {
            var list = oRepository.Query().Select(p => new { Key = p.订单号, Id = p.订单号 }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStaff()
        {
            var list = sRepository.Query().Select(p => new { Key = p.姓名, Id = p.人员编码 }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 印品类别 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PCategory()
        {
            var list = pCategoryRepository.Query().Select(p => new { Key = p.印品类别名称, Id = p.印品类别编码 }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单计量单位
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUnit()
        {
            var list = oRepository.Query().GroupBy(p => p.计量单位).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 显示部件信息
        /// </summary>
        public ActionResult GetParts()
        {
            var query = odRepository.Query().GroupBy(p => p.印品部件).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取纸张信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaper()
        {
            var query = paperRepository.Query().Select(p => new { Key = p.纸张名称, Id = p.纸张编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取工艺
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProcess()
        {
            var query = processRepository.Query().Select(p => new { Key = p.项目名称, Id = p.项目编码, Flag = p.统计编码, unit = p.计量单位 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取材料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMaterial()
        {
            var query = mRepository.Query().Select(p => new { Key = p.材料名称, Id = p.材料编码, spec = p.规格型号 ,unit=p.计量单位}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获得开数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOpenNumber()
        {
            var query = openRepository.Query().Select(p => new { Key = p.开数名称, Id = p.所属开式 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult RunPage(int page)
        {
            var total = oRepository.Query().Count();
            var query = oRepository.Query().OrderByDescending(p => p.订单号).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.制单日期,
                p.业务员编码,
                p.客户编码,
                p.联系方式,
                p.成品数量,
                p.客户单号,
                p.成品尺寸,
                p.印品类别,
                p.计量单位,
                p.专色色别,
                p.交货日期,
                p.总P数,
                p.样稿随货带走,
                p.留版标识,
                p.送货联系方式,
                p.制单人编码,
                p.客户盯色,
                p.加工要求,
                p.变更说明,
                p.包装材料,
                p.客户地址,
                p.审核人编码,
                p.审核日期
            }).Skip(page).Take(1).FirstOrDefault();
            //return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="parmNo">订单号</param>
        /// <returns></returns>
        public ActionResult Refresh(string paramNo)
        {
            var query = oRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => new
            {
                p.订单号,
                p.订单名称,
                p.制单日期,
                p.业务员编码,
                p.客户编码,
                p.联系方式,
                p.成品数量,
                p.客户单号,
                p.成品尺寸,
                p.印品类别,
                p.计量单位,
                p.专色色别,
                p.交货日期,
                p.总P数,
                p.样稿随货带走,
                p.留版标识,
                p.送货联系方式,
                p.制单人编码,
                p.客户盯色,
                p.加工要求,
                p.变更说明,
                p.包装材料,
                p.客户地址,
                p.审核人编码,
                p.审核日期
            }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 刷新OrderDetail
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult GetOrderDetail(string paramNo)
        {
            var query = odRepository.QueryWhere(p => p.订单号 == paramNo).Select(p => new
            {
                p.订单号,
                p.行号,
                p.印品部件,
                p.纸张客户编码,
                p.纸张编码,
                p.正用数量,
                p.后道加放,
                p.印刷加放,
                p.分切数量,
                p.版数,
                p.拼数,
                p.正面色数,
                p.背面色数,
                p.贴数,
                p.上版方式,
                p.上机开数,
                p.机切尺寸
            }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 工艺datagrid
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult GetOrderProcess(string paramNo)
        {
            var sql = "select a.行号, a.订单号,a.订单detail号, a.印品部件, a.项目编码, a.数量,b.项目名称 from T_OMS_订单_Detail_工艺 a inner join T_OMS_工艺项目编码表 b on a.项目编码=b.项目编码 and a.订单号=@orderNo";
            SqlParameter par = new SqlParameter("@orderNo", paramNo);
            var query = odpRepository.SqlQuery2<订单_Detail_工艺>(sql, par).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///材料datagrid
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult GetOrderMaterial(string paramNo)
        {
            var sql = "select  a.订单号, a.行号,a.订单detail号,a.材料编码, a.材料来源, a.计量单位, a.数量,a.单价 ,b.材料名称 from T_OMS_订单_Detail_材料 a inner join T_MIMS_材料编码表 b on a.材料编码=b.材料编码 and a.订单号=@orderNo";
            SqlParameter par = new SqlParameter("@orderNo", paramNo);
            var query = odmRepository.SqlQuery2<订单_Detail_材料>(sql, par).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加修改 T_OMS_订单_Detail
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateOrderDetail(T_OMS_订单_Detail data, string flag)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            if (flag == "add")
            {
                var count = odRepository.QueryWhere(p => p.纸张编码 == data.纸张编码 && p.订单号 == data.订单号).Count();
                if (count > 0)
                    return Json(new { success = -1, msg = "该纸张已添加过,可以修改数量达到您的要求" }, JsonRequestBehavior.AllowGet);
                odRepository.Add(data);
                var query = paperRepository.QueryWhere(p => p.纸张编码 == data.纸张编码).Select(p => new { 纸型 = p.纸型.Trim(), p.重量, p.计量单位, p.规格, p.销售单价, }).FirstOrDefault();
                var chars = query.规格.Split('*');
                var chars2 = data.机切尺寸.Split('*');
                decimal  area = 1;
                var charArr = query.纸型 == "卷筒" ? chars2 : query.纸型 == "平张" ? chars : null;
                foreach (var i in charArr) {
                    area *= Convert.ToDecimal(i);    
                }
                decimal? price = area / 1000000 * query.重量 / 1000;
                decimal? total = price * query.重量;
                T_OMS_订单_Detail_计价 orderPrice = new T_OMS_订单_Detail_计价() { 订单Detail号 = data.行号,
                    订单号 = data.订单号, 印品部件 = data.印品部件, 工艺类别 = "纸张", 项目编码 = "CL-ZZF",
                    计量单位 = query.计量单位, 单价 = price, 系数 = 1, 数量 = data.分切数量, 金额 = total, 标准单价 = query.销售单价 ,统计编码="材料"};

                odpriceRepository.Add(orderPrice);
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
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除T_OMS_订单_Detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteOrderDetail(List<T_OMS_订单_Detail> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            List<int?> intList = new List<int?>();
            foreach (var item in data)
            {

                intList.Add(item.行号);
                odRepository.Delete(item, true);
            }
            var process = odpRepository.QueryWhere(p => intList.Contains(p.订单detail号) && p.订单号 == orderNo).ToList();
            var material =odmRepository.QueryWhere(p => intList.Contains(p.订单detail号) && p.订单号 == orderNo).ToList();
            foreach (var item in process) { odpRepository.Delete(item, true); }
            foreach (var item in material) { odmRepository.Delete(item, true); }
            try
            {
                odRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 添加修改T_OMS_订单_Detail_工艺
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateOrderProcess(T_OMS_订单_Detail_工艺 data, string flag)
        {
            var auditor = oRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            JsonResult jr = null;
            if (flag == "add")
            {
                var query = processRepository.QueryWhere(p => p.项目编码 == data.项目编码).Select(p => new { p.销售单价, 统计编码 = p.统计编码.Trim(), p.计量单位, p.工艺类别 }).FirstOrDefault();
                data.T_OMS_订单_Detail_计价.Add ( new T_OMS_订单_Detail_计价()
                {
                    订单工艺号 = data.行号,
                    订单号 = data.订单号,
                    印品部件 = data.印品部件,
                    工艺类别 = query.工艺类别,
                    项目编码 = data.项目编码,
                    计量单位 = query.计量单位,
                    单价 = query.销售单价,
                    系数 = 1,
                    数量 = data.数量,
                    金额 = (query.销售单价 * data.数量),
                    标准单价 = query.销售单价,
                    统计编码 = query.统计编码
                });
                odpRepository.Add(data);
            }
            else if (flag == "edit")
            {
                odpRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                odpRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除T_OMS_订单_Detail_工艺
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteOrderProcess(List<T_OMS_订单_Detail_工艺> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                odpRepository.Delete(item, true);
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

        /// <summary>
        /// 添加修改T_OMS_订单_Detail_材料
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateOrderMaterial(T_OMS_订单_Detail_材料 data, string flag)
        {
            JsonResult jr = null;
            var auditor = oRepository.QueryWhere(p => p.订单号 == data.订单号).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            if (flag == "add")
            {
               data.T_OMS_订单_Detail_计价.Add  ( new T_OMS_订单_Detail_计价()
                {
                    订单材料号 = data.行号,
                    订单号 = data.订单号,
                    印品部件 = data.印品部件,
                    工艺类别 = "材料",
                    项目编码 = "CL-ZZF",
                    计量单位 = data.计量单位,
                    单价 = data.单价,
                    系数 = 1,
                    数量 = data.数量,
                    金额 = (data.单价 * data.数量),
                    标准单价 = data.单价,
                    统计编码 = "材料"
                });
                odmRepository.Add(data);
            }
            else if (flag == "edit")
            {
                string[] arr = { "数量", "计量单位", "材料来源", "材料编码","单价" };
                odmRepository.Edit(data, arr);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                odmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功", id = data.行号 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = 0, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// s删除T_OMS_订单_Detail_材料
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteOrderMaterial(List<T_OMS_订单_Detail_材料> data)
        {
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var item in data)
            {
                odmRepository.Delete(item, true);
            }
            try
            {
                odmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ImportProcess(List<T_OMS_订单_Detail_工艺> data)
        {
            ///try cache 可以用过滤器代替
            JsonResult jr = null;
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var i in data)
            {
                var query = processRepository.QueryWhere(p => p.项目编码 == i.项目编码).Select(p => new { p.销售单价, 统计编码 = p.统计编码.Trim(), p.计量单位, p.工艺类别 }).FirstOrDefault();
                i.T_OMS_订单_Detail_计价.Add( new T_OMS_订单_Detail_计价() { 订单工艺号 = i.行号, 订单号 = i.订单号, 印品部件 = i.印品部件,
                    工艺类别 = query.工艺类别, 项目编码 =i.项目编码, 计量单位 = query.计量单位, 单价 = query .销售单价, 系数 = 1, 数量 = i.数量, 金额 = (query.销售单价*i.数量), 标准单价 = query.销售单价,统计编码= query.统计编码});
                odpRepository.Add(i);
            }
            try
            {
                odpRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        [HttpPost]
        public ActionResult ImportMaterial(List<T_OMS_订单_Detail_材料> data)
        {
            JsonResult jr = null;
            //材料默认为本场
            var factory = System.Web.Configuration.WebConfigurationManager.AppSettings["factory"];
            var orderNo = data[0].订单号;
            var auditor = oRepository.QueryWhere(p => p.订单号 == orderNo).Select(p => p.审核人编码).FirstOrDefault();
            if (!string.IsNullOrEmpty(auditor)) { return Json(new { success = -1, msg = "该单号已审核" }, JsonRequestBehavior.AllowGet); }
            foreach (var i in data)
            {
                i.材料来源 = factory;
                i.T_OMS_订单_Detail_计价.Add( new T_OMS_订单_Detail_计价()
                {
                    订单材料号 = i.行号,
                    订单号 = i.订单号,
                    印品部件 = i.印品部件,
                    工艺类别 = "材料",
                    项目编码 = "CL-ZZF",
                    计量单位 = i.计量单位,
                    单价 = i.单价,
                    系数 = 1,
                    数量 = i.数量,
                    金额 = (i.单价*i.数量),
                    标准单价 = i.单价,
                    统计编码 = "材料"
                });
                odmRepository.Add(i);
            }
            try
            {
                odmRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
        /// <summary>
        /// 导入工艺（所要工艺类别）
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessCategory()
        {
            var query = processRepository.Query().Select(p => new { p.项目编码, p.项目名称, p.工艺类别, p.计数标识 }).GroupBy(p => p.工艺类别).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导入材料（所要材料）
        /// </summary>
        /// <returns></returns>

        public ActionResult ShowMaterial()
        {
            var query = mRepository.Query().Select(p => new { p.材料编码, p.材料名称, p.材料类别编码, p.T_MIMS_材料类别编码表.材料类别名称,p.计量单位 }).GroupBy(p => p.材料类别编码).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult TTTT()
        //{
        //    var strDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
        //    var api = new Api();
        //    api.Key = "78d88123-b454-4498-8841-291a652bb99b,B2F1B5CE506B4086A711342CEE84D942";
        //    api.Method =/* "EPCUpdateOrderStatus"*/"GET_POCODE_LIST";
        //    api.Timestamp = strDate;
        //    api.ApiVersion = "1.0";
        //    api.ClientVersion = "1.0";
        //    api.ClientName = "印达ERP";
        //    api.ClientCode = "YINDAERP";
        //    api.Content = new Content();
        //    using (var client = new HttpClient())
        //    {

        //        string jsonString = JsonConvert.SerializeObject(api);
        //        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        //        using (StreamContent sc = new StreamContent(new MemoryStream(bytes)))
        //        {
        //            sc.Headers.ContentLength = bytes.Length;
        //            sc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //            var result = client.PostAsync("http://www.bdzdys.com:81/api", sc).Result;
        //            //var objectResult = JsonConvert.DeserializeObject<Student>(result.Content.ReadAsstringAsync().Result);
        //            var str = result.Content.ReadAsStringAsync().Result;
        //            return Content(str);
        //        }

        //    }
        //}

        /// <summary>
        /// 业务计价单清单
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderPriceList()
        {
            return View();
        }
        public ActionResult ShowOrderList(SearchParams data,int page=1,int rows=10)
        {
            List<SqlParameter> pars = new List<SqlParameter>() {
                 new SqlParameter("@page",(page -1)*rows +1),
                 new SqlParameter("@rows",rows*page)
            };
            string sql = " V_OrderPriceList where 1=1";
            if (!string.IsNullOrEmpty(data.orderName)) {
                sql += " and 订单名称 like @orderName";
                pars.Add(new SqlParameter("@orderName", "%"+ data.orderName + "%"));
            }
            if (!string.IsNullOrEmpty(data.orderNo)) {
                sql += " and 订单号 =@orderNo";
                pars.Add(new SqlParameter("@orderNo", data.orderNo));
            }
            if (!string.IsNullOrEmpty(data.printCategory)) {
                sql += " and 印品类别编码=@printCategory";
                pars.Add(new SqlParameter("@printCategory", data.printCategory));
            }
            if (!string.IsNullOrEmpty(data.customerNo)) {
                sql += " and 客户编码 =@customerNo";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (!string.IsNullOrEmpty(data.remark)) {
                sql += " and 加工要求 like @remark";
                pars.Add(new SqlParameter("@remark", "%" + data.remark + "%"));
            }
            if (!string.IsNullOrEmpty(data.saleMan)) {
                sql += " and 业务员编码=@saleMan";
                pars.Add(new SqlParameter("@saleMan", data.saleMan));
            }
            if (data.zhidanStart != null && data.zhidanEnd != null) {
                sql += " and 制单日期 between @zhidanStart and @zhidanEnd";
                pars.Add(new SqlParameter("@zhidanStart", data.zhidanStart));
                pars.Add(new SqlParameter("@zhidanEnd", data.zhidanEnd));
            }
            if (data.deliveryStart != null && data.deliveryEnd != null) {
                sql += " and 交货日期 between @deliveryStart and @deliveryEnd";
                pars.Add(new SqlParameter("@deliveryStart", data.deliveryStart));
                pars.Add(new SqlParameter("@deliveryEnd", data.deliveryEnd));
            }
            var strSql = "select count(*) from " + sql;
            var total = oRepository.SqlQuery2<int>(strSql , pars.ToArray()).SingleOrDefault();
            sql = " select 交货日期,订单号,订单名称,制单日期,金额,制单人,业务员,客户名称,审核人 from (select * , row_number() over( order  by 订单号 desc)as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var par = pars.Select(p => ((ICloneable)p).Clone()).ToArray();
            var query = oRepository.SqlQuery2<V_OrderPriceList>(sql,par).ToList();
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
            string sql = " V_OrderPriceList where 1=1";
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
            if (!string.IsNullOrEmpty(data.printCategory))
            {
                sql += " and 印品类别编码=@printCategory";
                pars.Add(new SqlParameter("@printCategory", data.printCategory));
            }
            if (!string.IsNullOrEmpty(data.customerNo))
            {
                sql += " and 客户编码 =@customerNo";
                pars.Add(new SqlParameter("@customerNo", data.customerNo));
            }
            if (!string.IsNullOrEmpty(data.remark))
            {
                sql += " and 加工要求 like @remark";
                pars.Add(new SqlParameter("@remark", "%" + data.remark + "%"));
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
            if (data.deliveryStart != null && data.deliveryEnd != null)
            {
                sql += " and 交货日期 between @deliveryStart and @deliveryEnd";
                pars.Add(new SqlParameter("@deliveryStart", data.deliveryStart));
                pars.Add(new SqlParameter("@deliveryEnd", data.deliveryEnd));
            }

            sql = " select 交货日期,订单号,订单名称,制单日期,金额,制单人,业务员,客户名称,审核人 from (select * , row_number() over( order  by 订单号 desc)as rowId from  " + sql + ") a where  rowid between @page and @rows";
            var query = oRepository.SqlQuery2<V_OrderPriceList>(sql, pars.ToArray());
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult OrderList()
        {
            return View();
        }
    }

    


    public class Factory
    {
        public string FactoryNo { get; set; }
        public string FactoryName { get; set; }

        public string Alias { get; set; }
    }


  
}