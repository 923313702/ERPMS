using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace ERPMS.IRepository.Sys
{
    /// <summary>
    /// Table_功能编码表 接口
    /// </summary>
    public interface IFunctionRepository:IBaseRepository<Table_功能编码表>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms);
    }
}