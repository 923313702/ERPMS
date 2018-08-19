using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class V_QuotationList
    {
        public V_QuotationList()
        {
            var bao = 包装费率 == null ? 0 : 包装费率;
            var kai = 开票税率 == null ? 0 : 开票税率;
            var yun = 运输费率 == null ? 0 : 运输费率;
            var cang = 仓贮费率 == null ? 0 : 仓贮费率;
            金额 = 金额 + 金额 * bao + 金额 * kai + 金额 * yun + 金额 * cang;

        }
        public string 订单号 { get; set; }
        public string 订单名称 { get; set; }
        public string 业务单号 { get; set; }
        public DateTime? 制单日期 { get; set; }

        public Nullable<decimal> 包装费率 { get; set; }
        public Nullable<decimal> 开票税率 { get; set; }

        public Nullable<decimal> 运输费率 { get; set; }

        public Nullable<decimal> 仓贮费率 { get; set; }
        public string 客户名称 { get; set; }
        public string 业务员 { get; set; }
        public string 审核人 { get; set; }

        public Nullable<decimal> 金额 { get; set; }
    }
}


    