<?php
if(isset($_POST['numofbooks']) && isset($_POST['borrowtime']) && isset($_POST['message']) && isset($_POST['phone']) && isset($_POST['address']) && isset($_POST['email']) && isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']))
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
        $numofbooks = $_POST['numofbooks'];
        $borrowtime = $_POST['borrowtime'];
        $message = $_POST['message'];
        $phone = $_POST['phone'];
        $address = $_POST['address'];
        $email = $_POST['email'];
        if(strlen($numofbooks)<=3 && cleanPhoneAndTaz($numofbooks) && strlen($borrowtime)<=3 && cleanPhoneAndTaz($borrowtime) && strlen($numofbooks)<=5000 && strlen($phone)<=12 && strlen($phone)>=8 && cleanPhoneAndTaz($phone) && cleanMail($email) && strlen($address) <= 50)
        {
            $q = $mysql->prepare("SELECT * FROM lib_settings");
            $q->execute(array());
            if($q->rowCount() <= 0)
            {
                $q = $mysql->prepare("INSERT INTO lib_settings SET borrow_books = ?,borrow_time = ?,message = ?, phone = ?, address = ?, email = ?");
                $q->execute(array($numofbooks,$borrowtime,$message,$phone,$address,$email));
                echo 'SUCCESS';
            }
            else
            {
                $q = $mysql->prepare("UPDATE lib_settings SET borrow_books = ?,borrow_time = ?,message = ?, phone = ?, address = ?, email = ?");
                $q->execute(array($numofbooks,$borrowtime,$message,$phone,$address,$email));
                echo 'SUCCESS';
            }
        }
        else
        {
            echo 'ERROR';
        }
    }
}
else
{
    exit;
}

