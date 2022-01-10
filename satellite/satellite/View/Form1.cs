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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Point loc1; //loc picture box1
        private Point center; // center 
        private Point center_new; // center_new

        Controller controller = null;

        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen pen = new Pen(Color.Black);
        Bitmap bm;
        Graphics g;
        Image im;

        public Form1()
        {
            InitializeComponent();
            label5.Text = "" + 0;

            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bm);

            im = Image.FromFile("H:\\projects\\mai\\isrpps\\cursed\\img\\earth.png");
            //g.DrawImage(im, center_new);
            this.Size = new Size(800, 450);
            loc1 = new Point((ClientSize.Width / 2) - (pictureBox1.Width / 2), (ClientSize.Height / 2) - (pictureBox1.Height / 2));
            center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            center_new = new Point(pictureBox1.Width / 2 - 50, pictureBox1.Height / 2 - 50);
            pictureBox1.Location = loc1;
            
            /////////////
            controller = new Controller(
                this,
                ref im,
                ref g,
                ref bm,
                ref center,
                ref pictureBox1,
                ref center_new,
                ref label6
                );
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // задаем коэф. n, который будет домножаться на angle
            controller.velo_coef(trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            controller.sat_size(trackBar2.Value);
        }

        private void Form1_Load(object sender, EventArgs e) // done
        {   
            controller.timer_init(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int Count = controller.Add(); // добавление спутника 
            if(Count == -1)
            {
                MessageBox.Show(" Для начала задайте точку приемника \n Тыкните в любое место на Земле");
                label5.Text = "" + (Count + 1);
            }
            else
            {
                label5.Text = "" + Count;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int Count = controller.Delete();
            if (Count == -1)
            {
                MessageBox.Show("STACK IS EMPTY !");
                label5.Text = "" + (Count + 1);
            }
            else
            {
                label5.Text = Count + " Visible Satellites" ;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            Point point = mouseEventArgs.Location;
            int flag = controller.rece_point_set(point);
            
            switch(flag)
            { 
                case 1:
                    MessageBox.Show("Точка должна быть 'внутри' Земли ");
                    break;
                case 54:
                    MessageBox.Show("Точка задана успешно !");
                    break;
                case -1:
                    MessageBox.Show("Точка уже была задана до этого ");
                    break;
            }


        }
    }
}
