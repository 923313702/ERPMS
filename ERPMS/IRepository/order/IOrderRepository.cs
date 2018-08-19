using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_订单_Master 接口
    /// </summary>
    public interface IOrderRepository : IBaseRepository<T_OMS_订单_Master>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
        int ExecProc2(ObjectParameter par, T_OMS_订单_Master order);
        bool ExecuteSqlCommand(string sql, params SqlParameter[] parms);

    }
}