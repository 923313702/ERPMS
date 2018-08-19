using ERPMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPMS.Util
{
    public class ObjectEquality
    {
     
    }
    public class PostEquality : IEqualityComparer<Table_功能岗位编码表>
    {
        public bool Equals(Table_功能岗位编码表 x, Table_功能岗位编码表 y)
        {
            bool flag = false;
            if (x.岗位编码 == y.岗位编码 && x.功能编码 == y.功能编码)
            {
                flag = true;
            }
            return flag;
        }
        public int GetHashCode(Table_功能岗位编码表 obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().GetHashCode();
        }
    }

    public class AccountListEquality : IEqualityComparer<T_OMS_客户_客户对账单>
    {
        public bool Equals(T_OMS_客户_客户对账单 x, T_OMS_客户_客户对账单 y)
        {
            bool flag = false;
            if (x.订单号 == y.订单号 )
            {
                flag = true;
            }
            return flag;
        }
        public int GetHashCode(T_OMS_客户_客户对账单 obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.ToString().GetHashCode();
        }
    }

}