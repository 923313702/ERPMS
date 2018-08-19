using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.IRepository.quotation
{
    /// <summary>
    /// T_OMS_报价单_Detail_材料 接口
    /// </summary>
    public interface IQuotationDetailMaterialRepository:IBaseRepository <T_OMS_报价单_Detail_材料>
    {
        IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms);
    }
}