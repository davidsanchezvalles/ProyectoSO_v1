using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading; 
using System.Runtime.InteropServices; //para poder cambiar color barra




namespace prueba_juego
{

    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Bitmap Image;
            int a = 0; //variable global 
            bool stop = false;
            ProgressBar pbar= new ProgressBar();
            
        

 
        }
 

 

   

        private void pBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            timer1.Enabled = true;
            timer1.Interval = 150;
            pBar1.Maximum = 10;
            pBar1.Minimum = 0;
            pBar1.SetState(2); //color rojo
            pBar2.Maximum = 10;
            pBar2.Minimum = 0;
            
            timer1.Tick += new EventHandler(timer1_Tick);
            pBar1.Value = 0;

            timer1.Start();

            timer2.Enabled = true;
            timer2.Interval = 50;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Stop();


           
        
        }
     

       
        public int max=0;
        private void timer1_Tick(object sender, EventArgs e)
        {

                
                if (max==0)
                {
                    pBar1.Value++;
                    pBar2.Value++;
                    if (pBar1.Value == 10){
                        max=1;
                    }
                
                }
                if (max == 1)
                   
                {
                    pBar1.Value--;
                    pBar2.Value--;
                    if (pBar1.Value == 0)
                    {
                        max = 0;
                    }
                    
                
                }    
            

        }

    


        /// method to rotate an image either clockwise or counter-clockwise

        // <param name="img">the image to be rotated</param>
        // <param name="rotationAngle">the angle (in degrees).
        // NOTE: 
        // Positive values will rotate clockwise
        // negative values will rotate counter-clockwise
        // </param>
        // <returns></returns>
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            //gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Vx = pBar1.Value * Math.Cos((45*2*PI)/360)*3.95; //numero 27 para cuadrar
            Voy = pBar1.Value * Math.Sin((45 * 2 * PI) / 360) * 3.95;
            timer2.Start();

        }

        public double t=0,h,Vy,grado=45,a=1.89; //acceleracion escogida convenientemente para los datos del problema
        public double Vx , Voy;
        public int x = 0, y;
        public const double PI = 3.1415926535897931;
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            t = t + 0.33;//temporizador puesto en 500ms=0,5s
            x = Convert.ToInt32(250 + Vx * t);
            Vy =  Voy - a * t;
            y =Convert.ToInt32(146 - Voy * t + 0.5 * a * t*t); //parabola invertida ja que el eix Y va de adalt cap abaix i no de abaix cap adalt
           
            pictureBox3.Location = new Point(x, y);
          
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
        

     
        
                

     
    }
    public static class ModifyProgressBarColor  //top level static class
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }

    }
}
