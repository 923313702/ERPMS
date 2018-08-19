using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 订单_Detail_材料
    {
        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 材料来源 { get; set; }
        //public string 材料类别编码 { get; set; }
        public string 材料编码 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        //public Nullable<decimal> 仓库数量 { get; set; }
        //public Nullable<decimal> 请购数量 { get; set; }
        // public Nullable<decimal> 单价 { get; set; }
        //public Nullable<decimal> 金额 { get; set; }
        // public string 备注 { get; set; }
        //public string 变更单号 { get; set; }
        //public string 项目编码 { get; set; }
        public string 计量单位 { get; set; }

        public string 材料名称 { get; set; }
        public string 规格型号 { get; set; }
    }
}