using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public class 工艺项目编码表
    {

        public string 项目编码 { get; set; }
        public string 项目名称 { get; set; }
        public string 工艺类别 { get; set; }
        // public string 上级编码 { get; set; }
        // public string 统计编码 { get; set; }
        public string 统计编码 { get; set; }
        public string 计量单位 { get; set; }

        //  public Nullable<int> 排序码 { get; set; }
        public Nullable<int> 报价标识 { get; set; }
        public Nullable<int> 工艺标识 { get; set; }

        public Nullable<decimal> 换单金额 { get; set; }
        public Nullable<decimal> 内部单价 { get; set; }
        public string 部门编码 { get; set; }
        public string 班组编码 { get; set; }

        public Nullable<decimal> 小时产能 { get; set; }
        public string 机台编码 { get; set; }
        //  public string 相关工艺 { get; set; }
        // public Nullable<decimal> 计件工价 { get; set; }
        // public Nullable<int> 统计标识 { get; set; }
        public Nullable<decimal> 销售单价 { get; set; }
        // public string 备注 { get; set; }
        // public Nullable<int> 选择 { get; set; }
        // public Nullable<int> 开数编码 { get; set; }
        // public Nullable<decimal> 换贴金额 { get; set; }
        // public Nullable<int> 工艺排序码 { get; set; }
        public string 计数标识 { get; set; }
    }


    public class 工艺项目编码表2
    {

        public string 项目编码 { get; set; }
        public string 项目名称 { get; set; }
        public string 工艺类别 { get; set; }

        public string 统计名称 { get; set; }
        public string 计量 { get; set; }
        public string 计数 { get; set; }
        public Nullable<int> 报价 { get; set; }
        public Nullable<int> 工艺 { get; set; }
        public Nullable<decimal> 换单金额 { get; set; }
        public Nullable<decimal> 内部单价 { get; set; }
        public Nullable<decimal> 销售单价 { get; set; }
        public string 部门 { get; set; }
        public string 班组 { get; set; }

        public Nullable<decimal> 小时产能 { get; set; }
        public string 加工机台 { get; set; }




    }
}