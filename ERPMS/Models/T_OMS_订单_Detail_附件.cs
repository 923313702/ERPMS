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
    
    public partial class T_OMS_订单_Detail_附件
    {
        public int 行号 { get; set; }
        public string 订单号 { get; set; }
        public string 版号 { get; set; }
        public string 名称 { get; set; }
        public Nullable<int> 拼数 { get; set; }
        public Nullable<int> 成品数 { get; set; }
        public string 用料尺寸 { get; set; }
        public string 用料数量 { get; set; }
        public Nullable<decimal> 单价 { get; set; }
        public Nullable<decimal> 金额 { get; set; }
        public Nullable<int> 入库完成 { get; set; }
    }
}
