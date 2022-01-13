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
        public Model.Model model = null;
        public Model.Model.Earth earth = null;

        private Point center; // center 
        private Point EarthPosition; // EarthPosition

        /*=========*/
        private int SetReceiverFlag; // flag for mouse set position
        private Point ReceiverPosition; // point of rece -> to Model
        public bool ReceiverSign = true; // true - for increasing x (+), false - .;. (-)
        public double ReceiverBias; // bias of receiver
        public bool VisibleReceiverFlag = true; // -> controller */ // flag for rece dot visibility
        /*=========*/

        private int coef; // for evert sat
        private int VisibleSatelliteCount; // visible satellite count
        private int size; // size of satellite
        public int SatelliteCount; 

        // Create solid brush.
        SolidBrush Brush = new SolidBrush(Color.BlueViolet);
        SolidBrush brownBrush = new SolidBrush(Color.Brown);

        Pen pen = new Pen(Color.Black);
        Pen pen2 = new Pen(Color.Red);

        // +++++++++ 
        Bitmap bm;
        Graphics g;
        Image im; // of Earth (path)
        PictureBox pictureBox1;

        /*=========*/
        SolidBrush BackColorBrush; // for invisibility
        /*=========*/

        //--for debug 
        Label label6;
        // +++++++++ 

        public Controller()
        { }

        public Controller(
            Form1 form,
            ref Graphics g,
            ref Bitmap bm,
            ref Point center,
            ref PictureBox pictureBox1,
            ref Point EarthPosition,
            ref Label label6
            )
        {
            MessageBox.Show("Выберите точку на поверхности Земли", "(!)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.form = form;
            this.model = new Model.Model(this);

            this.g = g;
            this.bm = bm;
            this.center = center;
            this.pictureBox1 = pictureBox1;
            this.EarthPosition = EarthPosition;

            this.coef = 1; // коэффициент скорости, который домножается на угол смещения
            this.size = 4; // размер спутника
            this.SetReceiverFlag = 0; // флаг, отвечающий за существование точки приемника  
            this.label6 = label6;

            /*=========*/
            Color brushColor = Color.FromArgb(250 / 100 * 25, 255, 0, 0); // invisible color
            this.BackColorBrush = new SolidBrush(brushColor);
            /*=========*/

            earth = new Model.Model.Earth(center, ReceiverPosition);
            this.EarthPosition = earth.EarthPosition;
            this.im = earth.im;
            Draw_Earth(this.im, this.EarthPosition);
            pictureBox1.Image = bm;
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
            Draw_Earth(this.im, this.EarthPosition);

            /* changing of ReceiverPosition*/
            Point tmp_rece_point = new Point(ReceiverPosition.X - 3, ReceiverPosition.Y - 3);
            Rectangle rec = new Rectangle(tmp_rece_point, new Size(6, 6));
            // Fill ellipse on screen.
            g.FillEllipse(brownBrush, rec);

            this.VisibleSatelliteCount = 0;
            foreach (var satellite in this.model.list)
            {
                satellite.position();
                satellite.velo_coef = coef;

                int visi_flag = 0; //

                if (satellite.r_track <= satellite.orbit.b) // рад-вектор до сата из ресивера 
                {
                    visi_flag += 1;
                    this.VisibleSatelliteCount += 1;
                    Point tmp = new Point(satellite.loc.X + size, satellite.loc.Y + size);
                    g.DrawLine(pen2, ReceiverPosition, tmp);
                }

                if(visi_flag == 1)
                {
                    this.Brush = new SolidBrush(Color.Red);
                }
                else
                {
                    this.Brush = new SolidBrush(Color.BlueViolet);
                }
                Draw_all(satellite.loc, satellite.orbit.a, satellite.orbit.b, satellite.orbit.angleStart, satellite.orbit.angle_deg, this.Brush);

                pictureBox1.Image = bm;
                satellite.angle_change(satellite.loc);
            }

            label6.Text = "Visible Satellite: " + this.VisibleSatelliteCount;
        }

        public DialogResult AddSatellite()
        {
            //добавляем в список новый объект
            if (SetReceiverFlag == 0) // enum type ++
            {
                return DialogResult.Retry;
            }
            else
            {
                model.list.Add(new Model.Model.Satellite(center, ReceiverPosition));  // Satellite ++
                this.SatelliteCount = model.list.Count;
                return DialogResult.OK;
            }
        }

        public DialogResult DeleteSatellite()
        {
            // добавить dispose object !
            if (model.list.Count == 0)
            {
                return DialogResult.No;
            }
            else
            {
                model.list.RemoveAt(model.list.Count - 1);
                this.SatelliteCount = model.list.Count;
                return DialogResult.OK;
            }
        }

        public void velo_coef(int coef)
        {
            this.coef = coef;
        }

        public void sat_size(int size)
        {
            this.size = size;
        }

        public DialogResult rece_point_set(Point point)// enum type !!!!!!!!!
        {
            int r = (int)Math.Sqrt(Math.Pow(center.X - point.X, 2) + Math.Pow(center.Y - point.Y, 2));
            if (SetReceiverFlag == 0) // nothin set
            {
                if (r <= 50)
                {
                    this.ReceiverPosition = point; // 
                    this.SetReceiverFlag = 1;
                    return DialogResult.OK;
                }
                else
                {
                    return DialogResult.Retry;
                }
            }
            else
            {
                return DialogResult.Cancel;
            }
        }

        private void Draw_Up(Point loc, int a, int b, double angleStart, double angle, SolidBrush brush)
        {
            Rectangle sat = new Rectangle(loc, new Size(size * 2, size * 2)); // 
            // Fill ellipse on screen.
            g.FillEllipse(brush, sat);
            //g.DrawImage(im, EarthPosition);
            g.TranslateTransform(center.X, center.Y); //это центр вращения
            g.RotateTransform((float)angle); //Поворачиваем
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawArc(pen, (float)center.X - b, (float)center.Y - a, b * 2, a * 2, (float)angleStart, 360 - (float)angleStart * 2);
            g.ResetTransform(); //Возвращаем точку отчета на 0, чтоб дальше рисовать как обычно
        }

        private void Draw_Down(Point loc, int a, int b, double angleStart, double angle, SolidBrush brush)
        {
            //g.DrawImage(im, EarthPosition);
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

        private void Draw_Earth(Image im, Point EarthPosition)
        {
            g.DrawImage(im, EarthPosition);
        }
    }
}
