//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERPMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_MIMS_纸张期初库存表
    {
        public int RowID { get; set; }
        public string 纸张仓库编码 { get; set; }
        public string 纸张库位编码 { get; set; }
        public string 纸张客户编码 { get; set; }
        public string 纸张编码 { get; set; }
        public Nullable<decimal> 数量令 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public string 会计期 { get; set; }
        public string 类别 { get; set; }
        public string 纸型 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public string 建帐日期 { get; set; }
        public Nullable<byte> 建帐完成标志 { get; set; }
        public string 确认人编码 { get; set; }
        public Nullable<System.DateTime> 确认时间 { get; set; }
        public string 批次 { get; set; }
    
        public virtual T_MIMS_纸张编码表 T_MIMS_纸张编码表 { get; set; }
    }
}
