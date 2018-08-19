using ERPMS.IRepository.Sys;
using ERPMS.Models;
using ERPMS.Repository.Sys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERPMS.Controllers.Sys
{
    public class ERP_PostController : Controller
    {
        /// <summary>
        /// 岗位编码表接口
        /// </summary>
        IPostRepository pRepository = new PostRepository();
        /// <summary>
        /// 岗位人员编码表接口
        /// </summary>
        IPostPersonRepository ppRepository = new PostPersonRepository();
        // GET: ERP_Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPosts(int page = 1, int rows = 10)
        {
            int total = 0;
            var list = pRepository.QueryByPage(page,rows, out total, p => true, p => true, true).Select(p => new { p.岗位名称, p.岗位编码 }).ToList();
            //var list = db.Table_岗位编码表.AsExpandable().Where(ex).OrderBy(p=>p.岗位编码).Skip((page-1)*rows).Take(rows).Select(p => new { p.岗位名称, p.岗位编码 }).ToList();
            return Json(new { total = total, rows = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPostPerson(string postNo)
        {
            SqlParameter par = new SqlParameter("@postNo", postNo);
            string sql = "select  a.岗位编码 as 岗位编码 , a.人员编码 as 人员编码,  b.姓名 as 人员姓名, 授权人编码, 授权日期  ,a.备注 as 备注  from Table_岗位人员编码表 a inner join  Table_人员编码表 b on a.人员编码= b.人员编码  and a.岗位编码=@postNo" ;
            var list = ppRepository.SqlQuery(sql,par).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult SaveOrUpdatePost(Table_岗位编码表 data, string flag)
        {
            JsonResult jr = null;
           
            if (flag == "add")
            {
                // db.Table_岗位编码表.Add(post);
                pRepository.Add(data);
            }
            else if (flag == "edit")
            {
                //db.Entry(post).State = EntityState.Modified;
                pRepository.Edit(data);
            }
            else {
                return Json(new { success = -1, msg = "没有获取到操作标志" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //db.SaveChanges();
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
        /// 删除岗位
        /// 用触发器删除（[Table_岗位人员编码表]，[Table_功能岗位编码表]）
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePosts(List<Table_岗位编码表> data)
        {
            JsonResult jr = null;
            try
            {
                foreach (var item in data)
                {
                    //db.Entry(item).State = EntityState.Deleted;
                    pRepository.Delete(item, true);
                }
                //db.SaveChanges();
                pRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }



        public ActionResult SaveOrUpdatePerson(Table_岗位人员编码表 data,string flag)
        {
            JsonResult jr = null;

            if (flag == "add")
            {
                // db.Table_岗位编码表.Add(post);
                ppRepository.Add(data);
            }
            else if (flag == "edit")
            {
                //db.Entry(post).State = EntityState.Modified;
                ppRepository.Edit(data);
            }
            else
            {
                return Json(new { success = -1, msg = "没有获取到操作标志" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //db.SaveChanges();
                ppRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }

        [HttpPost]
        public ActionResult DeletePerson(List<Table_岗位人员编码表> data)
        {
            JsonResult jr = null;
            try
            {
                foreach (var item in data)
                {
                    // db.Entry(item).State = EntityState.Deleted;
                    ppRepository.Delete(item, true);
                }
                //db.SaveChanges();
                ppRepository.SaverChanges();
                jr = Json(new { success = 0, msg = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jr = Json(new { success = -1, msg = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return jr;
        }
    }
}