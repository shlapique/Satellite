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
        private Point loc_earth; // location of Earth
        private Point center; // center 

        private Point center_new; // center_new

        private Point center_new2;

        double angle, angle1, angle2 = 0;


        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen pen = new Pen(Color.Black);
        Bitmap bm;

        Bitmap bm2;

        Graphics g;

        Graphics g2;

        Image im;

        Point tmpPonit = new Point();

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);

            im = Image.FromFile("H:\\projects\\mai\\isrpps\\cursed\\img\\earth.jpg");


            bm2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g2 = Graphics.FromImage(bm2);

            this.Size = new Size(800, 450);
            loc1 = new Point((ClientSize.Width / 2) - (pictureBox1.Width / 2), (ClientSize.Height / 2) - (pictureBox1.Height / 2));
            loc_earth = new Point(loc1.X + (pictureBox1.Width / 2) - (pictureBox2.Width / 2), loc1.Y + (pictureBox1.Height / 2) - (pictureBox2.Height / 2));
            center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

            center_new = new Point(pictureBox1.Width / 2 - 50, pictureBox1.Height / 2 - 50);

            //center_new2 = new Point()

            pictureBox1.Location = loc1;
            //pictureBox2.Location = loc_earth;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);

            int x, y;

            x = (int)(center.X + 20 * Math.Cos(angle));
            y = (int)(center.Y + 160 * Math.Sin(angle));

            Point loc = new Point(x - 7, y - 7);

            double angleStart = Math.Acos(20.0 / 50.0);
            angleStart = angleStart * 180.0 / Math.PI;

            double b = Math.Abs(loc.X - center.X); // b

            double tmp_angle = Math.Acos(b / 50.0);
            tmp_angle = tmp_angle * 180.0 / Math.PI;

            if (tmpPonit.Y >= loc.Y & (tmp_angle <= angleStart || tmp_angle >= 360 - angleStart * 2.0))
            {

                Rectangle sat = new Rectangle(loc, new Size(14, 14));
                // Fill ellipse on screen.
                //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс


                g.FillEllipse(redBrush, sat);

                g.DrawImage(im, center_new);

                g.DrawArc(pen, (float)center.X - 20, (float)center.Y - 160, 40, 320, (float)angleStart, 360 - (float)angleStart * 2);
            }
            else
            {      
                //означает, что точка двигается вниз = > точка и ось должна быть выше пикчи Земли
                g.DrawImage(im, center_new);

                Rectangle sat = new Rectangle(loc, new Size(14, 14));
                // Fill ellipse on screen.
                //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс


                //g.DrawArc(pen, (float)center.X - 20, (float)center.Y - 160, 40, 320, 90, 220);
                g.DrawArc(pen, (float)center.X - 20, (float)center.Y - 160, 40, 320, (float)angleStart, 360 - (float)angleStart * 2);

                g.FillEllipse(redBrush, sat);

            }
            tmpPonit = loc;

            /*

            /////////////////////////////////
            int x1, y1;

            g.DrawImage(im, loc_earth);

            x1 = (int)(center.X + 120 * Math.Cos(angle));
            y1 = (int)(center.Y + 150 * Math.Sin(angle));
            Point loc1 = new Point(x1 - 7, y1 - 7);
            Rectangle sat1 = new Rectangle(loc1, new Size(14, 14));
            // Fill ellipse on screen.
            g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 120, center.Y + 150, center.X + 120, center.Y - 150)); //Рисует эллипс
            g.FillEllipse(redBrush, sat1);
            /////////////////////////////////
            int x2, y2;

            x2 = (int)(center.X + 20* Math.Cos(angle));
            y2 = (int)(center.Y + 160 * Math.Sin(angle));
            Point loc2 = new Point(x2 - 7, y2 - 7);
            Rectangle sat2 = new Rectangle(loc2, new Size(14, 14));
            // Fill ellipse on screen.

           

            g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс
            g.FillEllipse(redBrush, sat2);

           */

            pictureBox1.Image = bm;

            label1.Text = "X: " + loc.X + " Y: " + loc.Y + "\n  angle: " + angleStart + " \n angle_tmp: " + tmp_angle + " \n center: " + center.X + "," + center.Y;

            angle -= 0.0123; Task.Delay(1);
        }



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
