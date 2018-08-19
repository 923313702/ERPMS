using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public partial class T_OMS_订单_Master
    {
        public T_OMS_订单_Master(string orderName,string saleMan,string customerNo,
            string tel,int? number,string size,string type,
            string ask,int? isTicket,string personNo,DateTime date)
        {
            this.订单名称 = orderName;
            this.业务员编码 = saleMan;
            this.客户编码 = customerNo;
            this.联系方式 = tel;
            this.成品数量 = number;
            this.成品尺寸 = size;
            this.印品类别编码 = type;
            this.加工要求 = ask;
            this.是否开票 = isTicket;
            this.制单人编码 = personNo;
            this.制单日期 = date;
        }

    }
}