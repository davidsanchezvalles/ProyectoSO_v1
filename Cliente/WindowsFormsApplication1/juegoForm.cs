using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.InteropServices;  //para poder cambiar color barra



namespace WindowsFormsApplication1
{
    public partial class juegoForm : Form
    {
        Socket sock;
        int i = 0;
        int tur = 0;

        public juegoForm()
        {

        }
        public void inicializarForm(int a, Socket server)
        {
            InitializeComponent();
            int turno = a;
            sock = server;
        }
        public void setValores(int a, double Fx, double Fy)
        {
            grado = a;
            Vx = Fx;
            Voy = Fy;
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

            timer1.Start();

            timer2.Enabled = true; //timer de la posicion flecha
            timer2.Interval = 25;
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

            timer_turno.Interval = 500;
            timer_turno.Tick += new EventHandler(timer_turno_Tick);
            timer_turno.Start();

            pBar5.Maximum = 20;
            pBar5.Minimum = 0;
            pBar5.Value = 20;
            pBar5.SetState(3);
        }
        private void power()
        {

            timer1.Stop();

        }

        private void f_grade()
        {
            if (turno == 1 && tur%2==0)//el tur%2 es per saber a qui toca tirar, si és num parell tira el de turno 1
            {
                string mensaje; 
                string[] trozos = label1.Text.Split(' ');
                grado = Convert.ToInt32(trozos[0]);
                Vx = pBar1.Value * Math.Cos((grado * 2 * PI) / 360) * 3.95; //numero  3.95 elegido convenientemente
                Voy = pBar1.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;
                timer3.Stop();
                mensaje = "10/"+grade+"/"+Vx+"/"+Voy;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                sock.Send(msg);
                vida = 0; // indicamos que le puede quitar vida(una vez por tirada)
                timer2.Start();
            }
            else if (turno == 2 && tur%2!=0)//es senar, tira el de turno 2
            {
                //he comentat tot el que ve ara pq només estic intentant q un tiri la fletxa i aquesta fletxa reprodueixi el moviment pero per part de laltre jugador(només estem jugant amb un jugador, seria com un mirall la idea)


               // string[] trozos = label2.Text.Split(' ');
               // grado = Convert.ToInt32(trozos[0]);
               // Vx = pBar2.Value * Math.Cos((grado * 2 * PI) / 360) * 3.95; //numero  3.95 elegido convenientemente
               // Voy = pBar2.Value * Math.Sin((grado * 2 * PI) / 360) * 3.95;

                byte[] msg2 = new byte[80];
                sock.Receive(msg2);

                string recibido = Encoding.ASCII.GetString(msg2).TrimEnd('\0');
                string[] trozosR = recibido.Split('/');
              /*  grado = Convert.ToInt32(trozosR[0]);
                Vx = Convert.ToDouble(trozosR[1]);
                Voy = Convert.ToDouble(trozosR[2]);*/


                timer3.Stop();
                vida = 0; // indicamos que le puede quitar vida(una vez por tirada)
                timer2.Start();


            }
            tur++;//cada vegada que tiri algu, s'incrementa el valor 
        }

        public int turno = 1;
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

        public int max1 = 0, max2=0;
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
            {//com que els valors del moviment de la fletxa del jugador 2 venen donats pels valors del jugador 1, no necessitem el que ve ara, per això està comentat(evidentment només ara que es una prova)

                /*if (max2 == 0)
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


                }*/

            }
        }  //timer de las progress bar , la fuerza


        public double t = 0, h, Vy, grado, a = 1.89; //acceleracion escogida convenientemente para los datos del problema
        public double Vx, Voy; //equivalen a la fuerza/velocidad 
        public int x = 0, y, vida = 0;
        public const double PI = 3.1415926535897931;
        
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
                    if (x > 870 && y > 160 && y < 260)
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
                    if (x < 170 && y > 155 && y < 265)
                    {

                        if (pBar3.Value == 5)
                        {
                            MessageBox.Show("Ganaste!!");
                        }
                        pBar3.Value = pBar3.Value - 5;
                        vida = 1; //tenemos que controloar que solo quita vida una vez, ya que la condicion se cumpliria mas de una vez ya que lo compara cada 50ms
                    }
                }
                tur++;//sha de canviar per fer online, s'haurà de treure
                
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
                timer1.Start();
                timer3.Start();
                pBar5.Value = 20;

            }
        }

        public int max_g = 0, grade = 0;
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
        }

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
