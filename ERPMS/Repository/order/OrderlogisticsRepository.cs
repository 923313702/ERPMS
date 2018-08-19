using ERPMS.IRepository.order;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.order
{
    public class OrderlogisticsRepository : BaseRepository<T_PDMS_物流申请单_Master>, IOrderlogisticsRepository
    {
        public int ExecProc2(ObjectParameter par, T_PDMS_物流申请单_Master master)
        {
            return db.p_Insert_Logistics_Master(par, master.物流公司编码, master.日期, master.摘要, master.制单人编码,
                master.制单日期, master.审核人编码, master.审核日期, master.到货地址, master.联系方式, master.到货地址1,
                master.联系方式1, master.到货地址2, master.联系方式2, master.发货人, master.物流经办人, master.物流方式, master.结账标志);
        }
    }
}