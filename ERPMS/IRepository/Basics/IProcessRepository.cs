using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.Basics
{
    /// <summary>
    /// T_OMS_工艺项目编码表接口
    /// </summary>
    public interface IProcessRepository:IBaseRepository<T_OMS_工艺项目编码表>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);
    }
}