using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DoppelPendel
{
    public class Pendulum
    {
        public void setup(Line line1, Line line2, Ellipse ball1, Ellipse ball2, double rBall)
        {
            line1.Stroke = Brushes.White;
            line1.StrokeThickness = 1;
            ball1.Width = rBall;
            ball1.Height = rBall;
            ball1.Stroke = Brushes.White;
            ball1.Fill = Brushes.White;
            line2.Stroke = Brushes.White;
            line2.StrokeThickness = 1;
            ball2.Width = rBall;
            ball2.Height = rBall;
            ball2.Stroke = Brushes.White;
            ball2.Fill = Brushes.White;
        }

        public double[] calculate(double g, double m1, double m2, double length1, double length2, double angle1, double angle2, double angle1V, double angle2V)
        {
            //https://de.wikipedia.org/wiki/Doppelpendel#Herleitung_der_Bewegungsgleichungen

            var num1 = -g * (2 * m1 + m2) * Math.Sin(angle1);
            var num2 = -m2 * g * Math.Sin(angle1 - 2 * angle2);
            var num3 = -2 * Math.Sin(angle1 - angle2) * m2;
            var num4 = ((angle2V * angle2V) * length2) + ((angle1V * angle1V) * length1 * Math.Cos(angle1 - angle2));
            var den = length1 * (2 * m1 + m2 - m2 * Math.Cos(2 * angle1 - 2 * angle2));
            double Angle1Acc = ((num1 + num2 + (num3 * num4)) / den);

            num1 = 2 * Math.Sin(angle1 - angle2);
            num2 = (angle1V * angle1V * length1 * (m1 + m2));
            num3 = g * (m1 + m2) * Math.Cos(angle1);
            num4 = angle2V * angle2V * length2 * m2 * Math.Cos(angle1 - angle2);
            den = length2 * (2 * m1 + m2 - m2 * Math.Cos(2 * angle1 - 2 * angle2));
            double Angle2Acc = ((num1 * (num2 + num3 + num4)) / den);

            return new double[] { Angle1Acc, Angle2Acc };
        }
    }
}
