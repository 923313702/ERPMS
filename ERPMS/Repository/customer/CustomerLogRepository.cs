using ERPMS.IRepository.customer;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace ERPMS.Repository.customer
{
    public class CustomerLogRepository : BaseRepository<T_OMS_客户日志记录表>, ICustomerLogRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}