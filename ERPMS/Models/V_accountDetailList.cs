using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    /// <summary>
    /// 客户对账单清单
    /// </summary>
    public class V_accountDetailList
    {
    
        public string 对账单号 { get; set; }
        public string 对账日期 { get; set; }
        public string 客户名称 { get; set; }
        public string 订单号 { get; set; }
        public string 制单日期 { get; set; }
        public string 订单名称 { get; set; }
        public string 计量单位 { get; set; }
        public decimal? 成品数量 { get; set; }
        public decimal? 金额 { get; set; }
        public string  备注 { get; set; }
        public string  制单人 { get; set; }
        public string 审核人 { get; set; }
      
    }

    public class V_customer_logDetail
    {
        public DateTime? 订单日期 { get; set; }
        public DateTime? 制单日期 { get; set; }
        public DateTime? 确认日期 { get; set; }
        public string 手工单号 { get; set; }
        public string 订单说明 { get; set; }
        public string 客户名称 { get; set; }
        public string 发货说明 { get; set; }
        public string 结余说明 { get; set; }
        public string 办款说明 { get; set; }
        public string 确认说明 { get; set; }
        public string 制单人 { get; set; }
        public string 确认人 { get; set; }

    }
}