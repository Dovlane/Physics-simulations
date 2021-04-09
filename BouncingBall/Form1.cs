using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
    public partial class Form1 : Form
    {
        double xBall, yBall, r = 20;
        double g = 9.81;
        double Velocity_X = 0.0;
        double Velocity_Y = 0.0;
        double delta_t = 0.1;
        SolidBrush sb = new SolidBrush(Color.Blue);

        public Form1()
        {
            InitializeComponent();
            
        }

        bool inHorisontalBoundaries(double x, double r)
        {
            return x - r >= 0 && x + r <= Width;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xBall = Width / 2.0;
            yBall = Height * 3.0 / 4;
        }

        bool inVerticalBoundaries(double y, double r)
        {
            return y - r >= 0 && y + r <= Height;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(sb, (float)(xBall - r), (float)(yBall - r), (float)(2*r), (float)(2*r));

            if (!inHorisontalBoundaries(xBall, r))
                Velocity_X *= -1;
            if (!inVerticalBoundaries(yBall, r))
                Velocity_Y *= -1;

            Velocity_Y += g * delta_t;
            yBall += Velocity_Y * delta_t;
        }

    }
}
