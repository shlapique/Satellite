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

        private Point loc1; //loc picture box1
        private Point center; // center 
        private Point center_tmp; // center
        private Point center_new; // center_new

        // Create solid brush.
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen pen = new Pen(Color.Black);

        Point tmpPonit = new Point();
        
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
      
        public Controller(Form1 form, ref Image im, ref Graphics g, ref Bitmap bm, ref Point center, ref PictureBox pictureBox1, ref Point center_new, ref Label label6)
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

            // ______
            this.label6 = label6;
        }

        public void timer_init(object sender, EventArgs e)
        {
            //MessageBox.Show("timer");
            Timer x = new Timer();
            x.Interval = 1;
            x.Start();
            x.Tick += new EventHandler(timer1_Tick);
        }

        /*
        private void draw()
        {
            g.Clear(pictureBox1.BackColor);

        }
        */
        ////
        int countTick;
        ////
        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            foreach (var obj in this.model.stack)
            {
                obj.position(center);
                //obj.entry_flag = false;
                //if(obj.tmpPonit.Y >= obj.loc.Y & obj.r <= 50.0)
                if (obj.r <= 50.0)
                {
                    //obj.entry_flag = true;

                    Draw_Up(obj.loc, obj.a, obj.b, obj.angleStart, obj.angle_deg);
                }
                else
                {
                    Draw_Down(obj.loc, obj.a, obj.b, obj.angleStart, obj.angle_deg);
                }
                //////
                countTick += 1;
                //////
                label6.Text = "obj.loc.X = " + obj.loc.X + "obj.loc.Y = " + obj.loc.Y + " TICK " + countTick + "\nTMPPONIT = " + obj.tmpPonit + "\n a = " + obj.a;
                pictureBox1.Image = bm;
                //////
                obj.angle_change(obj.loc);
            }
        }

        public int Add()
        {
            //добавляем в стек новый объект
            MessageBox.Show("VOID ADD IN CONTROLLER");
            model.stack.Push(new Model.Model.Sat(center));
            return model.stack.Count;
        }

        public int Delete()
        {
            model.stack.Pop();
            return model.stack.Count;
        }

        private void Draw_Up(Point loc, int a, int b, double angleStart, double angle)
        {
            //label6.Text = "LOC " + loc;
            Rectangle sat = new Rectangle(loc, new Size(14, 14)); // 
            // Fill ellipse on screen.
            //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс
            g.FillEllipse(redBrush, sat);
            g.DrawImage(im, center_new);
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            //label2.Text = "center_tmp: " + center.X / 2 + ", " + -center.Y / 2;
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
        }

        private void Draw_Down(Point loc, int a, int b, double angleStart, double angle)
        {
            //label6.Text = "LOC " + loc;
            //означает, что точка двигается вниз = > точка и ось должна быть выше пикчи Земли
            g.DrawImage(im, center_new);
            Rectangle sat = new Rectangle(loc, new Size(14, 14));
            // Fill ellipse on screen.
            //g.DrawEllipse(pen, Rectangle.FromLTRB(center.X - 20, center.Y + 160, center.X + 20, center.Y - 160)); //Рисует эллипс
            //---------
            //g.DrawArc(pen, (float)center.X - 20, (float)center.Y - 160, 40, 320, 90, 220);
            //g.DrawArc(pen, (float)radius_elli_x, (float)radius_elli_y, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            //label2.Text = "center_tmp: " + center.X / 2 + ", " + -center.Y / 2;
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
            g.FillEllipse(redBrush, sat);
        }
    }
}
