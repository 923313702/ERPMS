using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_PDMS_物流申请单_Detail 接口
    /// </summary>
    public interface IOrderlogisticsDetailRepository : IBaseRepository<T_PDMS_物流申请单_Detail>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);

    }
}