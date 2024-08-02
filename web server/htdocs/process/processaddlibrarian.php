<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
if(isset($_POST['username']) && isset($_POST['uname']) && isset($_POST['mail']) && isset($_POST['phone']) && isset($_POST['address']) && isset($_POST['userid']) && isset($_SESSION['login']) && isset($_SESSION['admin']) && $_SESSION['admin'] == 1 && isset($_SESSION['ip']))
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
        $uname = $_POST['username'];
        $name = $_POST['uname'];
        $mail = $_POST['mail'];
        $phone = $_POST['phone'];
        $address = $_POST['address'];
        $taz = $_POST['userid'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (clean($uname) && cleanName($name) && cleanMail($mail) && cleanPhoneAndTaz($phone) && cleanAddress($address) && cleanPhoneAndTaz($taz) && strlen($uname) >= 3 && strlen($uname) <= 20 && strlen($name) >= 3 && strlen($name) <= 50 && strlen($mail) <= 70 && strlen($phone) >= 8 && strlen($phone) <= 12 && strlen($address) >= 3 && strlen($address) <= 50 && strlen($taz) >= 8 && strlen($taz) <= 10 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE username = ? LIMIT 1");
            $q->execute(array($uname));
            if($q->rowCount() > 0)
            {
                $_SESSION['error'] ='שם משתמש שהוזן כבר קיים';
                Header('Location: ?page=librarians');
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM users WHERE userid = ? LIMIT 1");
                $q->execute(array($taz));
                if($q->rowCount() > 0)
                {
                    $_SESSION['error'] ="ת''ז שהוזן כבר קיים";
                    Header('Location: ?page=librarians');
                }
                else
                {
                    $salt = generateRandomStr(32);
                    $pass = generateRandomStr(16);
                    $pass1 = hash('sha256', $salt.$pepper.$pass);
                    $g = $mysql->prepare("INSERT INTO users SET username = ?, pass = ?, uname = ?, email = ?, phone = ?, addr = ?, userid = ?, salt = ?, comments = ?, permission = 1");
                    $g->execute(array($uname,$pass1,$name,$mail,$phone,$address,$taz,$salt,$comments));

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
                    $pmail->Subject = "Welcome to Seferly";
                    $pmail->Body = "Hello ".$name."! "."Your password is ".$pass; // Send the email
                    if (!$pmail->send()) {
                        $_SESSION['error'] ="לא ניתן לשלוח מייל לכתובת שהוזנה. ניתן להזין מייל אחר בעריכת משתמש ולאפס סיסמה";
                    }
                    else
                    {
                        $_SESSION['error'] ="ספרן הוסף בהצלחה";
                    }
                    Header('Location: ?page=librarians');
                }
            }
        }
        else
        {
            $_SESSION['error'] ="טעות באחד הפרטים. שם משתמש, דואר אלקטרוני יכולים להכיל רק אותיות באנגלית או ספרות, טלפון ות''ז יכולים להכיל רק ספרות";
            Header('Location: ?page=librarians');
        }
    }
}
else
{
    exit;
}
