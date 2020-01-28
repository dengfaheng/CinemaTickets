# 01 介绍
一个有会员制的电影院购票系统。具有会员注册功能，可区分会员和散客两种身份，实现会员及折扣管理。购票具有挑选电影场次，选择座位和查看电影信息等功能。

- 查看电影详情、获取排片信息。
- 选择场次座位，完成支付，获取取票信息。
- 注册成为影院会员，享受优惠折扣。

![](https://upload-images.jianshu.io/upload_images/10386940-ccca49013103e612.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


![](https://upload-images.jianshu.io/upload_images/10386940-5e98b3d7421fd2e7.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

![](https://upload-images.jianshu.io/upload_images/10386940-5117b6f7fc65dad3.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

![](https://upload-images.jianshu.io/upload_images/10386940-230f05b0d09e1e52.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

# 代码获取
**关注我们的公众号！在后台回复【CSTK】不包括【】即可获取。**



![](http://upload-images.jianshu.io/upload_images/10386940-ba0c519723650398.jpg?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


# 02 设计思路
在功能设计上，一个电影院购票系统，首先需要具备最基础的功能：影片选择、场次选择和座位选择。在用户提交选择后，会需要支付模块提示用户付款并完成出票。为了吸引用户，我们增加了会员的注册和登录模块，为会员用户提供折扣。

注册与购票的支付我们的处理是预留一个接口，当做简单模拟，实际使用可以调用支付宝或微信的支付接口。

在界面设计上，我们为系统添加了好看的背景图片。通过Detail栏展示用户信息与折扣，通过Hot Movie栏在最吸引眼球展示热映电影的海报，提高用户的购买欲望。最后，作为主要部分的座位选择栏简介明了，座位之间间隔明显，有效的防止用户错误操作。

# 03 具体设计
通过三层架构来完成影院购票系统的开发，将真个业务应用划分为：界面层（UI层）、业务逻辑层（BLL层）、数据访问层（DAL层）。对于复杂的系统分层让结构清晰，便于对系统进行整体的理解、把握；而且便于维护，将各部分之间的相互影响的程度降低到最小，系统基本的架构可以通过工具自动生成代码。当数据库发生改变时，只用重新生成代码，改动业务逻辑层的部分代码即可。在实施的过程中，难点在于将三层结构进行划分，掌握各层之间的设计思路以及调用关系，下面内容就结合代码展示具体实现过程。

**1) Model层（ 封装数据，使数据在三层中传输）**

例如Movie：
```C#
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
```

**2) DAL层（提供基本的数据访问）**

实现代码（以Movies为例）：
```C#
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
```
**3) BLL层（负责处理业务逻辑，在本次的系统开发中，包括了与用户和影票信息相关的处理）**

实现代码（以TicketBLL为例）：
```C#
namespace BLL
{
    public class TicketBLL
    {
        public static bool AddTickets(List<Ticket> tickets)
        {
            return true;
        }
    }
}

```

**4) UI层（负责显示和采集用户操作）**

系统总共包含五个界面，分别为：用户登录界面、用户注册界面、影院主页、票务信息确认界面以及支付界面。同时，使用Winform皮肤插件来实现对系统界面整体风格的把控。下面将以界面的为单位来对其实现过程进行描述：

- **用户登录界面**

用户将身份信息写入文本框后，用其输入的信息创建新的customer对象，通过调用BLL层的功能将输入内容与用户信息比对，最后用判断语句激活弹窗反馈登陆结果，登陆成功后进入到售票系统首页。

```C#
private void button1_Click(object sender, EventArgs e)
        {
            Customer cLogin = new Customer
            {
                UserName = this.textBox1.Text,
                PassWord = this.textBox2.Text
            };

            if (CustomerBLL.Login(cLogin))
            {
                MessageBox.Show("登录成功");
                MainForm mainForm = new MainForm();
                mainForm.customerVIP = cLogin;
                mainForm.Show();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }
```

同时，用户可以点击注册按钮，跳转到注册界面完成新用户的注册。
```C#
        private void button3_Click(object sender, EventArgs e)
        {
            UserRegisterForm userRegisterForm = new UserRegisterForm();
            userRegisterForm.Show();
            this.Visible = false;
        }
```


- **用户注册界面**

用户将身份信息写入文本框后，用其输入的信息创建新的customer对象，通过调用BLL层的服务将新的用户信息写入数据库，最后用判断语句激活弹窗对注册结果予以反馈。

```C#
private void button1_Click(object sender, EventArgs e)
        {
            PayForm payform = new PayForm(true);
            payform.ShowDialog();
            //Thread.Sleep(7000);
            //payform.Visible = false;

            Customer cRegister = new Customer
            {
                UserName = this.textBox1.Text,
                PassWord = this.textBox2.Text
            };


            if (CustomerBLL.Register(cRegister))
            {
                MessageBox.Show("注册成功");
                UserLoginForm userLoginForm = new UserLoginForm();
                userLoginForm.Show();
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("注册失败");
            }
        }
```
- **排片详情获取**
```C#
/// <summary>
        /// 初始化TreeView控件
        /// </summary>
        private void InitTreeView()
        {
            tvMovies.BeginUpdate();
            tvMovies.Nodes.Clear();

            TreeNode movieNode = null;
            foreach(Movie m in MovieDAL.GetAllMovies())
            {
                movieNode = new TreeNode(m.MovieName);
                tvMovies.Nodes.Add(movieNode);

                foreach (Schedule s in ScheduleDAL.GetSchedulesByMovieID(m.MovieID))
                {
                    TreeNode timeNode = new TreeNode(s.DateTime);
                    timeNode.Name = s.ScheduleID.ToString();
                    movieNode.Nodes.Add(timeNode);
                }
            }
            tvMovies.EndUpdate();
        }
```

- **影厅初始化**
```C#
 /// <summary> 
        /// 初始化放映厅座位
        /// </summary>
        /// <param name="seatRow">行数</param>
        /// <param name="seatCol">列数</param>
        /// <param name="tb"></param>
        //普通厅放映
        private void InitSeatsCheckBox(int seatRow, int seatCol, TabPage tb, Dictionary<string, CheckBox> ckBox, Dictionary<string, Seat> cSeats)
        {
            CheckBox checkBox;
            Seat seat;
            for (int i = 0; i < seatRow; i++)                   /* 加载一号放映厅的位置*/
            {
                for (int j = 0; j < seatCol; j++)
                {
                    checkBox = new CheckBox();
                    //设置背景颜色
                    checkBox.BackColor = Color.LightBlue;
                    //设置字体
                    checkBox.Font = font1;                                                                                                         //new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,((byte)(134)));
                    //设置尺寸
                    checkBox.AutoSize = false;
                    checkBox.Size = new System.Drawing.Size(60, 25);
                    //设置座位号
                    checkBox.Text = (j+1).ToString() + "-" + (i+1).ToString();
                    checkBox.TextAlign = ContentAlignment.MiddleCenter;
                    //设置位置
                    checkBox.Location = new Point(60 + (i * 90), 60 + (j * 60));
                    //所有的标签都绑定到同一事件
                    //checkBox.Click += new System.EventHandler(lblSeat_Click);
                    tb.Controls.Add(checkBox);
                    ckBox.Add(checkBox.Text, checkBox);
                    //实例化一个座位
                    seat = new Seat(checkBox.Text, Color.LightBlue);
                    //保存的座位集合
                    cSeats.Add(seat.SeatNum, seat);
                }
            }
            Label lb = new Label();
            lb.BackColor = Color.LightGray;
            lb.Font = font1;                                                                                                                                   //设置尺寸
            lb.AutoSize = false;
            lb.Size = new System.Drawing.Size(100, 25);
            //
            lb.Text = "荧幕中央";
            lb.TextAlign = ContentAlignment.MiddleCenter;
            //设置位置
            lb.Location = new Point(310, 20);
            tb.Controls.Add(lb);

        }
```
- **购票**
```C#
private void tvMovies_AfterSelect(object sender, TreeViewEventArgs e)
        {

        /// <summary>
        /// 选择一场电影事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
            TreeNode node = tvMovies.SelectedNode;
            if (node == null) return;
            if (node.Level != 1) return;
            int sID = int.Parse(node.Name);

            Schedule s = null;
            Movie m = null;
            s= ScheduleDAL.GetScheduleByScheduleID(sID);

            if (s == null)
            {
                MessageBox.Show("s should not be null");
                return;
            }
            m = MovieDAL.GetMovieByMovieID(s.MovieID);
            if (m == null)
            {
                MessageBox.Show("m should not be null");
                return;
            }

            //将详细信息显示
            this.lblMovieName.Text = m.MovieName;
            this.lblDirector.Text = m.Director;
            this.lblActor.Text = m.Actor;
            this.lblPrice.Text = s.Price.ToString();
            this.lblTime.Text = s.DateTime;
            this.lblType.Text = m.MovieType;
            this.picMovie.Image = Image.FromFile(m.Poster);

            if(this.customerVIP != null)
            {
                this.lblCalcPrice.Text = (s.Price * 0.8).ToString();
            }
            else
            {
                this.lblCalcPrice.Text = lblPrice.Text;
            }

            otherHallIDs.Clear();
            foreach (int hID in AllHallIDs)
            {
                if (hID == s.HallID)
                {
                    continue;
                }
                otherHallIDs.Add(hID);
            }

            //清空座位
            ReSetSeats(s.HallID);
            //遍历该场电影的座位销售情况
            foreach (Ticket t in TicketDAL.GetTicketsByScheduleID(sID))
            {
                foreach (Seat seat in seats[s.HallID].Values)
                {
                    if (t.DetailSeat == seat.SeatNum)
                    {
                        seat.Color = Color.LightCoral;
                    }
                }
            }
            UpdateSeats(s.HallID);
            tbSeat.SelectedTab = tbSeat.TabPages[s.HallID-1];
        }
```
- **购票信息确认界面**

```C#
        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.lblMovieName.Text))
            {
                MessageBox.Show("您还没选择电影!", "提示");
                return;
            }
            int hID = tbSeat.SelectedIndex + 1;

            TreeNode node = tvMovies.SelectedNode;
            if (node == null) return;
            if (node.Level != 1) return;
            int sID = int.Parse(node.Name);
            double totalCost = 0;
            List<Ticket> selectSeats = new List<Ticket>();
            string confireInfo = "您选择的电影票信息如下，请确认：\n\n";
            foreach(CheckBox cb in checkBoxs[hID].Values)
            {
                if (cb.Checked && cb.Enabled == true)
                {
                    selectSeats.Add(new Ticket(sID, cb.Text));
                    confireInfo += "电影名：" + lblMovieName.Text + " 场次：" + lblTime.Text + " 大厅：" + hID + " 座位："+cb.Text+"\n";
                    totalCost += double.Parse(lblCalcPrice.Text);
                }
            }
            if (selectSeats.Count < 1)
            {
                MessageBox.Show("您没有选择任何座位！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            confireInfo += "\n总金额：" + totalCost.ToString()+" 元";
            if (MessageBox.Show(confireInfo, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                PayIsSuccess = false;
                PayForm pf = new PayForm(false);
                pf.Owner = this;
                pf.payMoney = totalCost;
                pf.ShowDialog();
                //支付成功
                if (PayIsSuccess)
                {
                    //保存到数据库成功
                    if (TicketDAL.AddTickets(selectSeats))
                    {
                        //生成取票码

                        //更新位置信息
                        foreach (Ticket t in selectSeats)
                        {
                            Seat s = null;
                            if(seats[hID].TryGetValue(t.DetailSeat, out s))
                            {
                                s.Color = Color.LightCoral;
                            }
                        }
                        //更新UI
                        UpdateSeats(hID);
                        TicketForm tf = new TicketForm();
                        tf.Owner = this;
                        tf.ticketCode = DateTime.Now.ToFileTimeUtc().ToString();
                        tf.ShowDialog();
                    }
                }
            }
        }
```
- **支付界面（获取购票信息，计算总票价，提示支付）**

```C#
private void TicketForm_Load(object sender, EventArgs e)
        {
            label2.Text = ticketCode;
            Image qr = getqrcode(ticketCode);
            imageList1.Images.Add(qr);
            imageList1.ImageSize = new Size(150, 150);
            pictureBox1.Image = imageList1.Images[0];
        }


      

        public Image getqrcode(string content)
        {
            var encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = encoder.Encode(content);
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(12, QuietZoneModules.Two), Brushes.Black, Brushes.White);//如需改变二维码大小，调整12即可
            DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
            Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
            Graphics g = Graphics.FromImage(map);
            render.Draw(g, qrCode.Matrix);
            return map;
        }
```
- **取票信息界面（包括取票二维码以及取票序列号的实现）**

```C#
private void button1_Click(object sender, EventArgs e)
        {
            MainForm m = (MainForm)Owner;
            m.PayIsSuccess = true;
            this.Close();
        }

        private void PayForm_Load(object sender, EventArgs e)
        {
           
            label1.Text = payMoney.ToString();
        }
```

# 05 数据库设计
数据库采用的是SQLSERVER，可以复制下面的脚本到查询框执行，即可得到数据库和样本数据。
系统中采用DbContext方式直接连接数据库。各表的设计如下：

- **Customer**
![](https://upload-images.jianshu.io/upload_images/10386940-d046857121db7d89.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **Hall**
![](https://upload-images.jianshu.io/upload_images/10386940-0be084762cbed711.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **Movie**
![](https://upload-images.jianshu.io/upload_images/10386940-9d307091acd4c4fe.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **Schedule**
![](https://upload-images.jianshu.io/upload_images/10386940-9b7a827b06044766.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **Ticket**
![](https://upload-images.jianshu.io/upload_images/10386940-3987d7b11bea06a1.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)


**数据库脚本：**

```sql
USE [master]
GO
/****** Object:  Database [U201715959]    Script Date: 12/12/2019 17:34:20 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'U201715959')
BEGIN
CREATE DATABASE [U201715959] ON  PRIMARY 
( NAME = N'U201715959', FILENAME = N'D:\Database\U201715959\U201715959.mdf' , SIZE = 3072KB , MAXSIZE = 716800KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'U201715959_log', FILENAME = N'D:\Database\U201715959\U201715959_log.ldf' , SIZE = 1024KB , MAXSIZE = 204800KB , FILEGROWTH = 10%)
END
GO
ALTER DATABASE [U201715959] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [U201715959].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [U201715959] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [U201715959] SET ANSI_NULLS OFF
GO
ALTER DATABASE [U201715959] SET ANSI_PADDING OFF
GO
ALTER DATABASE [U201715959] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [U201715959] SET ARITHABORT OFF
GO
ALTER DATABASE [U201715959] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [U201715959] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [U201715959] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [U201715959] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [U201715959] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [U201715959] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [U201715959] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [U201715959] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [U201715959] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [U201715959] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [U201715959] SET  DISABLE_BROKER
GO
ALTER DATABASE [U201715959] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [U201715959] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [U201715959] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [U201715959] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [U201715959] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [U201715959] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [U201715959] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [U201715959] SET  READ_WRITE
GO
ALTER DATABASE [U201715959] SET RECOVERY FULL
GO
ALTER DATABASE [U201715959] SET  MULTI_USER
GO
ALTER DATABASE [U201715959] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [U201715959] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'U201715959', N'ON'
GO
USE [U201715959]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/12/2019 17:34:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
DROP TABLE [dbo].[Customer]
GO
/****** Object:  Table [dbo].[Hall]    Script Date: 12/12/2019 17:34:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hall]') AND type in (N'U'))
DROP TABLE [dbo].[Hall]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 12/12/2019 17:34:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movie]') AND type in (N'U'))
DROP TABLE [dbo].[Movie]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 12/12/2019 17:34:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') AND type in (N'U'))
DROP TABLE [dbo].[Schedule]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 12/12/2019 17:34:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ticket]') AND type in (N'U'))
DROP TABLE [dbo].[Ticket]
GO
/****** Object:  User [U201715959]    Script Date: 12/12/2019 17:34:20 ******/
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'U201715959')
DROP USER [U201715959]
GO
/****** Object:  User [U201715959]    Script Date: 12/12/2019 17:34:20 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'U201715959')
CREATE USER [U201715959] FOR LOGIN [U201715959] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 12/12/2019 17:34:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ticket]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Ticket](
	[TicketID] [int] IDENTITY(1,1) NOT NULL,
	[ScheduleID] [int] NULL,
	[DetailSeat] [nvarchar](50) NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[TicketID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Ticket] ON
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (1, 5, N'1-1')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (2, 5, N'1-2')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (3, 1, N'3-4')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (4, 5, N'1-3')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (5, 5, N'3-1')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (6, 18, N'1-1')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (7, 4, N'1-1')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (8, 4, N'1-2')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (9, 5, N'1-4')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (10, 5, N'1-5')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (11, 5, N'1-6')
INSERT [dbo].[Ticket] ([TicketID], [ScheduleID], [DetailSeat]) VALUES (12, 5, N'2-6')
SET IDENTITY_INSERT [dbo].[Ticket] OFF
/****** Object:  Table [dbo].[Schedule]    Script Date: 12/12/2019 17:34:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Schedule](
	[ScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[MovieID] [int] NULL,
	[HallID] [int] NULL,
	[Price] [int] NULL,
	[DateTime] [nvarchar](50) NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[ScheduleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Schedule] ON
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (1, 4, 1, 32, N'12-06 9:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (2, 2, 1, 35, N'12-06 10:40')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (3, 3, 1, 45, N'12-06 13:10')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (4, 5, 1, 38, N'12-06 15:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (5, 1, 1, 35, N'12-06  18:00')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (6, 5, 1, 40, N'12-06 20:10')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (7, 3, 1, 36, N'12-06 22:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (8, 3, 2, 36, N'12-06 9:25')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (9, 5, 2, 36, N'12-06 11:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (10, 4, 2, 40, N'12-06 13:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (11, 2, 2, 36, N'12-06 15:40')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (12, 2, 2, 40, N'12-06 17:55')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (13, 3, 2, 36, N'12-06 20:10')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (14, 1, 2, 36, N'12-06 10:20')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (15, 5, 3, 100, N'12-06 13:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (16, 3, 3, 100, N'12-06 15:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (17, 2, 3, 100, N'12-06 20:05')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (18, 1, 3, 100, N'12-05 10:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (19, 4, 3, 100, N'12-05 15:30')
INSERT [dbo].[Schedule] ([ScheduleID], [MovieID], [HallID], [Price], [DateTime]) VALUES (20, 5, 3, 100, N'12-05 20:20')
SET IDENTITY_INSERT [dbo].[Schedule] OFF
/****** Object:  Table [dbo].[Movie]    Script Date: 12/12/2019 17:34:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movie]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Movie](
	[MovieID] [int] IDENTITY(1,1) NOT NULL,
	[MovieName] [nvarchar](50) NULL,
	[Actor] [nvarchar](50) NULL,
	[Director] [nvarchar](50) NULL,
	[Duration] [int] NULL,
	[MovieType] [nvarchar](50) NULL,
	[Poster] [nvarchar](50) NULL,
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[MovieID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Movie] ON
INSERT [dbo].[Movie] ([MovieID], [MovieName], [Actor], [Director], [Duration], [MovieType], [Poster]) VALUES (1, N'海蒂和爷爷', N'阿努克·斯特芬', N'阿兰·葛斯彭纳', 111, N'剧情、家庭', N'海蒂和爷爷.jpg')
INSERT [dbo].[Movie] ([MovieID], [MovieName], [Actor], [Director], [Duration], [MovieType], [Poster]) VALUES (2, N'海上钢琴师', N'蒂姆·罗斯', N'吉赛贝·托纳多雷', 125, N'剧情', N'海上钢琴师.jpg')
INSERT [dbo].[Movie] ([MovieID], [MovieName], [Actor], [Director], [Duration], [MovieType], [Poster]) VALUES (3, N'冰雪奇缘2', N'安娜、艾莎', N'詹妮弗·李', 112, N'喜剧，冒险', N'冰雪奇缘.jpg')
INSERT [dbo].[Movie] ([MovieID], [MovieName], [Actor], [Director], [Duration], [MovieType], [Poster]) VALUES (4, N'夏目友人帐', N'井上和彦', N'大森贵弘', 106, N'妖怪，治愈，温暖', N'夏目友人帐.jpg')
INSERT [dbo].[Movie] ([MovieID], [MovieName], [Actor], [Director], [Duration], [MovieType], [Poster]) VALUES (5, N'利刃出鞘', N'丹尼尔·克雷格', N'莱恩·约翰逊', 130, N'喜剧 、悬疑 、犯罪', N'利刃出鞘.jpg')
SET IDENTITY_INSERT [dbo].[Movie] OFF
/****** Object:  Table [dbo].[Hall]    Script Date: 12/12/2019 17:34:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hall]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Hall](
	[HallID] [int] IDENTITY(1,1) NOT NULL,
	[rowsCount] [int] NULL,
	[colsCount] [int] NULL,
 CONSTRAINT [PK_Hall] PRIMARY KEY CLUSTERED 
(
	[HallID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Hall] ON
INSERT [dbo].[Hall] ([HallID], [rowsCount], [colsCount]) VALUES (1, 7, 4)
INSERT [dbo].[Hall] ([HallID], [rowsCount], [colsCount]) VALUES (2, 7, 4)
INSERT [dbo].[Hall] ([HallID], [rowsCount], [colsCount]) VALUES (3, 7, 4)
SET IDENTITY_INSERT [dbo].[Hall] OFF
/****** Object:  Table [dbo].[Customer]    Script Date: 12/12/2019 17:34:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customer](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[PassWord] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Customer] ON
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (1, N'张三', N'123456')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (2, N'小兔子', N'123456')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (3, N'lalala', N'123')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (4, N'可爱宋', N'123456')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (5, N'可爱郝', N'123456')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (6, N'小名', N'123')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (7, N'Angela', N'1234567')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (8, N'小张', N'123')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (9, N'123', N'123')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (10, N'12345', N'123')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (11, N'445', N'234')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (12, N'黎明', N'111')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (13, N'Lina', N'666666')
INSERT [dbo].[Customer] ([CustomerID], [UserName], [PassWord]) VALUES (14, N'酷酷酷', N'111')
SET IDENTITY_INSERT [dbo].[Customer] OFF
```


