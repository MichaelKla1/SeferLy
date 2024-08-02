<?php
if(isset($_POST['ipaddr']) && isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']))
{
    if($clientip!=$_SESSION['ip'])
    {
        exit;
    }
    include('config/settings.php');
    $allowed = false;
    if(compareIP($adminipmask, $clientip))
    {
        $allowed = true;
    }
    if($allowed)
    {
        $ipaddr = $_POST['ipaddr'];
        if(strlen($ipaddr)<=15 && is_ip($ipaddr))
        {
            $q = $mysql->prepare("DELETE FROM site_ip WHERE ipmask = ?");
            $q->execute(array($ipaddr));
            echo 'SUCCESS';
        }
    }
}
else
{
    exit;
}

