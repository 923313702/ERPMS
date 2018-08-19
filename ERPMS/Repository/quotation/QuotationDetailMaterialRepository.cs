using ERPMS.IRepository.quotation;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPMS.IRepository;
using System.Linq.Expressions;

namespace ERPMS.Repository.quotation
{
    public class QuotationDetailMaterialRepository : BaseRepository<T_OMS_报价单_Detail_材料>, IQuotationDetailMaterialRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}