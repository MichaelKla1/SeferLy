<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
if(isset($_POST['sess']) && isset($_POST['code']) && !isset($_SESSION['login']) && !isset($_SESSION['ip']))
{
    include('config/connect.php');
    $banned = false;
    $q = $mysql->prepare("SELECT * FROM app_fail_login WHERE ipaddr = ? AND faildatetime > DATE_SUB(NOW(),INTERVAL 20 MINUTE) LIMIT 1");
    $q->execute(array($clientip));
    if($q->rowCount() > 0)
    {
        $row = $q->fetch();
        if($row['failnum'] >= 3)
        {
            $banned = true;
        }
    }
    if(!$banned)
    {
        include('config/settings.php');
        $sess = $_POST['sess'];
        $code = $_POST['code'];
        if (clean($sess) && strlen($sess)==32 && cleanPhoneAndTaz($code) && strlen($code) == 8)
        {
            $g = $mysql->prepare("SELECT * FROM temp_codes t, users u WHERE session = ? AND code = ? AND t.username = u.username AND code_created > DATE_SUB(NOW(),INTERVAL 5 MINUTE) LIMIT 1");
            $g->execute(array($sess,$code));
            if($g->rowCount() > 0)
            {
                $row = $g->fetch();
                $email = $row['email'];
                $uid = $row['uid'];
                $name = $row['uname'];

                $salt = $row['salt'];
                $pass = generateRandomStr(16);
                $pass1 = hash('sha256', $salt.$pepper.$pass);
                $q = $mysql->prepare("UPDATE users SET pass = ? WHERE uid = ?");
                $q->execute(array($pass1,$uid));
                
                $g = $mysql->prepare("INSERT INTO pass_chng_log SET ipaddr = ?, userid = ?, logdate = NOW(), whochanged  = ?");
                $g->execute(array($clientip,$uid,$uid));

                $mail = new PHPMailer(true);

                $mail = new PHPMailer();
                $mail->isSMTP();
                $mail->SMTPAuth = true;
                $mail->Host = 'smtp.rambler.ru';
                $mail->Username = 'cola1233@rambler.ru';
                $mail->Password = 'KMg935jhhHg835i6';
                $mail->SMTPSecure = 'ssl';
                $mail->Port = 465; // Define recipient, subject, and message
                $mail->setFrom('cola1233@rambler.ru');
                $mail->addAddress($email);
                $mail->Subject = "Password reset for SeferLy";
                $mail->Body = "Hello ".$name."! "."Your password is ".$pass; // Send the email
                if (!$mail->send()) {
                    echo 'לא ניתן לשלוח מייל לכתובת שלך. ניתן לפנות לעובד הספרייה';
                }
                else
                {
                    echo 'SUCCESS';
                }
                
                
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM app_fail_login WHERE ipaddr = ? AND faildatetime > DATE_SUB(NOW(),INTERVAL 20 MINUTE) LIMIT 1");
                $q->execute(array($clientip));
                if($q->rowCount() > 0)
                {
                    $row = $q->fetch();
                    if($row['failnum'] < 3)
                    {
                        $g = $mysql->prepare("UPDATE app_fail_login SET failnum = failnum + 1 WHERE ipaddr = ?");
                        $g->execute(array($clientip));
                    }
                }
                else 
                {
                    $g = $mysql->prepare("DELETE FROM app_fail_login WHERE ipaddr = ?");
                    $g->execute(array($clientip));
                    $g = $mysql->prepare("INSERT INTO app_fail_login SET ipaddr = ?, faildatetime = NOW()");
                    $g->execute(array($clientip));
                }
                echo 'קוד שהוזן לא נכון. הקוד תקף 5 דקות';
            }
            
            
        }
        else
        {
            $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = 0, logdate = NOW(), status = 0, app_or_web = 1, clean = 0");
            $g->execute(array($clientip));
            $q = $mysql->prepare("SELECT * FROM app_fail_login WHERE ipaddr = ? AND faildatetime > DATE_SUB(NOW(),INTERVAL 20 MINUTE) LIMIT 1");
            $q->execute(array($clientip));
            if($q->rowCount() > 0)
            {
                $row = $q->fetch();
                if($row['failnum'] < 3)
                {
                    $g = $mysql->prepare("UPDATE app_fail_login SET failnum = failnum + 1 WHERE ipaddr = ?");
                    $g->execute(array($clientip));
                }
            }
            else 
            {
                $g = $mysql->prepare("DELETE FROM app_fail_login WHERE ipaddr = ?");
                $g->execute(array($clientip));
                $g = $mysql->prepare("INSERT INTO app_fail_login SET ipaddr = ?, faildatetime = NOW()");
                $g->execute(array($clientip));
            }
            echo 'קוד שהוזן לא נכון';
        }
    }
    else
    {
        echo 'המחשב חסום, נסה שנית בעוד 20 דקות';
    }
}
else
{
    exit;
}
