using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.Basics
{
    /// <summary>
    /// 客户编码表 接口
    /// </summary>
    public interface ICustomerRepository : IBaseRepository<T_OMS_客户编码表>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);
    }
}