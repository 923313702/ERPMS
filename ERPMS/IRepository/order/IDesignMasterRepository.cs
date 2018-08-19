using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_MCMS_设计订单_Master 接口
    /// </summary>
    public interface IDesignMasterRepository : IBaseRepository<T_MCMS_设计订单_Master>
    {
        int ExecProc2(ObjectParameter par, T_MCMS_设计订单_Master master);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}