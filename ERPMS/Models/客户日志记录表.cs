using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 客户日志记录表
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> 订单日期 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public string 制单人编码 { get; set; }
        public string 手工单号 { get; set; }
        public string 业务员编码 { get; set; }
        public string 客户编码 { get; set; }
        public string 订单说明 { get; set; }
        public string 发货说明 { get; set; }
        public string 结余说明 { get; set; }
        public string 办款说明 { get; set; }
        public Nullable<int> 确认标识 { get; set; }
        public string 确认人编码 { get; set; }
        public Nullable<System.DateTime> 确认日期 { get; set; }
        public string 确认说明 { get; set; }
        public string 单据类型 { get; set; }
        public string 客户名称 { get; set; }
    }
}