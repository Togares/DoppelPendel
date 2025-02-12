using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Angle
{
    public static double DegToRad(double degValue)
    {
        return ((2 * Math.PI) / 360) * degValue;
    }

    public static double RadToDeg(double radValue)
    {
        return (360 / (2 * Math.PI)) * radValue;
    }
}
