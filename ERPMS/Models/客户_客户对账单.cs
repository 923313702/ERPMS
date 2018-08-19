using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 客户_客户对账单
    {
        public System.DateTime 开始日期 { get; set; }
        public System.DateTime 结束日期 { get; set; }
        public Nullable<System.DateTime> 日期 { get; set; }
        public string 订单号 { get; set; }
        public string 订单名称 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public string 客户编码 { get; set; }
        public string 业务员编码 { get; set; }
        public Nullable<decimal> 成品数量 { get; set; }
        public string 计量单位 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public string 备注 { get; set; }
    
    }
}