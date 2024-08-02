using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApplication2.Properties;
using System.Runtime;

namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        

        [STAThread]
        static void Main()
        {
            try
            {
                if (!File.Exists("settings.ini"))
                {
                    using (FileStream fs = File.Create("settings.ini"))
                    {

                    }
                    using (StreamWriter writetext = new StreamWriter("settings.ini"))
                    {
                        writetext.WriteLine("[ServerAddress] = http://127.0.0.1/");
                    }
                }
                string[] lines = File.ReadAllLines("settings.ini");
                bool found_server_address = false;
                foreach (string line in lines)
                {
                    if(line.StartsWith("[ServerAddress]"))
                    {
                        string line_parameter = line.Substring(line.IndexOf("[ServerAddress]") + "[ServerAddress]".Length, line.Length - "[ServerAddress]".Length).Replace(" ", "");
                        if(line_parameter.StartsWith("="))
                        {
                            line_parameter = line_parameter.Substring(1, line_parameter.Length - 1);
                            Globals.serverAddr = line_parameter;
                            found_server_address = true;
                        }
                        else
                        {
                            throw new IOException(@"settings.ini file not in correct format. Correct example:"+ Environment.NewLine+"[ServerAddress] = http://127.0.0.1/");
                        }
                    }
                }
                if (found_server_address)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form1 f1 = new Form1();
                    f1.FormBorderStyle = FormBorderStyle.FixedSingle;
                    Globals.mainForm = f1;
                    Application.Run(f1);
                }
                else
                {
                    throw new IOException(@"Server address not found. Correct example:"+ Environment.NewLine+"[ServerAddress] = http://127.0.0.1/");
                }
            }
            catch(Exception e)
            {
                
                MessageBox.Show(e.Message);
            }
        }
    }
}
