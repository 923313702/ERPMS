using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.quotation
{
    /// <summary>
    /// T_OMS_报价单_Master
    /// </summary>
    public interface IQuotationMasterRepository:IBaseRepository<T_OMS_报价单_Master>
    {
        int ExecProc2(ObjectParameter par, T_OMS_报价单_Master master);
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);

    }
}