using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace satellite.Controller
{
    public class Controller
    {
        Form1 form = null;
        Model.Model model = null;

        private Point center; // center 
        private Point center_new; // center_new
        private int coef; // for evert sat

        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.BlueViolet);
        Pen pen = new Pen(Color.Black);

        // +++++++++ 
        Bitmap bm;
        Graphics g;
        Image im;
        PictureBox pictureBox1;

        //--for debug 
        Label label6;
        // +++++++++ 

        public Controller()
        { }
      
        public Controller(
            Form1 form, 
            ref Image im, 
            ref Graphics g, 
            ref Bitmap bm, 
            ref Point center, 
            ref PictureBox pictureBox1, 
            ref Point center_new, 
            ref Label label6
            )
        {
            MessageBox.Show("Connection controler");
            this.form = form;
            this.model = new Model.Model(this);

            this.im = im;
            this.g = g;
            this.bm = bm;
            this.center = center;
            this.pictureBox1 = pictureBox1;
            this.center_new = center_new;

            this.coef = 1;
            // ______
            this.label6 = label6;
            // ______
        }

        public void timer_init(object sender, EventArgs e)
        {
            Timer x = new Timer();
            x.Interval = 1;
            x.Start();
            x.Tick += new EventHandler(timer1_Tick);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            foreach (var obj in this.model.list)
            {
                obj.position(center);
                obj.velo_coef = coef;
                
                if(obj.r <= 50 & obj.r_loc_tmp_begin <= obj.r_final)
                {
                    Draw_Up(obj.loc, obj.a, obj.b, obj.angleStart, obj.angle_deg);
                }
                else
                {
                    Draw_Down(obj.loc, obj.a, obj.b, obj.angleStart, obj.angle_deg);
                }
                pictureBox1.Image = bm;
                obj.angle_change(obj.loc);

                //////
                label6.Text = "apogee1 = " + obj.apogee_1 + " apogee2 = " + obj.apogee_2 + " \n obj.r_vector1_1 = " + obj.r_vector1_1
                    + " obj.r_vector2_1 = " + obj.r_vector2_1 + " obj.r_vector1_2 = " + obj.r_vector1_2 + " obj.r_vector2_2 = " + obj.r_vector2_2
                    + "\n one = " + obj.one + " two = " + obj.two + " r_stat = " + obj.r_stat + 
                    " \n loc_begin = " + obj.loc_begin;
                //////
            }
        }

        public int Add()
        {
            //добавляем в список новый объект
            model.list.Add(new Model.Model.Sat(center));
            return model.list.Count;
        }

        public int Delete()
        {
            model.list.RemoveAt(model.list.Count - 1);
            return model.list.Count;
        }

        public void velo_coef(int coef)
        {
            this.coef = coef;
        }
        private void Draw_Up(Point loc, int a, int b, double angleStart, double angle)
        {
            Rectangle sat = new Rectangle(loc, new Size(8, 8)); // 
            // Fill ellipse on screen.
            g.FillEllipse(redBrush, sat);
            g.DrawImage(im, center_new);
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
        }

        private void Draw_Down(Point loc, int a, int b, double angleStart, double angle)
        {
            g.DrawImage(im, center_new);
            Rectangle sat = new Rectangle(loc, new Size(8, 8));
            // Fill ellipse on screen.
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
            g.FillEllipse(redBrush, sat);
        }
    }
}
