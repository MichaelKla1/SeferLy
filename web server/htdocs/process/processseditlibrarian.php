<?php
if(isset($_POST['uid']) && isset($_POST['uname']) && isset($_POST['mail']) && isset($_POST['phone']) && isset($_POST['address']) && isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']))
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
        $name = $_POST['uname'];
        $mail = $_POST['mail'];
        $phone = $_POST['phone'];
        $address = $_POST['address'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (cleanName($name) && cleanMail($mail) && cleanPhoneAndTaz($phone) && cleanPhoneAndTaz($uid) && cleanAddress($address) && strlen($name) >= 3 && strlen($name) <= 50 && strlen($mail) <= 70 && strlen($phone) >= 8 && strlen($phone) <= 12 && strlen($address) >= 3 && strlen($address) <= 50 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND permission = 1 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                exit;
            }
            else
            {
                $q = $mysql->prepare("UPDATE users SET uname = ?, email = ?, phone = ?, addr = ?, comments = ? WHERE uid = ?");
                $q->execute(array($name,$mail,$phone,$address,$comments,$uid));
                echo 'SUCCESS';
            }
        }
        else
        {
            echo "טעות באחד הפרטים. שם משתמש, דואר אלקטרוני יכולים להכיל רק אותיות באנגלית או ספרות, טלפון ות''ז יכולים להכיל רק ספרות";
        }
    }
}
else
{
    exit;
}
