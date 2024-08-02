using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication2.FormComponents;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddUserPanel aup = new AddUserPanel();
            aup.Visible = false;
            EditUserPanel eup = new EditUserPanel();
            eup.Visible = false;
            AddBookPanel abp = new AddBookPanel();
            abp.Visible = false;
            EditBookPanel ebp = new EditBookPanel();
            ebp.Visible = false;
            AddBorrowPanel abop = new AddBorrowPanel();
            abop.Visible = false;
            EditBorrowPanel ebop = new EditBorrowPanel();
            ebop.Visible = false;
            NavigationPanel np = new NavigationPanel();
            np.Visible = false;
            UserManagementPanel ump = new UserManagementPanel(aup,eup);
            ump.Visible = false;
            BookManagementPanel bmp = new BookManagementPanel(abp, ebp);
            bmp.Visible = false;
            BorrowManagementPanel bmop = new BorrowManagementPanel(abop, ebop);
            bmop.Visible = false;
            this.Controls.Add(np);
            this.Controls.Add(ump);
            this.Controls.Add(aup);
            this.Controls.Add(eup);
            this.Controls.Add(bmp);
            this.Controls.Add(abp);
            this.Controls.Add(ebp);
            this.Controls.Add(bmop);
            this.Controls.Add(abop);
            this.Controls.Add(ebop);


            this.MaximizeBox = false;
            Globals.mainFormSizeNavigationPanel = new Size(np.Width, np.Height);
            Globals.mainFormSizeUserManagementPanel = new Size(ump.Width + 30, ump.Height + 35);
            Globals.mainFormSizeNavigationPanel = new Size(this.Width, this.Height);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void loginPanel1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            
        }
    }
}
