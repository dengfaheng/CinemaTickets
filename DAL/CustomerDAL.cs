using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public class CustomerDAL
    {
        public static List<Customer> CheckLogin(Customer c)
        {
            var CustomersQuery = from b in CinemaDbContext.CDbContext.Customers
                                 where b.UserName == c.UserName && b.PassWord == c.PassWord
                                 select b;
            List<Customer> CustomersList = CustomersQuery.ToList();
            return CustomersList;
        }
        public static bool AddCustomer(Customer c)
        {
            CinemaDbContext.CDbContext.Customers.Add(c);            
            return CinemaDbContext.CDbContext.SaveChanges() > 0;
        }


    }
}
