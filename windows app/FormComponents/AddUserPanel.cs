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
using System.Windows.Forms;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class AddUserPanel : UserControl
    {
        public AddUserPanel()
        {
            InitializeComponent();
        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 'a' || e.KeyChar > 'z')
            {
                if (e.KeyChar < 'A' || e.KeyChar > 'Z')
                {
                    if (e.KeyChar < '0' || e.KeyChar > '9')
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
                    if (e.KeyChar < 'א' || e.KeyChar > 'ת')
                    {
                        if (e.KeyChar != 8 && e.KeyChar != ' ' && e.KeyChar != '\'')//backspace and space
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void rjTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 'a' || e.KeyChar > 'z')
            {
                if (e.KeyChar < 'A' || e.KeyChar > 'Z')
                {
                    if (e.KeyChar < '0' || e.KeyChar > '9')
                    {
                        if (e.KeyChar != 8 && e.KeyChar != '+' && e.KeyChar != '.' && e.KeyChar != '@')//backspace and special signs
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void rjTextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                if (e.KeyChar != 8)//backspace
                {
                    e.Handled = true;
                }
            }
        }

        private void rjTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 'a' || e.KeyChar > 'z')
            {
                if (e.KeyChar < 'A' || e.KeyChar > 'Z')
                {
                    if (e.KeyChar < '0' || e.KeyChar > '9')
                    {
                        if (e.KeyChar < 'א' || e.KeyChar > 'ת')
                        {
                            if (e.KeyChar != 8 && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != '\'')//backspace and special signs
                            {
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            if(rjTextBox1.Texts.Length < 3 || rjTextBox1.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של שם משתמש הוא 3";
                label1.Visible = true;
                return;
            }
            if (rjTextBox2.Texts.Length < 3 || rjTextBox2.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של שם מלא הוא 3";
                label1.Visible = true;
                return;
            }
            //check correctness of e-mail
            if(rjTextBox3.Texts.IndexOf('@')<=0 || rjTextBox3.Texts.LastIndexOf('.') < rjTextBox3.Texts.IndexOf('@') || rjTextBox3.Texts.IndexOf('@') != rjTextBox3.Texts.LastIndexOf('@') || rjTextBox3.Texts.LastIndexOf('.')==rjTextBox3.Texts.Length-1)
            {
                label1.Text = "דואר אלקטרוני בפורמט לא נכון";
                label1.Visible = true;
                return;
            }
            if (rjTextBox4.Texts.Length < 8 || rjTextBox4.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של טלפון הוא 8";
                label1.Visible = true;
                return;
            }
            if (rjTextBox5.Texts.Length < 3 || rjTextBox5.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של כתובת הוא 3";
                label1.Visible = true;
                return;
            }
            if (rjTextBox6.Texts.Length < 8 || rjTextBox6.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של ת''ז הוא 8";
                label1.Visible = true;
                return;
            }

            //all entered data is correct. send request to server
            try
            {
                string com = "";
                if (!rjTextBox7.isPlaceholder())
                {
                    com = rjTextBox7.Texts;
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "uname=" + rjTextBox1.Texts;
                postData += "&name=" + rjTextBox2.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&mail=" + rjTextBox3.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&phone=" + rjTextBox4.Texts;
                postData += "&address=" + rjTextBox5.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&taz=" + rjTextBox6.Texts;
                postData += "&comments=" + com.Replace("&","%26").Replace("\r\n", "%0D%0A");
                postData += "&adduser=1";
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
                    label1.Text = "הגישה ממחשב זה אינו מאושרת";
                    label1.Visible = true;
                }
                else if (responseString == "1")
                {
                    label1.Text = "שם משתמש שהוזן כבר קיים";
                    label1.Visible = true;
                }
                else if (responseString == "2")
                {
                    label1.Text = "המשתמש הוסף בהצלחה";
                    label1.Visible = true;
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
                else if (responseString == "5")
                {
                    label1.Text = "ת''ז שהוזן כבר קיים";
                    label1.Visible = true;
                }
                else if (responseString == "9")
                {
                    label1.Text = "לא ניתן לשלוח מייל לכתובת שהוזנה. ניתן להזין מייל אחר בעריכת משתמש ולאפס סיסמה";
                    label1.Visible = true;
                }
                else
                {
                    label1.Text = "קרתה שגיאה";
                    label1.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("קרתה שגיאה בהתחברות לשרת. בדוק חיבור לאינטרנט.");
            }
        }

        private void AddUserPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
