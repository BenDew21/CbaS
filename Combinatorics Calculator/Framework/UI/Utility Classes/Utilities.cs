using System;

namespace Combinatorics_Calculator.Framework.UI.Utility_Classes
{
    public static class Utilities
    {
        public static Tuple<double, double> GetSnap(double x, double y, double snap)
        {
            // If we don't want to snap, then simply return the X,Y
            if (snap == 0)
            {
                return new Tuple<double, double>(x, y);
            }

            double newX = 0, newY = 0;

            double xMod = x % snap;
            double yMod = y % snap;

            if (xMod < (snap / 2))
            {
                newX = x - xMod;
            }
            else
            {
                newX = x - xMod + snap;
            }

            if (yMod < (snap / 2))
            {
                newY = y - yMod;
            }
            else
            {
                newY = y - yMod + snap;
            }

            return new Tuple<double, double>(newX, newY);
        }
    }
}