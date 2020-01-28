using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
    
}
