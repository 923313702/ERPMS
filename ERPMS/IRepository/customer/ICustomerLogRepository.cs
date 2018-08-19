using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.customer
{
    /// <summary>
    /// T_OMS_客户日志记录表 接口
    /// </summary>
    public interface ICustomerLogRepository:IBaseRepository<T_OMS_客户日志记录表>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}