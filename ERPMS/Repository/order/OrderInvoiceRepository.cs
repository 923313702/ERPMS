using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class OrderInvoiceRepository : BaseRepository<T_OMS_订单_发货申请单>, IOrderInvoiceRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_订单_发货申请单 order)
        {

            return db.Insert_Invoice(par, order.日期, order.订单号, order.申请人编码,
                order.客户编码, order.业务员编码, order.发货地址, order.发货地址二, order.发货地址三, order.联系方式, order.联系方式二, order.联系方式三, order.印品名称, order.数量, order.发货数量, order.备注, order.制单人编码, order.制单日期, order.发货人编码);
        }
    }
}