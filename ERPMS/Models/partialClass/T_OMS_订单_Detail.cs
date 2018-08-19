using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Models
{
    public partial class T_OMS_订单_Detail
    {
        public T_OMS_订单_Detail( string orderNo,string part ,string customer,string paperNo ,int?positive,int?opposite,decimal? banshu ,
            string shangban,decimal?pinshu,decimal?tieshu,int?shangjikaishu,decimal?zhengyong,decimal?yJiafang,
            decimal?houdaojia,int?fenqie,string jiqie)
        {
            this.订单号 = orderNo;
            this.印品部件 = part;
            this.纸张客户编码 = customer;
            this.纸张编码 = paperNo;
            this.正面色数 = positive;
            this.背面色数 = opposite;
            this.版数 = banshu;
            this.上版方式 = shangban;
            this.拼数 = pinshu;
            this.贴数 = tieshu;
            this.上机开数 = shangjikaishu;
            this.正用数量 = zhengyong;
            this.印刷加放 = yJiafang;
            this.后道加放 = houdaojia;
            this.分切数量 = fenqie;
            this.机切尺寸 = jiqie;
            this.T_OMS_订单_Detail_计价 = new HashSet<T_OMS_订单_Detail_计价>();
            this.T_OMS_订单_Detail_工艺=new HashSet<T_OMS_订单_Detail_工艺>();
            this.T_OMS_订单_Detail_材料=new HashSet<T_OMS_订单_Detail_材料>();
        }
    }
}