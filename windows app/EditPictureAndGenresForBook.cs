using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApplication2.FormComponents;
using WindowsFormsApplication2.RJControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApplication2
{
    public partial class EditPictureAndGenresForBook : Form
    {
        string bookuid;
        string booklibisbn;
        private class TableRow
        {
            public string genre { get; set; }
            public string description { get; set; }
        }
        private List<TableRow> rowsOfTable = new List<TableRow>();
        private List<TableRow> rowsOfTable1 = new List<TableRow>();
        private List<RJPanel> panelsOfTableRows = new List<RJPanel>();
        private List<RJPanel> panelsOfTableRows1 = new List<RJPanel>();
        public string choice1 = null;
        public string choice2 = null;
        public EditPictureAndGenresForBook(string uid,string libisbn)
        {
            bookuid = uid;
            booklibisbn = libisbn;
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
                curRow.Cursor = Cursors.Hand;
                curRow.Click += book_Genre_Row_Click;

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
                    if (i == 0)//genre label
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.genre.Length <= 15)
                        {
                            mylab.Text = tr.genre;
                        }
                        else
                        {
                            mylab.Text = tr.genre.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.genre);
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
                        if (tr.description.Length <= 20 && (tr.description.IndexOf("\r\n") < 0))
                        {
                            mylab.Text = tr.description;
                        }
                        else
                        {
                            if (tr.description.Substring(0, Math.Min(tr.description.Length, 20)).IndexOf("\r\n") < 0)
                            {
                                mylab.Text = tr.description.Substring(0, Math.Min(tr.description.Length, 20)) + "...";
                                toolTip1.SetToolTip(mylab, tr.description);
                            }
                            else
                            {
                                mylab.Text = tr.description.Substring(0, tr.description.Substring(0, Math.Min(tr.description.Length, 20)).IndexOf("\r\n")) + "...";
                                toolTip1.SetToolTip(mylab, tr.description);
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
                }
                foreach (Control c in curRow.Controls)
                {
                    c.Click += book_Genre_Row_Click;
                }
                customTable1.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            customTable1.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable1.Location.Y;
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
            foreach (TableRow tr in rowsOfTable1)//add rows from rowsOfTable list
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
                curRow.Click += genre_Row_Click;

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
                    if (i == 0)//genre label
                    {
                        System.Windows.Forms.Label mylab = new System.Windows.Forms.Label();
                        if (tr.genre.Length <= 15)
                        {
                            mylab.Text = tr.genre;
                        }
                        else
                        {
                            mylab.Text = tr.genre.Substring(0, 15) + "...";
                            toolTip1.SetToolTip(mylab, tr.genre);
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
                        if (tr.description.Length <= 20 && (tr.description.IndexOf("\r\n") < 0))
                        {
                            mylab.Text = tr.description;
                        }
                        else
                        {
                            if (tr.description.Substring(0, Math.Min(tr.description.Length, 20)).IndexOf("\r\n") < 0)
                            {
                                mylab.Text = tr.description.Substring(0, Math.Min(tr.description.Length, 20)) + "...";
                                toolTip1.SetToolTip(mylab, tr.description);
                            }
                            else
                            {
                                mylab.Text = tr.description.Substring(0, tr.description.Substring(0, Math.Min(tr.description.Length, 20)).IndexOf("\r\n")) + "...";
                                toolTip1.SetToolTip(mylab, tr.description);
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

                }
                foreach (Control c in curRow.Controls)
                {
                    c.Click += genre_Row_Click;
                }
                customTable2.Controls.Add(curRow);
                curRow.BringToFront();

                offset++;
            }
            customTable2.Height = headerRowSample.Location.Y + headerRowSample.Height + 1 + offset * (rowSample.Height + 1) + customTable2.Location.Y;
        }
        private void sendGenresFindRequest(string action)
        {
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "action=" + action;
                postData += "&getgenres=1";
                postData += "&uid=" + bookuid;
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
                    this.Close();
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
                    if (action == "1")
                    {
                        rowsOfTable1.Clear();
                        foreach (string row in tablerows)
                        {
                            TableRow tr = JsonConvert.DeserializeObject<TableRow>(row);
                            rowsOfTable1.Add(tr);
                        }
                        updateTableRows1();
                    }
                    else if (action == "0")
                    {
                        rowsOfTable.Clear();
                        foreach (string row in tablerows)
                        {
                            TableRow tr = JsonConvert.DeserializeObject<TableRow>(row);
                            rowsOfTable.Add(tr);
                        }
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
        }
        private void sendPictureFindRequest()
        {
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "getpicture=1";
                postData += "&uid=" + bookuid;
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
                    this.Close();
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
                    foreach (string row in tablerows)//parsing image
                    {
                        if (row != "\"0\"")//if there is image
                        {
                            try
                            {
                                string base64string = JsonConvert.DeserializeObject<string>(row);  // Put the full string here

                                // Convert Base64 String to byte[]
                                //string file = @"C:\7-Zip\designseferly.png";
                                //string bb = Convert.ToBase64String(File.ReadAllBytes(file));



                                byte[] imageBytes = Convert.FromBase64String(base64string);
                                MemoryStream ms = new MemoryStream(imageBytes, 0,imageBytes.Length);

                                // Convert byte[] to Image
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                Image image = Image.FromStream(ms, true);

                                pictureBox1.Image = image;
                            }
                            catch
                            {
                                
                            }
                        }
                        else
                        {
                            pictureBox1.Image = null;
                        }
                        break;
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
        private void EditPictureAndGenresForBook_Load(object sender, EventArgs e)
        {
            this.Text = " עריכת תמונה וג'נרים לספר" + booklibisbn;
            sendGenresFindRequest("0");
            sendGenresFindRequest("1");
            sendPictureFindRequest();
        }
        private void book_Genre_Row_Click(object sender, EventArgs e)
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
            RJPanel rowSample = customTable1.getRowPanel();
            foreach (RJPanel p in panelsOfTableRows)
            {
                if (p == choice)
                {
                    choice1 = rowsOfTable.ElementAt(i).genre;
                    p.GradientBottomColor = Color.Gold;
                    p.GradientTopColor = Color.Orange;
                    p.Cursor = Cursors.Default;
                    p.Click -= book_Genre_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= book_Genre_Row_Click;
                    }
                }
                else
                {
                    p.GradientBottomColor = rowSample.GradientBottomColor;
                    p.GradientTopColor = rowSample.GradientTopColor;
                    p.Cursor = Cursors.Hand;
                    p.Click -= book_Genre_Row_Click;
                    p.Click += book_Genre_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= book_Genre_Row_Click;
                        c.Click += book_Genre_Row_Click;
                    }
                }
                i++;
            }
        }
        private void genre_Row_Click(object sender, EventArgs e)
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
                    choice2 = rowsOfTable1.ElementAt(i).genre;
                    p.GradientBottomColor = Color.Gold;
                    p.GradientTopColor = Color.Orange;
                    p.Cursor = Cursors.Default;
                    p.Click -= genre_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= genre_Row_Click;
                    }
                }
                else
                {
                    p.GradientBottomColor = rowSample.GradientBottomColor;
                    p.GradientTopColor = rowSample.GradientTopColor;
                    p.Cursor = Cursors.Hand;
                    p.Click -= genre_Row_Click;
                    p.Click += genre_Row_Click;
                    foreach (Control c in p.Controls)
                    {
                        c.Click -= genre_Row_Click;
                        c.Click += genre_Row_Click;
                    }
                }
                i++;
            }
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            if (rjTextBox1.Texts.Length < 2 || rjTextBox1.isPlaceholder())
            {
                MessageBox.Show("אורך מינימלי של שם ג'נר הוא 2");
                return;
            }
            //all entered data is correct. send request to server
            try
            {
                string com = "";
                if (!rjTextBox2.isPlaceholder())
                {
                    com = rjTextBox2.Texts;
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "genre=" + rjTextBox1.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&comments=" + com.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&addgenre=1";
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
                    choice2 = null;
                    sendGenresFindRequest("1");
                }
                else if (responseString == "4")
                {
                    MessageBox.Show("שם ג'נר כבר קיים");
                }
                else if (responseString == "3")//session timed out
                {
                    this.Close();
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

        private void rjButton2_Click(object sender, EventArgs e)
        {
            if (choice2 == null)
            {
                MessageBox.Show("בחר ג'נר למחיקה");
                return;
            }
            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "genre=" + choice2;
                postData += "&deletegenre=1";
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
                    MessageBox.Show("לא ניתן למחוק ג'נר כי הוא כבר נימצא כג'נר של ספרים קיימים");
                }
                else if (responseString == "2")
                {
                    choice2 = null;
                    sendGenresFindRequest("1");
                }
                else if (responseString == "4")
                {
                    choice2 = null;
                    sendGenresFindRequest("1");
                }
                else if (responseString == "3")//session timed out
                {
                    this.Close();
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

        private void rjButton8_Click(object sender, EventArgs e)
        {
            if (choice2 == null)
            {
                MessageBox.Show("בחר ג'נר להוספה בשביך ספר");
                return;
            }
            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "genre=" + choice2;
                postData += "&addgenreforbook=1";
                postData += "&uid=" + bookuid;
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
                    choice2 = null;
                    choice1 = null;
                    sendGenresFindRequest("1");
                    sendGenresFindRequest("0");
                }
                else if (responseString == "4")
                {
                    choice2 = null;
                    choice1 = null;
                    sendGenresFindRequest("1");
                    sendGenresFindRequest("0");
                }
                else if (responseString == "3")//session timed out
                {
                    this.Close();
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

        private void rjButton1_Click(object sender, EventArgs e)
        {
            if (choice1 == null)
            {
                MessageBox.Show("בחר ג'נר למחיקה בשביך ספר");
                return;
            }
            //all entered data is correct. send request to server
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "genre=" + choice1;
                postData += "&deletegenreforbook=1";
                postData += "&uid=" + bookuid;
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
                    choice2 = null;
                    choice1 = null;
                    sendGenresFindRequest("1");
                    sendGenresFindRequest("0");
                }
                else if (responseString == "4")
                {
                    choice2 = null;
                    choice1 = null;
                    sendGenresFindRequest("1");
                    sendGenresFindRequest("0");
                }
                else if (responseString == "3")//session timed out
                {
                    this.Close();
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

        private void rjButton5_Click(object sender, EventArgs e)
        {
            //perform search request
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Images (*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;|*.jpeg;|*.gif";
                openFileDialog1.FilterIndex = 0;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string file = openFileDialog1.FileName;
                string converted_file = Convert.ToBase64String(File.ReadAllBytes(file));
                if((new FileInfo(file).Length) > 20971520)
                {
                    MessageBox.Show("גודל קובץ מקבי הוא 20 MB");
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "addpictureforbook=1";
                postData += "&uid=" + bookuid;
                postData += "&picture=" + converted_file.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
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
                    this.Close();
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
                else if (responseString == "2")
                {
                    sendPictureFindRequest();
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

        private void rjButton4_Click(object sender, EventArgs e)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "deletepictureforbook=1";
                postData += "&uid=" + bookuid;
              
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
                    this.Close();
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
                else if (responseString == "2")
                {
                    sendPictureFindRequest();
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
