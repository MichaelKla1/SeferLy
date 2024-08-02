<?php
if(isset($_POST['oldpass']) && isset($_POST['newpass1']) && isset($_POST['newpass2']) && isset($_SESSION['login']) && isset($_SESSION['ip']))
{
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
            $oldpass = $_POST['oldpass'];
            $newpass1 = $_POST['newpass1'];
            $newpass2 = $_POST['newpass2'];
            $uid = $_SESSION['login'];
            if (cleanPass($oldpass) && cleanPass($newpass1) && cleanPass($newpass2) && strlen($oldpass) >= 6 && strlen($oldpass) <= 30  && strlen($newpass1) >= 6 && strlen($newpass1) <= 30  && strlen($newpass2) >= 6 && strlen($newpass2) <= 30 && $newpass1 == $newpass2)
            {
                    $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? LIMIT 1");
                    $q->execute(array($uid));
                    if($q->rowCount() > 0)
                    {
                            $row = $q->fetch();
                            $salt = $row['salt'];
                            $pass1 = hash('sha256', $salt.$pepper.$oldpass);
                            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND pass = ? AND (permission = 0) LIMIT 1");
                            $q->execute(array($uid,$pass1));
                            if($q->rowCount() > 0)
                            {
                                    $pass = $newpass1;
                                    $pass1 = hash('sha256', $salt.$pepper.$pass);
                                    $q = $mysql->prepare("UPDATE users SET pass = ? WHERE uid = ?");
                                    $q->execute(array($pass1,$uid));
                                    $g = $mysql->prepare("INSERT INTO pass_chng_log SET ipaddr = ?, userid = ?, logdate = NOW(), whochanged  = ?");
                                    $g->execute(array($clientip,$uid,$_SESSION['login']));
                                    $_SESSION['error'] = "הסיסמה עודכנה בהצלחה";
                                    Header('Location: ?page=account');
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
                                    $_SESSION['error'] ='סיסמה ישנה לא נכונה';
                                    Header('Location: ?page=account');
                            }
                    }
                    else
                    {
                            exit;
                    }
            }
            else
            {
                    $_SESSION['error'] = "טעות באחד הפרטים";

            }
	}
	else
	{
		$_SESSION['error'] ='המחשב חסום. נסה שנית בעוד 20 דקות';
		Header('Location: ?page=account');
	}
}