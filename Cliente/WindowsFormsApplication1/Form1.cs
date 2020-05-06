using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;

        delegate void DelegadoParaPonerConectados(string[] texto);
        delegate void DelegadoParaVisualBox(string[] texto);

        public Form1()
        {
            InitializeComponent();
            // CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }

        private void PonConectados(string[] trozos)
        {
            int i = 1; // i=0 tiene el código 6 
            groupBox2.Visible = true;
            listBox1.Items.Clear();
            string jugador;
            while (i < trozos.Length - 1)
            {
                jugador = trozos[i].Split('\0')[0];
                // MessageBox.Show(jugador);
                listBox1.Items.Add(jugador);
                i++;
            }
        }

        private void atenderserver()
        {

            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string recibido = Encoding.ASCII.GetString(msg2).TrimEnd('\0');
                // MessageBox.Show(recibido);
                string[] trozos = recibido.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje;
                switch (codigo)
                {

                    case 1: // insertar
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show(mensaje);
                        break;

                    case 2:  //acceder
                        mensaje = trozos[1].Split('\0')[0];
                        break;

                    case 3:  //ganador
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("El ganador de la partida es: " + mensaje);
                        break;

                    case 4:  //posicion
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("La posicion es: " + mensaje);
                        break;

                    case 5:  //duracion
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("La duracion es: " + mensaje);
                        break;


                    case 6:
                        DelegadoParaPonerConectados delegado = new DelegadoParaPonerConectados(PonConectados);
                        groupBox2.Invoke(delegado, new object[] { trozos });
                        break;
                }
            }

        }     //procesado de las respuestas del servidor

        private void Form1_Load(object sender, EventArgs e)
        {

            
            

            ListBox listbox = new ListBox();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos

            IPAddress direc = IPAddress.Parse("192.168.56.102");


            IPEndPoint ipep = new IPEndPoint(direc, 9050);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");


            }
            catch (SocketException)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }


            ThreadStart st = delegate { atenderserver(); };
            atender = new Thread(st);
            atender.Start();
        }   //conectar

        private void registrar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "1/" + usuario.Text + "/" + contra.Text + "/" + email.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);


            }
            catch { MessageBox.Show("Error al registrar."); }


        }   //registrar

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //Mensaje de desconexión
                string mensaje = "0/";
                groupBox2.Visible = false;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                // Nos desconectamos
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                atender.Abort();
            }
            catch
            {
                MessageBox.Show("No está conectado.");
            }


        }    //desconectar

        private void acceder_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "2/" + usuario.Text + "/" + contra.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                //mensaje = "10/";


            }
            catch { MessageBox.Show("Error al acceder."); }

        }    //acceder

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gana.Checked)
                {

                    string mensaje = "3/" + idpartida.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }

                else if (pos.Checked)
                {
                    string mensaje = "4/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }

                else if (tiemp.Checked)
                {
                    string mensaje = "5/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }


            }
            catch { MessageBox.Show("Error al realizar la consulta."); }
            //consultas   

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {


        }
    }
}
