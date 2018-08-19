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
    /// 材料申购
    /// </summary>
    public class ERP_OrderApplyPurchaseController : Controller
    {
        /// <summary>
        /// T_PHMS_采购申请单_Master 接口
        /// </summary>
        IPurchaseApplyRepository paRepository = new PurchaseApplyRepository();
        /// <summary>
        /// T_PHMS_采购申请单_Detail 接口
        /// </summary>
        IPurchaseApplyDetailRepository padRepository = new PurchaseApplyDetailRepository();

        /// <summary>
        /// 组织编码表接口
        /// </summary>
        IOrganizeRepository oRepository = new OrganizeRepository();

        /// <summary>
        ///  Table_单据类型编码表 接口
        /// </summary>

        IBillRepository bRepository = new BillRepository();
        /// <summary>
        /// T_MIMS_纸张编码表 接口
        /// </summary>
        IPaperRepository pRepository = new PaperRepository();
        /// <summary>
        /// 材料编码表
        /// </summary>
        IMaterialRepository mRepository = new MaterialRepository();

        /// <summary>
        /// 纸张库存 接口
        /// </summary>
        IPaperstockRepository psRepository = new PaperstockRepository();

        /// <summary>
        /// 材料库存
        /// </summary>

        IMaterialstockRepository msRepository = new MaterialstockRepository();
        /// <summary>
        /// 订单Master 接口
        /// </summary>
        IOrderRepository orderRepository = new OrderRepository();
        // GET: ERP_OrderApplyPurchase
        public ActionResult Index(string orderNo,string orderName)
        {
            TempData["orderApply_orderNo"] = orderNo;
            TempData["orderApply_orderName"] = orderName;
            return View();
        }

        public ActionResult Apply()
        {
            var orderNo = TempData["orderApply_orderNo"].ToString();
            var orderName = TempData["orderApply_orderName"].ToString();
            var identity = User.Identity.Name;
            T_PHMS_采购申请单_Master query = null;
            ObjectParameter par = null;
            if (!string.IsNullOrEmpty(orderNo))
            {
                query = paRepository.QueryWhere(p => p.订单号 == orderNo).FirstOrDefault();
                if (query == null)
                {
                    query = new T_PHMS_采购申请单_Master();
                    query.订单号 = orderNo;
                    query.日期 = DateTime.Now;
                    query.制单人编码 = identity;
                    query.摘要 = orderName;
                    par = new ObjectParameter("number", "");
                    int result = paRepository.ExecProc2(par, query);
                    if (result > 0)
                    {
                        query.申请单号 = par.Value.ToString();
                    }
                }
            }
            var response = JsonConvert.SerializeObject(query);
            return Content(response);
            //return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// T_PHMS_采购申请单_Detail 集合
        /// </summary>
        /// <param name="paramNo">申请单号</param>
        /// <returns></returns>
        public ActionResult GetPurchaseDetail(string paramNo)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter ("@applyNo",paramNo)
            };
            string sql = "select 材料编码 , 序号 , 申请单号 , 材料名称 , 规格 , 计量单位 , 客户编码 ,V_库存数量  , 数量    , 订单号 , Flag  , 备注 from V_Purchase_paper where  申请单号=@applyNo order by Flag";
            var query = paRepository.SqlQuery2<V_采购申请清单_Detail>(sql, pars).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>

        public ActionResult GetDepartment()
        {
            var query = oRepository.Query().Select(p => new { Key = p.名称, Id = p.编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBill()
        {
            var query = bRepository.Query().Select(p => new { Key = p.单据类型名称, Id = p.单据类型编码, Model = p.所属模块 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取纸张信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaper()
        {
            var query = pRepository.Query().Select(p => new { Key = p.纸张名称, Id = p.纸张编码 ,Model=p.规格}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取材料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMaterial()
        {
            var query = mRepository.Query().Select(p => new { Key = p.材料名称, Id = p.材料编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 纸张库存数量
        /// </summary>
        /// <param name="paperNo"></param>
        /// <returns></returns>
        public ActionResult GetTotalPaper(string paperNo, string customerNo)
        {
            var count = psRepository.QueryWhere(p => p.纸张编码 == paperNo && p.纸张客户编码 == customerNo).Select(p => p.数量).FirstOrDefault();
            return Content(count.ToString());
        }

        /// <summary>
        /// 订单集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOrder()
        {
            var query = orderRepository.Query().Select(p => new { Key = p.订单名称, Id = p.订单号, cNo = p.客户编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUnit()
        {
            var query = padRepository.Query().GroupBy(p => p.计量单位).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 材料数量
        /// </summary>
        /// <param name="materialNo"></param>
        /// <returns></returns>
        public ActionResult GetTotalMaterial(string materialNo)
        {
            decimal? count = null;
            if (!string.IsNullOrEmpty(materialNo))
            {
                count = msRepository.QueryWhere(p => p.材料编码 == materialNo).Select(p => p.数量).FirstOrDefault();
            }
            return Content(count.ToString());
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult RunPage(int page = 0)
        {
            var total = paRepository.Query().Count();
            var query = paRepository.Query().OrderByDescending(p => p.申请单号).Select(p => new { p.部门编码, p.制单人编码, p.审核日期, p.摘要, p.申请人, p.单据类型, p.物料类别, p.负责人, p.审核人编码, p.日期, p.申请单号, p.订单号 }).Skip(page).Take(1).FirstOrDefault();
            var response = JsonConvert.SerializeObject(new { total = total, rows = query });
            return Content(response);
            //return Json(new { total = total, query = query }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateApply(T_PHMS_采购申请单_Master data)
        {
            JsonResult jr = null;
            string[] strArr = { "部门编码", "日期","摘要","制单人编码","申请人","负责人" ,"单据类型"};
            paRepository.Edit(data,strArr);
            try
            {
                paRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "修改失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult DeleteApply(T_PHMS_采购申请单_Master data)
        {
            JsonResult jr = null;
            paRepository.Delete(data, true);
            try
            {
                paRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult AddApply(T_PHMS_采购申请单_Master data)
        {
            // data.制单人编码 = User.Identity.Name;
            data.制单人编码 = User.Identity.Name;
            ObjectParameter par = new ObjectParameter("number", "");
            int result = paRepository.ExecProc2(par, data);
            if (result > 0)
            {
                return Json(new { success = 0, msg = "添加成功", id = par.Value }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public ActionResult Auditor(T_PHMS_采购申请单_Master data)
        {
            JsonResult jr = null;

            //var identity = User.Identity.Name;
            data.审核人编码 = User.Identity.Name;
            data.审核日期 = DateTime.Now;
            string[] strArr = { "审核人编码", "审核日期" };
            paRepository.Edit(data, strArr);
            try
            {
                paRepository.SaverChanges();
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
        public ActionResult UnAuditor(T_PHMS_采购申请单_Master data)
        {
            JsonResult jr = null;
            data.审核人编码 = null;
            data.审核日期 = null;
            string[] strArr = { "审核人编码", "审核日期" };
            paRepository.Edit(data, strArr);
            try
            {
                paRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "撤审成功", AuditorNo = data.审核人编码, Date = data.审核日期 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "撤审失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }


        public ActionResult Refresh(string paramNo)
        {
            JsonResult jr = null;
            T_PHMS_采购申请单_Master single = null;
            if (!string.IsNullOrEmpty(paramNo))
            {
                single = paRepository.QueryWhere(p => p.申请单号 == paramNo).FirstOrDefault();
            }
            var response = JsonConvert.SerializeObject(single);
            return Content(response);
            //return Json(single, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 添加采购Detail 
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveOrUpdateDetail(T_PHMS_采购申请单_Detail data, string flag)
        {
            JsonResult jr = null;
            if (flag == "add")
            {
                padRepository.Add(data);
            }
            else if (flag == "edit")
            {
                padRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标识" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                padRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除采购Detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult DeleteSaleDetail(List<T_PHMS_采购申请单_Detail> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                padRepository.Delete(item, true);
            }
            try
            {
                padRepository.SaverChanges();
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