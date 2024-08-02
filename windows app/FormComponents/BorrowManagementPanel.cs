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
using System.Windows.Forms;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class BorrowManagementPanel : UserControl
    {
        private bool addBBorrowOpened = false;
        private bool editBorrowOpened = false;
        private AddBorrowPanel aup = null;
        private EditBorrowPanel eup = null;
        public BorrowManagementPanel(AddBorrowPanel aup, EditBorrowPanel eup)
        {
            this.aup = aup;
            this.eup = eup;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (addBBorrowOpened)
            {
                addBBorrowOpened = false;
                aup.Visible = false;
            }
            else
            {
                addBBorrowOpened = true;
                aup.Visible = true;
                editBorrowOpened = false;
                eup.Visible = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (editBorrowOpened)
            {
                editBorrowOpened = false;
                eup.Visible = false;
            }
            else
            {
                editBorrowOpened = true;
                eup.Visible = true;
                addBBorrowOpened = false;
                aup.Visible = false;
            }
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            Globals.mainForm.AutoScroll = false;
            NavigationPanel.checkSession();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
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
                        if (uc is NavigationPanel)
                        {
                            uc.Visible = true;
                            Globals.mainForm.Size = Globals.mainFormSizeNavigationPanel;
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

        private void UserManagementPanel_Load(object sender, EventArgs e)
        {
            aup.Location = new Point(10, label1.Location.Y + label1.Height + 5);
            eup.Location = new Point(label2.Location.X, label2.Location.Y + label2.Height + 5);
        }

        private void UserManagementPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Globals.mainForm.AutoScroll = true;
                Globals.mainForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                Globals.mainForm.MaximizeBox = true;
                aup.BringToFront();
                eup.BringToFront();
            }
            else
            {
                Globals.mainForm.AutoScroll = false;
                Globals.mainForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
                Globals.mainForm.MaximizeBox = false;
            }
        }

        private void customTable1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
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
                        if (uc is NavigationPanel)
                        {
                            uc.Visible = true;
                            Globals.mainForm.WindowState = FormWindowState.Normal;
                            Globals.mainForm.Size = Globals.mainFormSizeNavigationPanel;
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
    }
}
