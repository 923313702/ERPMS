using ERPMS.IRepository.quotation;
using ERPMS.Models;
using ERPMS.Repository.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.quotation
{
    public class QuotationDetailRepository : BaseRepository<T_OMS_报价单_Detail>, IQuotationDetailRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}