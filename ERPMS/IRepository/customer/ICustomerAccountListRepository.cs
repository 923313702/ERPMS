using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.customer
{
    public interface ICustomerAccountListRepository:IBaseRepository<T_OMS_客户_客户对账单>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}