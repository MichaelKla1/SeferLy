<?php
if(isset($_POST['uid']) && isset($_SESSION['login']) && isset($_SESSION['ip']))
{
    include('config/connect.php');
    $uid = $_POST['uid'];
    if (strlen($uid)<=20 && cleanPhoneAndTaz($uid))
    {
        $q = $mysql->prepare("SELECT * FROM borrows WHERE uid = ? AND status = 0 AND userid = ? AND returndate > CURDATE() LIMIT 1");
        $q->execute(array($uid,$_SESSION['login']));
        if($q->rowCount() <= 0)
        {
            echo 'ERROR';
        }
        else
        {
            $row = $q->fetch();
            if($row['extended']==0)
            {
                $q = $mysql->prepare("UPDATE borrows SET returndate = DATE_ADD(returndate, INTERVAL 1 MONTH), extended = 1 WHERE uid = ?");
                $q->execute(array($uid));
                echo 'SUCCESS';
            }
            else
            {
                echo 'ALREADY_EXTENDED';
            }
        }
    }
    else
    {
        exit;
    }
}