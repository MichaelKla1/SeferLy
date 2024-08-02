<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
if(isset($_POST['uid'])&& isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']))
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
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20)
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
                $salt = $row['salt'];
                $pass = generateRandomStr(16);
                $pass1 = hash('sha256', $salt.$pepper.$pass);
                $q = $mysql->prepare("UPDATE users SET pass = ? WHERE uid = ?");
                $q->execute(array($pass1,$uid));
                $q = $mysql->prepare("SELECT email FROM users WHERE uid = ?");
                $q->execute(array($uid));
                if($q->rowCount() <= 0)
                {
                    exit;
                }
                else
                {
                    $g = $mysql->prepare("INSERT INTO pass_chng_log SET ipaddr = ?, userid = ?, logdate = NOW(), whochanged  = ?");
                    $g->execute(array($clientip,$uid,$_SESSION['login']));
                    $row = $q->fetch();
                    $mail = $row['email'];
                    
                    $pmail = new PHPMailer(true);

                    $pmail = new PHPMailer();
                    $pmail->isSMTP();
                    $pmail->SMTPAuth = true;
                    $pmail->Host = 'smtp.rambler.ru';
                    $pmail->Username = 'cola1233@rambler.ru';
                    $pmail->Password = 'KMg935jhhHg835i6';
                    $pmail->SMTPSecure = 'ssl';
                    $pmail->Port = 465; // Define recipient, subject, and message
                    $pmail->setFrom('cola1233@rambler.ru');
                    $pmail->addAddress($mail);
                    $pmail->Subject = "Password reset for Seferly";
                    $pmail->Body = "Hello ".$name."! "."Your password is ".$pass; // Send the email
                    if (!$pmail->send()) {
                        echo "לא ניתן לשלוח מייל לכתובת שהוזנה. ניתן להזין מייל אחר בעריכת משתמש ולאפס סיסמה";
                    }
                    else
                    {
                        echo 'SUCCESS';
                    }
                   
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
