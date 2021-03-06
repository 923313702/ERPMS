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
    
    public partial class T_OMS_合同_Master
    {
        public string 合同号 { get; set; }
        public string 客户编码 { get; set; }
        public string 客户法定人 { get; set; }
        public string 业务员编码 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public string 订单号 { get; set; }
        public string 合同内容 { get; set; }
        public string 工艺要求 { get; set; }
        public Nullable<System.DateTime> 交货日期 { get; set; }
        public string 交货方式 { get; set; }
        public string 付款方式 { get; set; }
        public string 制单人编码 { get; set; }
        public Nullable<System.DateTime> 制单日期 { get; set; }
        public Nullable<System.DateTime> 签定日期 { get; set; }
        public string 审核人编码 { get; set; }
        public Nullable<System.DateTime> 审核日期 { get; set; }
        public string 补充条款 { get; set; }
        public string 评审单号 { get; set; }
        public string 执行状态 { get; set; }
        public string provider { get; set; }
        public string buyer { get; set; }
    
        public virtual T_OMS_合同_Detail_工艺 T_OMS_合同_Detail_工艺 { get; set; }
    }
}
