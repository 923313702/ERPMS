using BotDetect.Web.Mvc;
using ERPMS.IRepository.Sys;
using ERPMS.Models;
using ERPMS.Repository.Sys;
using ERPMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ERPMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 用户编码表接口
        /// </summary>
        IUserRepository uRepository = new UserRepository();
        /// <summary>
        /// 功能编码表接口
        /// </summary>
        IFunctionRepository fRepository = new FunctionRepository();
        public ActionResult Index()
        {
            ViewData["Account"] = User.Identity.Name;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string Account ,string Password)
        {
            if (string.IsNullOrEmpty(Account)) { return Json(new { success = -1, msg = "账号不能为空" },JsonRequestBehavior.AllowGet); }
            if(string .IsNullOrEmpty (Password)) { return Json(new { success = -1, msg = "密码不能为空" }, JsonRequestBehavior.AllowGet); }
            Password = Password.Sha1();
            int count = uRepository.QueryWhere(p => p.登录名 == Account && p.密码 == Password).Count();
           if(count>0)
            {
                WebHelper.SetFormsAuthentication(Account,"");
                ViewData["Account"] =Account;
                return Json(new { success = 0, msg = "/Home/Index" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = -1, msg = "账号密码错误" }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult CheckCaptchaCode(string CaptchaCode, string BDC_VCID_ExampleCaptcha)
        //{
        //    //MvcCaptcha loginationCaptcha = new MvcCaptcha("ExampleCaptcha");
        //    //var captchaId = loginationCaptcha.CaptchaId;
        //   // var instaneId = loginationCaptcha.CurrentInstanceId;
        //    // string instaneId = CustomCaptcha.CurrentInstanceId;
        //    bool captchaValid = MvcCaptcha.Validate("ExampleCaptcha", CaptchaCode, BDC_VCID_ExampleCaptcha);
        //    return Json(captchaValid, JsonRequestBehavior.AllowGet);
        //    //MvcCaptcha loginationCaptcha = new MvcCaptcha("ExampleCaptcha");
        //    //var captchaId = loginationCaptcha.CaptchaId;
        //    //var instaneId = loginationCaptcha.CurrentInstanceId;
        //    //// string instaneId = CustomCaptcha.CurrentInstanceId;
        //    //bool captchaValid = MvcCaptcha.Validate(captchaId, CaptchaCode, instaneId);
        //    //return Json(captchaValid, JsonRequestBehavior.AllowGet);
        //}

        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult GetMenu()
        {
            //var menu = (from p in db.SYS_用户信息表 join s in db.Table_岗位人员编码表 on p.登录名 equals s.人员编码 join m in db.Table_功能岗位编码表 on s.岗位编码 equals m.岗位编码 join v in db.Table_功能编码表 on m.功能编码 equals v.功能编码 where p.登录名=="super" &&  v.是否菜单==1 select new  Tree{Id =v.功能编码,Name=v.功能名称,ParentId=v.上级编码,Url=v.执行程序}  ).Distinct().ToList();
            string sql = "select distinct( d.功能编码) as Id ,d.功能名称 as Name,上级编码 as ParentId ,执行程序 as Url  from SYS_用户信息表 a inner join Table_岗位人员编码表 b on a.登录名=b.人员编码  inner join Table_功能岗位编码表 c on b.岗位编码=c.岗位编码 inner join Table_功能编码表 d on c.功能编码=d.功能编码  and a.登录名='demo' and  d.是否菜单=1 ";
            var menu = fRepository.SqlQuery2<Tree>(sql).ToList();
            var tree = Node.RawCollectionToTree(menu, true);
            //var tree = Node.InitTree(menu);
            string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            return Content(json);

        }

        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult test2()
        {
            return Content("ajax test");
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}