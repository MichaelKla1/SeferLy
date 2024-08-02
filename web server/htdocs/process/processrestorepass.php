<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
if(!isset($_POST['g-recaptcha-response']) || empty($_POST['g-recaptcha-response'])) {
        echo 'בדיקה "אני לא רובוט" לא עברה';
    } else {
        $secret = '6LeaoxkqAAAAAMuCzaf1D13tOLl0EldMnI1XRbm0';

        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, 'https://www.google.com/recaptcha/api/siteverify?secret='.$secret.'&response='.$_POST['g-recaptcha-response']);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        $response = curl_exec($ch);
        curl_close($ch);
        $response = json_decode($response);

        if($response->success) {
            if(isset($_POST['username']) && isset($_POST['email']) && isset($_POST['userid']) && !isset($_SESSION['login']) && !isset($_SESSION['ip']))
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
                    $username = $_POST['username'];
                    $email = $_POST['email'];
                    $userid = $_POST['userid'];
                    if (clean($username) && strlen($username) >= 3 && strlen($username) <= 20 && cleanMail($email) && cleanPhoneAndTaz($userid) && strlen($email) <= 70 && strlen($userid) <= 10 && strlen($userid) >= 8)
                    {
                        $q = $mysql->prepare("SELECT * FROM users WHERE username = ? AND email = ? AND userid = ? AND permission = 0 AND deleted = 0 LIMIT 1");
                        $q->execute(array($username,$email,$userid));
                        if($q->rowCount() > 0)
                        {
                            $g = $mysql->prepare("SELECT * FROM temp_codes t, users u WHERE ipaddr = ? AND code_created > DATE_SUB(NOW(),INTERVAL 5 MINUTE) LIMIT 1");
                            $g->execute(array($clientip));
                            if($g->rowCount() <= 0)
                            {
                                $code = generateRandomNum(8);
                                $sess = generateRandomStr(32);
                                $g = $mysql->prepare("INSERT INTO temp_codes SET ipaddr = ?, code = ?, code_created = NOW(), username = ?, session = ?");
                                $g->execute(array($clientip,$code,$username,$sess));
                                
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
                                $mail->Subject = "Your temp code for reset password on SeferLy";
                                $mail->Body = "Hello! Your temp code for reset password is ".$code; // Send the email
                                if (!$mail->send()) {
                                    echo 'לא ניתן לשלוח מייל לכתובת שלך. ניתן לפנות לעובד הספרייה';
                                }
                                else
                                {
                                    echo 'SUCCESS['.$sess.']';
                                }
                            }
                            else
                            {
                                echo 'ניתן לבקש שחזור סיסמה רק פעם אחד ב-5 דקות';
                            }
                        }
                        else
                        {
                            $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 0, app_or_web = 1");
                            $g->execute(array($clientip,$username));
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
                            echo 'פרטים שהוזנו אינם נכונים';
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
                        echo 'פרטים שהוזנו אינם נכונים';
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

        } 
        else 
        {
            echo 'בדיקה "אני לא רובוט" לא עברה';
        }
    }
    exit;




