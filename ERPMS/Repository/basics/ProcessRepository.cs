using ERPMS.IRepository.Basics;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ERPMS.Repository.basics
{
    public class ProcessRepository : BaseRepository<T_OMS_工艺项目编码表>, IProcessRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }

    }
}