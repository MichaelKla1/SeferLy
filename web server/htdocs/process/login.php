<?php
if(!isset($_POST['g-recaptcha-response']) || empty($_POST['g-recaptcha-response'])) {
        $_SESSION['error'] ='בדיקה "אני לא רובוט" לא עברה';
        Header('Location: ?page=login');
    } else {
        $secret = '6LeaoxkqAAAAAMuCzaf1D13tOLl0EldMnI1XRbm0';

        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, 'https://www.google.com/recaptcha/api/siteverify?secret='.$secret.'&response='.$_POST['g-recaptcha-response']);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        $response = curl_exec($ch);
        curl_close($ch);
        $response = json_decode($response);

        if($response->success) {
            if(isset($_POST['uname']) && isset($_POST['pass']) && !isset($_SESSION['login']) && !isset($_SESSION['ip']))
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
                    $uname = $_POST['uname'];
                    $pass = $_POST['pass'];
                    if (clean($uname) && cleanPass($pass) && strlen($uname) >= 3 && strlen($uname) <= 20 && strlen($pass) >= 6 && strlen($pass) <= 30)
                    {
                        $q = $mysql->prepare("SELECT * FROM users WHERE username = ? AND deleted = 0 LIMIT 1");
                        $q->execute(array($uname));
                        if($q->rowCount() > 0)
                        {
                            $row = $q->fetch();
                            $salt = $row['salt'];
                            $pass1 = hash('sha256', $salt.$pepper.$pass);
                            $q = $mysql->prepare("SELECT * FROM users WHERE username = ? AND pass = ? AND (permission = 0 OR permission = 2) LIMIT 1");
                            $q->execute(array($uname,$pass1));
                            if($q->rowCount() > 0)
                            {
                                $row = $q->fetch();
                                if($row['permission']==0)
                                {
                                    $_SESSION['login'] = $row['uid'];
                                    $_SESSION['ip'] = $clientip;
                                    $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 1, app_or_web = 1");
                                    $g->execute(array($clientip,$row['username']));
                                    Header('Location: ?page=account');
                                }
                                else if($row['permission']==2)
                                {
                                    $_SESSION['login'] = $row['uid'];
                                    $_SESSION['ip'] = $clientip;
                                    $_SESSION['admin'] = 1;
                                    $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 1, app_or_web = 1");
                                    $g->execute(array($clientip,$row['username']));
                                    Header('Location: ?page=serversettings');
                                }
                            }
                            else 
                            {
                                $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 0, app_or_web = 1");
                                $g->execute(array($clientip,$row['username']));
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
                                $_SESSION['error'] ='שם משתמש או סיסמא לא נכונים';
                                Header('Location: ?page=login');
                            }
                        }
                        else
                        {
                            $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 0, app_or_web = 1");
                            $g->execute(array($clientip,$uname));
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
                            $_SESSION['error'] ='שם משתמש או סיסמא לא נכונים';
                            Header('Location: ?page=login');
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
                        $_SESSION['error'] ='שם משתמש או סיסמא לא נכונים';
                        Header('Location: ?page=login');
                    }
                }
                else
                {
                    $_SESSION['error'] ='המחשב חסום, נסה שנית בעוד 20 דקות';
                    Header('Location: ?page=login');
                }
                
            }
            else
            {
                exit;
            }
        } 
        else 
        {
            $_SESSION['error'] ='בדיקה "אני לא רובוט" לא עברה';
            Header('Location: ?page=login');
        }
    }
    exit;

