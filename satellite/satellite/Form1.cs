using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Point loc1; //loc picture box1
        private Point center; // center 
        private Point center_tmp; // center 


        private Point center_new; // center_new

        double angle, angle1, angle2 = 0;

        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen pen = new Pen(Color.Black);
        Bitmap bm;

        //Bitmap bm2;

        Graphics g;

        //Graphics g2;

        Image im;

        Point tmpPonit = new Point();

        // радиусы эллипсов
        int a = 0; int b = 0;

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);

            im = Image.FromFile("H:\\projects\\mai\\isrpps\\cursed\\img\\earth.jpg");


            //bm2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            //g2 = Graphics.FromImage(bm2);

            this.Size = new Size(800, 450);
            loc1 = new Point((ClientSize.Width / 2) - (pictureBox1.Width / 2), (ClientSize.Height / 2) - (pictureBox1.Height / 2));
            center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            center_new = new Point(pictureBox1.Width / 2 - 50, pictureBox1.Height / 2 - 50);

            pictureBox1.Location = loc1;
        }
        // //////////////////
        // ф-я поворота эллипса 
        public void rotation(ref int x, ref int y, double angle_rot)
        {
            int x_t = x;
            x = (int)(x * Math.Cos(angle_rot) - y * Math.Sin(angle_rot));
            y = (int)(x_t * Math.Sin(angle_rot) + y * Math.Cos(angle_rot));
        }

        public void rotation(ref double x, ref double y, double angle_rot)
        {
            double x_t = x;
            x = x * Math.Cos(angle_rot) - y * Math.Sin(angle_rot);
            y = x_t * Math.Sin(angle_rot) + y * Math.Cos(angle_rot);
        }

        public void rotation(ref Point point, double angle_rot)
        {
            int point_x = point.X;
            point.X = (int)(point.X * Math.Cos(angle_rot) - point.Y * Math.Sin(angle_rot));
            point.Y = (int)(point_x * Math.Sin(angle_rot) + point.Y * Math.Cos(angle_rot));
        }


        // /////////////////////
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);

            a = 160; b = 20;

            int x, y;

            double coord_x = b * Math.Cos(angle);
            double coord_y = a * Math.Sin(angle);

            double radius_elli_x = center.X - b;
            double radius_elli_y = center.Y - a;

            // повернем x, y
            rotation(ref coord_x, ref coord_y, 0.5);
            //// повернем арку
            //rotation(ref radius_elli_x, ref radius_elli_y, 0.5);

            ///+++++++++++++++++++
            center_tmp = center;
            rotation(ref center_tmp, 0.5);


            x = (int)(center.X + coord_x);
            y = (int)(center.Y + coord_y);


            Point loc = new Point(x - 7, y - 7);

            double angleStart = Math.Acos((double)b / 50.0);
            angleStart = angleStart * 180.0 / Math.PI;

            // считаем радиус-вектор до центра 
            double del_x, del_y;

            del_x = loc.X - center.X;
            del_y = loc.Y - center.Y;
            double r = Math.Sqrt(Math.Pow(del_x, 2) + Math.Pow(del_y, 2)); // нашли гипотенузу

            if (tmpPonit.Y >= loc.Y & r <= 50.0)
            {
                Rectangle sat = new Rectangle(loc, new Size(14, 14)); // 
                // Fill ellipse on screen.
                //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс

                g.FillEllipse(redBrush, sat);

                g.DrawImage(im, center_new);

                g.TranslateTransform(center.X, center.Y); //это центр вращения
                label2.Text = "center_tmp: " + center.X / 2 + ", " + -center.Y / 2;
                g.RotateTransform(30.0F); //Поворачиваем
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
                g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
            }
            else
            {      
                //означает, что точка двигается вниз = > точка и ось должна быть выше пикчи Земли
                g.DrawImage(im, center_new);

                Rectangle sat = new Rectangle(loc, new Size(14, 14));
                // Fill ellipse on screen.
                //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс
                //---------
                //g.DrawArc(pen, (float)center.X - 20, (float)center.Y - 160, 40, 320, 90, 220);
                //g.DrawArc(pen, (float)radius_elli_x, (float)radius_elli_y, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
                g.TranslateTransform(center.X, center.Y); //это центр вращения
                label2.Text = "center_tmp: " + center.X / 2 + ", " + -center.Y / 2;
                g.RotateTransform(30.0F); //Поворачиваем
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawArc(pen, (float)radius_elli_x, (float)radius_elli_y, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
                g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно

                g.FillEllipse(redBrush, sat);

                

            }
            tmpPonit = loc;

            /////////////////////////////////
            int x1, y1;

            //g.DrawImage(im, center_new);

            x1 = (int)(center.X + 120 * Math.Cos(angle));
            y1 = (int)(center.Y + 150 * Math.Sin(angle));

            Point loc1 = new Point(x1 - 7, y1 - 7);
            Rectangle sat1 = new Rectangle(loc1, new Size(14, 14));
            // Fill ellipse on screen.
            g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 120, center.Y + 150, center.X + 120, center.Y - 150)); //Рисует эллипс
            g.FillEllipse(redBrush, sat1);

            pictureBox1.Image = bm;

            label1.Text = "X: " + loc.X + " Y: " + loc.Y + "\n  angle: " + angleStart + " \n r: " + r + " \n center: " + center.X + "," + center.Y;

            angle -= 0.0123; Task.Delay(1);
        }
        // 2 вида орбит: пересекающая Землю - в двух местах сразу всегда
        // 


        public class Earth
        {
            public int orbit_count = 1;



        }

        private class Orbit : Earth
        {
            public Orbit() { }
            private int r = 150;

            public int a { get { return r; } set { r = value; } }

        }

        private class Sat : Orbit
        {
            //Rectangle sat = new Rectangle();


        }


        private void button1_Click(object sender, EventArgs e)
        {

            Orbit orbit = new Orbit();
            int r = orbit.a;
            g = this.CreateGraphics();
            pen = new Pen(Color.Black); // Нашему карандашу присваиваем зеленый цвет

            if (true)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bmp);
                {
                    //g.Clear(Color.White);
                    g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - r, center.Y + r, center.X + r, center.Y - r)); //Рисует эллипс
                }
                pictureBox1.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer x = new Timer();
            x.Interval = 1;
            x.Start();
            x.Tick += new EventHandler(timer1_Tick);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }


    }
}
