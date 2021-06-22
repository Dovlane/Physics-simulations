using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BouncingBall
{
    class ListBall : IEnumerable
    {
        private List<Ball> balls;
        private static Color initialColor = Color.Blue;
        private static Color colorOfChosen = Color.Red;
        private Thread collisionOfBalls;

        public ListBall()
        {
            balls = new List<Ball>();
            collisionOfBalls = new Thread(Collisions);
            collisionOfBalls.Start();
        }

        public void ZatvaramListBall()
        {
            if (collisionOfBalls == null)
                return;

            collisionOfBalls.Abort();
            collisionOfBalls.Join();

            collisionOfBalls = null;
        }

        public void Add(int xBall, int yBall)
        {
            balls.Add(new Ball(xBall, yBall, initialColor));
        }
        public IEnumerator GetEnumerator()
        {
            foreach (Ball b in balls)
            {
                yield return b;
            }
        }

        // you should account here and interaction between balls...
        public void Paint(Graphics gr)
        {
            foreach (Ball b in balls)
                b.Paint(gr);
        }

        private void Collisions()
        {

            while (true)
            {
                if (balls.Count() >= 2)
                {
                    for (int i = 0; i < balls.Count() - 1; i++)
                        for (int j = i + 1; j < balls.Count(); j++)
                            CollisionOfTwoBalls(balls[i], balls[j]);
                }
                if (Thread.CurrentThread.ThreadState == ThreadState.AbortRequested)
                    return;
            }
        }

        public void changeVelocity(string Velocity_X_input, string Velocity_Y_input)
        {
            double result;
            if (double.TryParse(Velocity_X_input, out result))
                foreach (Ball b in balls)
                    if (b.ColorOfBall == colorOfChosen)
                        b.Velocity.X = result;
            if (double.TryParse(Velocity_Y_input, out result))
                foreach (Ball b in balls)
                    if (b.ColorOfBall == colorOfChosen)
                        b.Velocity.Y = result;
        }

        private void CollisionOfTwoBalls(Ball firstBall, Ball secondBall)
        {
            double xBall_1 = firstBall.XBall;
            double yBall_1 = firstBall.YBall;
            double xBall_2 = secondBall.XBall;
            double yBall_2 = secondBall.YBall;
            if (ballsAreColliding(xBall_1, yBall_1, xBall_2, yBall_2))
            { 
                // V - vector of collision, normalized
                // V_B1 - vector of velocity of ball 1
                // V_B2 - vector of velocity of ball 2

                Vector2D V = new Vector2D(xBall_2 - xBall_1, yBall_2 - yBall_1);
                V.Normalize();

                Vector2D V_B1 = firstBall.Velocity;
                Vector2D V_B2 = secondBall.Velocity;

                Vector2D V_B1_n = V * (V_B1 * V);
                Vector2D V_B2_n = V * (V_B2 * V);

                Vector2D V_B1_t = V_B1 - V_B1_n;
                Vector2D V_B2_t = V_B2 - V_B2_n;

                // I gave the same masses to my two balls;
                CollisionOfTwoBallsAlongTheSameLine(1, ref V_B1_n, 1, ref V_B2_n);

                firstBall.Velocity = V_B1_n + V_B1_t;
                secondBall.Velocity = V_B2_n + V_B2_t;

                MakeDistanceBetweenTheBalls(ref firstBall, ref secondBall);
            }
        }

        private void CollisionOfTwoBallsAlongTheSameLine(double m1, ref Vector2D V1, double m2, ref Vector2D V2)
        {
            Vector2D newV1 = V1.Clone() as Vector2D;
            Vector2D newV2 = V2.Clone() as Vector2D;

            newV1 = V1 * (m1 - m2) / (m1 + m2) + V2 * (2 * m2) / (m1 + m2);
            newV2 = V1 * (2 * m1) / (m1 + m2) + V2 * (m2 - m1) / (m1 + m2);

            V1 = newV1;
            V2 = newV2;
        }

        private void MakeDistanceBetweenTheBalls (ref Ball firstBall, ref Ball secondBall)
        {
            double xBall_1 = firstBall.XBall;
            double yBall_1 = firstBall.YBall;
            double xBall_2 = secondBall.XBall;
            double yBall_2 = secondBall.YBall;

            Vector2D distance = new Vector2D(xBall_2 - xBall_1, yBall_2 - yBall_1);

            distance *= 1.1;

            double scalar = 0.05;
            xBall_1 -= distance.X * scalar;
            yBall_1 -= distance.Y * scalar;
            xBall_2 += distance.X * scalar;
            yBall_2 += distance.Y * scalar;

            firstBall.GiveNewCoordinates(xBall_1, yBall_1);
            secondBall.GiveNewCoordinates(xBall_2, yBall_2);
        }

        private bool ballsAreColliding(double xBall_1, double yBall_1, double xBall_2, double yBall_2)
        {
            double distance = Math.Sqrt((xBall_2 - xBall_1) * (xBall_2 - xBall_1) + (yBall_2 - yBall_1) * (yBall_2 - yBall_1));
            if (distance <= 2 * Ball.Radius)
                return true;
            return false;
        }

        public void ReactionTo_Form1_MouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double x = e.X * 1.0;
                double y = e.Y * 1.0;
                double minDistanceOfAllBalls = double.MaxValue;
                bool ball_Chosen = false;
                foreach (Ball b in balls)
                {
                    double distanceOfBall;
                    if (b.IsClicked(e.X, e.Y, out distanceOfBall))
                    {
                        b.ColorOfBall = colorOfChosen;
                        ball_Chosen = true;
                        break;
                    }
                    minDistanceOfAllBalls = Math.Min(minDistanceOfAllBalls, distanceOfBall);
                }
                if (!ball_Chosen && minDistanceOfAllBalls >= Ball.Radius * 2)
                    this.Add(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Right)
            {
                double x = e.X * 1.0;
                double y = e.Y * 1.0;
                foreach (Ball b in balls)
                {
                    if (b.ColorOfBall == colorOfChosen)
                    {
                        double distanceOfBall;
                        if (b.IsClicked(e.X, e.Y, out distanceOfBall))
                        {
                            b.ColorOfBall = initialColor;
                            break;
                        }
                    }
                }
            }
            
        }
    }
}
