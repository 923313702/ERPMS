using ERPMS.IRepository.Basics;
using ERPMS.Models;
using ERPMS.Repository;
using ERPMS.Repository.basics;
using ERPMS.Util;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Management.basics
{
    /// <summary>
    /// 工艺编码表
    /// </summary>
    public class ERP_ProcessController : Controller
    {
        /// <summary>
        /// 工艺编码表
        /// </summary>
        IProcessRepository pRepository = new ProcessRepository();
        /// <summary>
        /// 生产设备
        /// </summary>
        IEquipmentRepository eRepository = new EquipmentRepository();
        /// <summary>
        /// 组织编码表
        /// </summary>
        IOrganizeRepository oRepository = new OrganizeRepository();
        // GET: ERP_Process
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProcess(int page = 1, int rows = 10, string pCategory = "")
        {
            //最好不要用*
            string sql = "";
            var total = 0;
            Expression<Func<T_OMS_工艺项目编码表, bool>> ex = PredicateBuilder.New<T_OMS_工艺项目编码表>(true);
            List< SqlParameter> pars = new List<SqlParameter>{
                new SqlParameter ("@skip",((page - 1) * rows + 1)),
                new SqlParameter ("@take",(page * rows))
            };
            if (string.IsNullOrEmpty(pCategory))
            {
                sql = "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process) t) m where m.rowId between @skip and @take";
            }
            else {
                sql = "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process where 工艺类别 = @category ) t) m where m.rowId between @skip and @take";
                pars.Add(new SqlParameter("@category", pCategory));
                ex = ex.And(p => p.工艺类别 == pCategory);
            }
            total = pRepository.QueryWhere(ex).Count();
            var query = pRepository.SqlQuery2<工艺项目编码表>(sql, pars.ToArray()).ToList();
            return Json(new { total = total, rows = query }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询所有工艺类别(查询用的)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCategory()
        {
            var result = pRepository.Query().GroupBy(p => p.工艺类别).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取计量单位
        /// </summary>
        /// <returns></returns>
        public ActionResult Getmetering()
        {
            var result = pRepository.Query().GroupBy(p => p.计量单位).Select(p => new { Key = p.Key, Id = p.Key }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 检查工艺编码是否重复
        /// </summary>
        /// <param name="processNo"></param>
        /// <returns></returns>
        public ActionResult CheckProcessNo(string processNo)
        {
            var result = pRepository.QueryWhere(p => p.项目编码 == processNo).Count() > 0;
            return Content(result.ToString());
        }

        /// <summary>
        /// 展现部门
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult ShowDept()
        {
            var query = oRepository.QueryWhere(p => p.班组标识 == 0).Select(p => new { Key = p.名称, Id = p.编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 展现班组
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowTeam()
        {
            var query = oRepository.QueryWhere(p => p.班组标识 == 1).Select(p => new { Key = p.名称, Id = p.编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设备
        /// </summary>
        public ActionResult ShowEquipment()
        {
            var query = eRepository.Query().Select(p => new { Key = p.设备名称, Id = p.设备编码 }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加/修改 工艺项目编码表
        /// </summary>
        /// <param name="process"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult SaveorUpdateProcess(T_OMS_工艺项目编码表 data, string flag)
        {
           // process.统计编码 = process.统计编码.Trim();
            JsonResult jr = null;
            if (flag == "add")
            {
                pRepository.Add(data);
            }
            else if (flag == "edit")
            {
                pRepository.Edit(data);
            }
            else
            {
                jr = Json(new { success = -1, msg = "没有获取到正确的操作标志" }, JsonRequestBehavior.AllowGet);
                return jr;
            }
            try
            {
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        /// <summary>
        /// 删除工艺项目编码表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleteProcess(List<T_OMS_工艺项目编码表> data)
        {
            JsonResult jr = null;
            foreach (var item in data)
            {
                pRepository.Delete(item, true);
            }
            try
            {
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        public ActionResult ExportExcel(int page, int rows, string pCategory="")
        {
            string sql= "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process) t) m where m.rowId between @skip and @take"; ;
            List<SqlParameter> pars = new List<SqlParameter> {
                new SqlParameter ("@skip",((page - 1) * rows + 1)),
                new SqlParameter ("@take",(page * rows))
            };
            if (!string.IsNullOrEmpty(pCategory))
            {
                sql = "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process where 工艺类别 = @category ) t) m where m.rowId between @skip and @take";
                pars.Add(new SqlParameter("@category", pCategory));
            }
            var query = pRepository.SqlQuery2<工艺项目编码表>(sql, pars.ToArray());
            DataTable tb = query.ToDataTable(p => new object[] { query });
            var stream = NPOIExcel.ExportExcel(tb);
            var fileName = DateTime.Now.ToString("yy-MM-dd hh-mm-ss") + ".xlsx";
            return File(stream, "application/vnd.ms-excel", fileName);
        }

        public ActionResult PrintPage(int page, int rows, string pCategory)
        {
            TempData["process_page"] = page;
            TempData["process_rows"] = rows;
            TempData["pCategory"] = pCategory;
            return View();
        }
        public ActionResult GetPageData(SearchParams data)
        {
            //最好不要用*
            string sql = "";
            var page = Convert.ToInt32(TempData["process_page"].ToString());
            var rows = Convert.ToInt32(TempData["process_rows"].ToString());
            var pCategory = TempData["pCategory"].ToString();
            List<SqlParameter> pars = new List<SqlParameter>{
                new SqlParameter ("@skip",((page - 1) * rows + 1)),
                new SqlParameter ("@take",(page * rows))
            };
            if (string.IsNullOrEmpty(pCategory))
            {
                sql = "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process) t) m where m.rowId between @skip and @take";
            }
            else
            {
                sql = "select 项目编码,项目名称,工艺类别,统计编码 as 统计名称,计量单位 as 计量  ,计数标识 as 计数 ,报价标识 as 报价 ,工艺标识 as 工艺,换单金额,内部单价,销售单价,部门编码 as 部门 ,班组编码 as 班组 , 小时产能,机台编码 as 加工机台" +
                    " from (select * , ROW_NUMBER() over(order by 项目编码) as rowId from V_Process where 工艺类别 = @category ) m where m.rowId between @skip and @take";
                pars.Add(new SqlParameter("@category", pCategory));
            }
            var query = pRepository.SqlQuery2<工艺项目编码表2>(sql, pars.ToArray()).ToList();
            //List<SqlParameter> pars = new List<SqlParameter>{
            //    new SqlParameter ("@skip",((data.page - 1) * data.rows + 1)),
            //    new SqlParameter ("@take",(data.page * data.rows))
            //};
            //if (string.IsNullOrEmpty(data.pCategory))
            //{
            //    sql = "select * from (select * , ROW_NUMBER() over(order by t.项目编码) as rowId from( select * from V_Process) t) m where m.rowId between @skip and @take";
            //}
            //else
            //{
            //    sql = "select 项目编码,项目名称,工艺类别,统计编码 as 统计名称,计量单位 as 计量  ,计数标识 as 计数 ,报价标识 as 报价 ,工艺标识 as 工艺,换单金额,内部单价,销售单价,部门编码 as 部门 ,班组编码 as 班组 , 小时产能,机台编码 as 加工机台" +
            //        " from (select * , ROW_NUMBER() over(order by 项目编码) as rowId from V_Process where 工艺类别 = @category ) m where m.rowId between @skip and @take";
            //    pars.Add(new SqlParameter("@category", data.pCategory));
            //}
            //var query = pRepository.SqlQuery2<工艺项目编码表2>(sql, pars.ToArray()).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}