namespace ControlFirmaElectronica
{
    partial class wfrAlerta
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
            this.dgAlerta = new System.Windows.Forms.DataGridView();
            this.lblAlerta = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.linkImprimir = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgAlerta)).BeginInit();
            this.SuspendLayout();
            // 
            // dgAlerta
            // 
            this.dgAlerta.AllowUserToAddRows = false;
            this.dgAlerta.AllowUserToDeleteRows = false;
            this.dgAlerta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAlerta.Location = new System.Drawing.Point(25, 37);
            this.dgAlerta.Name = "dgAlerta";
            this.dgAlerta.ReadOnly = true;
            this.dgAlerta.Size = new System.Drawing.Size(602, 216);
            this.dgAlerta.TabIndex = 0;
            this.dgAlerta.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAlerta_CellContentClick);
            // 
            // lblAlerta
            // 
            this.lblAlerta.AutoSize = true;
            this.lblAlerta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlerta.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAlerta.Location = new System.Drawing.Point(11, 9);
            this.lblAlerta.Name = "lblAlerta";
            this.lblAlerta.Size = new System.Drawing.Size(547, 20);
            this.lblAlerta.TabIndex = 1;
            this.lblAlerta.Text = "Se han encontrado las siguientes resoluciones del  Regisro Público";
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(221, 272);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(177, 36);
            this.btnImprimir.TabIndex = 2;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // linkImprimir
            // 
            this.linkImprimir.AutoSize = true;
            this.linkImprimir.Location = new System.Drawing.Point(296, 311);
            this.linkImprimir.Name = "linkImprimir";
            this.linkImprimir.Size = new System.Drawing.Size(0, 13);
            this.linkImprimir.TabIndex = 3;
            this.linkImprimir.Visible = false;
            this.linkImprimir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkImprimir_LinkClicked);
            this.linkImprimir.Click += new System.EventHandler(this.linkImprimir_Click);
            // 
            // wfrAlerta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(656, 347);
            this.ControlBox = false;
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.lblAlerta);
            this.Controls.Add(this.dgAlerta);
            this.Controls.Add(this.linkImprimir);
            this.Name = "wfrAlerta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alerta de Resoluciones";
            ((System.ComponentModel.ISupportInitialize)(this.dgAlerta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgAlerta;
        private System.Windows.Forms.Label lblAlerta;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.LinkLabel linkImprimir;
    }
}