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
using System.Xml.Linq;
using WindowsFormsApplication2.RJControls;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class NavigationPanel : UserControl
    {
        public NavigationPanel()
        {
            InitializeComponent();
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            checkSession();
        }
        public static void hideRecursivePanel(Control c)
        {
            foreach (Control uc in c.Controls)
            {
                uc.Visible = false;
                hideRecursivePanel(uc);
            }
        }
        public static void showRecursivePanel(Control c)
        {
            foreach (Control uc in c.Controls)
            {
                uc.Visible = true;
                hideRecursivePanel(uc);
            }
        }
        public static void checkSession()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;

                var postData = "logout=1";
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

                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "2" || responseString == "3")
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is LoginPanel)
                        {
                            uc.Visible = true;
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeLoginPanel;
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
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;

                var postData = "check=1";
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

                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "2")
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is UserManagementPanel)
                        {
                            Globals.mainForm.Size = Globals.mainFormSizeUserManagementPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
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
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;

                var postData = "check=1";
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

                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "2")
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is BookManagementPanel)
                        {
                            Globals.mainForm.Size = Globals.mainFormSizeBookManagementPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
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

        private void NavigationPanel_Load(object sender, EventArgs e)
        {

        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;

                var postData = "check=1";
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

                responseString = responseString.Replace("\r\n", string.Empty);
                if (responseString == "2")
                {
                    foreach (Control uc in Globals.mainForm.Controls)
                    {
                        if (uc is BorrowManagementPanel)
                        {
                            Globals.mainForm.Size = Globals.mainFormSizeBorrowManagementPanel;
                            uc.Visible = true;
                        }
                        else
                        {
                            uc.Visible = false;
                        }
                    }
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

        private void NavigationPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.Parent.Location = new Point(Math.Max(0, Parent.Location.X), Math.Max(0, Parent.Location.Y));
            }
        }
    }
}
