using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_订单_Detail_计价 接口
    /// </summary>
    public interface IOrderDetailPriceRepository:IBaseRepository<T_OMS_订单_Detail_计价>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);
    }
}