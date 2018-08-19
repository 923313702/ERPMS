using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.contract
{
    /// <summary>
    /// T_OMS_合同_Master 接口
    /// </summary>
    public interface IContractRepository:IBaseRepository<T_OMS_合同_Master>
    {
        int ExecProc2(ObjectParameter par, T_OMS_合同_Master master);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}