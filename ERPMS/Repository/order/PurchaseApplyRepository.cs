using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class PurchaseApplyRepository : BaseRepository<T_PHMS_采购申请单_Master>, IPurchaseApplyRepository
    {
        public int ExecProc2(ObjectParameter par, T_PHMS_采购申请单_Master master)
        {
            return db.Insert_Purchase_trigger(par, master.订单号, master.单据类型, master.部门编码, master.日期, master.摘要, master.物料类别, master.制单人编码, master.申请人, master.负责人, master.类别, master.会计期);
        }

        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params SqlParameter[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}