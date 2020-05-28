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

           // Bitmap Image;//no implementat!!!!


          
        

 
        }
 
        private void power(){

            timer1.Stop();

        }

        private void f_grade()
        {

            string[] trozos = label3.Text.Split(' ');
            grado = Convert.ToInt32(trozos[0]);
            Vx = pBar1.Value * Math.Cos((grado * 2 * PI) / 360) * 3.95; //numero  3.95 elegido convenientemente
            Voy = pBar1.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;
            timer3.Stop();
            vida = 0;
            timer2.Start();

        }


        private void KeyEvent(object sender, KeyEventArgs e) //Keyup Event 
        {
            if (e.KeyCode == Keys.F)
            {
                power();
            }
            if (e.KeyCode == Keys.G)
            {
                f_grade();
            }
            
            
        }

        private void pBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyEvent);
            
            
            
            timer1.Enabled = true; //timer de la fuerza
            timer1.Interval = 150;
            pBar1.Maximum = 10;
            pBar1.Minimum = 0;
            pBar2.Maximum = 10;
            pBar2.Minimum = 0;
            
            timer1.Tick += new EventHandler(timer1_Tick);
            pBar1.Value = 0;

            timer1.Start();

            timer2.Enabled = true; //timer de la posicion flecha
            timer2.Interval = 50;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Stop();//nose porque se pone en marcha si no le doy start,por eso lo pongo

            timer3.Enabled = true;
            timer3.Interval = 20;
            timer3.Tick += new EventHandler(timer3_Tick);
            timer3.Start();

            pBar3.Maximum = 15;
            pBar3.Minimum = 0;
            pBar3.Value = 15;
            pBar3.SetState(2);//color rojo

            pBar4.Maximum = 15;
            pBar4.Minimum = 0;
            pBar4.Value = 15;
            pBar4.SetState(2);//color rojo

            

        


           
        
        }
     

       
        public int max=0;
        private void timer1_Tick(object sender, EventArgs e)
        {

                
                if (max==0)
                {
                    pBar1.Value++;
                    //pBar2.Value++;
                    if (pBar1.Value == 10){
                        max=1;
                    }
                
                }
                if (max == 1)
                   
                {
                    pBar1.Value--;
                    //pBar2.Value--;
                    if (pBar1.Value == 0)
                    {
                        max = 0;
                    }
                    
                
                }    
            

        }

        //aquest troz no està implementat!!!!!!---------------------------------------------------------------------------------------------


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
        //-------------------------------------------------------------------------------------------------------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
           

        }

        public double t=0,h,Vy,grado,a=1.89; //acceleracion escogida convenientemente para los datos del problema
        public double Vx , Voy;
        public int x = 0, y,vida=0;
        public const double PI = 3.1415926535897931;
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            t = t + 0.33;
            x = Convert.ToInt32(250 + Vx * t);
            Vy =  Voy - a * t;
            y =Convert.ToInt32(146 - Voy * t + 0.5 * a * t*t); //parabola invertida ja que el eix Y va de adalt cap abaix i no de abaix cap adalt
            pictureBox3.Location = new Point(x, y);


            if (vida == 0)
            {
                if (x > 780 && y>170 && y<270)
                {
                    
                   
                    pBar4.Value = pBar4.Value - 5;
                    vida = 1; //tenemos que controloar que solo quita vida una vez, ya que la condicion se cumpliria mas de una vez ya que lo compara cada 50ms
                }

                    

            }




            if (x > 966 || y > 435) //si se sale por la derecha o por abajo retorna a la posición original
            {
                
                x = 0;
                y = 0;
                t = 0;
                pictureBox3.Location = new Point(250, 146); 
                timer2.Stop();
                timer1.Start();
                timer3.Start();
             
            }
          
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public int max_g = 0,grade=0;
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (max_g == 0)
            {
                grade++;
                label3.Text = Convert.ToString(grade) + " º";
                if (grade == 90)
                {

                    max_g = 1;
                }
            }

            if(max_g==1)
            {
                grade--;
                label3.Text = Convert.ToString(grade) + " º";
                if (grade == 0)
                {

                    max_g = 0;
                }
            }



            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] trozos = label3.Text.Split(' ');
            grado = Convert.ToInt32(trozos[0]);
            Vx = pBar1.Value * Math.Cos((grado*2*PI)/360) * 3.95; //numero  3.95 elegido convenientemente
            Voy = pBar1.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;
            timer3.Stop();
            vida = 0;
            timer2.Start();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
              

            }
        }

        private void pBar4_Click(object sender, EventArgs e)
        {

        }

  

     
        
                

     
    }
    public static class ModifyProgressBarColor  //top level static class, funció per poder cambiar el color de les progress bar
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }

    }
}
