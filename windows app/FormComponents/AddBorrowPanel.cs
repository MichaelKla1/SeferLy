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

    public partial class AddBorrowPanel : UserControl
    {
        private class TableRow1
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

            public int borrowed { get; set; }
        }
        private List<TableRow1> rowsOfTable1 = new List<TableRow1>();
        private List<RJPanel> panelsOfTableRows1 = new List<RJPanel>();
        private bool searchEnabled1 = true;
        System.Timers.Timer timer1 = new System.Timers.Timer(2000);


        private class TableRow
        {
            public int uid { get; set; }
            public string username { get; set; }
            public string fullname { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string id { get; set; }
            public string comments { get; set; }
            public int deleted { get; set; }
        }
        private List<TableRow> rowsOfTable = new List<TableRow>();
        private List<RJPanel> panelsOfTableRows = new List<RJPanel>();
        private delegate void MyDelegate();
        private bool searchEnabled = true;
        private int userChoice = 0;
        private int bookChoice = 0;

        System.Timers.Timer timer = new System.Timers.Timer(2000);
        public AddBorrowPanel()
        {
            InitializeComponent();
        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {
            timer.Stop();
            userChoice = 0;
            if (rjTextBox1.isElementFocused() && rjTextBox1.Texts.Length >= 2)
            {
                timer.Start();
            }
        }
        private void sendSearchUsersRequest(Object source, ElapsedEventArgs e)
        {
            searchEnabled = false;
            timer.Stop();
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "searchstr=" + rjTextBox1.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&getusers=1";
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
            panel2.VerticalScroll.Value = 0;
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
                curRow.Cursor = Cursors.Hand;
                curRow.Click += user_Row_Click;

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
                    if (i == 0)//username label
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.username.Length <= 15)
                        {
                            mylab.Text = tr.username;
                        }
                        else
                        {
                            mylab.Text = tr.username.Substring(0, 15) + "...";
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
                    else if (i == 1)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.fullname.Length <= 15)
                        {
                            mylab.Text = tr.fullname;
                        }
                        else
                        {
                            mylab.Text = tr.fullname.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.fullname);
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
                        if (tr.email.Length <= 15)
                        {
                            mylab.Text = tr.email;
                        }
                        else
                        {
                            mylab.Text = tr.email.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.email);
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
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.phone.Length <= 15)
                        {
                            mylab.Text = tr.phone;
                        }
                        else
                        {
                            mylab.Text = tr.phone.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.phone);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 4)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.address.Length <= 15)
                        {
                            mylab.Text = tr.address;
                        }
                        else
                        {
                            mylab.Text = tr.address.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.address);
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
                        if (tr.id.Length <= 15)
                        {
                            mylab.Text = tr.id;
                        }
                        else
                        {
                            mylab.Text = tr.id.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.id);
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
                        if (tr.comments.Length <= 20 && (tr.comments.IndexOf("\r\n")<0))
                        {
                            mylab.Text = tr.comments;
                        }
                        else
                        {
                            if (tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)).IndexOf("\r\n") < 0)
                            {
                                mylab.Text = tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)) + "...";
                                toolTip1.SetToolTip(mylab, tr.comments);
                            }
                            else
                            {
                                mylab.Text = tr.comments.Substring(0, tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)).IndexOf("\r\n")) + "...";
                                toolTip1.SetToolTip(mylab, tr.comments);
                            }
                        }
                        mylab.UseMnemonic = false;
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
                        if (tr.deleted == 0)
                        {
                            mylab.Text = "זמין";
                        }
                        else
                        {
                            mylab.Text = "לא זמין";
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable1.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    
                }
                foreach(Control c in curRow.Controls)
                {
                    c.Click += user_Row_Click;
                }
                customTable1.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            customTable1.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
            panel2.Invalidate();
        }
        private void updateTableRows1()//updates table rows from rowsOfTable list
        {
            int offset = 0;//3+40+offset*40
            foreach (RJPanel p in panelsOfTableRows1)//remove old rows
            {
                p.Dispose();
            }
            panelsOfTableRows1.Clear();
            RJPanel rowSample = customTable2.getRowPanel();
            RJPanel headerRowSample = customTable2.getHeaderRowPanel();
            panel1.VerticalScroll.Value = 0;
            foreach (TableRow1 tr in rowsOfTable1)//add rows from rowsOfTable list
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
                curRow.Cursor = Cursors.Hand;
                curRow.Click += book_Row_Click;

                panelsOfTableRows1.Add(curRow);
                //add elements to row panel

                for (int i = 0; i < customTable2.ColumnArr.Length; i++)
                {
                    double percentage = customTable2.ColumnArr[i];
                    double sumperc = 0;
                    for (int j = 0; j < i; j++)
                    {
                        sumperc += customTable2.ColumnArr[j];
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
                        mylab.Font = customTable2.HeaderFont;
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
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 2)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.author.Length <= 15)
                        {
                            mylab.Text = tr.author;
                        }
                        else
                        {
                            mylab.Text = tr.author.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.author);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 3)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.bname.Length <= 15)
                        {
                            mylab.Text = tr.bname;
                        }
                        else
                        {
                            mylab.Text = tr.bname.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.bname);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 4)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.binfo.Length <= 20 && (tr.binfo.IndexOf("\r\n") < 0))
                        {
                            mylab.Text = tr.binfo;
                        }
                        else
                        {
                            if (tr.binfo.Substring(0, Math.Min(tr.binfo.Length,20)).IndexOf("\r\n") < 0)
                            {
                                mylab.Text = tr.binfo.Substring(0, Math.Min(tr.binfo.Length, 20)) + "...";
                                toolTip1.SetToolTip(mylab, tr.binfo);
                            }
                            else
                            {
                                mylab.Text = tr.binfo.Substring(0, tr.binfo.Substring(0, Math.Min(tr.binfo.Length, 20)).IndexOf("\r\n")) + "...";
                                toolTip1.SetToolTip(mylab, tr.binfo);
                            }
                        }
                        mylab.UseMnemonic = false;
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 5)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.byear.Length <= 15)
                        {
                            mylab.Text = tr.byear;
                        }
                        else
                        {
                            mylab.Text = tr.byear.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.byear);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 6)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.comments.Length <= 20 && (tr.comments.IndexOf("\r\n") < 0))
                        {
                            mylab.Text = tr.comments;
                        }
                        else
                        {
                            if (tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)).IndexOf("\r\n") < 0)
                            {
                                mylab.Text = tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)) + "...";
                                toolTip1.SetToolTip(mylab, tr.comments);
                            }
                            else
                            {
                                mylab.Text = tr.comments.Substring(0, tr.comments.Substring(0, Math.Min(tr.comments.Length, 20)).IndexOf("\r\n")) + "...";
                                toolTip1.SetToolTip(mylab, tr.comments);
                            }
                        }
                        mylab.UseMnemonic = false;
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 7)//dateadded
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.dateadded.Length <= 15)
                        {
                            mylab.Text = tr.dateadded;
                        }
                        else
                        {
                            mylab.Text = tr.dateadded.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.dateadded);
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    else if (i == 8)
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.deleted == 0)
                        {
                            if (tr.borrowed == 0)
                            {
                                mylab.Text = "זמין";
                            }
                            else
                            {
                                mylab.Text = "הושאל";
                                toolTip1.SetToolTip(mylab, "מספר השאלה\r\n"+tr.borrowed.ToString());
                            }
                        }
                        else
                        {
                            mylab.Text = "לא זמין";
                        }
                        mylab.BackColor = Color.Transparent;
                        mylab.Font = customTable2.HeaderFont;
                        mylab.AutoSize = false;
                        mylab.Width = (int)mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width + 5;
                        mylab.Location = new Point((int)(sumperc / 100 * curRow.Width + percentage / 100 * curRow.Width / 2 - mylab.CreateGraphics().MeasureString(mylab.Text, mylab.Font).Width / 2), curRow.Height / 2 - mylab.Height / 2);
                        mylab.BringToFront();
                        curRow.Controls.Add(mylab);
                    }
                    

                }
                foreach (Control c in curRow.Controls)
                {
                    c.Click += book_Row_Click;
                }
                customTable2.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            customTable2.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable2.Location.Y;
            panel1.Invalidate();
        }
        private void CurRow_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddBorrowPanel_Load(object sender, EventArgs e)
        {
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += sendSearchUsersRequest;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
            rjDatePicker1.Value = (new DateTime(Globals.current_year, Globals.current_month, Globals.current_day)).AddDays(Globals.defaultBorrowDays);

            timer1.Elapsed += sendSearchBooksRequest;
            timer1.AutoReset = true;
            timer1.Enabled = true;
            timer1.Stop();
        }
        private void sendSearchBooksRequest(Object source, ElapsedEventArgs e)
        {
            searchEnabled1 = false;
            timer1.Stop();
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "searchstr=" + rjTextBox2.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&getbooks=1";
                postData += "&forborrow=1";
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
                    rowsOfTable1.Clear();
                    foreach (string row in tablerows)
                    {
                        TableRow1 tr = JsonConvert.DeserializeObject<TableRow1>(row);
                        rowsOfTable1.Add(tr);
                    }
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MyDelegate(updateTableRows1));
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
            searchEnabled1 = true;
        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!searchEnabled)
            {
                e.Handled = true;
            }
        }
        private void rjTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!searchEnabled1)
            {
                e.Handled = true;
            }
        }
        private void user_Row_Click(object sender, EventArgs e)
        {
            RJPanel choice = null;
            if(sender is RJPanel)
            {
                choice = sender as RJPanel;
            }
            else
            {
                choice = (sender as Control).Parent as RJPanel;
            }
            int i = 0;
            RJPanel rowSample = customTable1.getRowPanel();
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == choice)
                {
                    if (rowsOfTable.ElementAt(i).deleted != 0)
                    {
                        MessageBox.Show("לא ניתן לבחור משתמש לא זמין. על מנת להפוך משתמש לזמין צריך לשחזר אותו");
                        return;
                    }
                }
                i++;
            }
            i = 0;
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == choice)
                {
                    userChoice = rowsOfTable.ElementAt(i).uid;
                    p.GradientBottomColor = Color.Gold;
                    p.GradientTopColor = Color.Orange;
                    p.Cursor = Cursors.Default;
                    p.Click -= user_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= user_Row_Click;
                    }
                }
                else
                {
                    p.GradientBottomColor = rowSample.GradientBottomColor;
                    p.GradientTopColor = rowSample.GradientTopColor;
                    p.Cursor = Cursors.Hand;
                    p.Click -= user_Row_Click;
                    p.Click += user_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= user_Row_Click;
                        c.Click += user_Row_Click;
                    }
                }
                i++;
            }
        }
        private void book_Row_Click(object sender, EventArgs e)
        {
            RJPanel choice = null;
            if (sender is RJPanel)
            {
                choice = sender as RJPanel;
            }
            else
            {
                choice = (sender as Control).Parent as RJPanel;
            }
            int i = 0;
            RJPanel rowSample = customTable2.getRowPanel();
            foreach (RJPanel p in panelsOfTableRows1)
            {
                if (p == choice)
                {
                    if (rowsOfTable1.ElementAt(i).deleted != 0)
                    {
                        MessageBox.Show("לא ניתן לבחור ספר לא זמין. על מנת להפוך ספר לזמין צריך לשחזר אותו");
                        return;
                    }
                    if (rowsOfTable1.ElementAt(i).borrowed != 0)
                    {
                        MessageBox.Show("לא ניתן לבחור ספר שכבר הושאל. מספר השאלה "+ rowsOfTable1.ElementAt(i).borrowed);
                        return;
                    }
                }
                i++;
            }
            i = 0;
            foreach (RJPanel p in panelsOfTableRows1)
            {
                if (p == choice)
                {
                    bookChoice = rowsOfTable1.ElementAt(i).uid;
                    p.GradientBottomColor = Color.Gold;
                    p.GradientTopColor = Color.Orange;
                    p.Cursor = Cursors.Default;
                    p.Click -= book_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= book_Row_Click;
                    }
                }
                else
                {
                    p.GradientBottomColor = rowSample.GradientBottomColor;
                    p.GradientTopColor = rowSample.GradientTopColor;
                    p.Cursor = Cursors.Hand;
                    p.Click -= book_Row_Click;
                    p.Click += book_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= book_Row_Click;
                        c.Click += book_Row_Click;
                    }
                }
                i++;
            }
        }

        private void rjTextBox2__TextChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            if (rjTextBox2.isElementFocused() && rjTextBox2.Texts.Length >= 2)
            {
                timer1.Start();
            }
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
            if(rjDatePicker1.Value.Date<new DateTime(Globals.current_year,Globals.current_month,Globals.current_day))
            {
                MessageBox.Show("תאריך החזרה חייב להיות לא יותר מוקדם מתאריך נוכחי");
                return;
            }
            if(userChoice==0)
            {
                MessageBox.Show("בחר משתמש");
                return;
            }
            if(bookChoice == 0)
            {
                MessageBox.Show("בחר ספר");
                return;
            }
            try
            {
                string com = "";
                if (!rjTextBox7.isPlaceholder())
                {
                    com = rjTextBox7.Texts;
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "bid=" + bookChoice.ToString();
                postData += "&uid=" + userChoice.ToString();
                postData += "&borrowdate=" + rjDatePicker1.Value.Date.ToString();
                postData += "&comments=" + com.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&addborrow=1";
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
                else if (responseString == "1")
                {
                    MessageBox.Show("ספר כבר הושאל");
                }
                else if (responseString == "6")
                {
                    MessageBox.Show("לא ניתן להוסיך השאלה. משתמש לא זמין");
                }
                else if (responseString == "4")
                {
                    MessageBox.Show("לא ניתן להוסיך השאלה. ספר לא זמין");
                }
                else if (responseString == "7")
                {
                    MessageBox.Show("למשתמש זה כבר יש מקסימום ספרים בהשאלה. בשביל להוסיף השאלה חדשה למשתמש זה, הוא צריך להחזיר ספרים");
                }
                else if (responseString == "2")
                {
                    MessageBox.Show("ההשאלה הוספה בהצלחה");
                    bookChoice = 0;
                    userChoice = 0;
                    rjTextBox1.Texts = "";
                    rjTextBox2.Texts = "";
                    rjTextBox7.Texts = "";
                    rjTextBox1.turnOnPlaceHolder();
                    rjTextBox2.turnOnPlaceHolder();
                    rjTextBox7.turnOnPlaceHolder();
                    rjDatePicker1.Value = (new DateTime(Globals.current_year, Globals.current_month, Globals.current_day)).AddDays(Globals.defaultBorrowDays);
                    foreach (RJPanel p in panelsOfTableRows)//remove old rows
                    {
                        p.Dispose();
                    }
                    foreach (RJPanel p in panelsOfTableRows1)//remove old rows
                    {
                        p.Dispose();
                    }
                    rowsOfTable.Clear();
                    panelsOfTableRows.Clear();
                    rowsOfTable1.Clear();
                    panelsOfTableRows1.Clear();
                    panel1.VerticalScroll.Value = 0;
                    panel1.Invalidate();
                    panel2.VerticalScroll.Value = 0;
                    panel2.Invalidate();
                    RJPanel rowSample = customTable2.getRowPanel();
                    RJPanel headerRowSample = customTable2.getHeaderRowPanel();
                    customTable2.Height = headerRowSample.Location.Y + headerRowSample.Height + 1  + customTable2.Location.Y;
                    rowSample = customTable1.getRowPanel();
                    headerRowSample = customTable1.getHeaderRowPanel();
                    customTable1.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + customTable2.Location.Y;
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
    }
}
