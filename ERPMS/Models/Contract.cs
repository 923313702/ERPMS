using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class Contract
    {
        public string CustomerName { get; set; }
        public string SaleMan { get; set; }
        public decimal? Money { get; set; }
        public string  Content { get; set; }
        public string  ContractNo { get; set; }
        public string Buyer { get; set; }
        public string Provider { get; set; }
        public string Ask { get; set; }
        public string Remark { get; set; }
    }
}