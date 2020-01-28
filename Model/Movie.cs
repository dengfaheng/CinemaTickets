using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Movie
    {
        [Key]
        public int    MovieID { get; set; }
        public string MovieName{ get; set;  } /// 电影名称
        public string Actor { get; set; } /// 主演
        public string Director { get; set; }/// 导演名
        public int    Duration { get; set; } //时长
        public string MovieType { get; set; }/// 电影类型
        public string Poster{ get; set; } /// 海报图片名
    }
}
