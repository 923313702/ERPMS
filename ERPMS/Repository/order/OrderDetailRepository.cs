using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace ERPMS.Repository.order
{
    public class OrderDetailRepository : BaseRepository<T_OMS_订单_Detail>, IOrderDetailRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_订单_Detail order)
        {
            return db.InsertOrderDetailToDetailPrice(order.分切数量, order.纸张编码, order.行号, order.订单号, order.印品部件, order.正用数量, order.后道加放, order.印刷加放, order.机切尺寸);
        }

        public void ExecuteTran(T_OMS_订单_Detail detail, T_OMS_订单_Detail_计价 price)
        {
           
            using (var tran = db.Database.BeginTransaction())
            {
                // do something...
                try
                {
                    db.T_OMS_订单_Detail.Add(detail);
                    db.T_OMS_订单_Detail_计价.Add(price);
                    db.SaveChanges();
                    tran.Commit();

                }
                catch (Exception)
                {

                    tran.Rollback(); ;
                }
            }
           
        }
    }
}