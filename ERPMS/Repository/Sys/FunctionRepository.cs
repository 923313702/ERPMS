using ERPMS.IRepository.Sys;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ERPMS.Repository.Sys
{
    public class FunctionRepository : BaseRepository<Table_功能编码表>, IFunctionRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}