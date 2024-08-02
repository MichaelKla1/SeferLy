using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApplication2.RJControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class EditBorrowPanel : UserControl
    {
        private class TableRow
        {
            public int uid { get; set; }
            public string borrowdate { get; set; }
            public string returndate { get; set; }
            public string comments { get; set; }
            public string username { get; set; }
            public string uname { get; set; }
            public string userid { get; set; }
            public string libisbn { get; set; }
            public string bname { get; set; }
            public string author { get; set; }
            public int status { get; set; }
        }
        private List<TableRow> rowsOfTable = new List<TableRow>();
        private List<RJPanel> panelsOfTableRows = new List<RJPanel>();
        private delegate void MyDelegate();
        private bool searchEnabled = true;

        System.Timers.Timer timer = new System.Timers.Timer(2000);
        public EditBorrowPanel()
        {
            InitializeComponent();
        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {
            timer.Stop();
            if (rjTextBox1.isElementFocused() && rjTextBox1.Texts.Length >= 1)
            {
                timer.Start();
            }
        }
        private void updateTableRows()//updates table rows from rowsOfTable list
        {
            int offset = 0;//3+40+offset*40
            foreach (RJPanel p in panelsOfTableRows)//remove old rows
            {
                p.Dispose();
            }
            panelsOfTableRows.Clear();
            RJPanel rowSample = customTable1.getRowPanel();
            RJPanel headerRowSample = customTable1.getHeaderRowPanel();
            foreach (TableRow tr in rowsOfTable)//add rows from rowsOfTable list
            {

                RJPanel curRow = new RJPanel();


                //create row panel
                curRow.BackColor = rowSample.BackColor;
                curRow.BorderRadius = rowSample.BorderRadius;
                curRow.ForeColor = rowSample.ForeColor;
                curRow.GradienAngle = rowSample.GradienAngle;
                curRow.GradientBottomColor = rowSample.GradientBottomColor;
                curRow.GradientTopColor = rowSample.GradientTopColor;
                curRow.Location = new Point(headerRowSample.Location.X, headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1));
                curRow.Name = "tableRow" + offset.ToString();
                curRow.Size = rowSample.Size;
                curRow.TabIndex = rowSample.TabIndex;

                panelsOfTableRows.Add(curRow);
                //add elements to row panel

                for (int i = 0; i < customTable1.ColumnArr.Length; i++)
                {
                    double percentage = customTable1.ColumnArr[i];
                    double sumperc = 0;
                    for (int j = 0; j < i; j++)
                    {
                        sumperc += customTable1.ColumnArr[j];
                    }
                    if (i == 0)//borrow uid
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.uid.ToString().Length <= 5)
                        {
                            mylab.Text = tr.uid.ToString();
                        }
                        else
                        {
                            mylab.Text = tr.uid.ToString().Substring(0, 4) + "...";
                            toolTip1.SetToolTip(mylab, tr.uid.ToString());
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 1)//borrow date
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.borrowdate.Length <= 10)
                        {
                            mylab.Text = tr.borrowdate;
                        }
                        else
                        {
                            mylab.Text = tr.borrowdate.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.borrowdate);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 2)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.returndate.Length <= 10)
                        {
                            mylab.Text = tr.returndate;
                        }
                        else
                        {
                            mylab.Text = tr.returndate.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.returndate);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 3)
                    {
                        RJTextBox t = new RJTextBox();
                        t.BackColor = System.Drawing.SystemColors.Control;
                        t.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        t.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
                        t.BorderRadius = 7;
                        t.BorderSize = 3;
                        t.EnableRightClickMenu = false;
                        t.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold);
                        t.ForeColor = System.Drawing.Color.DimGray;
                        t.Margin = new System.Windows.Forms.Padding(4);
                        t.MaxLength = 5000;
                        t.Multiline = false;
                        t.Name = "rjTextBox" + i.ToString();
                        t.Padding = new System.Windows.Forms.Padding(7);
                        t.PasswordChar = false;
                        t.Multiline = true;
                        t.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                        t.TabIndex = 24;
                        t.Texts = tr.comments;
                        t.UnderlinedStyle = false;
                        t.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - t.Width / 2), curRow.Height / 2 - t.Height / 2);
                        t.BringToFront();
                        t.ToolTipEnabled = true;
                        toolTip1.SetToolTip(t, tr.comments);
                        curRow.Controls.Add(t);
                    }
                    else if (i == 4)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.username.Length <= 8)
                        {
                            mylab.Text = tr.username;
                        }
                        else
                        {
                            mylab.Text = tr.username.Substring(0, 8) + "...";
                            toolTip1.SetToolTip(mylab, tr.username);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 5)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.uname.Length <= 8)
                        {
                            mylab.Text = tr.uname;
                        }
                        else
                        {
                            mylab.Text = tr.uname.Substring(0, 8) + "...";
                            toolTip1.SetToolTip(mylab, tr.uname);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 6)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.userid.Length <= 10)
                        {
                            mylab.Text = tr.userid;
                        }
                        else
                        {
                            mylab.Text = tr.userid.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.userid);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 7)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.libisbn.Length <= 10)
                        {
                            mylab.Text = tr.libisbn;
                        }
                        else
                        {
                            mylab.Text = tr.libisbn.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.libisbn);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 8)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.bname.Length <= 10)
                        {
                            mylab.Text = tr.bname;
                        }
                        else
                        {
                            mylab.Text = tr.bname.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.bname);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 9)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.author.Length <= 10)
                        {
                            mylab.Text = tr.author;
                        }
                        else
                        {
                            mylab.Text = tr.author.Substring(0, 10) + "...";
                            toolTip1.SetToolTip(mylab, tr.author);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 10)
                    {
                        RJButton b = new RJButton();
                        b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderRadius = 7;
                        b.BorderSize = 3;
                        b.Text = "שמור";
                        b.FlatAppearance.BorderSize = 0;
                        b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        b.Font = new System.Drawing.Font("Ebrima", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        b.ForeColor = System.Drawing.Color.White;
                        b.Name = "rjButton" + i.ToString();
                        b.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                        b.TabIndex = 31;
                        b.TextColor = System.Drawing.Color.White;
                        b.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - b.Width / 2), curRow.Height / 2 - b.Height / 2);
                        b.UseVisualStyleBackColor = false;
                        b.Click += new System.EventHandler(this.save_Button_Click);
                        b.BringToFront();
                        curRow.Controls.Add(b);
                    }
                    else if (i == 11)
                    {
                        if (tr.status == 0)
                        {
                            RJButton b = new RJButton();
                            b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderRadius = 7;
                            b.BorderSize = 3;
                            b.Text = "הערכה בחודש";
                            b.FlatAppearance.BorderSize = 0;
                            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            b.Font = new System.Drawing.Font("Ebrima", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            b.ForeColor = System.Drawing.Color.White;
                            b.Name = "rjButton" + i.ToString();
                            b.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                            b.TabIndex = 31;
                            b.TextColor = System.Drawing.Color.White;
                            b.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - b.Width / 2), curRow.Height / 2 - b.Height / 2);
                            b.UseVisualStyleBackColor = false;
                            b.Click += new System.EventHandler(this.extend_Button_Click);
                            b.BringToFront();
                            curRow.Controls.Add(b);
                        }
                        else if (tr.status == 1)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "הוחזר";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                        else if (tr.status == 2)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "מבוטל";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                    }
                    else if (i == 12)
                    {
                        if (tr.status == 0)
                        {
                            RJButton b = new RJButton();
                            b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderRadius = 7;
                            b.BorderSize = 3;
                            b.Text = "החזרה";
                            b.FlatAppearance.BorderSize = 0;
                            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            b.Font = new System.Drawing.Font("Ebrima", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            b.ForeColor = System.Drawing.Color.White;
                            b.Name = "rjButton" + i.ToString();
                            b.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                            b.TabIndex = 31;
                            b.TextColor = System.Drawing.Color.White;
                            b.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - b.Width / 2), curRow.Height / 2 - b.Height / 2);
                            b.UseVisualStyleBackColor = false;
                            b.Click += new System.EventHandler(this.return_Button_Click);
                            b.BringToFront();
                            curRow.Controls.Add(b);
                        }
                        else if(tr.status == 1)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "הוחזר";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                        else if (tr.status == 2)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "מבוטל";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                    }
                    else if (i == 13)
                    {
                        if (tr.status == 0)
                        {
                            RJButton b = new RJButton();
                            b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                            b.BorderRadius = 7;
                            b.BorderSize = 3;
                            b.Text = "ביטול";
                            b.FlatAppearance.BorderSize = 0;
                            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            b.Font = new System.Drawing.Font("Ebrima", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            b.ForeColor = System.Drawing.Color.White;
                            b.Name = "rjButton" + i.ToString();
                            b.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                            b.TabIndex = 31;
                            b.TextColor = System.Drawing.Color.White;
                            b.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - b.Width / 2), curRow.Height / 2 - b.Height / 2);
                            b.UseVisualStyleBackColor = false;
                            b.Click += new System.EventHandler(this.cancel_Button_Click);
                            b.BringToFront();
                            curRow.Controls.Add(b);
                        }
                        else if (tr.status == 1)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "הוחזר";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                        else if (tr.status == 2)
                        {
                            System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                            mylab.Text = "מבוטל";
                            mylab.BackColor = Color.Transparent;
                            mylab.Font = customTable1.HeaderFont;
                            mylab.AutoSize = false;
                            mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                            mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                            mylab.BringToFront();
                            curRow.Controls.Add(mylab);
                        }
                    }

                }

                customTable1.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            this.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
            customTable1.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
        }
        private void sendSearchBorrowsRequest(Object source, ElapsedEventArgs e)
        {
            searchEnabled = false;
            timer.Stop();
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "searchstr=" + rjTextBox1.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&getborrows=1";
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                //later:
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.url.com/foobar");
                //request.CookieContainer = myContainer;
                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "0")
                {
                    MessageBox.Show("הגישה ממחשב זה אינו מאושרת");

                }
                else if (responseString == "3")//session timed out
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                }
                else if (responseString.StartsWith("["))//json received
                {
                    string json = responseString;
                    //TableRow Ldata = JsonConvert.DeserializeObject<TableRow>(json);
                    List<string> tablerows = JsonConvert.DeserializeObject<List<string>>(json);
                    rowsOfTable.Clear();
                    foreach (string row in tablerows)
                    {
                        TableRow tr = JsonConvert.DeserializeObject<TableRow>(row);
                        rowsOfTable.Add(tr);
                    }
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MyDelegate(updateTableRows));
                    }
                    else
                    {
                        updateTableRows();
                    }
                }
                else
                {
                    MessageBox.Show("קרתה שגיאה");
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
            searchEnabled = true;
        }
        private void EditBorrowPanel_Load(object sender, EventArgs e)
        {
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += sendSearchBorrowsRequest;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
        }

        private void save_Button_Click(object sender, EventArgs e)
        {
            RJPanel curRow = (sender as System.Windows.Forms.Button).Parent as RJPanel; //row panel where button was clicked
            TableRow tr = new TableRow();
            int i = 0;
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == curRow)
                {
                    tr = rowsOfTable.ElementAt(i);
                    break;
                }
                i++;
            }
            string uid = tr.uid.ToString();
            string comments = ((RJTextBox)(curRow.Controls.Find("rjTextBox3", true)[0])).Texts;

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uid=" + uid;
                postData += "&comments=" + comments.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&editborrow=1";
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                //later:
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.url.com/foobar");
                //request.CookieContainer = myContainer;
                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "0")
                {
                    MessageBox.Show("הגישה ממחשב זה אינו מאושרת");
                }
                else if (responseString == "2")
                {
                    MessageBox.Show("נשמר בהצלחה");
                }
                else if (responseString == "3")//session timed out
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("קרתה שגיאה");
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }
        private void extend_Button_Click(object sender, EventArgs e)
        {
            RJPanel curRow = (sender as System.Windows.Forms.Button).Parent as RJPanel; //row panel where button was clicked
            TableRow tr = new TableRow();
            int i = 0;
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == curRow)
                {
                    tr = rowsOfTable.ElementAt(i);
                    break;
                }
                i++;
            }
            string uid = tr.uid.ToString();

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uid=" + uid;
                postData += "&extendborrow=1";
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                //later:
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.url.com/foobar");
                //request.CookieContainer = myContainer;
                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "0")
                {
                    MessageBox.Show("הגישה ממחשב זה אינו מאושרת");
                }
                else if (responseString == "2")
                {
                    MessageBox.Show("נשמר בהצלחה");
                    sendSearchBorrowsRequest(null, null);
                }
                else if (responseString == "3")//session timed out
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("קרתה שגיאה");
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }
        private void cancel_Button_Click(object sender, EventArgs e)
        {
            RJPanel curRow = (sender as System.Windows.Forms.Button).Parent as RJPanel; //row panel where button was clicked
            TableRow tr = new TableRow();
            int i = 0;
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == curRow)
                {
                    tr = rowsOfTable.ElementAt(i);
                    break;
                }
                i++;
            }
            string uid = tr.uid.ToString();

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uid=" + uid;
                postData += "&cancelborrow=1";
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                //later:
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.url.com/foobar");
                //request.CookieContainer = myContainer;
                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "0")
                {
                    MessageBox.Show("הגישה ממחשב זה אינו מאושרת");
                }
                else if (responseString == "2")
                {
                    MessageBox.Show("נשמר בהצלחה");
                    sendSearchBorrowsRequest(null, null);
                }
                else if (responseString == "3")//session timed out
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("קרתה שגיאה");
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }
        private void return_Button_Click(object sender, EventArgs e)
        {
            RJPanel curRow = (sender as System.Windows.Forms.Button).Parent as RJPanel; //row panel where button was clicked
            TableRow tr = new TableRow();
            int i = 0;
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == curRow)
                {
                    tr = rowsOfTable.ElementAt(i);
                    break;
                }
                i++;
            }
            string uid = tr.uid.ToString();

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uid=" + uid;
                postData += "&returnborrow=1";
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                //later:
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.url.com/foobar");
                //request.CookieContainer = myContainer;
                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "0")
                {
                    MessageBox.Show("הגישה ממחשב זה אינו מאושרת");
                }
                else if (responseString == "2")
                {
                    MessageBox.Show("נשמר בהצלחה");
                    sendSearchBorrowsRequest(null, null);
                }
                else if (responseString == "3")//session timed out
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("קרתה שגיאה");
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!searchEnabled)
            {
                e.Handled = true;
            }
        }
    }
}
