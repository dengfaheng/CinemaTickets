using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ScheduleDAL
    {
        public static List<Schedule> GetSchedulesByMovieID(int mID)
        {
            var SchedulesQuery = from s in CinemaDbContext.CDbContext.Schedules
                                 where s.MovieID == mID
                                 select s;
            return SchedulesQuery.ToList();
        }

        public static Schedule GetScheduleByScheduleID(int sID)
        {
            return CinemaDbContext.CDbContext.Schedules.Find(sID);
        }
    }
}
