using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 业务报价单_Detail
    {
        public int 行号 { get; set; }
        public string 报价单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 纸张客户编码 { get; set; }
        public string 纸张编码 { get; set; }
        public string  客户名称 { get; set; }
    }
}