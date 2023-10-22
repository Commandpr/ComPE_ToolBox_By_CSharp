using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
      
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SetWindowR();
        }
        public void UpdateLabel(string text)
        {
            label1.Text = text;
        }
        private void SetWindowR()
        {
            GraphicsPath gPath = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 5, this.Width, this.Height - 5);
            gPath = GetRoundedRP(rect,360); //后面的30是圆的角度，数值越大圆角度越大
            this.Region = new Region(gPath);
        }

        private GraphicsPath GetRoundedRP(Rectangle rect, int a)
        {
            int diameter = a;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            Rectangle arcRect2 = new Rectangle(rect.Location, new Size(1, 1));
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diameter;
            arcRect2.X = rect.Right;
            gp.AddArc(arcRect2, 270, 90);
            arcRect.Y = rect.Bottom - diameter;
            arcRect2.Y = rect.Bottom;
            gp.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            arcRect2.X = rect.Left;
            gp.AddArc(arcRect2, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
