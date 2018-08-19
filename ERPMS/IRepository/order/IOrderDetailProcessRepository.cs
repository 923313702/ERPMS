using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_订单_Detail_工艺 接口
    /// </summary>
    public interface IOrderDetailProcessRepository : IBaseRepository<T_OMS_订单_Detail_工艺>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);

    }
}