using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }
        public int MovieID { get; set; }
        public int HallID { get; set; }
        public int Price { get; set; }
        public string DateTime { get; set; } 
    }
}
