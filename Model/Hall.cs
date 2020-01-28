using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Hall
    {
        [Key]
        public int HallID { get; set; }
        public int rowsCount { get; set; }
        public int colsCount { get; set; }
    }
}
