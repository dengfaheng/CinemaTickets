using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MovieDAL
    {
        public static List<Movie> GetAllMovies()
        {
            var MoviesQuery = from m in CinemaDbContext.CDbContext.Movies
                              select m;
            return MoviesQuery.ToList();
        }

        public static Movie GetMovieByMovieID(int mID)
        {
            return CinemaDbContext.CDbContext.Movies.Find(mID);
        }
    }
}
