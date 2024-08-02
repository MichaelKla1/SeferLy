namespace WindowsFormsApplication2.FormComponents
{
    partial class CustomTable
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
            this.rjPanel1 = new WindowsFormsApplication2.RJControls.RJPanel();
            this.rjPanel2 = new WindowsFormsApplication2.RJControls.RJPanel();
            this.SuspendLayout();
            // 
            // rjPanel1
            // 
            this.rjPanel1.BackColor = System.Drawing.Color.White;
            this.rjPanel1.BorderRadius = 40;
            this.rjPanel1.ForeColor = System.Drawing.Color.Black;
            this.rjPanel1.GradienAngle = 120F;
            this.rjPanel1.GradientBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(55)))), ((int)(((byte)(210)))));
            this.rjPanel1.GradientTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(70)))), ((int)(((byte)(231)))));
            this.rjPanel1.Location = new System.Drawing.Point(3, 3);
            this.rjPanel1.Name = "rjPanel1";
            this.rjPanel1.Size = new System.Drawing.Size(648, 39);
            this.rjPanel1.TabIndex = 0;
            // 
            // rjPanel2
            // 
            this.rjPanel2.BackColor = System.Drawing.Color.White;
            this.rjPanel2.BorderRadius = 40;
            this.rjPanel2.ForeColor = System.Drawing.Color.Black;
            this.rjPanel2.GradienAngle = 120F;
            this.rjPanel2.GradientBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            this.rjPanel2.GradientTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(193)))), ((int)(((byte)(247)))));
            this.rjPanel2.Location = new System.Drawing.Point(3, 43);
            this.rjPanel2.Name = "rjPanel2";
            this.rjPanel2.Size = new System.Drawing.Size(648, 68);
            this.rjPanel2.TabIndex = 1;
            // 
            // CustomTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rjPanel2);
            this.Controls.Add(this.rjPanel1);
            this.Name = "CustomTable";
            this.Size = new System.Drawing.Size(654, 516);
            this.Load += new System.EventHandler(this.CustomTable_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RJControls.RJPanel rjPanel1;
        private RJControls.RJPanel rjPanel2;
    }
}
