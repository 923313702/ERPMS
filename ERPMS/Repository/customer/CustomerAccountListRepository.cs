using ERPMS.IRepository.customer;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.customer
{
    public class CustomerAccountListRepository : BaseRepository<T_OMS_客户_客户对账单>, ICustomerAccountListRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}