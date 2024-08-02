using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApplication2.RJControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class EditBookPanel : UserControl
    {
        private class TableRow
        {
            public int uid { get; set; }
            public string libisbn { get; set; }
            public string isbn { get; set; }
            public string author { get; set; }
            public string bname { get; set; }
            public string binfo { get; set; }
            public string byear { get; set; }
            public string dateadded { get; set; }
            public string comments { get; set; }
            public int deleted { get; set; }
        }
        private List<TableRow> rowsOfTable = new List<TableRow>();
        private List<RJPanel> panelsOfTableRows = new List<RJPanel>();
        private delegate void MyDelegate();
        private bool searchEnabled = true;

        System.Timers.Timer timer = new System.Timers.Timer(2000);
        public EditBookPanel()
        {
            InitializeComponent();
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
                    if (i == 0)//libisbn label
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.libisbn.Length <= 15)
                        {
                            mylab.Text = tr.libisbn;
                        }
                        else
                        {
                            mylab.Text = tr.libisbn.Substring(0, 15) + "...";
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
                    else if (i == 1)//isbn
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.libisbn.Length <= 15)
                        {
                            mylab.Text = tr.isbn;
                        }
                        else
                        {
                            mylab.Text = tr.isbn.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.isbn);
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
                        RJTextBox t = new RJTextBox();
                        t.BackColor = System.Drawing.SystemColors.Control;
                        t.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        t.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
                        t.BorderRadius = 7;
                        t.BorderSize = 3;
                        t.EnableRightClickMenu = false;
                        t.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
                        t.ForeColor = System.Drawing.Color.DimGray;
                        t.Margin = new System.Windows.Forms.Padding(4);
                        t.MaxLength = 50;
                        t.Multiline = false;
                        t.Name = "rjTextBox" + i.ToString();
                        t.Padding = new System.Windows.Forms.Padding(7);
                        t.PasswordChar = false;
                        t.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                        t.TabIndex = 24;
                        t.Texts = tr.author;
                        t.UnderlinedStyle = false;
                        t.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - t.Width / 2), curRow.Height / 2 - t.Height / 2 + 10);
                        t.BringToFront();
                        t.ToolTipEnabled = true;
                        toolTip1.SetToolTip(t, tr.author);
                        curRow.Controls.Add(t);
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
                        t.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
                        t.ForeColor = System.Drawing.Color.DimGray;
                        t.Margin = new System.Windows.Forms.Padding(4);
                        t.MaxLength = 50;
                        t.Multiline = false;
                        t.Name = "rjTextBox" + i.ToString();
                        t.Padding = new System.Windows.Forms.Padding(7);
                        t.PasswordChar = false;
                        t.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                        t.TabIndex = 24;
                        t.Texts = tr.bname;
                        t.UnderlinedStyle = false;
                        t.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - t.Width / 2), curRow.Height / 2 - t.Height / 2 + 10);
                        t.BringToFront();
                        t.ToolTipEnabled = true;
                        toolTip1.SetToolTip(t, tr.bname);
                        curRow.Controls.Add(t);
                    }
                    else if (i == 4)
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
                        t.Texts = tr.binfo;
                        t.UnderlinedStyle = false;
                        t.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - t.Width / 2), curRow.Height / 2 - t.Height / 2);
                        t.BringToFront();
                        t.ToolTipEnabled = true;
                        toolTip1.SetToolTip(t, tr.binfo);
                        curRow.Controls.Add(t);
                    }
                    else if (i == 5)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        mylab.Text = "עריכה";
                        
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.Cursor = Cursors.Hand;
                        mylab.ForeColor = Color.DarkCyan;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        mylab.Click += picture_And_Genres_Edit_Label_Click;
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 6)
                    {
                        RJTextBox t = new RJTextBox();
                        t.BackColor = System.Drawing.SystemColors.Control;
                        t.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        t.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(36)))), ((int)(((byte)(88)))));
                        t.BorderRadius = 7;
                        t.BorderSize = 3;
                        t.EnableRightClickMenu = false;
                        t.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold);
                        t.ForeColor = System.Drawing.Color.DimGray;
                        t.Margin = new System.Windows.Forms.Padding(4);
                        t.MaxLength = 4;
                        t.Multiline = false;
                        t.Name = "rjTextBox" + i.ToString();
                        t.Padding = new System.Windows.Forms.Padding(7);
                        t.PasswordChar = false;
                        t.Size = new System.Drawing.Size((int)((double)curRow.Width / 100.0 * percentage) - 6, curRow.Height - 6);
                        t.TabIndex = 24;
                        t.Texts = tr.byear;
                        t.UnderlinedStyle = false;
                        t.Location = new System.Drawing.Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - t.Width / 2), curRow.Height / 2 - t.Height / 2 + 10);
                        t.BringToFront();
                        t.ToolTipEnabled = true;
                        t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.byear_Textbox_KeyPress);
                        toolTip1.SetToolTip(t, tr.byear);
                        curRow.Controls.Add(t);
                    }
                    else if (i == 7)
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
                    else if (i == 8)//dateadded
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();

                        mylab.Text = tr.dateadded.Substring(0, tr.dateadded.IndexOf(' '));

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
                        RJButton b = new RJButton();
                        b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderRadius = 7;
                        b.BorderSize = 3;
                        b.Text = "שמור";
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
                        b.Click += new System.EventHandler(this.save_Button_Click);
                        b.BringToFront();
                        curRow.Controls.Add(b);
                    }
                    else if (i == 10)
                    {
                        RJButton b = new RJButton();
                        b.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(68)))), ((int)(((byte)(170)))));
                        b.BorderRadius = 7;
                        b.BorderSize = 3;
                        b.Text = "שחזר";
                        if (tr.deleted == 0)
                        {
                            b.Text = "מחק";
                        }
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
                        b.Click += new System.EventHandler(this.delete_Button_Click);
                        b.BringToFront();
                        curRow.Controls.Add(b);
                    }
                    
                }

                customTable1.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            this.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
            customTable1.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {
            timer.Stop();
            if (rjTextBox1.isElementFocused() && rjTextBox1.Texts.Length >= 2)
            {
                timer.Start();
            }
        }
        private void sendSearchBooksRequest(Object source, ElapsedEventArgs e)
        {
            searchEnabled = false;
            timer.Stop();
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "searchstr=" + rjTextBox1.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&getbooks=1";
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
        private void EditBookPanel_Load(object sender, EventArgs e)
        {
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += sendSearchBooksRequest;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!searchEnabled)
            {
                e.Handled = true;
            }
        }
        private void byear_Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                if (e.KeyChar != 8)//backspace
                {
                    e.Handled = true;
                }
            }
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
            string author = ((RJTextBox)(curRow.Controls.Find("rjTextBox2", true)[0])).Texts;
            string bname = ((RJTextBox)(curRow.Controls.Find("rjTextBox3", true)[0])).Texts;
            string binfo = ((RJTextBox)(curRow.Controls.Find("rjTextBox4", true)[0])).Texts;
            string byear = ((RJTextBox)(curRow.Controls.Find("rjTextBox6", true)[0])).Texts;
            string comments = ((RJTextBox)(curRow.Controls.Find("rjTextBox7", true)[0])).Texts;
            if (author.Length < 2)
            {
                MessageBox.Show("אורך מינימלי של שם מחבר הוא 2");
                return;
            }
            //check correctness of e-mail
            if (bname.Length < 2)
            {
                MessageBox.Show("אורך מינימלי של שם הספר הוא 2");
                return;
            }
            if (byear.Length != 4)
            {
                MessageBox.Show("אורך של שנת הוצאה לאור חייב להיות 4");
                return;
            }
            else
            {
                int entered_year = int.Parse(byear);
                if (entered_year < 1900 || entered_year > Globals.current_year)
                {
                    MessageBox.Show("שנת הוצאה לאור חייב להיות מספר בין 1900 לשנה נוכחת");
                    return;
                }
            }

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "author=" + author.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&bname=" + bname.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&binfo=" + binfo.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&byear=" + byear;
                postData += "&uid=" + uid;
                postData += "&comments=" + comments.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&editbook=1";
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
                    sendSearchBooksRequest(null, null);
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
        private void delete_Button_Click(object sender, EventArgs e)
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
            int deleted = tr.deleted;

            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uid=" + uid;
                postData += "&deletebook=1";
                if (deleted == 0)
                {
                    postData += "&action=1";
                }
                else
                {
                    postData += "&action=0";
                }
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
                    if (deleted == 0)
                    {
                        tr.deleted = 1;
                        (sender as RJButton).Text = "שחזר";
                    }
                    else
                    {
                        tr.deleted = 0;
                        (sender as RJButton).Text = "מחק";
                    }
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
        private void picture_And_Genres_Edit_Label_Click(object sender, EventArgs e)
        {
            RJPanel curRow = (sender as System.Windows.Forms.Label).Parent as RJPanel; //row panel where button was clicked
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
            string libisbמ = tr.libisbn;
            EditPictureAndGenresForBook ee = new EditPictureAndGenresForBook(uid, libisbמ);
            ee.ShowDialog();
        }
    }
}
