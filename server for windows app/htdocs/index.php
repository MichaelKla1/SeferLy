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
                    case 'processwinapp': include('process/winapp.php'); break;
                    default: break;
            }
            
        }
        else
        {
            echo "הגישה ממחשב זה אינה מאושרת";
        }
    }
}
?>
