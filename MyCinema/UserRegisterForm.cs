using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCinema
{
    public partial class UserRegisterForm : Form
    {
        public UserRegisterForm()
        {
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

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

        private void UserRegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
