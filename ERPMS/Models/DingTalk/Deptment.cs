using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models.DingTalk
{
    public class Deptment
    {
        public string id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父部门id
        /// </summary>
        
        public string  parentid { get; set; }
        /// <summary>
        /// 在父部门中的次序值
        /// </summary>

        public string order { get; set; }

        ///// <summary>
        ///// tocken(票据)
        ///// </summary>
        //public string  access_token { get; set; }
    }
}