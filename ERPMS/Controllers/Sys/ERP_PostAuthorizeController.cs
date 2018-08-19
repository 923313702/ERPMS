using ERPMS.IRepository.Sys;
using ERPMS.Models;
using ERPMS.Repository.Sys;
using ERPMS.Util;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Sys
{
    public class ERP_PostAuthorizeController : Controller
    {
        IPostRepository pRepository = new PostRepository();
        IFunctionRepository fRepository = new FunctionRepository();
        IPostFunctionRepository pfRepository = new PostFunctionRepository();
        // GET: ERP_Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPosts(int page = 1, int rows = 10, string postName = "")
        {
            Expression<Func<Table_岗位编码表, bool>> ex = PredicateBuilder.New<Table_岗位编码表>(true);
            if (!string.IsNullOrEmpty(postName))
            {
                ex = ex.And(p => p.岗位名称.Contains(postName));
            }
            var total = pRepository.QueryWhere(ex).Count();
            var list = pRepository.AsExpandable(ex, p => p.岗位编码, true).Skip((page - 1) * rows).Take(rows).Select(p => new { p.岗位名称, p.岗位编码 }).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPostTree(string postId, bool flag)
        {
            List<Tree> menu = null;
            if (flag)
            {
                menu = fRepository.Query().Select(p => new Tree { Id = p.功能编码, Name = p.功能名称, ParentId = p.上级编码 }).ToList();
                //menu = (from p in db.Table_功能编码表 select new Tree { Id = p.功能编码, Name = p.功能名称, ParentId = p.上级编码 });
            }
            else
            {
                string sql = "select distinct( b.功能编码) as Id, b.功能名称 as Name, b.上级编码 as  ParentId  from Table_功能岗位编码表 a inner join Table_功能编码表 b on a.功能编码= b.功能编码 and a.岗位编码=@postId";
                SqlParameter par = new SqlParameter("@postId", postId);
                menu = fRepository.SqlQuery2<Tree>(sql, par).ToList();
            }

            var tree = Node.RawCollectionToTree(menu, true);
            string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
           new JsonSerializerSettings
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
               NullValueHandling = NullValueHandling.Ignore
           });
            return Content(json);
        }


        [HttpPost]
        public ActionResult DeletePost(List<Table_功能岗位编码表> data)
        {
            JsonResult jr = null;
            try
            {
                foreach (var item in data)
                {
                    pfRepository.Delete(item, true);
                }
                pfRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        [HttpPost]
        public ActionResult AddPost(List<Table_功能岗位编码表> data)
        {
            JsonResult jr = null;
            var postId = data[0].岗位编码;
            var table_data = pfRepository.QueryWhere(p => p.岗位编码 == postId).ToList();
            var list = data.Except(table_data, new PostEquality()).ToList();
            if (list.Count == 0) return Json(new { success = 0m, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            foreach (var item in list)
            {
                //todo:授权人编码 要改掉!!!!
                item.授权人编码 = "Hello,Wrold";
                item.授权日期 = DateTime.Now;
            }
            try
            {
                pfRepository.Add(list);
                pfRepository.SaverChanges();
                jr = Json(new { success = 0m, msg = "添加成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "添加失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

    }
}