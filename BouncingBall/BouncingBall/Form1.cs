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
    // class Ball \ Ball()
    // Add limits for values of xBall and yBall

    // Make a list of Balls, and make chosenBall variable
    
    public partial class Form1 : Form
    {
        Ball ball = null;

        public Form1()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ball.bordersOfForm(ClientRectangle.Width * 1.0, ClientRectangle.Height * 1.0);
            Ball.timeTickChanged(timer1.Interval);

            ball = new Ball(ClientRectangle.Width * 1.0 / 2, ClientRectangle.Height * 3.0 / 4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                label1.Visible = true;
                textBox1.Visible = true;
                label2.Visible = true;
                textBox2.Visible = true;

                button1.Text = "Continue";
            }
            else if (button1.Text == "Continue")
            {
                ball.changeVelocity(textBox1.Text, textBox2.Text);

                label1.Visible = false;
                textBox1.Visible = false;
                label2.Visible = false;
                textBox2.Visible = false;

                button1.Text = "Stop";
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            Ball.bordersOfForm(ClientRectangle.Width * 1.0,
                ClientRectangle.Height * 1.0);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (ball != null)
                ball.Paint(e);
        }

    }
}
