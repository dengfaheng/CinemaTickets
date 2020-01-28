using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CinemaDbContext : DbContext
    {
        public static CinemaDbContext CDbContext = new CinemaDbContext();

        public CinemaDbContext() : base("data source=.\\SQLSERVER1;initial catalog=Cinema;Integrated Security=SSPI;") //构造函数，指定数据库名称的约定连接
        {
            //Code first会在第一次ef查询的时候会对__MigrationHistory访问，是为了检查数据库和model是否匹配，以保证ef能正常运行
            Database.SetInitializer<CinemaDbContext>(null);
        }

        //public BookDbContext() : base("Data Source=.;Initial Catalog=Students;Integrated Security=SSPI;") { } 

        //DbSet是一个模版类，<>中代表的是模版类中的实体类
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //取消复数表名惯例
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
        
    }
}
