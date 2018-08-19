using ERPMS.IRepository.Basics;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.basics
{
    public class CustomerRepository : BaseRepository<T_OMS_客户编码表>, ICustomerRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}