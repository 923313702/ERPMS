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
    
    public partial class T_PHMS_采购申请单_Detail
    {
        public int 序号 { get; set; }
        public string 申请单号 { get; set; }
        public string 材料名称 { get; set; }
        public string 规格 { get; set; }
        public string 计量单位 { get; set; }
        public Nullable<decimal> 数量_令 { get; set; }
        public Nullable<decimal> 库存数量 { get; set; }
        public Nullable<decimal> 数量 { get; set; }
        public string 订单号 { get; set; }
        public string 备注 { get; set; }
        public string 类别 { get; set; }
        public Nullable<System.DateTime> 到货日期 { get; set; }
        public Nullable<int> 采购标识 { get; set; }
        public string 物料类别 { get; set; }
        public string 材料编码 { get; set; }
        public Nullable<int> 申请选择 { get; set; }
        public Nullable<int> 选择 { get; set; }
        public string 入库单号 { get; set; }
        public Nullable<int> Flag { get; set; }
    }
}
