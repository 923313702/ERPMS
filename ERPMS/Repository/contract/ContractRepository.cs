using ERPMS.IRepository.contract;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace ERPMS.Repository.contract
{
    public class ContractRepository : BaseRepository<T_OMS_合同_Master>, IContractRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_合同_Master master)
        {
            return db.Insert_Contract_proc(par, master.客户编码,master.制单人编码, master.制单日期, master.provider, master.buyer);
        }

        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}