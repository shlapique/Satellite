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
using System.Drawing.Imaging;

namespace satellite.Model
{
    public class Model
    {
        Random rand = new Random();

        public List<Satellite> list;// создадим список сателлитов

        public Controller.Controller controller { set; get; } = null;
        public Model(Controller.Controller controller)
        {
            this.controller = controller;
            this.list = new List<Satellite>();
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

        // ----------------
        public void rece_position(Point center, ref Point rece_pos,ref double rece_bias, ref bool rece_sign, ref bool visi_rece_flag) // position for rece dot
        {
            rece_pos.X = (int)(rece_pos.X + rece_bias);

            if (radius_vector(rece_pos, center) < 50.0)
            {

                /* pseudocode
                if(visi_rece_flag == false)
                {
                    fillrect with backcolor
                }
                else
                {
                    fillrect with purple
                }
                */
            }
            else
            {
                if ((rece_pos.X - center.X) > 0) // right side relatively center
                {
                    rece_sign = false;
                    visi_rece_flag = false;
                }
                else
                {
                    rece_sign = true;
                    visi_rece_flag = true;
                }
            }
        }

        public void rece_bias_change(bool rece_sign, ref double rece_bias)
        {
            /* sign settings for rece position */
            if (rece_sign == true)
            {
                if (rece_bias < 0)
                {
                    rece_bias = 0;
                }
                rece_bias += 1;
            }
            else
            {
                if(rece_bias > 0)
                {
                    rece_bias = 0;
                }
                rece_bias -= 1;
            }
            Task.Delay(1);
        }
        // ----------------

        //classes +++++++++
        public class Earth //
        {
            public Earth()
            { 

            }

            //image proc ***********
            public void im_make_draw(
                Image im, //подразумевается передача пути:
                //im = Image.FromFile("H:\\projects\\mai\\isrpps\\cursed\\img\\earth_no_color_test.png"); // im передает Earth
                Bitmap bm, // битмап 100 * 100
                Point point, //точка начала рисования на окружности
                Graphics g // 
                )
            {
                //++
                bm = new Bitmap(100, 100);
                //ImageCodecInfo; // png
                Point center = new Point(50, 50); // центр картинки
                SolidBrush brush = new SolidBrush(Color.Black);
                Rectangle dot = new Rectangle(point, new Size(14, 14));
                double r = radius_vector(center, point);

                //++

                for(int i = 0; i < 100; ++i)
                {
                    point.X += i;
                    dot = new Rectangle(point, new Size(14, 14));
                    if (r <= 50)
                    {
                        g.FillEllipse(brush, dot);
                    }
                    else
                    {
                        point.X = center.X - (int)Math.Sqrt(Math.Pow(50, 2) - Math.Pow(center.Y - point.Y, 2));
                    }
                }


            }
            //image proc ***********
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
            public Point center;

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
            public void apogee_pos()
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

        public class Satellite : Orbit
        {
            //void movement()   
            public Point loc;
            //public Point loc_tmp;
            public Point tmpPonit;
            int x, y; // начальное положение
            double coord_x, coord_y;
            public double r; // radius-vector
            public double r_vector;
            // ***********
            public double r_track;
            public int sat_stat;
            public bool rece_sign = true; // true - for increasing x (+), false - .;. (-)
            public double rece_bias;
            public bool visi_rece_flag = true; // -> controller */
            public Point rece_pos;
            // *********** 

            /*-------------*/
            public double velo_coef; // коэфф скорости 
            public int size;
            /*-------------*/
            Random rand = new Random();
            public Point loc_begin;
            public int sign; // направл вращения 


            public Satellite(Point center, Point rece_pos)
            {
                this.tmpPonit = new Point();
                start_position();
                this.center = center;
                this.rece_pos = rece_pos;
                loc_begin = new Point();
                loc_begin = loc;
                sign = rand.Next(0, 100);
                velo_coef = 1;
                size = 4;
            }

            public void start_position()
            {
                this.coord_x = b * Math.Cos(angle_rotation);
                this.coord_y = a * Math.Sin(angle_rotation);
                // повернем x, y
                rotation(ref coord_x, ref coord_y, angle);

                this.x = (int)(center.X + coord_x);
                this.y = (int)(center.Y + coord_y);

                this.loc = new Point(x - size, y - size); // default location
            }

            public void position() // position every tick
            {
                this.coord_x = b * Math.Cos(angle_rotation);
                this.coord_y = a * Math.Sin(angle_rotation);
                // повернем x, y
                rotation(ref coord_x, ref coord_y, angle);
                this.x = (int)(center.X + coord_x);
                this.y = (int)(center.Y + coord_y);
                this.loc = new Point(x - size, y - size);

                angleStart = Math.Acos(b / 50.0);
                angleStart = angleStart * 180.0 / Math.PI; //deg

                // считаем радиус-вектор до центра 
                double del_x, del_y;

                del_x = loc.X - center.X;
                del_y = loc.Y - center.Y;
                r = Math.Sqrt(Math.Pow(del_x, 2) + Math.Pow(del_y, 2)); // нашли гипотенузу
                //////////
                r_track = radius_vector(loc, rece_pos);
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

                /* sign settings for rece position */
                if (rece_sign == true)
                {
                    this.rece_bias += 0.005;
                }
                else
                {
                    this.rece_bias -= 0.005;
                }

                this.tmpPonit = loc;
                Task.Delay(1);
            }
        }
    }
}
