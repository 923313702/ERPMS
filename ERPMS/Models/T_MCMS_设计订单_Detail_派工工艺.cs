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
    
    public partial class T_MCMS_设计订单_Detail_派工工艺
    {
        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 项目编码 { get; set; }
        public string 系数编码 { get; set; }
        public string 人员编码 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public Nullable<int> 完成数量 { get; set; }
        public Nullable<System.DateTime> 完工时间 { get; set; }
    }
}
