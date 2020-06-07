namespace WindowsFormsApplication1
{
    partial class Formmenu
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.usuario = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.acceder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textoserver = new System.Windows.Forms.Label();
            this.genteconectada = new System.Windows.Forms.Label();
            this.tiemp = new System.Windows.Forms.RadioButton();
            this.pos = new System.Windows.Forms.RadioButton();
            this.gana = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.idpartida = new System.Windows.Forms.TextBox();
            this.registrar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contra = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Usuario";
            // 
            // usuario
            // 
            this.usuario.Location = new System.Drawing.Point(173, 31);
            this.usuario.Name = "usuario";
            this.usuario.Size = new System.Drawing.Size(164, 20);
            this.usuario.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(53, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "conectar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // acceder
            // 
            this.acceder.Location = new System.Drawing.Point(262, 131);
            this.acceder.Name = "acceder";
            this.acceder.Size = new System.Drawing.Size(75, 23);
            this.acceder.TabIndex = 5;
            this.acceder.Text = "Acceder";
            this.acceder.UseVisualStyleBackColor = true;
            this.acceder.Click += new System.EventHandler(this.acceder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.registrar);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.contra);
            this.groupBox1.Controls.Add(this.email);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.acceder);
            this.groupBox1.Controls.Add(this.usuario);
            this.groupBox1.Location = new System.Drawing.Point(25, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 208);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox2.Controls.Add(this.textoserver);
            this.groupBox2.Controls.Add(this.genteconectada);
            this.groupBox2.Controls.Add(this.tiemp);
            this.groupBox2.Controls.Add(this.pos);
            this.groupBox2.Controls.Add(this.gana);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.idpartida);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 208);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Consultas al servidor";
            this.groupBox2.Visible = false;
            // 
            // textoserver
            // 
            this.textoserver.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoserver.Location = new System.Drawing.Point(25, 88);
            this.textoserver.Name = "textoserver";
            this.textoserver.Size = new System.Drawing.Size(142, 95);
            this.textoserver.TabIndex = 12;
            this.textoserver.Text = "Respuestas";
            // 
            // genteconectada
            // 
            this.genteconectada.AutoSize = true;
            this.genteconectada.Location = new System.Drawing.Point(190, 157);
            this.genteconectada.Name = "genteconectada";
            this.genteconectada.Size = new System.Drawing.Size(0, 13);
            this.genteconectada.TabIndex = 11;
            // 
            // tiemp
            // 
            this.tiemp.AutoSize = true;
            this.tiemp.Location = new System.Drawing.Point(193, 86);
            this.tiemp.Name = "tiemp";
            this.tiemp.Size = new System.Drawing.Size(110, 17);
            this.tiemp.TabIndex = 9;
            this.tiemp.TabStop = true;
            this.tiemp.Text = "Tiempo en partida";
            this.tiemp.UseVisualStyleBackColor = true;
            // 
            // pos
            // 
            this.pos.AutoSize = true;
            this.pos.Location = new System.Drawing.Point(193, 60);
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(115, 17);
            this.pos.TabIndex = 8;
            this.pos.TabStop = true;
            this.pos.Text = "Posición en partida";
            this.pos.UseVisualStyleBackColor = true;
            // 
            // gana
            // 
            this.gana.AutoSize = true;
            this.gana.Location = new System.Drawing.Point(193, 34);
            this.gana.Name = "gana";
            this.gana.Size = new System.Drawing.Size(116, 17);
            this.gana.TabIndex = 7;
            this.gana.TabStop = true;
            this.gana.Text = "Ganador de partida";
            this.gana.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(203, 120);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 34);
            this.button2.TabIndex = 4;
            this.button2.Text = "Consultar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(44, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 25);
            this.label6.TabIndex = 1;
            this.label6.Text = "ID Partida";
            // 
            // idpartida
            // 
            this.idpartida.Location = new System.Drawing.Point(45, 53);
            this.idpartida.Name = "idpartida";
            this.idpartida.Size = new System.Drawing.Size(92, 20);
            this.idpartida.TabIndex = 3;
            // 
            // registrar
            // 
            this.registrar.Location = new System.Drawing.Point(173, 131);
            this.registrar.Name = "registrar";
            this.registrar.Size = new System.Drawing.Size(75, 23);
            this.registrar.TabIndex = 13;
            this.registrar.Text = "Registrarse";
            this.registrar.UseVisualStyleBackColor = true;
            this.registrar.Click += new System.EventHandler(this.registrar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Email";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Contraseña";
            // 
            // contra
            // 
            this.contra.Location = new System.Drawing.Point(173, 83);
            this.contra.Name = "contra";
            this.contra.Size = new System.Drawing.Size(164, 20);
            this.contra.TabIndex = 10;
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(173, 57);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(164, 20);
            this.email.TabIndex = 9;
            this.email.Text = "(Solo si desea registrarse)";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Red;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(198, 40);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(145, 32);
            this.button3.TabIndex = 10;
            this.button3.Text = "desconectar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(432, 117);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(157, 147);
            this.listBox1.TabIndex = 11;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(429, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Lista de conectados";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(463, 283);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "Invitar";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(412, 267);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(199, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "(Seleccione al jugador que desee invitar)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(734, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "Chat";
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(651, 66);
            this.listBox2.Name = "listBox2";
            this.listBox2.ScrollAlwaysVisible = true;
            this.listBox2.Size = new System.Drawing.Size(206, 186);
            this.listBox2.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(651, 261);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(159, 20);
            this.textBox1.TabIndex = 17;
            // 
            // button5
            // 
            this.button5.Image = global::WindowsFormsApplication1.Properties.Resources.icono_enviar_p;
            this.button5.Location = new System.Drawing.Point(816, 261);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(41, 23);
            this.button5.TabIndex = 18;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Formmenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 331);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Formmenu";
            this.Text = "Juego";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usuario;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button acceder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox contra;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Button registrar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton tiemp;
        private System.Windows.Forms.RadioButton pos;
        private System.Windows.Forms.RadioButton gana;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox idpartida;
        private System.Windows.Forms.Label genteconectada;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label textoserver;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button5;
    }
}

