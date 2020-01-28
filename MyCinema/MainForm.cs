using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Model;
using DAL;
using System.Drawing.Drawing2D;

namespace MyCinema
{
    public partial class MainForm : Form
    {
        Font font1 = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        Font font2 = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        private Dictionary<string, CheckBox>[] checkBoxs = new Dictionary<string, CheckBox>[4];
        private Dictionary<string, Seat>[] seats = new Dictionary<string, Seat>[4];

        public Customer customerVIP = null;
        private List<int> AllHallIDs = new List<int>();
        private List<int> otherHallIDs = new List<int>();

        public bool PayIsSuccess = false;

        public MainForm()
        {
            InitializeComponent();
            this.pictureBox1.BackgroundImage = Image.FromFile(@"Resources\爱丽儿.jpg");
            this.pictureBox4.BackgroundImage = Image.FromFile(@"Resources\冰雪奇缘.jpg");
            this.pictureBox3.BackgroundImage = Image.FromFile(@"Resources\利刃出鞘.jpg");
            this.pictureBox5.BackgroundImage = Image.FromFile(@"Resources\海蒂和爷爷.jpg");
            this.pictureBox2.BackgroundImage = Image.FromFile(@"Resources\海上钢琴师.jpg");
            this.pictureBox6.BackgroundImage = Image.FromFile(@"Resources\夏目友人帐.jpg");
            
            this.skinEngine1.SkinFile = "RealOne.ssk";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.lblActor.Text = "";    /* 程序加载时，初始化3个放映厅*/
            this.lblDirector.Text = "";
            this.lblMovieName.Text = "";
            this.lblPrice.Text = "";
            this.lblTime.Text = "";
            this.lblType.Text = "";
            this.lblCalcPrice.Text = "";

            //Movie m = CinemaDbContext.CDbContext.Movies.Find(1);
            //Hall h = CinemaDbContext.CDbContext.Halls.Find(1);
            //Schedule s = CinemaDbContext.CDbContext.Schedules.Find(1);
            //Ticket t = CinemaDbContext.CDbContext.Tickets.Find(1);
            //MessageBox.Show(m.MovieName);
            //MessageBox.Show(h.colsCount.ToString());
            //MessageBox.Show(s.DateTime);
            //MessageBox.Show(t.DetailSeat);

            if (this.customerVIP != null)
            {
                this.LabelCustInfo.Text = "VIP";
                this.LabelDisCount.Text = "8折优惠";
            }
            else
            {
                this.LabelCustInfo.Text = "普通用户";
                this.LabelDisCount.Text = "无优惠";
            }
            for (int i = 1; i <=3; ++i)
            {
                checkBoxs[i] = new Dictionary<string, CheckBox>();
                seats[i] = new Dictionary<string, Seat>();
            }

            foreach (Hall h in HallDAL.GetAllHalls())
            {
                AllHallIDs.Add(h.HallID);
            }

            ///初始化放映厅座位
            
            foreach(int hID in AllHallIDs)
            {
                InitSeatsCheckBox(HallDAL.GetRowsCount(hID), HallDAL.GetColsCount(hID), tbSeat.TabPages[hID - 1], checkBoxs[hID], seats[hID]);
            }

            InitTreeView();


            //头像更改
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pictureBox1.ClientRectangle);
            Region region = new Region(gp);
            pictureBox1.Region = region;
            gp.Dispose();
            region.Dispose();

        }

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


        //选择“继续销售”
        private void tsmiMovies_Click(object sender, EventArgs e)
        {
            //判断放映列表是否为空
            
            InitTreeView();
        }

        //选择“获取最新播放列表”
        private void tsmiNew_Click(object sender, EventArgs e)
        {
            
            InitTreeView();
        }

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

        /// <summary>
        /// 清空座位
        /// </summary>
        private void ReSetSeats(int hID)
        {
            foreach (Seat seat in seats[hID].Values)
            {
                seat.Color = Color.LightBlue;
            }
            foreach (CheckBox cB in checkBoxs[hID].Values)
            {
                cB.Enabled = true;
                cB.Checked = false;
            }
        }
        /// <summary>
        /// 更新座位状态 
        /// </summary>
        private void UpdateSeats(int hID)
        {
            foreach (string key in seats[hID].Keys)
            {
                checkBoxs[hID][key].BackColor = seats[hID][key].Color;
            }
            foreach(CheckBox cB in checkBoxs[hID].Values)
            {
                if(cB.BackColor == Color.LightCoral)
                {
                    cB.Enabled = false;
                    cB.Checked = true;
                }
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tbSeat_Selecting(object sender, TabControlCancelEventArgs e)
        {
            
            foreach (int hID in otherHallIDs)
            {
                if (e.TabPageIndex == hID-1)
                {
                    e.Cancel = true;
                    MessageBox.Show("该厅没有该场次的电影");
                }
            }
        }

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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}