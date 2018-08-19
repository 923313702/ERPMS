using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class OfferRepository : BaseRepository<T_OMS_业务报价单_Master>, IOfferRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_业务报价单_Master master)
        {
            return db.Insert_OrderPrice_Offer(par, master.订单号, master.订单名称, master.客户编码, master.业务员编码,
                master.合同编号, master.印张, master.成品数量, master.计量单位, master.成品尺寸, master.开数编码,
                master.装订方法, master.制单日期);
        }

      
    }
}