using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.quotation
{
    public interface IQuotationDetailRepository:IBaseRepository<T_OMS_报价单_Detail>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}