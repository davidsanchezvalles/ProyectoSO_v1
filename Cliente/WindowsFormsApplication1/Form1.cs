using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Socket server;
        Thread atender;
        
        public Form1()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls=false;
        }

        public class HeavyTaskResponse
        {
            private readonly string message;

            public HeavyTaskResponse(string msg)
            {
                this.message = msg;
            }

            public string Message { get { return message; } }
        }

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
            IPAddress direc = IPAddress.Parse("192.168.1.208");
            IPEndPoint ipep = new IPEndPoint(direc, 9040);
            

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

            }
            catch (SocketException )
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
            try{
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
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                 
                }

                else if (pos.Checked)
                {
                    string mensaje = "4/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show("Tu posicón en partida es: " + mensaje);
                }

                else if (tiemp.Checked)
                {
                    string mensaje = "5/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    MessageBox.Show("Tu tiempo en partida es: " + mensaje + "min");
                }

                else if (genteconec.Checked)
                {
                    string mensaje = "6/";
                  
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    MessageBox.Show(Encoding.ASCII.GetString(msg));  //-----------------

                    //Recibimos la respuesta del servidor
                    //byte[] msg2 = new byte[80];
                    //server.Receive(msg2);
                    //mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    //MessageBox.Show("Los usuarios conectados son: " + mensaje);


                   

                    //Recibimos la respuesta del servidor
                    byte[] msg2 = new byte[80];

                    MessageBox.Show("1");

                    int i = 0;
                

                    MessageBox.Show("2");
                    
                    server.Receive(msg2);

                    MessageBox.Show("3");

                    string[] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    //int codigo = Convert.ToInt32(trozos[0]);
                    
                    while (trozos[i] != null)
                    {
                        MessageBox.Show(trozos[i]);
                        listBox1.Items.Add(trozos[i]);
                        i++;
                     }

                    
                }
            }
            catch { MessageBox.Show("Error al realizar la consulta."); }
        }    //consultas



        private void atenderserver()
        {
            while (true)
            {
                int i = 0;

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string [] trozos = Encoding.ASCII.GetString(msg2).Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje;

                switch (codigo){
                    
                    case 1:

                    
                        mensaje =trozos[1].Split('\0')[0];
                        MessageBox.Show(mensaje);
                        groupBox2.Visible = true;


                        break;



                    case 2:

                   
                        mensaje = trozos[1].Split('\0')[0];
                        if (mensaje.Length > 2)
                        {
                            MessageBox.Show(mensaje);
                            groupBox2.Visible = true;
                            
                        }
                        else MessageBox.Show("Usuario o contraseña incorrectos");


                        break;




                    case 3:


                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("El ganador de la partida es: " + mensaje);


                        break;



                    case 6:


                        while (trozos[i] != null)
                        {
                            mensaje = trozos[i].Split('\0')[0];
                            listBox1.Items.Add(mensaje);
                            i++;

                        }




                        break;


            }   
            }
        
        }     //procesado de las respuestas del servidor

        
    }
}
