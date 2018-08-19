using ERPMS.Models;
using ERPMS.Repository.order;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_订单_Detail 接口
    /// </summary>
    public interface IOrderDetailRepository : IBaseRepository<T_OMS_订单_Detail>
    {
        int ExecProc2(ObjectParameter par, T_OMS_订单_Detail order);
        void ExecuteTran(T_OMS_订单_Detail detail, T_OMS_订单_Detail_计价 price);
    }
}