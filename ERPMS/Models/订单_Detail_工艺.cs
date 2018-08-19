using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 订单_Detail_工艺
    {
        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 项目编码 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 变更单号 { get; set; }
       // public Nullable<int> 排序码 { get; set; }
    
        public string 项目名称 { get; set; }
    }
}