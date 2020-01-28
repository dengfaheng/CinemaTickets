using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;

namespace BLL
{
    public class CustomerBLL
    {
        public static bool Login(Customer c)
        {
            if (CustomerDAL.CheckLogin(c).Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool Register(Customer c)
        {
            if (CustomerDAL.AddCustomer(c))
            {
                return true;
            }
            return false;
        }
    }
}
