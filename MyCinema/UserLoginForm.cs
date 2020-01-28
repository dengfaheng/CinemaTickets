using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLL;
using DAL;
using Model;

namespace MyCinema
{
    public partial class UserLoginForm : Form
    {
        public UserLoginForm()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "Vista2_color2.ssk";
        }

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

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserRegisterForm userRegisterForm = new UserRegisterForm();
            userRegisterForm.Show();
            this.Visible = false;
        }

        private void UserLoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
