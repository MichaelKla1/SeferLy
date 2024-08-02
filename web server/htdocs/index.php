<?php

session_start();
include('config/session_activity.php');
$clientip = $_SERVER['REMOTE_ADDR'];
if (filter_var($clientip, FILTER_VALIDATE_IP))
{
    include('func/func.php');
    if (isset($_GET['page']))
    {
        $page = $_GET['page'];
    }
    else
    {
        $page = '';
    }
    if (clean($page))
    {
        $allowed = false;
        if($page != 'processwinapp')
        {
            include('config/connect.php');
            $q = $mysql->prepare("SELECT * FROM site_ip");
            $q->execute();
            $allowed = false;
            while($row = $q->fetch())
            {
                if(compareIP($row['ipmask'], $clientip))
                {
                    include 'func/Exception.php';
                    include 'func/PHPMailer.php';
                    include 'func/SMTP.php';
                    include('cron/cron.php');
                    $allowed = true;
                    break;
                }
            }
        }
        else
        {
            $allowed = true;
        }
        if($allowed)
        {
            switch($page)
            {
                    case '': include('theme/head.php'); include('page/index.php'); echo "</div></body></html>"; break;
                    case 'home': include('theme/head.php'); include('page/index.php'); echo "</div></body></html>"; break;
                    case 'login': include('theme/head.php'); include('page/login.php'); echo "</div></body></html>"; break;
                    case 'account': include('theme/head.php'); include('page/account.php'); echo "</div></body></html>"; break;
                    case 'books': include('theme/head.php'); include('page/books.php'); echo "</div></body></html>"; break;
                    case 'contact': include('theme/head.php'); include('page/contact.php'); echo "</div></body></html>"; break;
                    case 'mybooks': include('theme/head.php'); include('page/mybooks.php'); echo "</div></body></html>"; break;
                    case 'librarians': include('theme/head.php'); include('page/librarians.php'); echo "</div></body></html>"; break;
                    case 'serversettings': include('theme/head.php'); include('page/serversettings.php'); echo "</div></body></html>"; break;
                    case 'forgotpassword': include('theme/head.php'); include('page/forgotpassword.php'); echo "</div></body></html>"; break;
                    case 'processlogin': include('process/login.php'); break;
                    case 'processlogout': include('process/logout.php'); break;
                    case 'processaccount': include('process/account.php'); break;
                    case 'processextendborrow': include('process/extendborrow.php'); break;
                    case 'processleaverating': include('process/processleaverating.php'); break;
                    case 'processsearchbooks': include('process/processsearchbooks.php'); break;
                    case 'processdeletesiteip': include('process/processdeletesiteip.php'); break;
                    case 'processaddsiteip': include('process/processaddsiteip.php'); break;
                    case 'processdeleteappip': include('process/processdeleteappip.php'); break;
                    case 'processaddappip': include('process/processaddappip.php'); break;
                    case 'processsavelibsettings': include('process/processsavelibsettings.php'); break;
                    case 'processaddlibrarian': include('process/processaddlibrarian.php'); break;
                    case 'processseditlibrarian': include('process/processseditlibrarian.php'); break;
                    case 'processlibrarianresetpass': include('process/processlibrarianresetpass.php'); break;
                    case 'processdeletelibrarian': include('process/processdeletelibrarian.php'); break;
                    case 'processrestorepass': include('process/processrestorepass.php'); break;
                    case 'processrestorepasscode': include('process/processrestorepasscode.php'); break;
                    default: include('theme/head.php'); include('page/404.php'); break;
            }
            
        }
        else
        {
            echo "הגישה ממחשב זה אינה מאושרת";
        }
    }
}
?>
