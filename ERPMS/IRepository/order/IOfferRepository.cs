using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.order
{
    /// <summary>
    /// T_OMS_业务报价单_Master
    /// </summary>
    public interface IOfferRepository : IBaseRepository<T_OMS_业务报价单_Master>
    {
        int ExecProc2(ObjectParameter par, T_OMS_业务报价单_Master master);
        
    }
}