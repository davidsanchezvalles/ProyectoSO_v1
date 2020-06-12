using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;


namespace WindowsFormsApplication1
{
    public partial class Formmenu : Form          //vfinal_ser_sin entorno
    {
        Socket server;
        Thread atender;
        Thread formulario;
        List<juegoForm> listaform = new List<juegoForm>();    

        delegate void DelegadoParaPonerConectados(string[] texto);
        delegate void DelegadoParaRespuestas(string[] texto);
        delegate void DelegadoParaVisualBox();
        delegate void DelegadoForm(string[] texto);
        delegate void DelegadoTiro(string[] texto);

        public Formmenu()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {


            ListBox listbox = new ListBox();  //necessari?
            label8.Text = DateTime.Now.ToLongDateString();

        }



        //funciones------------------------------------
        private void Crearform(string[] trozos)
        {

            juegoForm game = new juegoForm(server);
            game.inicializar(trozos[1], trozos[2], Convert.ToInt32(trozos[3]));
            game.Show();
            
            
            listaform.Add(game);
            
        }

        private void Tiro(string[] trozos)
        {
            listaform[0].simulartiro( Convert.ToInt32(trozos[1]) ,Convert.ToDouble(trozos[2]), Convert.ToDouble(trozos[3]));
        }

       /* private void threadforms()
        {
            ThreadStart ts = delegate { Crearform(); };
            formulario = new Thread(ts);
            formulario.Start();

        }  */
        
        private void PonConectados(string[] trozos)
        {
            int i = 1; // i=0 tiene el código 6 
            groupBox2.Visible = true;
            listBox1.Items.Clear();
            string jugador;
            while (i < trozos.Length - 1)
            {
                jugador = trozos[i].Split('\0')[0];
                listBox1.Items.Add(jugador);
                i++;
            }
        }

        private void Respuestas(string[] trozos)
        {
            string respuesta = trozos[1].Split('\0')[0];
            textoserver.Text = "La respuesta es: " +respuesta;

        }

        private void Chat(string[] trozos)
        {
            string mensaje = trozos[1].Split('\0')[0];
            listBox2.Items.Add(mensaje);

        }

        private void VisualBox()
        {
            groupBox1.BackColor = Color.LightSkyBlue;

        }

        private void atenderserver()
        {

            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string recibido = Encoding.ASCII.GetString(msg2).TrimEnd('\0');
                 //MessageBox.Show(recibido);
                string[] trozos = recibido.Split('/');
                int codigo = Convert.ToInt32(trozos[0]);
                string mensaje;
                switch (codigo)
                {

                    case 1: // registrar
                         
                   

                        DelegadoParaVisualBox d_visual = new DelegadoParaVisualBox(VisualBox);
                        groupBox1.Invoke(d_visual);
                      
                        break;

                    case 2:  //No sesta fent servir!!
                        mensaje = trozos[1].Split('\0')[0];
                        break;

                    case 3:  //ganador

                        DelegadoParaRespuestas del_respuestas = new DelegadoParaRespuestas(Respuestas);
                        textoserver.Invoke(del_respuestas, new object[] { trozos });
                        
                       
                        break;

                    case 4:  //posicion
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("La posicion es: " + mensaje);
                        break;

                    case 5:  //duracion
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show("La duracion es: " + mensaje);
                        break;


                    case 6:  //conectados
                        DelegadoParaPonerConectados delegado = new DelegadoParaPonerConectados(PonConectados);
                        groupBox2.Invoke(delegado, new object[] { trozos });
                        
                        break;


                    case 7:  //recibimos invitacion
                        mensaje = trozos[1].Split('\0')[0];
                        DialogResult result;
                        result = MessageBox.Show(mensaje, "invitacion", MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            string men = "8/SI";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(men);
                            server.Send(msg);

                        }
                        else
                        {
                            string men = "8/NO";
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(men);
                            server.Send(msg);

                        }

                        break;


                    case 8:  //respuesta invitacion
                        

                        //creamos el formulario del juego
                        DelegadoForm formdelegate = new DelegadoForm(Crearform);
                        Invoke(formdelegate, new object[] { trozos });


                      
                        break;


                    case 9:  //chat

                        DelegadoParaPonerConectados del_chat = new DelegadoParaPonerConectados(Chat);
                        listBox2.Invoke(del_chat, new object[] { trozos });

                        break;

                    case 10:  //simulacion del tiro

                           //la partida o furmolario
                        DelegadoTiro tirodelegate = new DelegadoTiro(Tiro);
                        Invoke(tirodelegate, new object[] { trozos });


                        break;
                    case 12:   //notificación de eliminar
                        mensaje = trozos[1].Split('\0')[0];
                        MessageBox.Show(mensaje);

                        break;

                }
            }

        }     //procesado de las respuestas del servidor


       
        
        //objetos------------------------------------
     
        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.1.208");
            IPEndPoint ipep = new IPEndPoint(direc, 9020);


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                //this.BackColor = Color.Green;
                this.groupBox1.BackColor = Color.LawnGreen;
                
                ;


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
                string mensaje = "1/" + usuario.Text + "/" + contra.Text;
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
                groupBox1.BackColor = Color.LightGray;
                atender.Abort();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                
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

                /*else if (pos.Checked)
                {
                    string mensaje = "4/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }*/

                else if (tiemp.Checked)
                {
                    string mensaje = "5/" + idpartida.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }



            }
            catch { MessageBox.Show("Error al realizar la consulta."); }

        }   //consultas

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string nombre = listBox1.SelectedItem.ToString();
                    string mensaje = "7/" + nombre;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);


                }
                else
                {
                    MessageBox.Show("Debe seleccionar a un jugador conectado");
                }
            }
            catch
            {
                MessageBox.Show("Error al enviar invitación, inténtelo de nuevo.");
            }

        }   //invitar

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != null)
                {
                    string mensaje = "9/" + textBox1.Text;
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Debe escribir un mensaje");
                }
            }
            catch
            {
                MessageBox.Show("Error al enviar mensaje.");
            }
        }  //chat

        private void button6_Click_1(object sender, EventArgs e)
        {
            DialogResult dardebaja;
            dardebaja = MessageBox.Show("¿Está seguro que desea eliminar su cuenta?","Eliminar cuenta", MessageBoxButtons.YesNo);
            if (dardebaja == System.Windows.Forms.DialogResult.Yes)
            {
                string men = "12/" + usuario.Text + "/" + contra.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(men);
                server.Send(msg);

                string mensaje = "0/";
                byte[] mesage = System.Text.Encoding.ASCII.GetBytes(mensaje);
                groupBox2.Visible = false;

                server.Send(mesage);

                // Nos desconectamos
                groupBox1.BackColor = Color.LightGray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
                atender.Abort();

            }
        }




        private void button6_Click(object sender, EventArgs e)
        {
          
            
        }

        private void textoserver_Click(object sender, EventArgs e)
        {

        }

        private void idpartida_TextChanged(object sender, EventArgs e)
        {

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }


    }
}