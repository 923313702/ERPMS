using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class DesignMasterRepository : BaseRepository<T_MCMS_设计订单_Master>, IDesignMasterRepository
    {
        public int ExecProc2(ObjectParameter par, T_MCMS_设计订单_Master m)
        {
            return db.insert_Design_Master(par, m.业务单号, m.印品类别编码, m.订单名称,
                m.客户编码, m.业务员编码, m.联系方式, m.页数, m.交稿日期,
                m.定稿日期, m.成品数量, m.展开尺寸, m.成品尺寸, m.开数编码,
                m.加工要求, m.备注, m.制单人编码, m.制单日期);
        }
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}