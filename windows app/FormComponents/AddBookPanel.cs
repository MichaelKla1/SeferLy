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
using BarcodeStandard;
using WindowsFormsApplication2.RJControls;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication2.FormComponents
{
    public partial class AddBookPanel : UserControl
    {
        private System.Timers.Timer timer = new System.Timers.Timer(2000);
        private bool searchEnabled = true;
        private delegate void MyDelegate();
        private class ISBNSample
        {
            public int copynum { get; set; }
            public string author { get; set; }
            public string bname { get; set; }
            public string byear { get; set; }
            public string binfo { get; set; }
        }
        public AddBookPanel()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            if (rjTextBox1.Texts.Length < 3 || rjTextBox1.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של ISBN הוא 3";
                label1.Visible = true;
                return;
            }
            if (rjTextBox2.Texts.Length < 2 || rjTextBox2.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של שם המחבר הוא 2";
                label1.Visible = true;
                return;
            }
            if (rjTextBox3.Texts.Length < 2 || rjTextBox2.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של שם הספר הוא 2";
                label1.Visible = true;
                return;
            }
            if (rjTextBox4.Texts.Length < 3 || rjTextBox4.isPlaceholder())
            {
                label1.Text = "אורך מינימלי של תקציר הספר הוא 3";
                label1.Visible = true;
                return;
            }
            if (rjTextBox5.Texts.Length == 4 && !rjTextBox5.isPlaceholder())
            {
                int entered_year = int.Parse(rjTextBox5.Texts);
                if (entered_year < 1900 || entered_year > Globals.current_year)
                {
                    label1.Text = "שנת הוצאה לאור חייב להיות מספר בין 1900 לשנה נוכחת";
                    label1.Visible = true;
                    return;
                }
            }
            else
            {
                label1.Text = "שנת הוצאה לאור חייב להיות מספר בין 1900 לשנה נוכחת";
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
                string binfo = "";
                if (!rjTextBox4.isPlaceholder())
                {
                    binfo = rjTextBox4.Texts;
                }
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "isbn=" + rjTextBox1.Texts;
                postData += "&author=" + rjTextBox2.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&bname=" + rjTextBox3.Texts.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+", "%2B").Replace("?", "%3F");
                postData += "&byear=" + rjTextBox5.Texts;
                postData += "&binfo=" + binfo.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&comments=" + com.Replace("&", "%26").Replace("\r\n", "%0D%0A").Replace("+","%2B").Replace("?","%3F");
                postData += "&addbook=1";
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
                else if (responseString == "2")
                {
                    label1.Text = "הספר הוסף בהצלחה";
                    label1.Visible = true;
                    rjTextBox1.Texts = "";
                    rjTextBox1.turnOnPlaceHolder();
                    rjTextBox2.Texts = "";
                    rjTextBox2.turnOnPlaceHolder();
                    rjTextBox3.Texts = "";
                    rjTextBox3.turnOnPlaceHolder();
                    rjTextBox4.Texts = "";
                    rjTextBox4.turnOnPlaceHolder();
                    rjTextBox7.Texts = "";
                    rjTextBox7.turnOnPlaceHolder();
                    rjTextBox5.Texts = "";
                    rjTextBox5.turnOnPlaceHolder();
                }
                else if (responseString == "6")
                {
                    label1.Text = "לא ניתן להוסיף יותר עותקים של ספר זה";
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
        private void sendISBNRequest()
        {
            searchEnabled = false;
            timer.Stop();
            //perform search request
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Globals.serverAddr + "?page=processwinapp");
                request.CookieContainer = Globals.cookiesContainer;
                var postData = "isbn=" + rjTextBox1.Texts;
                postData += "&getisbn=1";
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
                int copynum = 0;
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
                else if (responseString == "5")//ISBN not exists
                {
                    string barcode = rjTextBox1.Texts + "-000";
                    BarcodeStandard.Barcode barcode2 = new BarcodeStandard.Barcode()
                    {
                        EncodedType = BarcodeStandard.Type.Code128,
                    };

                    Image img = Image.FromStream(barcode2.Encode(BarcodeStandard.Type.Code128, barcode, pictureBox1.Width, pictureBox1.Height).Encode().AsStream());
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        Color customColor = Color.White;
                        SolidBrush shadowBrush = new SolidBrush(customColor);
                        Rectangle ee = new Rectangle(0, img.Height - 35, img.Width, 35);
                        g.FillRectangles(shadowBrush, new RectangleF[] { ee });
                        g.DrawString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))), Brushes.Black, img.Width / 2 - g.MeasureString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))).Width / 2, img.Height - 17 - g.MeasureString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))).Height / 2);
                    }
                    pictureBox1.Image = img;
                    
                }
                else if (responseString.StartsWith("["))//json received, ISBN exists in database, fill the fields accordingly
                {
                    string json = responseString;
                    //TableRow Ldata = JsonConvert.DeserializeObject<TableRow>(json);
                    List<string> tablerows = JsonConvert.DeserializeObject<List<string>>(json);
                    foreach (string row in tablerows)
                    {
                        ISBNSample tr = JsonConvert.DeserializeObject<ISBNSample>(row);
                        rjTextBox2.Texts = tr.author;
                        rjTextBox2.turnOffPlaceHolder();
                        rjTextBox3.Texts = tr.bname;
                        rjTextBox3.turnOffPlaceHolder();
                        rjTextBox5.Texts = tr.byear;
                        rjTextBox5.turnOffPlaceHolder();
                        rjTextBox4.Texts = tr.binfo;
                        rjTextBox4.turnOffPlaceHolder();
                        copynum = tr.copynum;
                        if(copynum>999)
                        {
                            MessageBox.Show("לא ניתן להוסיף יותר עותקים של ספר זה");
                            return;
                        }
                        string copy = "00";
                        if(copynum.ToString().Length==2)
                        {
                            copy = "0";
                        }
                        else if(copynum.ToString().Length==3)
                        {
                            copy = "";
                        }
                        
                        string barcode = rjTextBox1.Texts +"-"+ copy+copynum.ToString();
                        BarcodeStandard.Barcode barcode2 = new BarcodeStandard.Barcode()
                        {
                            EncodedType = BarcodeStandard.Type.Code128,
                        };

                        Image img = Image.FromStream(barcode2.Encode(BarcodeStandard.Type.Code128, barcode,pictureBox1.Width,pictureBox1.Height).Encode().AsStream());
                        using (Graphics g = Graphics.FromImage(img))
                        {
                            Color customColor = Color.White;
                            SolidBrush shadowBrush = new SolidBrush(customColor);
                            Rectangle ee = new Rectangle(0, img.Height-35, img.Width, 35);
                            g.FillRectangles(shadowBrush, new RectangleF[] { ee });
                            g.DrawString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))), Brushes.Black, img.Width/2 - g.MeasureString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))).Width/2, img.Height - 17 - g.MeasureString(barcode, new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))).Height / 2);
                        }
                        pictureBox1.Image = img;
                        return;
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
            finally
            {
                searchEnabled = true;
            }
        }
        private void sendISBNRequest1(Object source, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MyDelegate(sendISBNRequest));
            }
        }
        private void AddBookPanel_Load(object sender, EventArgs e)
        {
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += sendISBNRequest1;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();

        }

        private void rjTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!searchEnabled)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                if (e.KeyChar != 8 && e.KeyChar != '-')//backspace and '-' sign
                {
                    e.Handled = true;
                }
            }
        }

        private void rjTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < '0' || e.KeyChar > '9')
            {
                if (e.KeyChar != 8)//backspace
                {
                    e.Handled = true;
                }
            }
        }

        private void rjTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void rjTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void rjTextBox1__TextChanged(object sender, EventArgs e)
        {
            timer.Stop();
            if (rjTextBox1.isElementFocused() && rjTextBox1.Texts.Length >= 5)
            {
                timer.Start();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image!=null)
                Clipboard.SetImage(pictureBox1.Image);
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            
        }
    }
}
