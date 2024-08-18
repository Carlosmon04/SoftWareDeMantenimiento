namespace SoftwareProject.Formularios
{
    partial class NuevoExistente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnExistente = new System.Windows.Forms.Button();
            this.btnRegresar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNuevo
            // 
            this.btnNuevo.ForeColor = System.Drawing.Color.Black;
            this.btnNuevo.Location = new System.Drawing.Point(330, 231);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(191, 121);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Articulo Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnExistente
            // 
            this.btnExistente.ForeColor = System.Drawing.Color.Black;
            this.btnExistente.Location = new System.Drawing.Point(631, 231);
            this.btnExistente.Name = "btnExistente";
            this.btnExistente.Size = new System.Drawing.Size(191, 121);
            this.btnExistente.TabIndex = 1;
            this.btnExistente.Text = "Articulo Existente";
            this.btnExistente.UseVisualStyleBackColor = true;
            this.btnExistente.Click += new System.EventHandler(this.btnExistente_Click);
            // 
            // btnRegresar
            // 
            this.btnRegresar.ForeColor = System.Drawing.Color.Black;
            this.btnRegresar.Location = new System.Drawing.Point(829, 465);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(114, 36);
            this.btnRegresar.TabIndex = 2;
            this.btnRegresar.Text = "Regresar";
            this.btnRegresar.UseVisualStyleBackColor = true;
            // 
            // NuevoExistente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(27)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(1065, 636);
            this.Controls.Add(this.btnRegresar);
            this.Controls.Add(this.btnExistente);
            this.Controls.Add(this.btnNuevo);
            this.Name = "NuevoExistente";
            this.Text = "NuevoExistente";
            this.Load += new System.EventHandler(this.NuevoExistente_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnExistente;
        private System.Windows.Forms.Button btnRegresar;
    }
}