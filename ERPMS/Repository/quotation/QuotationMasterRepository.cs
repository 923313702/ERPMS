using ERPMS.IRepository.quotation;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace ERPMS.Repository.quotation
{
    public class QuotationMasterRepository : BaseRepository<T_OMS_报价单_Master>, IQuotationMasterRepository
    {
        public int ExecProc2(ObjectParameter par, T_OMS_报价单_Master master)
        {
            return db.Insert_Quotation(par, master.订单名称, master.是否开票, master.客户编码, master.联系方式, master.成品数量, master.计量单位,
                master.业务员编码, master.印品类别编码, master.成品尺寸, master.纸张放率, master.合同编号, master.生产单号, master.包装费率,
                master.运输费率, master.仓贮费率, master.开票税率, master.内部说明, master.报价要求, master.制单人编码, master.制单日期, 
                master.烫金尺寸, master.烫金起步单价, master.UV尺寸, master.UV起步单价);
        }

        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }
    }
}