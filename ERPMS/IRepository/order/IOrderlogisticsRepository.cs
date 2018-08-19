using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_PDMS_物流申请单_Master 接口
    /// </summary>
    public interface IOrderlogisticsRepository : IBaseRepository<T_PDMS_物流申请单_Master>
    {
        int ExecProc2(ObjectParameter par, T_PDMS_物流申请单_Master order);
    }
}