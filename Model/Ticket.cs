using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public int ScheduleID { get; set; }
        public string DetailSeat { get; set; }

        public Ticket() { }

        public Ticket(int sid, string ds)
        {
            this.ScheduleID = sid;
            this.DetailSeat = ds;
        }
    }
}
