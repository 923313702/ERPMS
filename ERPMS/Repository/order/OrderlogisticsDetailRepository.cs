using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class OrderlogisticsDetailRepository : BaseRepository<T_PDMS_物流申请单_Detail>, IOrderlogisticsDetailRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}