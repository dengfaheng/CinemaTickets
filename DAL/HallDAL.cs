using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HallDAL
    {
        public static int GetRowsCount(int TheHallID)
        {
            Hall h = CinemaDbContext.CDbContext.Halls.Find(TheHallID);
            if(h == null)
            {
                return -1;
            }
            return h.rowsCount;
        }

        public static int GetColsCount(int TheHallID)
        {
            Hall h = CinemaDbContext.CDbContext.Halls.Find(TheHallID);
            if (h == null)
            {
                return -1;
            }
            return h.colsCount;
        }

        public static List<Hall> GetAllHalls()
        {
            var HallQuery = from h in CinemaDbContext.CDbContext.Halls
                            select h;
            return HallQuery.ToList();
        }
    }
}
