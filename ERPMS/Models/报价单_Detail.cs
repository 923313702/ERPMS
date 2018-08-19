using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 报价单_Detail
    {

        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 纸张编码 { get; set; }
        public string 纸张名称 { get; set; }
        public string 上版方式 { get; set; }
        public string 计量单位 { get; set; }
        public Nullable<int> 正面色数 { get; set; }
        public Nullable<int> 背面色数 { get; set; }
        public Nullable<int> 上机开数 { get; set; }
        public string 开料尺寸 { get; set; }
        public Nullable<decimal> 版数 { get; set; }
        public Nullable<decimal> 贴数 { get; set; }
        public Nullable<decimal> 正用数量 { get; set; }
        public Nullable<decimal> 印刷加放 { get; set; }
        public Nullable<decimal> 后道加放 { get; set; }
        public Nullable<decimal> 合计张 { get; set; }
        public Nullable<decimal> 拼数 { get; set; }
    }
}