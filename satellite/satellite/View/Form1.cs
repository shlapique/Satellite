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
        private Point EarthPosition; // EarthPosition

        Controller controller = null;
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
            EarthPosition = new Point(pictureBox1.Width / 2 - 50, pictureBox1.Height / 2 - 50);
            pictureBox1.Location = loc1;
            
            /////////////
            controller = new Controller(
                this,
                ref im,
                ref g,
                ref bm,
                ref center,
                ref pictureBox1,
                ref EarthPosition,
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
            DialogResult dialogResult;
            dialogResult = controller.AddSatellite(); // добавление спутника

            switch (dialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Retry:
                    MessageBox.Show(" Для начала задайте точку приемника \n Тыкните в любое место на Земле", "(!)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            label5.Text = "" + controller.sat_count;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            dialogResult = controller.DeleteSatellite();

            switch (dialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.No:
                    MessageBox.Show(" Нет объектов в памяти! ", "(!)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            label5.Text = "" + controller.sat_count;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var mouseEventArgs = e as MouseEventArgs;
            Point point = mouseEventArgs.Location;
            DialogResult dialogResult;
            dialogResult = controller.rece_point_set(point);
            
            switch(dialogResult)
            { 
                case DialogResult.Retry:
                    MessageBox.Show("Точка должна быть 'внутри' Земли ", "(!)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case DialogResult.OK:
                    MessageBox.Show("Точка задана успешно !", "DONE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case DialogResult.Cancel:
                    MessageBox.Show("Точка уже была задана до этого ", "(!)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
