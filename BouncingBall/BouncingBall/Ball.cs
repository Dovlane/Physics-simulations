using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
    class Ball
    {
        double xBall, yBall;
        double r;
        public double Velocity_X {get; set;}
        public double Velocity_Y { get; set; }

        static double g = 9.81;
        static double widthOfForm, heightOfForm;
        static double delta_t;
        public static void bordersOfForm(double _widthOfForm, double _heightOfForm)
        {
            widthOfForm = _widthOfForm;
            heightOfForm = _heightOfForm;
        }
        public static void timeTickChanged(int timerInterval)
        {
            delta_t = 1.0 / timerInterval * 10;
        }
        
        public Ball(double xBall, double yBall)
        {
            this.r = 20.0;
            this.xBall = correctCoordinate(xBall, r, widthOfForm);
            this.yBall = correctCoordinate(yBall, r, heightOfForm);
            this.Velocity_X = 0.0;
            this.Velocity_Y = 0.0;
        }
        private double correctCoordinate(double coorBall, double r, double coorMaxValue)
        {
            if (coorBall - r < 0)
                return r;
            if (coorBall + r > coorMaxValue)
                return coorMaxValue - r;
            return coorBall;
        }
        private bool inVerticalBoundaries(double y, double r)
        {
            return y - r >= 0 && y + r <= heightOfForm;
        }
        private bool inHorisontalBoundaries(double x, double r)
        {
            return x - r >= 0 && x + r <= widthOfForm;
        }
        private void placeInBallBackInForm(double smallerBorder, double largerBorder, double currentPosition)
        {
            if (Math.Abs(smallerBorder - currentPosition) < Math.Abs(largerBorder - currentPosition))
                currentPosition = smallerBorder + r;
            else
                currentPosition = largerBorder - r;
        }

        public void changeVelocity(string Velocity_X_input, string Velocity_Y_input)
        {
            double result;
            if (double.TryParse(Velocity_X_input, out result))
                Velocity_X = result;
            if (double.TryParse(Velocity_Y_input, out result))
                Velocity_Y = result;
        }

        public void Paint(PaintEventArgs e)
        {
            SolidBrush sb = new SolidBrush(Color.Blue);
            e.Graphics.FillEllipse(sb, (float)(xBall - r), (float)(yBall - r), (float)(2 * r), (float)(2 * r));

            if (!inHorisontalBoundaries(xBall, r))
            {
                Velocity_X *= -1;
                placeInBallBackInForm(0, widthOfForm, xBall);
            }
            if (!inVerticalBoundaries(yBall, r))
            {
                Velocity_Y *= -1;
                placeInBallBackInForm(0, heightOfForm, yBall);
            }
            Velocity_Y += g * delta_t;
            yBall += Velocity_Y * delta_t;
            xBall += Velocity_X * delta_t;
        }
    }
}
