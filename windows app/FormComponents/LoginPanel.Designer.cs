namespace WindowsFormsApplication2.FormComponents
{
    partial class LoginPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.rjTextBox2 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.rjTextBox1 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.rjButton8 = new WindowsFormsApplication2.RJButton();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 54);
            this.label1.TabIndex = 22;
            this.label1.Text = "התחברות";
            // 
            // rjTextBox2
            // 
            this.rjTextBox2.BackColor = System.Drawing.SystemColors.Control;
            this.rjTextBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjTextBox2.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
            this.rjTextBox2.BorderRadius = 7;
            this.rjTextBox2.BorderSize = 3;
            this.rjTextBox2.EnableRightClickMenu = false;
            this.rjTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold);
            this.rjTextBox2.ForeColor = System.Drawing.Color.DimGray;
            this.rjTextBox2.Location = new System.Drawing.Point(18, 133);
            this.rjTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox2.MaxLength = 30;
            this.rjTextBox2.Multiline = false;
            this.rjTextBox2.Name = "rjTextBox2";
            this.rjTextBox2.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox2.PasswordChar = true;
            this.rjTextBox2.PlaceHolderText = "סיסמא";
            this.rjTextBox2.Size = new System.Drawing.Size(257, 53);
            this.rjTextBox2.TabIndex = 24;
            this.rjTextBox2.Texts = "סיסמא";
            this.rjTextBox2.ToolTipEnabled = false;
            this.rjTextBox2.UnderlinedStyle = false;
            this.rjTextBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rjTextBox2_KeyPress);
            // 
            // rjTextBox1
            // 
            this.rjTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.rjTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjTextBox1.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
            this.rjTextBox1.BorderRadius = 7;
            this.rjTextBox1.BorderSize = 3;
            this.rjTextBox1.EnableRightClickMenu = false;
            this.rjTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold);
            this.rjTextBox1.ForeColor = System.Drawing.Color.DimGray;
            this.rjTextBox1.Location = new System.Drawing.Point(18, 72);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox1.MaxLength = 20;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceHolderText = "שם משתמש";
            this.rjTextBox1.Size = new System.Drawing.Size(257, 53);
            this.rjTextBox1.TabIndex = 23;
            this.rjTextBox1.Texts = "שם משתמש";
            this.rjTextBox1.ToolTipEnabled = false;
            this.rjTextBox1.UnderlinedStyle = false;
            this.rjTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rjTextBox1_KeyPress);
            // 
            // rjButton8
            // 
            this.rjButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjButton8.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjButton8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjButton8.BorderRadius = 7;
            this.rjButton8.BorderSize = 3;
            this.rjButton8.FlatAppearance.BorderSize = 0;
            this.rjButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton8.Font = new System.Drawing.Font("Ebrima", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rjButton8.ForeColor = System.Drawing.Color.White;
            this.rjButton8.Location = new System.Drawing.Point(50, 194);
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(182, 45);
            this.rjButton8.TabIndex = 19;
            this.rjButton8.Text = "התחבר";
            this.rjButton8.TextColor = System.Drawing.Color.White;
            this.rjButton8.UseVisualStyleBackColor = false;
            this.rjButton8.Click += new System.EventHandler(this.rjButton8_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkRed;
            this.label2.Location = new System.Drawing.Point(15, 253);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 25;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // LoginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rjTextBox2);
            this.Controls.Add(this.rjTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rjButton8);
            this.Name = "LoginPanel";
            this.Size = new System.Drawing.Size(303, 283);
            this.Load += new System.EventHandler(this.LoginPanel_Load);
            this.VisibleChanged += new System.EventHandler(this.LoginPanel_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private RJButton rjButton8;
        private RJControls.RJTextBox rjTextBox1;
        private RJControls.RJTextBox rjTextBox2;
        private System.Windows.Forms.Label label2;
    }
}
