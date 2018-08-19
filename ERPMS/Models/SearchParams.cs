using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class SearchParams
    {
        public int page { get; set; }
        public int  rows { get; set; }
        public string  orderNo { get; set; }
        public string orderName { get; set; }
        public string  customerNo { get; set; }
        public string printCategory { get; set; }
        public DateTime? zhidanStart { get; set; }
        public DateTime? zhidanEnd { get; set; }
        public DateTime? deliveryStart { get; set; }
        public DateTime? deliveryEnd { get; set; }
        public string saleMan { get; set; }
        public string remark { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>
        public string contractNo { get; set; }

        //--------------------------- 客户对账单用的不想干程序加了属性-----
        public string 客户编码 { get; set; }
        public DateTime 开始日期 { get; set; }
        public DateTime 结束日期 { get; set; }
    }
}