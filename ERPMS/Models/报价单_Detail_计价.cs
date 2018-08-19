using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 报价单_Detail_计价
    {
        public string 项目名称 { get; set; }
        public string 统计编码 { get; set; }
        public int ID { get; set; }
        public string 项目编码 { get; set; }
        public Nullable<int> 报价工艺号 { get; set; }
        public Nullable<int> 报价材料号 { get; set; }
        public Nullable<int> 报价Detail号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 计量单位 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 系数 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public Nullable<decimal> 标准单价 { get; set; }
        public string 工艺类别 { get; set; }
    }
}