using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace MyCinema
{
    public partial class TicketForm : Form
    {
        public string ticketCode;

        public TicketForm()
        {
            InitializeComponent();
            
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
           
            


        }

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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TicketForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
