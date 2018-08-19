using ERPMS.Models.DingTalk;
using ERPMS.Util;
using ERPMS.Util.AccessToken;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace ERPMS.Controllers
{
    public class DingTalkController : Controller
    {
        // GET: DingTalk
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDeptMent()
        {
            List<Deptment> deptList = new List<Deptment>();
           
            string response = DingTackService.GetDeptMent();
            JObject jsonObj = JObject.Parse(response);
            string deptStr = "";
            if ((int)(jsonObj["errcode"]) == 0)
            {
                deptStr = jsonObj["department"].ToString();
            }
            Regex r = new Regex("parentid");
            deptStr = r.Replace(deptStr, "pId");
            //var str = JsonConvert.SerializeObject(deptList);
  
            return Content(deptStr);
        }

        public ActionResult CreateDeptment(Deptment dept)
        {
            string response = DingTackService.CreateDeptment(dept);
            return Content(response);
        }
        public ActionResult UpdateDeptment(Deptment dept)
        {
            string response = DingTackService.UpdateDeptment(dept);
            return Content(response);
        }

        public ActionResult DeleteDeptment(string id)
        {
            string response = DingTackService.DeleteDeptment(id);
            return Content(response);
        }

        public ActionResult GetComTree()
        {
            List<Tree> deptList = new List<Tree>();
            string response = DingTackService.GetDeptMent();
            JObject jsonObj = JObject.Parse(response);
            string deptStr = "";
            if ((int)(jsonObj["errcode"]) == 0)
            {
                deptStr = jsonObj["department"].ToString();
            }
            var menu = JsonConvert.DeserializeObject<List<Tree>>(deptStr);
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
        public ActionResult GetUserByDepartment(int id)
        {
            var response = DingTackService.GetUsersByDepartment(id);
            JObject jsonObj = JObject.Parse(response);

            var userlist = jsonObj["userlist"].ToString();
            var list = JsonConvert.DeserializeObject<List<User>>(userlist);
            return Json(list);

        }

        public ActionResult AddUser(User u)
        {
            string response = DingTackService.AddUser(u);
            return Content(response);
        }

        public ActionResult EditUser(User u)
        {
            string response = DingTackService.EditUser(u);
            return Content(response);

        }

        public ActionResult RemoveUser(RemoveUser data)
        {
            //todo:useridlist 有长度限制 count 在1-20 之间  
            string response = DingTackService.RemoveUser(data);
            return Content(response);

        }

        public ActionResult GetAttendance( attPars par)
        {
            var date1 = par.workDateFrom.ToString("yyyy-MM-dd hh:mm:ss");
                var date2 = par.workDateTo.ToString("yyyy-MM-dd hh:mm:ss");
            string response = DingTackService.Attendance(new {
                offset = par.page * par.rows,
                limit = par.rows,
                workDateFrom =date1,
                workDateTo =date2,
                userIdList = par.userIdList});
            JObject jsonObj = JObject.Parse(response);
            var userlist = "";
            if ((int)(jsonObj["errcode"]) == 0)
            {
                userlist = jsonObj["recordresult"].ToString();
                return Content(userlist);
            }
            else {
                return Content(response);
            }
           
        }
        public ActionResult Test(string str)
        {
            //var json = JsonConvert.DeserializeObject<User>(str); //序列化

            ////return Json(json);
            //string response = DingTackService.AddUser(json);
            string num= "1492531200000";
            DateTime response = ConvertStringToDateTime(num);
            return Json(new { response},JsonRequestBehavior.AllowGet);
        }

        public static DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

    }
    public class attPars
    {
        public int page { get; set; }
        public int rows { get; set; }
        public DateTime workDateFrom { get; set; }
        public DateTime workDateTo { get; set; }
        public List<string> userIdList { get; set; }
    }
    
}