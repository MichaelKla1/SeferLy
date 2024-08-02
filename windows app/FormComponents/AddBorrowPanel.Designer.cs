namespace WindowsFormsApplication2.FormComponents
{
    partial class AddBorrowPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rjButton8 = new WindowsFormsApplication2.RJButton();
            this.rjDatePicker1 = new WindowsFormsApplication2.RJDatePicker();
            this.rjTextBox7 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.rjTextBox1 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.customTable1 = new WindowsFormsApplication2.FormComponents.CustomTable();
            this.rjTextBox2 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.customTable2 = new WindowsFormsApplication2.FormComponents.CustomTable();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.customTable2);
            this.panel1.Location = new System.Drawing.Point(3, 432);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1655, 248);
            this.panel1.TabIndex = 47;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.customTable1);
            this.panel2.Location = new System.Drawing.Point(3, 105);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1649, 216);
            this.panel2.TabIndex = 48;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Mistral", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(686, 722);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(155, 27);
            this.label3.TabIndex = 50;
            this.label3.Text = "תאריך החזרה:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Mistral", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1500, 14);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(147, 27);
            this.label2.TabIndex = 53;
            this.label2.Text = "בחר משתמש:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mistral", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1537, 341);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(110, 27);
            this.label1.TabIndex = 54;
            this.label1.Text = "בחר ספר:";
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
            this.rjButton8.Location = new System.Drawing.Point(120, 733);
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(270, 52);
            this.rjButton8.TabIndex = 52;
            this.rjButton8.Text = "הוספת השאלה";
            this.rjButton8.TextColor = System.Drawing.Color.White;
            this.rjButton8.UseVisualStyleBackColor = false;
            this.rjButton8.Click += new System.EventHandler(this.rjButton8_Click);
            // 
            // rjDatePicker1
            // 
            this.rjDatePicker1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
            this.rjDatePicker1.BorderSize = 2;
            this.rjDatePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.rjDatePicker1.Location = new System.Drawing.Point(585, 761);
            this.rjDatePicker1.MinimumSize = new System.Drawing.Size(4, 35);
            this.rjDatePicker1.Name = "rjDatePicker1";
            this.rjDatePicker1.Size = new System.Drawing.Size(338, 35);
            this.rjDatePicker1.SkinColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjDatePicker1.TabIndex = 51;
            this.rjDatePicker1.TextColor = System.Drawing.Color.White;
            // 
            // rjTextBox7
            // 
            this.rjTextBox7.BackColor = System.Drawing.SystemColors.Control;
            this.rjTextBox7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
            this.rjTextBox7.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
            this.rjTextBox7.BorderRadius = 7;
            this.rjTextBox7.BorderSize = 3;
            this.rjTextBox7.EnableRightClickMenu = true;
            this.rjTextBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold);
            this.rjTextBox7.ForeColor = System.Drawing.Color.DimGray;
            this.rjTextBox7.Location = new System.Drawing.Point(979, 697);
            this.rjTextBox7.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox7.MaxLength = 5000;
            this.rjTextBox7.Multiline = true;
            this.rjTextBox7.Name = "rjTextBox7";
            this.rjTextBox7.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox7.PasswordChar = false;
            this.rjTextBox7.PlaceHolderText = "הערות";
            this.rjTextBox7.Size = new System.Drawing.Size(673, 143);
            this.rjTextBox7.TabIndex = 49;
            this.rjTextBox7.Texts = "הערות";
            this.rjTextBox7.ToolTipEnabled = false;
            this.rjTextBox7.UnderlinedStyle = false;
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
            this.rjTextBox1.Location = new System.Drawing.Point(1431, 45);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox1.MaxLength = 20;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceHolderText = "הזן פרטים";
            this.rjTextBox1.Size = new System.Drawing.Size(211, 53);
            this.rjTextBox1.TabIndex = 48;
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
        15,
        15,
        15,
        8,
        15,
        9,
        17,
        6};
            this.customTable1.ColumnTypeLabelTextboxOrButton = new int[] {
        0,
        1,
        1,
        1,
        1,
        0,
        1,
        2,
        2,
        2};
            this.customTable1.HeaderFont = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTable1.LabelArr = new string[] {
        "שם משתמש",
        "שם מלא",
        "דואר אלקטרוני",
        "טלפון",
        "כתובת",
        "ת\'\'ז",
        "הערות",
        "זמינות"};
            this.customTable1.Location = new System.Drawing.Point(0, 3);
            this.customTable1.Name = "customTable1";
            this.customTable1.RowLabelFont = null;
            this.customTable1.Size = new System.Drawing.Size(1640, 170);
            this.customTable1.TabIndex = 47;
            this.customTable1.TextboxBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customTable1.TextboxBorderFocusColor = System.Drawing.Color.HotPink;
            this.customTable1.TextboxBorderRadius = 0;
            this.customTable1.TextboxBorderSize = 2;
            this.customTable1.TextboxEnableRightClickMenu = true;
            this.customTable1.TextboxMaxLength = 0;
            this.customTable1.TextboxPlaceHolderText = "";
            this.customTable1.TextboxUnderlinedStyle = false;
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
            this.rjTextBox2.Location = new System.Drawing.Point(1431, 372);
            this.rjTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox2.MaxLength = 20;
            this.rjTextBox2.Multiline = false;
            this.rjTextBox2.Name = "rjTextBox2";
            this.rjTextBox2.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox2.PasswordChar = false;
            this.rjTextBox2.PlaceHolderText = "הזן פרטים";
            this.rjTextBox2.Size = new System.Drawing.Size(211, 53);
            this.rjTextBox2.TabIndex = 48;
            this.rjTextBox2.Texts = "הזן פרטים";
            this.rjTextBox2.ToolTipEnabled = false;
            this.rjTextBox2.UnderlinedStyle = false;
            this.rjTextBox2._TextChanged += new System.EventHandler(this.rjTextBox2__TextChanged);
            this.rjTextBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rjTextBox2_KeyPress);
            // 
            // customTable2
            // 
            this.customTable2.ButtonBorderColor = System.Drawing.Color.PaleVioletRed;
            this.customTable2.ButtonBorderRadius = 0;
            this.customTable2.ButtonBorderSize = 0;
            this.customTable2.ColumnArr = new int[] {
        13,
        13,
        14,
        14,
        14,
        5,
        14,
        7,
        6};
            this.customTable2.ColumnTypeLabelTextboxOrButton = new int[] {
        0,
        0,
        1,
        1,
        1,
        1,
        0,
        2,
        2};
            this.customTable2.HeaderFont = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTable2.LabelArr = new string[] {
        "מספר ספר",
        "ISBN",
        "שם המחבר",
        "שם הספר",
        "תקציר הספר",
        "שנת הוצאה",
        "הערות",
        "תאריך הכנסה",
        "זמינות"};
            this.customTable2.Location = new System.Drawing.Point(-3, 4);
            this.customTable2.Name = "customTable2";
            this.customTable2.RowLabelFont = null;
            this.customTable2.Size = new System.Drawing.Size(1652, 170);
            this.customTable2.TabIndex = 49;
            this.customTable2.TextboxBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customTable2.TextboxBorderFocusColor = System.Drawing.Color.HotPink;
            this.customTable2.TextboxBorderRadius = 0;
            this.customTable2.TextboxBorderSize = 2;
            this.customTable2.TextboxEnableRightClickMenu = true;
            this.customTable2.TextboxMaxLength = 0;
            this.customTable2.TextboxPlaceHolderText = "";
            this.customTable2.TextboxUnderlinedStyle = false;
            // 
            // AddBorrowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rjTextBox1);
            this.Controls.Add(this.rjTextBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rjButton8);
            this.Controls.Add(this.rjDatePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rjTextBox7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AddBorrowPanel";
            this.Size = new System.Drawing.Size(1661, 850);
            this.Load += new System.EventHandler(this.AddBorrowPanel_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private RJControls.RJTextBox rjTextBox2;
        private CustomTable customTable2;
        private System.Windows.Forms.Panel panel2;
        private RJControls.RJTextBox rjTextBox1;
        private CustomTable customTable1;
        private RJControls.RJTextBox rjTextBox7;
        private System.Windows.Forms.Label label3;
        private RJDatePicker rjDatePicker1;
        private RJButton rjButton8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
