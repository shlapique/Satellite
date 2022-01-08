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

        public Stack<Sat> stack;// создадим список сателлитов

        //private delegate void MyDelegate(int i); //объявление делегата
        Controller.Controller controller { set; get; } = null;
        public Model(Controller.Controller controller)
        {
            this.controller = controller;
            MessageBox.Show("MODEL !");
            this.stack = new Stack<Sat>();
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
           
            // rotatefunc // +++
        }

        public class Orbit : Earth
        {
            int angle_min = 0;
            int angle_max = 90; // 
            int rad_min = 8;
            int rad_max = 150;

            public int a, b; // rads
            public double angle;

            public double angle_rotation = 0;
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
                MessageBox.Show("ORBIT !");
                MessageBox.Show($"{a}, {b}, {angle}");
                this.angle_deg = angle;
                rad_to_deg(ref angle_deg);
            }

            public void random_orbit()
            {
                //задаем слачайные радиусы эллипсов и углы
                this.angle = rand.Next(angle_min, angle_max); // 
                this.a = rand.Next(rad_min, rad_max);
                if(a <= 50) 
                {
                    this.b = rand.Next(52, rad_max);
                }
                else
                {
                    this.b = rand.Next(rad_min, rad_max);
                }

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
                rotation(ref apogee_1, angle);
                rotation(ref apogee_2, angle);
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

            public bool entry_flag = false; // пересекал ли окружность 


            public Sat(Point center)
            {
                MessageBox.Show("SAT CONSTR !");
                this.tmpPonit = new Point();
                //start_position(center); // default location
                MessageBox.Show($"a = {a}, b = {b}, \n radiusellix = {radius_elli_x}, radiuselliy = {radius_elli_y}, \n STARTloc.x = {loc.X}, STARTloc.Y = {loc.Y}");
                apogee_pos(center);
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

                this.loc = new Point(x - 7, y - 7); // default location
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

                this.loc = new Point(x - 7, y - 7);

                angleStart = Math.Acos(b / 50.0);

                angleStart = angleStart * 180.0 / Math.PI;

                // считаем радиус-вектор до центра 
                double del_x, del_y;

                del_x = loc.X - center.X;
                del_y = loc.Y - center.Y;
                r = Math.Sqrt(Math.Pow(del_x, 2) + Math.Pow(del_y, 2)); // нашли гипотенузу
            }

            public void angle_change(Point loc)
            {
                this.tmpPonit = loc;
                this.angle_rotation -= 0.0123;
                Task.Delay(1);
            }
        }


    }
}
