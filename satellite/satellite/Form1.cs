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

        double angle, angle1 = 0;


        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen pen = new Pen(Color.Black);
        Bitmap bm;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);

            this.Size = new Size(800, 450);
            loc1 = new Point((ClientSize.Width / 2) - (pictureBox1.Width / 2), (ClientSize.Height / 2) - (pictureBox1.Height / 2));
            loc_earth = new Point(loc1.X + (pictureBox1.Width / 2) - (pictureBox2.Width / 2), loc1.Y + (pictureBox1.Height / 2) - (pictureBox2.Height / 2));
            center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            pictureBox1.Location = loc1;
            pictureBox2.Location = loc_earth;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);

            int x, y;

            x = (int)(center.X + 150 * Math.Cos(angle));
            y = (int)(center.Y + 150 * Math.Sin(angle));
            Point loc = new Point(x - 7, y - 7);
            Rectangle sat = new Rectangle(loc, new Size(14, 14));
            // Fill ellipse on screen.
            g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 150, center.Y + 150, center.X + 150, center.Y - 150)); //Рисует эллипс
            g.FillEllipse(redBrush, sat);
            /////////////////////////////////
            int x1, y1;

            x1 = (int)(center.X + 60 * Math.Cos(angle));
            y1 = (int)(center.Y + 150 * Math.Sin(angle));
            Point loc1 = new Point(x1 - 7, y1 - 7);
            Rectangle sat1 = new Rectangle(loc1, new Size(14, 14));
            // Fill ellipse on screen.
            g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 60, center.Y + 150, center.X + 60, center.Y - 150)); //Рисует эллипс
            g.FillEllipse(redBrush, sat1);


            pictureBox1.Image = bm;
            angle -= 0.0223; Task.Delay(1);
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
