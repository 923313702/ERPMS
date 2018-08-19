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
    public class OrderRepository : BaseRepository<T_OMS_订单_Master>, IOrderRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_订单_Master order)
        {

            return db.Insert_OrderData(par, order.订单名称, order.制单日期, order.业务员编码, order.客户编码.ToString(), order.联系方式, order.成品数量, order.客户单号,
                   order.成品尺寸, order.印品类别, order.计量单位, order.专色色别, order.交货日期, order.总P数, order.样稿类型,
                  order.样稿随货带走, order.留版标识, order.送货联系方式, order.制单人编码, order.客户盯色, order.加工要求, order.变更说明, order.包装材料, order.客户地址,order.是否开票);
        }

        public bool ExecuteSqlCommand(string sql, params SqlParameter[] parms)
        {
      
            return db.Database.ExecuteSqlCommand(sql, parms) > 0;
           
        }

        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
    
}