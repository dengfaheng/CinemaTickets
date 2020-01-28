using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TicketDAL
    {
        public static List<Ticket> GetAllTickets()
        {
            var TicketsQuery = from t in CinemaDbContext.CDbContext.Tickets
                              select t;
            return TicketsQuery.ToList();
        }

        public static List<Ticket> GetTicketsByScheduleID(int sID)
        {
            var TicketsQuery = from t in CinemaDbContext.CDbContext.Tickets
                               where t.ScheduleID == sID
                               select t;
            return TicketsQuery.ToList();
        }

        public static bool AddTickets(List<Ticket> tickets)
        {
            foreach (Ticket t in tickets)
            {
                CinemaDbContext.CDbContext.Tickets.Add(t);
                //CinemaDbContext.CDbContext.Entry(t).State = EntityState.Modified;
            }
            return CinemaDbContext.CDbContext.SaveChanges() > 0;
        }

    }
}
