using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace ERPMS.Repository.order
{
    public class OrderAdvanceRepository : BaseRepository<T_OMS_订单_预收款凭单>, IOrderAdvanceRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_订单_预收款凭单 advance)
        {
            return db.insert_payment_proc(par ,advance.订单号,advance.订单名称,advance.客户编码,advance.业务员编码,advance.成品数量,advance.计量单位,advance.成品尺寸, advance.金额,advance.制单日期,advance.送货联系方式,advance.预收金额,advance.交货日期,advance.制单人编码,advance.提货说明,advance.应收金额,advance.结款方式);
        }

        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms); 
        }
    }
}