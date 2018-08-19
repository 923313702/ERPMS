using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class V_DesignSheetList
    {
        public string 订单号 { get; set; }
        public string 订单名称 { get; set; }
        public DateTime? 定稿日期 { get; set; }
        public string 客户名称 { get; set; }
        public DateTime? 制单日期 { get; set; }
        public string 业务员 { get; set; }
        public string 制单人 { get; set; }
        public string 审核人 { get; set; }

    }
}