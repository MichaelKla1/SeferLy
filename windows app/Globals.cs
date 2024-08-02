using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using WindowsFormsApplication2.FormComponents;

namespace WindowsFormsApplication2
{
    public class Globals
    {
        public static Form1 mainForm;
        public static CookieContainer cookiesContainer;
        public static string serverAddr;
        public static Size mainFormSizeLoginPanel = new Size(344, 396);
        public static Size mainFormSizeNavigationPanel = new Size(321, 323);
        public static Size mainFormSizeUserManagementPanel = new Size(1769, 639);
        public static Size mainFormSizeBookManagementPanel = new Size(1769, 639);
        public static Size mainFormSizeBorrowManagementPanel = new Size(1800, 639);
        public static int current_year = 0;
        public static int current_month = 0;
        public static int current_day = 0;
        public static int defaultBorrowDays = 30;
    }
}
