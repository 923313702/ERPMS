using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.WebPages;

namespace ERPMS.Util
{
    public class WebHelper
    {
        /// <summary>
        /// 保存用户信息到cookies
        /// </summary>
        /// <param name="u"></param>
        public static void SetFormsAuthentication<T>(string key, T entity)
        {
            //var seed = Guid.NewGuid().GetHashCode();
            //var random = new Random(seed);
            //var result = random.Next(1, 10001);
            //string myKey = key + result;
            string userData = JsonConvert.SerializeObject(entity);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, key, DateTime.Now, DateTime.Now.AddDays(1), false, userData);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));//加密身份保存cookies
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFormsAuthentication<T>()
        {

            //获得cookies
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket Ticket = FormsAuthentication.Decrypt(cookie.Value);
            //解密
            // AccountModel account = (AccountModel)JsonConvert.DeserializeObject(Ticket.UserData);//反序列化
            var user = JsonConvert.DeserializeObject<T>(Ticket.UserData);
            return user;



        }

        #region Session操作
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        private static void WriteSession<T>(string key, T value)
        {
            if (key.IsEmpty())
                return;
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession(string key, string value)
        {
            WriteSession<string>(key, value);
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>        
        public static string GetSession(string key)
        {
            if (key.IsEmpty())
                return string.Empty;
            return HttpContext.Current.Session[key] as string;
        }
        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (key.IsEmpty())
                return;
            HttpContext.Current.Session.Contents.Remove(key);
        }

        #endregion
    }
}