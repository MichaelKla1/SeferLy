using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication2.RJControls
{
    [DefaultEvent("_TextChanged")]
    public partial class RJTextBox : UserControl
    {
        //Fields
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 2;
        private int borderRadius = 0;
        private string placeHolderText = "";
        private bool isPlaceHolder = true;
        private bool underlinedStyle = false;
        private Color borderFocusColor = Color.HotPink;
        private bool isFocused = false;
        private int maxLength = 0;
        private bool enableRightClickMenu = true;
        private bool toolTipEnabled = false;

        //Constructor
        public RJTextBox()
        {
            InitializeComponent();
        }

        //Events
        public event EventHandler _TextChanged;

        //Properties
        [Category("RJ Code Advance")]
        public bool EnableRightClickMenu
        {
            get { return enableRightClickMenu; }
            set
            {
                enableRightClickMenu = value;
                if(enableRightClickMenu)
                {
                    textBox1.ShortcutsEnabled = true;
                }
                else
                {
                    textBox1.ShortcutsEnabled = false;
                }
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                maxLength = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public string PlaceHolderText
        {
            get { return placeHolderText; }
            set
            {
                placeHolderText = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public bool PasswordChar
        {
            get { return textBox1.UseSystemPasswordChar; }
            set { textBox1.UseSystemPasswordChar = value; }
        }

        [Category("RJ Code Advance")]
        public bool Multiline
        {
            get { return textBox1.Multiline; }
            set { textBox1.Multiline = value; }
        }
        [Category("RJ Code Advance")]
        public bool ToolTipEnabled
        {
            get { return toolTipEnabled; }
            set { toolTipEnabled = value; }
        }
        [Category("RJ Code Advance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                textBox1.BackColor = value;
            }
        }

        [Category("RJ Code Advance")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }

        [Category("RJ Code Advance")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                textBox1.Font = value;
                if (this.DesignMode)
                    UpdateControlHeight();
            }
        }

        [Category("RJ Code Advance")]
        public string Texts
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        [Category("RJ Code Advance")]
        public Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }
        //Methods
        public bool isPlaceholder()
        {
            return isPlaceHolder;
        }
        public void turnOnPlaceHolder()
        {
            isPlaceHolder = true;
            textBox1.Text = placeHolderText;
        }
        public void turnOffPlaceHolder()
        {
            isPlaceHolder = false;
        }
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        //Overridden methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;
            Graphics graph = e.Graphics;

            //Draw border
            using (Pen penBorder = new Pen(borderColor, borderSize))
            {
                penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                if (isFocused) penBorder.Color = borderFocusColor;
                if (borderRadius <= 1)
                {
                    if (underlinedStyle) //Line Style
                        graph.DrawLine(penBorder, 0, this.Height - 1, this.Width, this.Height - 1);
                    else //Normal Style
                        graph.DrawRectangle(penBorder, 0, 0, this.Width - 0.5F, this.Height - 0.5F);
                }
                else
                {
                    using (GraphicsPath pathSurface = GetFigurePath(this.ClientRectangle, borderRadius))
                    using (GraphicsPath pathBorder = GetFigurePath(Rectangle.Inflate(this.ClientRectangle, -borderSize, -borderSize), borderRadius - borderSize))
                    using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                    using (Pen penBorder1 = new Pen(borderColor, borderSize))
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        //Button surface
                        this.Region = new Region(pathSurface);
                        //Draw surface border for HD result
                        e.Graphics.DrawPath(penSurface, pathSurface);
                        //Button border                    
                        if (borderSize >= 1)
                            //Draw control border
                            e.Graphics.DrawPath(penBorder, pathBorder);
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.DesignMode)
                UpdateControlHeight();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }

        //Private methods
        private void UpdateControlHeight()
        {
            if (textBox1.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", this.Font).Height + 1;
                textBox1.Multiline = true;
                textBox1.MinimumSize = new Size(0, txtHeight);
                textBox1.Multiline = false;

                this.Height = textBox1.Height + this.Padding.Top + this.Padding.Bottom;
            }
        }

        //TextBox events
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_TextChanged != null)
                _TextChanged.Invoke(sender, e);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (placeHolderText != "")
            {
                isFocused = true;
                if (isPlaceHolder)
                {
                    textBox1.Text = "";
                }
                this.Invalidate();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (placeHolderText != "")
            {
                isFocused = false;
                if (textBox1.Text == "")
                {
                    isPlaceHolder = true;
                    textBox1.Text = placeHolderText;
                }
                else
                {
                    isPlaceHolder = false;
                }
                this.Invalidate();
            }
        }
        public bool isElementFocused()
        {
            return isFocused;
        }
        private void RJTextBox_Load(object sender, EventArgs e)
        {
            if (placeHolderText != "")
            {
                textBox1.Text = placeHolderText;
            }
            if(toolTipEnabled)
            {
                toolTip1.SetToolTip(textBox1, this.Texts);
            }
        }

        private void RJTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(textBox1.Text.Length>=maxLength && e.KeyChar!=8 && maxLength > 0)
            {
                e.Handled = true;
            }
        }


        ///::::+
    }
}
