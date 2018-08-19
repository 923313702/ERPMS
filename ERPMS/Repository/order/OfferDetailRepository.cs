﻿using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ERPMS.Repository.order
{
    public class OfferDetailRepository : BaseRepository<T_OMS_业务报价单_Detail>, IOfferDetailRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}