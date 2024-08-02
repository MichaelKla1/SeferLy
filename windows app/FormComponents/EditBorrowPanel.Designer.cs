namespace WindowsFormsApplication2.FormComponents
{
    partial class EditBorrowPanel
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rjTextBox1 = new WindowsFormsApplication2.RJControls.RJTextBox();
            this.customTable1 = new WindowsFormsApplication2.FormComponents.CustomTable();
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
            this.rjTextBox1.Location = new System.Drawing.Point(1528, 4);
            this.rjTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.rjTextBox1.MaxLength = 20;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.PlaceHolderText = "הזן פרטים";
            this.rjTextBox1.Size = new System.Drawing.Size(211, 53);
            this.rjTextBox1.TabIndex = 34;
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
        6,
        7,
        6,
        9,
        7,
        8,
        7,
        9,
        7,
        7,
        4,
        12,
        4,
        7};
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
        "מספר השאלה",
        "תאריך השאלה",
        "תאריך החזרה",
        "הערות",
        "שם משתמש",
        "שם מלא",
        "ת\'\'ז",
        "מספר ספר",
        "שם ספר",
        "שם מחבר",
        "שמור",
        "הערכה בחודש",
        "החזרה",
        "ביטול"};
            this.customTable1.Location = new System.Drawing.Point(3, 64);
            this.customTable1.Name = "customTable1";
            this.customTable1.RowLabelFont = null;
            this.customTable1.Size = new System.Drawing.Size(1737, 170);
            this.customTable1.TabIndex = 33;
            this.customTable1.TextboxBorderColor = System.Drawing.Color.MediumSlateBlue;
            this.customTable1.TextboxBorderFocusColor = System.Drawing.Color.HotPink;
            this.customTable1.TextboxBorderRadius = 0;
            this.customTable1.TextboxBorderSize = 2;
            this.customTable1.TextboxEnableRightClickMenu = true;
            this.customTable1.TextboxMaxLength = 0;
            this.customTable1.TextboxPlaceHolderText = "";
            this.customTable1.TextboxUnderlinedStyle = false;
            // 
            // EditBorrowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rjTextBox1);
            this.Controls.Add(this.customTable1);
            this.Name = "EditBorrowPanel";
            this.Size = new System.Drawing.Size(1743, 150);
            this.Load += new System.EventHandler(this.EditBorrowPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomTable customTable1;
        private RJControls.RJTextBox rjTextBox1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
