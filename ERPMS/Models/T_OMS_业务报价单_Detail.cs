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
    
    public partial class T_OMS_业务报价单_Detail
    {
        public int 行号 { get; set; }
        public string 报价单号 { get; set; }
        public string 印品部件 { get; set; }
        public string 纸张客户编码 { get; set; }
        public string 纸张编码 { get; set; }
        public string 项目编码 { get; set; }
        public string 变更单号 { get; set; }
        public Nullable<int> 不印刷 { get; set; }
        public Nullable<int> 长 { get; set; }
        public Nullable<int> 宽 { get; set; }
        public string 计量单位 { get; set; }
        public Nullable<decimal> 成品数 { get; set; }
    }
}
