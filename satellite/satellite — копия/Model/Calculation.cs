/* Poor substitute of a calculation library
 * It was used in Satellite Tracking Project (not finished)
 * Here you can find some basic calculations for radius vectors,
 * conversion from degrees to radians, etc.
 */
using System;
using System.Drawing;

namespace satellite.Model
{
    public static class Calculation
    {
        public static void rad_to_deg(ref double angle_rad)
        {
            angle_rad = angle_rad * 180.0 / Math.PI;
        }

        public static void deg_to_rad(ref double angle_deg)
        {
            angle_deg = angle_deg * Math.PI / 180.0;
        }

        public static void rotation(ref int x, ref int y, double angle_rot)
        {
            int x_t = x;
            x = (int)(x * Math.Cos(angle_rot) - y * Math.Sin(angle_rot));
            y = (int)(x_t * Math.Sin(angle_rot) + y * Math.Cos(angle_rot));
        }

        public static void rotation(ref double x, ref double y, double angle_rot)
        {
            double x_t = x;
            x = x * Math.Cos(angle_rot) - y * Math.Sin(angle_rot);
            y = x_t * Math.Sin(angle_rot) + y * Math.Cos(angle_rot);
        }

        public static void rotation(ref Point point, double angle_rot)
        {
            int point_x = point.X;
            point.X = (int)(point.X * Math.Cos(angle_rot) - point.Y * Math.Sin(angle_rot));
            point.Y = (int)(point_x * Math.Sin(angle_rot) + point.Y * Math.Cos(angle_rot));
        }

        public static void rotation(ref Point point, Point center, double angle_rot) // поворот относительно точки
        {
            int point_x = point.X;
            point.X = (int)Math.Round((point.X - center.X) * Math.Cos(angle_rot) - (point.Y - center.Y) * Math.Sin(angle_rot) + center.X);
            point.Y = (int)Math.Round((point_x - center.X) * Math.Sin(angle_rot) + (point.Y - center.Y) * Math.Cos(angle_rot) + center.Y);
        }

        public static double radius_vector(Point a, Point b)
        {
            double r = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            return r;
        }
    }
}
