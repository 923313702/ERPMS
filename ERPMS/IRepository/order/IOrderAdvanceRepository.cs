using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    public interface IOrderAdvanceRepository:IBaseRepository<T_OMS_订单_预收款凭单>
    {
        int ExecProc2(ObjectParameter par, T_OMS_订单_预收款凭单 advance);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}