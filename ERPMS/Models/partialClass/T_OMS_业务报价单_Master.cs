using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public partial  class T_OMS_业务报价单_Master
    {
        public T_OMS_业务报价单_Master() { }
        public T_OMS_业务报价单_Master(string orderNo, string orderName, string customerNo, string saleMan,
            string contract, decimal? printNum, int? number, string unit, string size, int? open, string function, DateTime date
            )
        {
            this.订单号 = orderNo;
            this.订单名称 = orderName;
            this.客户编码 = customerNo;
            this.业务员编码 = saleMan;
            this.合同编号 = contract;
            this.印张 = printNum;
            this.成品数量 = number;
            this.计量单位 = unit;
            this.成品尺寸 = size;
            this.开数编码 = open;
            this.装订方法 = function;
            this.制单日期 = date;

        }
    }
}