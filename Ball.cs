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
        private double xBall, yBall; 
        public Color ColorOfBall { get; set; }
        public Vector2D Velocity {get; set;}

        static double g = 9.81;
        public static double Radius = 20;
        static double widthOfForm, heightOfForm;
        static double delta_t;

        public double XBall { get { return xBall; } }
        public double YBall { get { return yBall; } }

        public static void bordersOfForm(double _widthOfForm, double _heightOfForm)
        {
            widthOfForm = _widthOfForm;
            heightOfForm = _heightOfForm;
        }
        public static void timeTickChanged(int timerInterval)
        {
            delta_t = 1.0 / timerInterval * 10;
        }
        
        public Ball(double xBall, double yBall, Color color)
        {
            this.xBall = correctCoordinate(xBall, Radius, widthOfForm);
            this.yBall = correctCoordinate(yBall, Radius, heightOfForm);
            this.ColorOfBall = color;
            this.Velocity = new Vector2D(0.0, 0.0);
        }
        public void GiveNewCoordinates(double xBall, double yBall)
        {
            this.xBall = xBall;
            this.yBall = yBall;
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
        private void placeInBallBackInForm(double smallerBorder, double largerBorder, ref double currentPosition)
        {
            if (Math.Abs(smallerBorder - currentPosition) < Math.Abs(largerBorder - currentPosition))
                currentPosition = smallerBorder + Radius + 2;
            else
                currentPosition = largerBorder - Radius;
        }

        public void changeVelocity(string Velocity_X_input, string Velocity_Y_input)
        {
            double result;
            if (double.TryParse(Velocity_X_input, out result))
                Velocity.X = result;
            if (double.TryParse(Velocity_Y_input, out result))
                Velocity.Y = result;
        }

        // Checks whether mouse click was in the area of the ball.
        public bool IsClicked(double x, double y, out double distanceOfCenter)
        {
            distanceOfCenter = Math.Sqrt((xBall - x) * (xBall - x) + (yBall - y) * (yBall - y));
            return distanceOfCenter <= Radius;
        }

        public void Paint(Graphics gr)
        {
            SolidBrush sb = new SolidBrush(ColorOfBall);
            gr.FillEllipse(sb, (float)(xBall - Radius), (float)(yBall - Radius), (float)(2 * Radius), (float)(2 * Radius));

            Velocity.Y += g * delta_t;
            yBall += Velocity.Y * delta_t;
            xBall += Velocity.X * delta_t;
            if (!inHorisontalBoundaries(xBall, Radius))
            {
                Velocity.X *= -1;
                placeInBallBackInForm(0, widthOfForm, ref xBall);
            }
            if (!inVerticalBoundaries(yBall, Radius))
            {
                Velocity.Y *= -1;
                placeInBallBackInForm(0, heightOfForm, ref yBall);
            }
        }
    }
}
