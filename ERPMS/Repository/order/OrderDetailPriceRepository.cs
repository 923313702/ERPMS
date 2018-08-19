using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ERPMS.Repository.order
{
    public class OrderDetailPriceRepository : BaseRepository<T_OMS_订单_Detail_计价>, IOrderDetailPriceRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}