using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_订单_发货申请单
    /// </summary>
    public interface IOrderInvoiceRepository : IBaseRepository<T_OMS_订单_发货申请单>
    {
        int ExecProc2(ObjectParameter par, T_OMS_订单_发货申请单 order);
    }
}