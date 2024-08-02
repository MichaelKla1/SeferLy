using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace WindowsFormsApplication2.RJControls
{
    public class RJPanel : Panel
    {
        //Fields
        private int borderRadius { get; set; }
        private float gradientAngle { get; set; }
        private Color gradientTopColor { get; set; }
        private Color gradientBottomColor { get; set; }

        //Constructor
        public RJPanel()
        {
            borderRadius = 5;
            gradientAngle = 90F;
            gradientTopColor = Color.DodgerBlue;
            gradientBottomColor = Color.CadetBlue;
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Size = new Size(350, 200);

        }

        //Properties
        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public float GradienAngle
        {
            get
            {
                return gradientAngle;
            }
            set
            {
                gradientAngle = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color GradientTopColor
        {
            get
            {
                return gradientTopColor;
            }
            set
            {
                gradientTopColor = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public Color GradientBottomColor
        {
            get
            {
                return gradientBottomColor;
            }
            set
            {
                gradientBottomColor = value;
                this.Invalidate();
            }
        }

        //Methods
        private GraphicsPath GetPath(RectangleF rectangle, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Height - radius, radius, radius, 90, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Y, radius, radius, 270, 90);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        //Oveeriden Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            //Gradient
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,this.gradientTopColor,this.gradientBottomColor,this.gradientAngle);
            Graphics g = e.Graphics;
            g.FillRectangle(brush, ClientRectangle);
            //BorderRadius
            RectangleF rectangleF = new RectangleF(0, 0, this.Width, this.Height);
            if (borderRadius > 2)
            {
                using (GraphicsPath graphicsPath = GetPath(rectangleF, borderRadius))
                using (Pen pen = new Pen(this.Parent.BackColor, 2))
                {
                    this.Region = new Region(graphicsPath);
                    e.Graphics.DrawPath(pen, graphicsPath);
                }
            }
            else
            {
                this.Region = new Region(rectangleF);
            }
            base.OnPaint(e);
        }

    }
}
