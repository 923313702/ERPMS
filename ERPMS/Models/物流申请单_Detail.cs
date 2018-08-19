using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 物流申请单_Detail
    {
        public int 行号 { get; set; }
        public string 申请单号 { get; set; }
        public string 项目分类 { get; set; }
        public string 订单号 { get; set; }
        public string 订单名称 { get; set; }
        public string 客户编码 { get; set; }
        public string 业务员编码 { get; set; }
        public Nullable<decimal> 进仓费 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public string 客户名称 { get; set; }
        public string 姓名 { get; set; }
    }
}