namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(25, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "conectar";
            this.button1.UseVisualStyleBackColor = true;
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
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveBorder;
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
            this.groupBox2.Text = "Consulta";
            this.groupBox2.Visible = false;
            // 
            // tiemp
            // 
            this.tiemp.AutoSize = true;
            this.tiemp.Location = new System.Drawing.Point(173, 103);
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
            this.pos.Location = new System.Drawing.Point(173, 80);
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
            this.gana.Location = new System.Drawing.Point(173, 57);
            this.gana.Name = "gana";
            this.gana.Size = new System.Drawing.Size(116, 17);
            this.gana.TabIndex = 7;
            this.gana.TabStop = true;
            this.gana.Text = "Ganador de partida";
            this.gana.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
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
            this.idpartida.Location = new System.Drawing.Point(173, 31);
            this.idpartida.Name = "idpartida";
            this.idpartida.Size = new System.Drawing.Size(164, 20);
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
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(25, 332);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 53);
            this.button3.TabIndex = 10;
            this.button3.Text = "desconectar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 421);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Juego";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

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
    }
}

