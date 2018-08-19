using ERPMS.Util.AccessToken;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ERPMS.Models.DingTalk
{
    public class DingTackService
    {

        /// <summary>
        /// 获取部门(不转成对象了)
        /// </summary>
        /// <returns></returns>
        public static string GetDeptMent()
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/department/list?access_token=" + token;
            string content= GetResponse(strUrl);
            return content;
           
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        /// <returns></returns>
        public static string CreateDeptment(Deptment dept)
        {
          
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/department/create?access_token="+token;
            string content = PostData(strUrl, dept);
            return content;
        }
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string UpdateDeptment(Deptment dept)
        {
        
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/department/update?access_token=" + token;
            string content = PostData(strUrl, dept);
            return content;
        }

        public static string DeleteDeptment(string id)
        {
            
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/department/delete?access_token="+token+"&id="+id;
     
            string content = GetResponse(strUrl);
            return content;
        }
        public static string GetUsersByDepartment(int id)
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/user/list?access_token=" + token + "&department_id=" + id;
            string content = GetResponse(strUrl);
            return content;
        }

        public static string AddUser(User u)
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/user/create?access_token=" + token;
            string content = PostData(strUrl, u);
            return content;

        }
        public static string EditUser(User u)
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/user/update?access_token=" + token;
            string content = PostData(strUrl, u);
            return content;
        }
        public static string RemoveUser(RemoveUser data)
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/user/batchdelete?access_token=" + token;
            string content = PostData(strUrl, data);
            return content;
        }

        public static string Attendance(Object obj)
        {
            string token = TockenHelper.IsExistAccess_Token();
            string strUrl = "https://oapi.dingtalk.com/attendance/list?access_token=" + token;
            string content = PostData(strUrl, obj);
            return content;
        }

        /// <summary>
        /// 得到参数字典
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public static Dictionary<string, string> GetParams(Object obj)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    string tocken = TockenHelper.IsExistAccess_Token();
        //    dic.Add("access_token", tocken);
        //    if (obj != null)
        //    {
        //        foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
        //        {
        //            dic.Add(p.Name, p.GetValue(p).ToString());
        //        }
        //    }
        //    return dic;
        //}
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public  static  string GetResponse(string strUrl)
        {
            string content = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            
            using (WebResponse wr = req.GetResponse())
            {
                //HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                content = reader.ReadToEnd();
            }
            return content;
        }
        /// <summary>
        /// post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        //public static string PostData(string url, Dictionary<string ,string> parameters)
        //{
        //    string content = "";
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
        //    myRequest.Method = "POST";
        //    myRequest.ContentType = "application/x-www-form-urlencoded";
        //    //如果需要POST数据     
        //    if (!(parameters == null || parameters.Count == 0))
        //    {
        //        StringBuilder buffer = new StringBuilder();
        //        int i = 0;
        //        foreach (string key in parameters.Keys)
        //        {
        //            if (i > 0)
        //            {
        //                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
        //            }
        //            else
        //            {
        //                buffer.AppendFormat("{0}={1}", key, parameters[key]);
        //            }
        //            i++;
        //        }
        //        byte[] postdata = Encoding.UTF8.GetBytes(buffer.ToString());
        //        using (Stream stream = myRequest.GetRequestStream())
        //        {
        //            stream.Write(postdata, 0, postdata.Length);
        //        }
        //    }
        //    using (WebResponse wr = myRequest.GetResponse())
        //    {
        //        HttpWebResponse _myResponse = (HttpWebResponse)myRequest.GetResponse();
        //        StreamReader reader = new StreamReader(_myResponse.GetResponseStream(), Encoding.UTF8);
        //        content = reader.ReadToEnd();
        //    }
        //    return content;

        //}


        public static string PostData(string url, Object obj)
        {
            string content = "";
            var jsonStr = JsonConvert.SerializeObject(obj);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonStr);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentLength = buffer.Length;
             req.ContentType = "application/json";
            //req.ContentType = "application/x-www-form-urlencoded";
           
            using (var stream = req.GetRequestStream())
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            using (WebResponse resp = req.GetResponse())
            {
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                content = sr.ReadToEnd();
            }
            return content;
        }
    }
}