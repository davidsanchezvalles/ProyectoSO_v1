using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;  //para poder cambiar color barra
using System.Net;
using System.Net.Sockets;


namespace WindowsFormsApplication1
{
    public partial class juegoForm : Form
    {
        public juegoForm(Socket servidor)
        {
            InitializeComponent();
            server = servidor;
            
            
            
           
        }

        //variables globales
        public double t = 0, h, Vy, grado, a = 1.89; //acceleracion escogida convenientemente para los datos del problema
        public double Vx, Voy; //equivalen a la fuerza/velocidad 
        public int x = 0, y, vida = 0;
        public const double PI = 3.1415926535897931;
        public int max1 = 0, max2 = 0;
        public int max_g = 0, grade = 0;
        public int turno=2;
        public int recibido;
        public Socket server;
        public string jugador1, jugador2;
        public void inicializar( string jug1, string jug2, int turn)
        {
            turno = turn;
            jugador1 = jug1;
            jugador2 = jug2;
            label3.Text = jugador1;
            label4.Text = jugador2;
            label8.Text = "Le toca a: " + jugador1;
            if (turno == 1)
            {
                timer1.Start();
                timer3.Start();
            }

        }


        private void juegoForm_Load(object sender, EventArgs e)
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
            pBar2.Value = 0;

            //timer1.Start();

            timer2.Enabled = true; //timer de la posicion flecha
            timer2.Interval = 25;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Stop();//nose porque se pone en marcha si no le doy start,por eso lo pongo

            timer3.Enabled = true;
            timer3.Interval = 20;
            timer3.Tick += new EventHandler(timer3_Tick);
            //timer3.Start();

            pBar3.Maximum = 15;
            pBar3.Minimum = 0;
            pBar3.Value = 15;
            pBar3.SetState(2);//color rojo

            pBar4.Maximum = 15;
            pBar4.Minimum = 0;
            pBar4.Value = 15;
            pBar4.SetState(2);//color rojo

            timer_turno.Interval = 500;
            timer_turno.Tick += new EventHandler(timer_turno_Tick);
            //timer_turno.Start();

            pBar5.Maximum = 20;
            pBar5.Minimum = 0;
            pBar5.Value = 20;
            pBar5.SetState(3);
        }   //inicialització dels controls
        
        private void power()
        {

            timer1.Stop();

        }

        private void f_grade()
        {
            timer3.Stop();
            vida = 0;
         
            //depende del turno, cojeremos los valores de unos progresbar o otros, y realizaremos un tiro de derecha a izquierda o viceversa
            if (turno == 1)
            {

                string[] trozos = label1.Text.Split(' ');
                grado = Convert.ToInt32(trozos[0]);
                Vx = pBar1.Value * Math.Cos((grado * 2 * PI) / 360) * 3.95; //numero  3.95 elegido convenientemente
                Voy = pBar1.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;


                string men = "10/1/"+ Vx + "/" + Voy + "/" + pBar3.Value + "/" + pBar4.Value;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(men);
                server.Send(msg);
                label8.Text= "Le toca a :" + jugador2.ToString();
                
              
                    
              
            }
            else if (turno == 2)
            {

                string[] trozos = label2.Text.Split(' ');
                grado = Convert.ToInt32(trozos[0]);
                Vx = pBar2.Value * Math.Cos((grado * 2 * PI) / 360) * 3.95; //numero  3.95 elegido convenientemente
                Voy = pBar2.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;

                string men = "10/2/" + Vx + "/" + Voy + "/" + pBar4.Value  + "/" + pBar3.Value;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(men);
                server.Send(msg);
                label8.Text = "Le toca a :" + jugador1.ToString();
            }
            
            timer2.Start();
        }

        public void simulartiro(int torn, double vx, double voy)
        {
            
            Vx = vx;
            Voy = voy;
            vida = 0; //variable para controlar que solo le quita una vez la vida, y no varias veces en un mismo tiro
            turno = torn;  //de esta manera el timer2 , el que realiza la parábola, sabrá diferenciar el tiro y condiciones que tiene que cumplir
            //si el turno es 2, significa que primero tenemos que simular el tiro del turno 1, y viceversa

           
            recibido = 1;
            timer2.Start();  //empieza la simulacion del tiro del rival

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

       
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (turno == 1)
            {

                if (max1 == 0)
                {
                    pBar1.Value++;
                    if (pBar1.Value == 10)
                    {
                        max1 = 1;
                    }

                }
                if (max1 == 1)
                {
                    pBar1.Value--;
                    if (pBar1.Value == 0)
                    {
                        max1 = 0;
                    }


                }
            }
            else if (turno == 2)
            {

                if (max2 == 0)
                {
                  
                    pBar2.Value++;
                    if (pBar2.Value == 10)
                    {
                        max2 = 1;
                    }

                }
                if (max2 == 1)
                {
                    
                    pBar2.Value--;
                    if (pBar2.Value == 0)
                    {
                        max2 = 0;
                    }


                }

            }
        }  //timer de las progress bar , la fuerza
     
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            t = t + 0.5;

            if (turno == 1)
            {

                x = Convert.ToInt32(296 + Vx * t);
                y = Convert.ToInt32(137 - Voy * t + 0.5 * a * t * t); //parabola invertida ja que el eix Y va de adalt cap abaix i no de abaix cap adalt

                pictureBox3.Location = new Point(x, y);

                
                if (vida == 0)
                {
                    if ((x< 670 && x > 870) && (y > 90 && y < 220))
                    {

                        if (pBar4.Value == 5)
                        {
                            MessageBox.Show("Ganaste!!");
                        }
                        pBar4.Value = pBar4.Value - 5;
                        vida = 1; //tenemos que controloar que solo quita vida una vez, ya que la condicion se cumpliria mas de una vez ya que lo compara cada 50ms
                    }
                }





            }
            else if (turno == 2)
            {
                x = Convert.ToInt32(727 - Vx * t);
                y = Convert.ToInt32(137 - Voy * t + 0.5 * a * t * t); //parabola invertida ja que el eix Y va de adalt cap abaix i no de abaix cap adalt

                pictureBox4.Location = new Point(x, y);


                if (vida == 0)
                {
                    if ((x > 20 && x < 170) && (y > 58 && y < 230))
                    {

                        if (pBar3.Value == 5)
                        {
                            MessageBox.Show("Ganaste!!");
                        }
                        pBar3.Value = pBar3.Value - 5;
                        vida = 1; //tenemos que controloar que solo quita vida una vez, ya que la condicion se cumpliria mas de una vez ya que lo compara cada 50ms
                    }
                }
           }


            if (x > 1050 || x <0 || y > 435) //si se sale por la derecha o por abajo retorna a la posición original
            {

                x = 0;
                y = 0;
                t = 0;
                
                if (turno == 1)
                {
                    pictureBox3.Location = new Point(296, 137);
                    turno = 2;
                }
                else if (turno == 2)
                {
                    pictureBox4.Location = new Point(723, 137);
                    turno = 1;
                
                }
                timer2.Stop();
                if (recibido == 1)
                {
                     timer1.Start();
                     timer3.Start();
                     recibido = 0;

                }
                /*timer1.Start();
                timer3.Start();
                
                pBar5.Value = 20;*/

            }
        }  // timer del tiro parabolico

      
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (turno == 1)
            {

                if (max_g == 0)
                {
                    grade++;
                    label1.Text = Convert.ToString(grade) + " º";
                    if (grade == 90)
                    {

                        max_g = 1;
                    }
                }

                if (max_g == 1)
                {
                    grade--;
                    label1.Text = Convert.ToString(grade) + " º";
                    if (grade == 0)
                    {

                        max_g = 0;
                    }
                }
            }
            else if (turno == 2)
            {


                if (max_g == 0)
                {
                    grade++;
                    label2.Text = Convert.ToString(grade) + " º";
                    if (grade == 90)
                    {

                        max_g = 1;
                    }
                }

                if (max_g == 1)
                {
                    grade--;
                    label2.Text = Convert.ToString(grade) + " º";
                    if (grade == 0)
                    {

                        max_g = 0;
                    }
                }




            }
        }  //timer de los grados

       
        
        private void timer_turno_Tick(object sender, EventArgs e)
        {
            pBar5.Value--;
            
            if(pBar5.Value==0)
            {
                x = 0;
                y = 0;
                t = 0;
                timer2.Stop();
                timer1.Start();
                timer3.Start();

                if (turno == 1)
                {
                    pictureBox3.Location = new Point(296, 137);
                    turno = 2;
                }
                else if (turno == 2)
                {
                    pictureBox4.Location = new Point(723, 137);
                    turno = 1;

                }

              

                pBar5.Value = 20;
            }
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
