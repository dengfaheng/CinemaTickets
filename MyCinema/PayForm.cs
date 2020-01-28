using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace MyCinema
{
    public partial class PayForm : Form
    {
        public double payMoney;
        
        public PayForm(bool w)
        {
            
            InitializeComponent();
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
            if (w)
            {
                this.button1.Visible = false;
            }
        }

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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
