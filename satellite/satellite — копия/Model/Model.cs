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

        /*=========*/
        public void receiver_position(Point center, ref Point ReceiverPosition, ref double ReceiverBias, ref bool ReceiverSign, ref bool VisibleReceiverFlag) // position for rece dot
        {
            ReceiverPosition.X = (int)(ReceiverPosition.X + ReceiverBias);

            if (Calculation.radius_vector(ReceiverPosition, center) < 50.0)
            {
                /* pseudocode
                if(VisibleReceiverFlag == false)
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
                if ((ReceiverPosition.X - center.X) > 0) // right side relatively center
                {
                    ReceiverSign = false;
                    VisibleReceiverFlag = false;
                }
                else
                {
                    ReceiverSign = true;
                    VisibleReceiverFlag = true;
                }
            }
        }
        /*=========*/
        public void ReceiverBiasChange(bool ReceiverSign, ref double ReceiverBias)
        {
            /* sign settings for rece position */
            if (ReceiverSign == true)
            {
                if (ReceiverBias < 0)
                {
                    ReceiverBias = 0;
                }
                ReceiverBias += 1;
            }
            else
            {
                if(ReceiverBias > 0)
                {
                    ReceiverBias = 0;
                }
                ReceiverBias -= 1;
            }
            Task.Delay(1);
        }
        /*=========*/

        //classes +++++++++
        public class Earth //
        {
            public Point center;
            public Point ReceiverPosition;
            public Point EarthPosition; // EarthPosition
            public Image im;
            public Earth(Point center, Point ReceiverPosition)
            {
                this.center = center;
                this.ReceiverPosition = ReceiverPosition;
                this.EarthPosition = new Point(center.X - 50, center.Y - 50);
                string path = Environment.CurrentDirectory + "\\img\\earth.png";
                this.im = Image.FromFile(path);
            }

            /*=========image proc ** for earth rotation **********/
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
                double r = Calculation.radius_vector(center, point);
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
            /*=========image proc ************/
        }

        public class Orbit
        {
            int angle_min = 0;
            int angle_max = 180; // 
            int rad_min = 52;
            int rad_max = 150;

            public int a = 150;
            public int b;
            public int min_b = 0;
            public double angle;
            //
            public Point center;
            public Point ReceiverPosition;
            //
            public double angle_rotation;
            public double angleStart;
            public double angle_deg;

            /*=========*/
            public Point apogee_1; //верхний(левый) и 
            public Point apogee_2; //нижний(правый)
            /*=========*/

            Random rand = new Random();
            public Orbit(Point center, Point ReceiverPosition) 
            {
                random_orbit();
                this.angle_deg = angle;
                Calculation.rad_to_deg(ref angle_deg);
            }

            public void random_orbit()
            {
                //задаем слачайные радиусы эллипсов и углы
                this.b = rand.Next(rad_min, rad_max);
                this.angle = rand.Next(angle_min, angle_max); // 
                this.angle = this.angle * Math.PI / 180.0; // 0 .. pi
            }
            /*=========*/
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
                Calculation.rotation(ref apogee_1, center, angle);
                Calculation.rotation(ref apogee_2, center, angle);
            }
            /*=========*/
        }

        public class Satellite
        {
            public Point loc;
            /*========= for intersecting orbits */
            public Point tmpPonit;
            /*=========*/
            int x, y; // начальное положение
            double coord_x, coord_y;
            public double r; // radius-vector
            public double r_vector;
            // ***********
            public double r_track;
            public int sat_stat;
            public bool ReceiverSign = true; // true - for increasing x (+), false - .;. (-)
            public double ReceiverBias;
            public bool VisibleReceiverFlag = true; // -> controller */
            // *********** 
            public Orbit orbit = null;
            /*-------------*/
            public double velo_coef; // коэфф скорости 
            public int size;
            /*-------------*/
            Random rand = new Random();
            public Point loc_begin;
            public int sign; // направл вращения 

            public Satellite(Point center, Point ReceiverPosition)
            {
                this.tmpPonit = new Point();
                this.orbit = new Orbit(center, ReceiverPosition);
                loc_begin = new Point();
                start_position();
                this.orbit.center = center;
                this.orbit.ReceiverPosition = ReceiverPosition;
                loc_begin = loc;
                sign = rand.Next(0, 100);
                velo_coef = 1;
                size = 4;
            }

            public void start_position()
            {
                this.coord_x = orbit.b * Math.Cos(orbit.angle_rotation);
                this.coord_y = orbit.a * Math.Sin(orbit.angle_rotation);
                // повернем x, y
                Calculation.rotation(ref coord_x, ref coord_y, orbit.angle);

                this.x = (int)(orbit.center.X + coord_x);
                this.y = (int)(orbit.center.Y + coord_y);

                this.loc = new Point(x - size, y - size); // default location
            }

            public void position() // position every tick
            {
                this.coord_x = orbit.b * Math.Cos(orbit.angle_rotation);
                this.coord_y = orbit.a * Math.Sin(orbit.angle_rotation);
                // повернем x, y
                Calculation.rotation(ref coord_x, ref coord_y, orbit.angle);
                this.x = (int)(orbit.center.X + coord_x);
                this.y = (int)(orbit.center.Y + coord_y);
                this.loc = new Point(x - size, y - size);

                // считаем радиус-вектор до точки ресивера
                r_track = Calculation.radius_vector(loc, orbit.ReceiverPosition);
                /*=========*/
                double del_x, del_y;
                del_x = loc.X - orbit.center.X;
                del_y = loc.Y - orbit.center.Y;
                r = Math.Sqrt(Math.Pow(del_x, 2) + Math.Pow(del_y, 2)); // нашли гипотенузу
                /*=========*/
                //////////
            }

            public void angle_change(Point loc)
            {
                if(sign > 50)
                {
                    orbit.angle_rotation -= 0.01 * velo_coef;
                }
                else
                {
                    orbit.angle_rotation += 0.01 * velo_coef;
                }
                Task.Delay(1);

                /*=========*/
                /* sign settings for rece position */
                if (ReceiverSign == true)
                {
                    this.ReceiverBias += 0.005;
                }
                else
                {
                    this.ReceiverBias -= 0.005;
                }
                //for intersecting orbits
                this.tmpPonit = loc;
                /*=========*/
            }
        }
    }
}
