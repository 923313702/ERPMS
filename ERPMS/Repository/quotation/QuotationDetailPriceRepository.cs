using ERPMS.IRepository.quotation;
using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPMS.Repository.quotation
{
    public class QuotationDetailPriceRepository : BaseRepository<T_OMS_报价单_Detail_计价>, IQuotationDetailPriceRepository
    {
        public IEnumerable<TEntity> SqlQuery2<TEntity>(string sql, params object[] parms)
        {
            return db.Database.SqlQuery<TEntity>(sql, parms);
        }

        public IEnumerable<TEntity> SqlQuery3<TEntity>(string orderNo, int? detailNo, int? processNo, int? material)
        {
            List<object> pars = new List<object>();
            pars.Add(new SqlParameter("@orderNo", orderNo));
            string str = "select   印品部件 , 工艺类别 , 项目编码 , 计量单位, 系数 = 1, 数量, 金额 , 标准单价 ,统计编码, Flag from T_OMS_报价单_Detail_计价 where 1=1 and 订单号= @orderNo ";
            if (detailNo!=null)
            {
                str += " and 报价Detail号=@detailNo ";
                 pars.Add(new SqlParameter("@detailNo", detailNo));
            }
            if (processNo!=null)
            {
                str += " and 报价工艺号= @processNo ";
                pars.Add(new SqlParameter("@processNo", processNo));
            }
            if (material!=null)
            {
                str += " and 报价材料号=@material";
                pars.Add(new SqlParameter("@material", material));
            }
            return db.Database.SqlQuery<TEntity>(str, pars.ToArray());

        }
    }
}