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

        /*=========*/
        private int set_rece_flag; // flag for mouse set position
        private Point rece_pos; // point of rece -> to Model
        public bool rece_sign = true; // true - for increasing x (+), false - .;. (-)
        public double rece_bias;
        public bool visi_rece_flag = true; // -> controller */ // flag for rece dot visibility
        /*=========*/

        private int coef; // for evert sat
        private int sat_count; // satellite count

        private int size;


        // Create solid brush.
        SolidBrush Brush = new SolidBrush(Color.BlueViolet);
        SolidBrush brownBrush = new SolidBrush(Color.Brown);

        Pen pen = new Pen(Color.Black);
        Pen pen1 = new Pen(Color.Purple);
        Pen pen2 = new Pen(Color.Red);

        // +++++++++ 
        Bitmap bm;
        Graphics g;
        Image im;
        PictureBox pictureBox1;

        SolidBrush BackColorBrush;

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
            MessageBox.Show("Выберите точку на поверхности Земли");
            this.form = form;
            this.model = new Model.Model(this);

            this.im = im;
            this.g = g;
            this.bm = bm;
            this.center = center;
            this.pictureBox1 = pictureBox1;
            this.center_new = center_new;

            this.coef = 1;
            this.size = 4;
            this.set_rece_flag = 0;

            // ______
            this.label6 = label6;
            // ______
            Color brushColor = Color.FromArgb(250 / 100 * 25, 255, 0, 0); // invisible color
            this.BackColorBrush = new SolidBrush(brushColor); 
            //-----------
            //earth obj
            //earth = new Model.Model.Earth(); // new Earth !
            Draw_Earth(this.im, this.center_new);
            pictureBox1.Image = bm;
            //

            // ______
        }

        public void timer_init(object sender, EventArgs e)
        {
            Timer x = new Timer();
            x.Interval = 1; // 1ms
            x.Start();
            x.Tick += new EventHandler(timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            Draw_Earth(this.im, this.center_new);

            /* changing of rece_pos*/
            Point tmp_rece_point = new Point(rece_pos.X - 3, rece_pos.Y - 3);
            Rectangle rec = new Rectangle(tmp_rece_point, new Size(6, 6));
            // Fill ellipse on screen.
            g.FillEllipse(brownBrush, rec);

            this.sat_count = 0;
            foreach (var obj in this.model.list)
            {
                obj.position();
                obj.velo_coef = coef;

                int visi_flag = 0; // rename!

                if (obj.r_track <= obj.b) // рад-вектор до сата из ресивера 
                {
                    visi_flag += 1;
                    this.sat_count += 1;
                    Point tmp = new Point(obj.loc.X + size, obj.loc.Y + size);
                    g.DrawLine(pen2, rece_pos, tmp);
                }

                if(visi_flag == 1)
                {
                    this.Brush = new SolidBrush(Color.Red);
                }
                else
                {
                    this.Brush = new SolidBrush(Color.BlueViolet);
                }
                Draw_all(obj.loc, obj.a, obj.b, obj.angleStart, obj.angle_deg, this.Brush);

                pictureBox1.Image = bm;
                obj.angle_change(obj.loc);
            }


            label6.Text = "Visible Satellite: " + this.sat_count;
        }

        public int AddSatellite()
        {
            //добавляем в список новый объект
            if(set_rece_flag == 0) // enum type ++
            {
                return -1;
            }
            model.list.Add(new Model.Model.Satellite(center, rece_pos));  // Satellite ++ 
            return model.list.Count;
        }

        public int DeleteSatellite()
        {
            // добавить dispose object !
            if(model.list.Count == 0)
            {
                return -1;
            }
            model.list.RemoveAt(model.list.Count - 1);
            return model.list.Count;
        }

        public void velo_coef(int coef)
        {
            this.coef = coef;
        }

        public void sat_size(int size)
        {
            this.size = size;
        }

        public int rece_point_set(Point point)
        {
            int r = (int)Math.Sqrt(Math.Pow(center.X - point.X, 2) + Math.Pow(center.Y - point.Y, 2));
            if (set_rece_flag == 0) // nothin set
            {
                if (r <= 50)
                {
                    this.rece_pos = point; // 
                    this.set_rece_flag = 1;
                    return 54;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return -1;
            }
        }

        private void Draw_Up(Point loc, int a, int b, double angleStart, double angle, SolidBrush brush)
        {
            Rectangle sat = new Rectangle(loc, new Size(size * 2, size * 2)); // 
            // Fill ellipse on screen.
            g.FillEllipse(brush, sat);
            //g.DrawImage(im, center_new);
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
        }

        private void Draw_Down(Point loc, int a, int b, double angleStart, double angle, SolidBrush brush)
        {
            //g.DrawImage(im, center_new);
            Rectangle sat = new Rectangle(loc, new Size(size * 2, size * 2));
            // Fill ellipse on screen.
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
            g.FillEllipse(brush, sat);
        }

        private void Draw_all(Point loc, int a, int b, double angleStart, double angle, SolidBrush brush)
        {
            Draw_Up(loc, a, b, angleStart, angle, brush);
            Draw_Down(loc, a, b, angleStart, angle, brush);
        }

        private void Draw_Earth(Image im, Point center_new)
        {
            g.DrawImage(im, center_new);
        }
    }
}
