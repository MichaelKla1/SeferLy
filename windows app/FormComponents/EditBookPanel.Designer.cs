namespace WindowsFormsApplication2.FormComponents
{
    partial class EditBookPanel
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
            this.components = new System.ComponentModel.Container();
            this.rjTextBox1 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.customTable1 = new WindowsFormsApplication2.FormComponents.CustomTable();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
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
            this.rjTextBox1.Location = new System.Drawing.Point(1428, 4);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox1.MaxLength = 20;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceHolderText = "הזן פרטים";
            this.rjTextBox1.Size = new System.Drawing.Size(211, 53);
            this.rjTextBox1.TabIndex = 31;
            this.rjTextBox1.Texts = "הזן פרטים";
            this.rjTextBox1.ToolTipEnabled = false;
            this.rjTextBox1.UnderlinedStyle = false;
            this.rjTextBox1._TextChanged += new System.EventHandler(this.rjTextBox1__TextChanged);
            this.rjTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rjTextBox1_KeyPress);
            // 
            // customTable1
            // 
            this.customTable1.ButtonBorderColor = System.Drawing.Color.PaleVioletRed;
            this.customTable1.ButtonBorderRadius = 0;
            this.customTable1.ButtonBorderSize = 0;
            this.customTable1.ColumnArr = new int[] {
        12,
        12,
        11,
        11,
        11,
        6,
        5,
        13,
        7,
        6,
        6};
            this.customTable1.ColumnTypeLabelTextboxOrButton = new int[] {
        0,
        0,
        1,
        1,
        1,
        1,
        0,
        2,
        2};
            this.customTable1.HeaderFont = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTable1.LabelArr = new string[] {
        "מספר ספר",
        "ISBN",
        "שם המחבר",
        "שם הספר",
        "תקציר הספר",
        "תמונה וג\'נרים",
        "שנת הוצאה",
        "הערות",
        "תאריך הכנסה",
        "שמור",
        "מחק"};
            this.customTable1.Location = new System.Drawing.Point(3, 64);
            this.customTable1.Name = "customTable1";
            this.customTable1.RowLabelFont = null;
            this.customTable1.Size = new System.Drawing.Size(1652, 170);
            this.customTable1.TabIndex = 32;
            this.customTable1.TextboxBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customTable1.TextboxBorderFocusColor = System.Drawing.Color.HotPink;
            this.customTable1.TextboxBorderRadius = 0;
            this.customTable1.TextboxBorderSize = 2;
            this.customTable1.TextboxEnableRightClickMenu = true;
            this.customTable1.TextboxMaxLength = 0;
            this.customTable1.TextboxPlaceHolderText = "";
            this.customTable1.TextboxUnderlinedStyle = false;
            // 
            // EditBookPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customTable1);
            this.Controls.Add(this.rjTextBox1);
            this.Name = "EditBookPanel";
            this.Size = new System.Drawing.Size(1658, 150);
            this.Load += new System.EventHandler(this.EditBookPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RJControls.RJTextBox rjTextBox1;
        private CustomTable customTable1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
