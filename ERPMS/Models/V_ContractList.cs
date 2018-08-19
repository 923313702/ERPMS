using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class V_ContractList
    {
        public string 合同号 { get; set; }
        public DateTime? 制单日期 { get; set; }
        public string 客户名称 { get; set; }
        public DateTime? 交货日期 { get; set; }
        public decimal? 金额 { get; set; }
        public string 评审单号 { get; set; }
        public string 客户法定人 { get; set; }
        
        public DateTime? 审核日期 { get; set; }
        public string  审核人 { get; set; }


    }
    public class V_ContractList2
    {
        public string 合同号 { get; set; }
        public DateTime? 签订日期 { get; set; }
        public string 客户名称 { get; set; }
        public string 客户法定人 { get; set; }
        public DateTime? 交货日期 { get; set; }
        public string 评审单号 { get; set; }
        public decimal? 合同金额 { get; set; }

        public string 审核人 { get; set; }

        public DateTime? 审核日期 { get; set; }



    }
}