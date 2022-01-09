using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using satellite.Controller;

namespace satellite.Model
{
    public class Model
    {
        Random rand = new Random();

        public List<Sat> list;// создадим список сателлитов

        Controller.Controller controller { set; get; } = null;
        public Model(Controller.Controller controller)
        {
            this.controller = controller;
            this.list = new List<Sat>();
        }

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


        //classes +++ 
        public abstract class Earth
        {
            Image im; 
            protected const int MAX_ORBIT_COUNT = 8;
            public string Path 
            {
                get { return "H:\\projects\\mai\\isrpps\\cursed\\img\\earth.png"; } 
                set { } 
            }
        }

        public class Orbit : Earth
        {
            int angle_min = 0;
            int angle_max = 180; // 
            int rad_min = 52;
            int rad_max = 150;

            public int a = 150;
            public int b;
            public int min_b = 0;
            public double angle;

            public double angle_rotation;
            public double angleStart;
            public double angle_deg;

            ///+++++++++
            public Point apogee_1; //верхний(левый) и 
            public Point apogee_2; //нижний(правый)
            ///+++++++++
            Random rand = new Random();
            public Orbit()
            {
                random_orbit();
                this.angle_deg = angle;
                rad_to_deg(ref angle_deg);
            }

            public void random_orbit()
            {
                //задаем слачайные радиусы эллипсов и углы
                this.b = rand.Next(rad_min, rad_max);
                this.angle = rand.Next(angle_min, angle_max); // 
                this.angle = this.angle * Math.PI / 180.0; // 0 .. pi
            }
            public void apogee_pos(Point center)
            {
                if(a > b)
                {
                    this.apogee_1.X = center.X;
                    this.apogee_1.Y = center.Y - a;
                    this.apogee_2.X = center.X;
                    this.apogee_2.Y = center.Y + a;
                }
                else
                {
                    this.apogee_1.X = center.X - b;
                    this.apogee_1.Y = center.Y;
                    this.apogee_2.X = center.X + b;
                    this.apogee_2.Y = center.Y;
                }

                // поворачиваем на угол angle
                rotation(ref apogee_1, center, angle);
                rotation(ref apogee_2, center, angle);
            }
        }

        public class Sat : Orbit
        {
            //void movement()   
            public Point loc;
            //public Point loc_tmp;
            public Point tmpPonit;
            int x, y; // начальное положение
            double coord_x, coord_y, radius_elli_x, radius_elli_y;
            public double r; // radius-vector

            public double r_vector1_1; // 
            public double r_vector2_1; // 
            public double r_vector1_2; // 
            public double r_vector2_2; // 

            public double r_vector;

            public double r_final;
            public double r_loc_tmp_begin;

            // ____________
            public double velo_coef = 1; // коэфф скорости 

            Random rand = new Random();

            public Point begin_point;

            public int flag;
            public bool one;
            public bool two;
            public bool r_stat;
            public bool entry = false;

            public Point loc_begin;

            public double ratio_cos, ratio_sin;

            public int sign; // направл вращения 
            public Sat(Point center)
            {
                this.tmpPonit = new Point();
                apogee_pos(center);
                start_position(center);
                loc_begin = new Point();
                loc_begin = loc;
                sign = rand.Next(0, 100);
                velo_coef = 1;
            }

            public void start_position(Point center)
            {
                this.coord_x = b * Math.Cos(angle_rotation);
                this.coord_y = a * Math.Sin(angle_rotation);
                this.radius_elli_x = center.X - b;
                this.radius_elli_y = center.Y - a;
                // повернем x, y
                rotation(ref coord_x, ref coord_y, angle);

                this.x = (int)(center.X + coord_x);
                this.y = (int)(center.Y + coord_y);

                this.loc = new Point(x - 4, y - 4); // default location
            }

            public void position(Point center) // position every tick
            {
                this.coord_x = b * Math.Cos(angle_rotation);
                this.coord_y = a * Math.Sin(angle_rotation);
                this.radius_elli_x = center.X - b;
                this.radius_elli_y = center.Y - a;
                // повернем x, y
                rotation(ref coord_x, ref coord_y, angle);

                this.x = (int)(center.X + coord_x);
                this.y = (int)(center.Y + coord_y);

                this.loc = new Point(x - 4, y - 4);

                angleStart = Math.Acos(b / 50.0);

                angleStart = angleStart * 180.0 / Math.PI; //deg

                // считаем радиус-вектор до центра 
                double del_x, del_y;

                del_x = loc.X - center.X;
                del_y = loc.Y - center.Y;
                r = Math.Sqrt(Math.Pow(del_x, 2) + Math.Pow(del_y, 2)); // нашли гипотенузу

                ///////
                r_vector1_1 = radius_vector(tmpPonit, apogee_1);
                r_vector2_1 = radius_vector(loc, apogee_1);
                r_vector1_2 = radius_vector(tmpPonit, apogee_2);
                r_vector2_2 = radius_vector(loc, apogee_2);
                //++++++++++
                r_final = Math.Sqrt(Math.Pow(50, 2) - Math.Pow(b, 2));
                r_loc_tmp_begin = radius_vector(loc_begin, loc);
                //++++++++++
                begin_point.X = 0;
                begin_point.Y = 0;
                //////////
                one = r_vector1_1 > r_vector2_1;
                two = r_vector1_2 > r_vector2_2;
                r_stat = r <= 50.0;
                //////////
                r_vector = radius_vector(loc, begin_point);
                //////////
            }
            
            public void angle_change(Point loc)
            {
                if(sign > 50)
                {
                    this.angle_rotation -= 0.01 * velo_coef;
                }
                else
                {
                    this.angle_rotation += 0.01 * velo_coef;
                }
                this.tmpPonit = loc;
                Task.Delay(1);
            }
        }
    }
}
