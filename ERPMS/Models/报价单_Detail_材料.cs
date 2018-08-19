using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 报价单_Detail_材料
    {
        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 材料名称 { get; set; }
        public string 材料编码 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 规格型号 { get; set; }
        public Nullable<decimal> 匹配系数 { get; set; }
        public Nullable<int> 报价detail号 { get; set; }
        public string  计量单位 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
    }
}