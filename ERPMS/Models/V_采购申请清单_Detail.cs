using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class V_采购申请清单_Detail
    {
        public int 序号 { get; set; }
        public string 申请单号 { get; set; }
        public string 材料名称 { get; set; }
        public string 规格 { get; set; }
        public string 计量单位 { get; set; }
        public string 客户编码 { get; set; }
        public Nullable<decimal> V_库存数量 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 订单号 { get; set; }
        public string 备注 { get; set; }
        public string 材料编码 { get; set; }
        public Nullable<int> Flag { get; set; }
    }
}