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
    /// T_PHMS_采购申请单_Master 接口
    /// </summary>
    public interface IPurchaseApplyRepository : IBaseRepository<T_PHMS_采购申请单_Master>
    {
        int ExecProc2(ObjectParameter par, T_PHMS_采购申请单_Master master);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);

    }
}