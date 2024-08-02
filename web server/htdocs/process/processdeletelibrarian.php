<?php
if(isset($_POST['uid'])&& isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']) && isset($_POST['action']))
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
        include('config/settings.php');
        $uid = $_POST['uid'];
        $action = $_POST['action'];
        if (cleanPhoneAndTaz($uid)&&cleanPhoneAndTaz($action)&&  strlen($uid)<=20 && strlen($action)==1)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND permission = 1 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                exit;
            }
            else
            {
                $row = $q->fetch();
                if($action==0)
                {
                    $q = $mysql->prepare("UPDATE users SET deleted = 0 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo 'SUCCESS';
                }
                else if($action==1)
                {
                    $q = $mysql->prepare("UPDATE users SET deleted = 1 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo 'SUCCESS';
                }
                else
                {
                    exit;
                }
            }
        }
        else
        {
            exit;
        }
    }
}
else
{
    exit;
}

