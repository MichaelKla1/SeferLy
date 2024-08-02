using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            InitializeComponent();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
            try
            {
                label2.Visible = false;
                if (rjTextBox1.Texts.Length < 3 || rjTextBox1.Texts.Length > 20 || rjTextBox1.isPlaceholder())
                {
                    label2.Text = "אורך מינינלי של שם משתמש הוא 3";
                    label2.Visible = true;
                    return;
                }
                if (rjTextBox2.Texts.Length < 6 || rjTextBox2.Texts.Length > 30 || rjTextBox2.isPlaceholder())
                {
                    label2.Text = "אורך מינינלי של סיסמא הוא 6";
                    label2.Visible = true;
                    return;
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                Globals.cookiesContainer = new CookieContainer();
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uname=" + rjTextBox1.Texts;
                postData += "&pass=" + rjTextBox2.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&winlogin=1";
                var data = Encoding.ASCII.GetBytes(postData);

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
                    label2.Text = "הגישה ממחשב זה אינו מאושרת";
                    label2.Visible = true;
                }
                else if (responseString == "1")
                {
                    label2.Text = "שם משתמש או סיסמא לא נכונה";
                    label2.Visible = true;
                }
                else if (responseString == "2")
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is NavigationPanel)
                        {
                            Globals.mainForm.Size = Globals.mainFormSizeNavigationPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
                    rjTextBox1.Texts = "";
                    rjTextBox1.turnOnPlaceHolder();
                    rjTextBox2.Texts = "";
                    rjTextBox2.turnOnPlaceHolder();
                    //get current year from server
                    try
                    {
                        request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                        request.CookieContainer = Globals.cookiesContainer;
                        postData = "getdate=1";
                        data = Encoding.UTF8.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        response = (HttpWebResponse)request.GetResponse();

                        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


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
                            int i = 0;
                            foreach (string row in tablerows)
                            {
                                if (i==0)
                                    Globals.current_year = int.Parse(row);
                                else if(i==1)
                                    Globals.current_month = int.Parse(row);
                                else if(i==2)
                                    Globals.current_day = int.Parse(row);
                                else if(i==3)
                                    Globals.defaultBorrowDays = int.Parse(row);
                                i++;
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
                else if (responseString == "3")
                {
                    label2.Text = "המחשב חסום. נשה שנית בעוד 20 דקות";
                    label2.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar < 'a' || e.KeyChar > 'z')
            {
                if(e.KeyChar < 'A' || e.KeyChar > 'Z')
                {
                    if(e.KeyChar < '0' || e.KeyChar > '9')
                    {
                        if (e.KeyChar != 8)//backspace
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void rjTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 'a' || e.KeyChar > 'z')
            {
                if (e.KeyChar < 'A' || e.KeyChar > 'Z')
                {
                    if (e.KeyChar < '0' || e.KeyChar > '9')
                    {
                        if(e.KeyChar < '!' || e.KeyChar > '/')
                        {
                            if(e.KeyChar < ':' || e.KeyChar > '@')
                            {
                                if (e.KeyChar != 8)//backspace
                                {
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoginPanel_Load(object sender, EventArgs e)
        {
            
        }

        private void LoginPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.BringToFront();
                this.Parent.Location = new Point(Math.Max(0, Parent.Location.X), Math.Max(0, Parent.Location.Y));
                this.Parent.Size = Globals.mainFormSizeLoginPanel;
            }
        }
    }
}
