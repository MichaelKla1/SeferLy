using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication2.RJControls;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class CustomTable : UserControl
    {
        private int[] columnSizeArr = { };
        private int[] columnTypeLabelTextboxOrButton = { };
        private string[] columnTextArr = { };
        private Font headerFont = null;
        private int buttonBorderSize = 0;
        private int buttonBorderRadius = 0;
        private Color buttonBorderColor = Color.PaleVioletRed;
        private Font rowLabelFont = null;
        private Color textboxBorderColor = Color.MediumSlateBlue;
        private int textboxBorderSize = 2;
        private int textboxBorderRadius = 0;
        private string textboxPlaceHolderText = "";
        private bool textboxUnderlinedStyle = false;
        private Color textboxBorderFocusColor = Color.HotPink;
        private int textboxMaxLength = 0;
        private bool textboxEnableRightClickMenu = true;
        public CustomTable()
        {
            InitializeComponent();
        }
        public RJPanel getRowPanel()
        {
            return rjPanel2;
        }
        public RJPanel getHeaderRowPanel()
        {
            return rjPanel1;
        }
        [Category("RJ Code Advance")]
        public bool TextboxEnableRightClickMenu
        {
            get { return textboxEnableRightClickMenu; }
            set
            {
                textboxEnableRightClickMenu = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public bool TextboxUnderlinedStyle
        {
            get { return textboxUnderlinedStyle; }
            set
            {
                textboxUnderlinedStyle = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public string TextboxPlaceHolderText
        {
            get { return textboxPlaceHolderText; }
            set
            {
                textboxPlaceHolderText = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int TextboxMaxLength
        {
            get { return textboxMaxLength; }
            set
            {
                textboxMaxLength = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color TextboxBorderFocusColor
        {
            get { return textboxBorderFocusColor; }
            set
            {
                textboxBorderFocusColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int TextboxBorderSize
        {
            get { return textboxBorderSize; }
            set
            {
                textboxBorderSize = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public int TextboxBorderRadius
        {
            get { return textboxBorderRadius; }
            set
            {
                textboxBorderRadius = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color TextboxBorderColor
        {
            get { return textboxBorderColor; }
            set
            {
                textboxBorderColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int ButtonBorderSize
        {
            get { return buttonBorderSize; }
            set
            {
                buttonBorderSize = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public int ButtonBorderRadius
        {
            get { return buttonBorderRadius; }
            set
            {
                buttonBorderRadius = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color ButtonBorderColor
        {
            get { return buttonBorderColor; }
            set
            {
                buttonBorderColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Font RowLabelFont
        {
            get { return rowLabelFont; }
            set
            {
                rowLabelFont = value;
            }
        }
        [Category("RJ Code Advance")]
        public Font HeaderFont
        {
            get { return headerFont; }
            set
            {
                headerFont = value;
            }
        }
        [Category("RJ Code Advance")]
        public int[] ColumnTypeLabelTextboxOrButton
        {
            get { return columnTypeLabelTextboxOrButton; }
            set
            {
                columnTypeLabelTextboxOrButton = value;
            }
        }
        [Category("RJ Code Advance")]
        public int[] ColumnArr
        {
            get { return columnSizeArr; }
            set
            {
                columnSizeArr = value;
            }
        }
        [Category("RJ Code Advance")]
        public RJPanel HeaderPanel
        {
            get { return rjPanel1; }
            set
            {
                rjPanel1 = value;
            }
        }
        [Category("RJ Code Advance")]
        public RJPanel RowPanel
        {
            get { return rjPanel2; }
            set
            {
                rjPanel2 = value;
            }
        }
        [Category("RJ Code Advance")]
        public string[] LabelArr
        {
            get { return columnTextArr; }
            set
            {
                columnTextArr = value;
            }
        }

        private void CustomTable_Load(object sender, EventArgs e)
        {
            try
            {
                rjPanel1.Width = this.Width - 10;
                rjPanel2.Width = this.Width - 10;
                int i = 0;
                foreach (string l in columnTextArr)
                {
                    double percentage = columnSizeArr[i];
                    double sumperc = 0;
                    for (int j = 0; j < i; j++)
                    {
                        sumperc += columnSizeArr[j];
                    }
                    System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                    mylab.Text = l;
                    mylab.BackColor = Color.Transparent;
                    mylab.Font = headerFont;
                    mylab.Location = new Point((int)(sumperc / 100 * rjPanel1.Width + percentage / 100 * rjPanel1.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), (int)(rjPanel1.Height / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Height / 2));
                    //mylab.Location = new Point((int)(sumperc / 100 * rjPanel1.Width + percentage / 100 * rjPanel1.Width / 2 - mylab.Width / 2), rjPanel1.Height / 2 - mylab.Height / 2);
                    rjPanel1.Controls.Add(mylab);
                    mylab.BringToFront();
                    i++;
                    
                }
                rjPanel2.Visible = false;
                return;
            }
            catch
            {

            }
        }
    }
}
