<?php
include 'func/Exception.php';
include 'func/PHPMailer.php';
include 'func/SMTP.php';
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
include('config/connect.php');
$q = $mysql->prepare("SELECT * FROM app_ip");
$q->execute();
$allowed = false;
while($row = $q->fetch())
{
    if(compareIP($row['ipmask'], $clientip))
    {
        $allowed = true;
        break;
    }
}
if($allowed)
{
    if(isset($_POST['uname']) && isset($_POST['pass']) && !isset($_SESSION['login']) && !isset($_SESSION['librarian']) && !isset($_SESSION['ip']) && isset($_POST['winlogin']) && $_POST['winlogin']=='1')
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
                echo '3';
            }
        }
        if(!$banned)
        {
            include('config/settings.php');
            $uname = $_POST['uname'];
            $pass = $_POST['pass'];
            if (clean($uname) && cleanPass($pass) && strlen($uname) >= 3 && strlen($uname) <= 20 && strlen($pass) >= 6 && strlen($pass) <= 30)
            {
                $q = $mysql->prepare("SELECT * FROM users WHERE username = ? LIMIT 1");
                $q->execute(array($uname));
                if($q->rowCount() > 0)
                {
                    $row = $q->fetch();
                    $salt = $row['salt'];
                    $pass1 = hash('sha256', $salt.$pepper.$pass);
                    $q = $mysql->prepare("SELECT * FROM users WHERE username = ? AND pass = ? AND permission = 1 AND deleted = 0 LIMIT 1");
                    $q->execute(array($uname,$pass1));
                    if($q->rowCount() > 0)
                    {
                        $_SESSION['login'] = $row['uid'];
                        $_SESSION['librarian'] = 1;
                        $_SESSION['ip'] = $clientip;
                        $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 1, app_or_web = 0");
                        $g->execute(array($clientip,$row['username']));
                        echo '2';
                        exit;
                    }
                    else 
                    {
                        $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 0, app_or_web = 0");
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
                        echo '1';
                    }
                }
                else
                {
                    $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = ?, logdate = NOW(), status = 0, app_or_web = 0");
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
                    echo '1';
                }
            }
            else
            {
                $g = $mysql->prepare("INSERT INTO login_logs SET ipaddr = ?, userlog = 0, logdate = NOW(), status = 0, app_or_web = 0, clean = 0");
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
                echo '1';
            }
        }
    }
    else if(isset($_POST['logout']) && $_POST['logout']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        // remove all session variables
        session_unset();
        // destroy the session
        session_destroy(); 
        echo '2';
    }
    else if(isset($_POST['check']) && $_POST['check']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']))
    {

        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        echo '2';
    }
    else if(isset($_POST['adduser']) && $_POST['adduser']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uname']) && isset($_POST['name']) && isset($_POST['mail']) && isset($_POST['phone']) && isset($_POST['address']) && isset($_POST['taz']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uname = $_POST['uname'];
        $name = $_POST['name'];
        $mail = $_POST['mail'];
        $phone = $_POST['phone'];
        $address = $_POST['address'];
        $taz = $_POST['taz'];
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
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM users WHERE userid = ? LIMIT 1");
                $q->execute(array($taz));
                if($q->rowCount() > 0)
                {
                    echo '5';
                }
                else
                {
                    $salt = generateRandomStr(32);
                    $pass = generateRandomStr(16);
                    $pass1 = hash('sha256', $salt.$pepper.$pass);
                    $g = $mysql->prepare("INSERT INTO users SET username = ?, pass = ?, uname = ?, email = ?, phone = ?, addr = ?, userid = ?, salt = ?, comments = ?, permission = 0");
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
                        echo '9';
                    }
                    else
                    {
                        echo '2';
                    }
                    
                }
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['getusers']) && $_POST['getusers']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['searchstr']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $searchstr = $_POST['searchstr'];
        $users = array();
        if(strlen($searchstr) >= 2 && strlen($searchstr) <= 20)
        {
            $q = $mysql->prepare("SELECT * FROM users AS u1 WHERE u1.permission = 0 AND (u1.username LIKE :term1 OR u1.uname LIKE :term1 OR u1.email LIKE :term1 OR u1.phone LIKE :term1 OR u1.addr LIKE :term1 OR u1.userid LIKE :term1) ORDER BY deleted LIMIT 10");
            $q->execute(array(":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $curuser = new stdClass();
                $curuser->uid = $row['uid'];
                $curuser->username = $row['username'];
                $curuser->fullname = $row['uname'];
                $curuser->email = $row['email'];
                $curuser->phone = $row['phone'];
                $curuser->address = $row['addr'];
                $curuser->id = $row['userid'];
                $curuser->comments = $row['comments'];
                $curuser->deleted = $row['deleted'];

                $curuserJSON = json_encode($curuser);

                array_push($users,$curuserJSON);
            }
            $q = $mysql->prepare("SELECT * FROM users AS u2 WHERE u2.permission = 0 AND (u2.username LIKE :term2 OR u2.uname LIKE :term2 OR u2.email LIKE :term2 OR u2.phone LIKE :term2 OR u2.addr LIKE :term2 OR u2.userid LIKE :term2) AND u2.uid<>ALL(SELECT uid FROM users AS u1 WHERE u1.permission = 0 AND (u1.username LIKE :term1 OR u1.uname LIKE :term1 OR u1.email LIKE :term1 OR u1.phone LIKE :term1 OR u1.addr LIKE :term1 OR u1.userid LIKE :term1)) ORDER BY deleted LIMIT 10");
            $q->execute(array(":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $curuser = new stdClass();
                $curuser->uid = $row['uid'];
                $curuser->username = $row['username'];
                $curuser->fullname = $row['uname'];
                $curuser->email = $row['email'];
                $curuser->phone = $row['phone'];
                $curuser->address = $row['addr'];
                $curuser->id = $row['userid'];
                $curuser->comments = $row['comments'];
                $curuser->deleted = $row['deleted'];

                $curuserJSON = json_encode($curuser);

                array_push($users,$curuserJSON);
            }
            $response = json_encode($users);
            echo $response;
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['edituser']) && $_POST['edituser']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']) && isset($_POST['name']) && isset($_POST['mail']) && isset($_POST['phone']) && isset($_POST['address']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $name = $_POST['name'];
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
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND permission = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE users SET uname = ?, email = ?, phone = ?, addr = ?, comments = ? WHERE uid = ?");
                $q->execute(array($name,$mail,$phone,$address,$comments,$uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['deleteuser']) && $_POST['deleteuser']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']) && isset($_POST['action']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $action = $_POST['action'];
        if (cleanPhoneAndTaz($uid)&&cleanPhoneAndTaz($action)&&  strlen($uid)<=20 && strlen($action)==1)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND permission = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $row = $q->fetch();
                if($action==0)
                {
                    $q = $mysql->prepare("UPDATE users SET deleted = 0 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo '2';
                }
                else if($action==1)
                {
                    $q = $mysql->prepare("UPDATE users SET deleted = 1 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo '2';
                }
                else
                {
                    echo '4';
                }
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['resetpassuser']) && $_POST['resetpassuser']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND permission = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
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
                    echo '1';
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
                    $pmail->Subject = "Reset password for SeferLy";
                    $pmail->Body = "Hello! Your new password is ".$pass; // Send the email
                    if (!$pmail->send()) {
                        echo '9';
                    }
                    else
                    {
                        echo '2';
                    }
                }
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['getdate']) && $_POST['getdate']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $curdate = array();
        array_push($curdate,  date("Y"));
        array_push($curdate,  date("m"));
        array_push($curdate,  date("d"));
        $q = $mysql->prepare("SELECT borrow_time FROM lib_settings");
        $q->execute(array());
        if($q->rowCount() <= 0)
        {
            echo '1';
        }
        else
        {
            $row = $q->fetch();
            array_push($curdate, strval($row['borrow_time']));
            $response = json_encode($curdate);
            echo $response;
        }
        
    }
    else if(isset($_POST['getisbn']) && $_POST['getisbn']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['isbn']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $isbn = $_POST['isbn'];
        $isbnsample = array();
        if(cleanISBN($isbn))
        {
            $q = $mysql->prepare("SELECT t1.* FROM books t1 WHERE t1.isbn = ? ORDER BY libisbn");
            $q->execute(array($isbn));
            $prev = 0;
            while($row = $q->fetch())
            {
                if((int)(substr($row['libisbn'],strlen($row['libisbn'])-3,3))!=$prev)
                {
                    break;
                }
                $prev++;
            }
            if($prev>0)
            {
                $q = $mysql->prepare("SELECT t2.* FROM books_isbn t2 WHERE t2.isbn = ?");
                $q->execute(array($isbn));
                while($row = $q->fetch())
                {
                    $cursample = new stdClass();
                    $cursample->copynum = $prev;
                    $cursample->author = $row['author'];
                    $cursample->bname = $row['bookname'];
                    $cursample->byear = $row['bookyear'];
                    $cursample->binfo = $row['infobook'];

                    $curuserJSON = json_encode($cursample);

                    array_push($isbnsample,$curuserJSON);
                    $response = json_encode($isbnsample);
                    echo $response;
                    break;
                }
            }
            else
            {
                echo '5';
            }
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['addbook']) && $_POST['addbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['isbn']) && isset($_POST['author']) && isset($_POST['bname']) && isset($_POST['byear']) && isset($_POST['binfo']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $isbn = $_POST['isbn'];
        $bname = $_POST['bname'];
        $author = $_POST['author'];
        $byear = $_POST['byear'];
        $binfo = $_POST['binfo'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (cleanISBN($isbn) && cleanYear($byear) && strlen($author) >= 2 && strlen($author) <= 50 && strlen($bname) >= 2 && strlen($bname) <= 50 && strlen($binfo) <= 5000 && strlen($binfo) >= 3 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT t1.* FROM books t1 WHERE t1.isbn = ? ORDER BY libisbn");
            $q->execute(array($isbn));
            $copies = 0;
            while($row = $q->fetch())
            {
                if((int)(substr($row['libisbn'],strlen($row['libisbn'])-3,3))!=$copies)
                {
                    break;
                }
                $copies++;
            }
            if($copies<1000)
            {
                $q = $mysql->prepare("SELECT COUNT(*) as copies FROM books_isbn t1 WHERE t1.isbn = ?");
                $q->execute(array($isbn));
                $copies1=0;
                while($row = $q->fetch())
                {
                    if($row['copies']>0)
                    {
                        $copies1 = $row['copies'];
                    }
                    break;
                }
                if($copies1==0)
                {
                    $g = $mysql->prepare("INSERT INTO books_isbn SET isbn = ?, author = ?, bookname = ?, bookyear = ?, infobook = ?");
                    $g->execute(array($isbn,$author,$bname,$byear,$binfo));
                }
                else
                {
                    $g = $mysql->prepare("UPDATE books_isbn SET author = ?, bookname = ?, bookyear = ?, infobook = ? WHERE isbn = ?");
                    $g->execute(array($author,$bname,$byear,$binfo,$isbn));
                }
                $copy = '00';
                if($copies>9)
                {
                    $copy = '0';
                }
                else if($copies>99)
                {
                    $copy = '';
                }
                $copies = strval($copies);
                $libisbn=$isbn.'-'.$copy.$copies;
                $g = $mysql->prepare("INSERT INTO books SET isbn = ?, libisbn = ?, comments = ?, bookadded = NOW(), picture = '0'");
                $g->execute(array($isbn,$libisbn,$comments));
                echo '2';

            }
            else
            {
                echo '6';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['getbooks']) && $_POST['getbooks']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['searchstr']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $searchstr = $_POST['searchstr'];
        $users = array();
        $backusers = array();
        if(strlen($searchstr) >= 2 && strlen($searchstr) <= 20)
        {
            $q = $mysql->prepare("SELECT * FROM books b1, books_isbn b2 WHERE b1.isbn = b2.isbn AND (b1.isbn LIKE :term1 OR b1.libisbn LIKE :term1 OR b2.author LIKE :term1 OR b2.bookname LIKE :term1) ORDER BY deleted LIMIT 10");
            $q->execute(array(":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $t=true;
                $curuser = new stdClass();
                $curuser->uid = $row['uid'];
                $curuser->libisbn = $row['libisbn'];
                $curuser->isbn = $row['isbn'];
                $curuser->author = $row['author'];
                $curuser->bname = $row['bookname'];
                $curuser->binfo = $row['infobook'];
                $curuser->byear = $row['bookyear'];
                $curuser->dateadded = $row['bookadded'];
                $curuser->comments = $row['comments'];
                $curuser->deleted = $row['deleted'];
                if(isset($_POST['forborrow']) && $_POST['forborrow']=='1')
                {
                    $q1 = $mysql->prepare("SELECT uid as c FROM borrows WHERE bookid = ? AND status = 0");
                    $q1->execute(array($curuser->uid));
                    if($q1->rowCount()>0)
                    {
                        $row = $q1->fetch();
                        $curuser->borrowed = $row['c'];
                        $t=false;
                    }
                    else
                    {
                        $curuser->borrowed = 0;
                    }
                }

                if($t)
                {
                    $curuserJSON = json_encode($curuser);

                    array_push($users,$curuserJSON);
                }
                else
                {
                    $curuserJSON = json_encode($curuser);

                    array_push($backusers,$curuserJSON);
                }
            }
            $q = $mysql->prepare("SELECT * FROM books b1, books_isbn b2 WHERE b1.isbn = b2.isbn AND (b1.isbn LIKE :term2 OR b1.libisbn LIKE :term2 OR b2.author LIKE :term2 OR b2.bookname LIKE :term2) AND b1.uid<>ALL(SELECT b1.uid FROM books b1, books_isbn b2 WHERE b1.isbn = b2.isbn AND (b1.isbn LIKE :term1 OR b1.libisbn LIKE :term1 OR b2.author LIKE :term1 OR b2.bookname LIKE :term1)) ORDER BY deleted LIMIT 10");
            $q->execute(array(":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $t = true;
                $curuser = new stdClass();
                $curuser->uid = $row['uid'];
                $curuser->libisbn = $row['libisbn'];
                $curuser->isbn = $row['isbn'];
                $curuser->author = $row['author'];
                $curuser->bname = $row['bookname'];
                $curuser->binfo = $row['infobook'];
                $curuser->byear = $row['bookyear'];
                $curuser->dateadded = $row['bookadded'];
                $curuser->comments = $row['comments'];
                $curuser->deleted = $row['deleted'];
                if(isset($_POST['forborrow']) && $_POST['forborrow']=='1')
                {
                    $q1 = $mysql->prepare("SELECT uid as c FROM borrows WHERE bookid = ? AND status = 0");
                    $q1->execute(array($curuser->uid));
                    if($q1->rowCount()>0)
                    {
                        $row = $q1->fetch();
                        $curuser->borrowed = $row['c'];
                        $t = false;
                    }
                    else
                    {
                        $curuser->borrowed = 0;
                    }
                }
                if($t)
                {
                    $curuserJSON = json_encode($curuser);

                    array_push($users,$curuserJSON);
                }
                else
                {
                    $curuserJSON = json_encode($curuser);

                    array_push($backusers,$curuserJSON);
                }
            }
            $users = array_merge($users, $backusers);
            $response = json_encode($users);
            echo $response;
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['editbook']) && $_POST['editbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']) && isset($_POST['author']) && isset($_POST['bname']) && isset($_POST['byear']) && isset($_POST['binfo']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $bname = $_POST['bname'];
        $author = $_POST['author'];
        $byear = $_POST['byear'];
        $binfo = $_POST['binfo'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20 && cleanYear($byear) && strlen($author) >= 2 && strlen($author) <= 50 && strlen($bname) >= 2 && strlen($bname) <= 50 && strlen($binfo) <= 5000 && strlen($binfo) >= 3 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE books SET comments = ? WHERE uid = ?");
                $q->execute(array($comments,$uid));
                $q = $mysql->prepare("UPDATE books_isbn SET bookname = ?, author = ?, infobook = ?, bookyear = ? WHERE isbn = ANY(SELECT isbn FROM books WHERE uid = ?)");
                $q->execute(array($bname,$author,$binfo,$byear,$uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['deletebook']) && $_POST['deletebook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']) && isset($_POST['action']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $action = $_POST['action'];
        if (cleanPhoneAndTaz($uid)&&cleanPhoneAndTaz($action)&&  strlen($uid)<=20 && strlen($action)==1)
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $row = $q->fetch();
                if($action==0)
                {
                    $q = $mysql->prepare("UPDATE books SET deleted = 0 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo '2';
                }
                else if($action==1)
                {
                    $q = $mysql->prepare("UPDATE books SET deleted = 1 WHERE uid = ?");
                    $q->execute(array($uid));
                    echo '2';
                }
                else
                {
                    echo '4';
                }
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['addborrow']) && $_POST['addborrow']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['bid']) && isset($_POST['uid']) && isset($_POST['borrowdate']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $bid = $_POST['bid'];
        $uid = $_POST['uid'];
        $borrowdate = $_POST['borrowdate'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        $bdate = date('Y-m-d',strtotime($borrowdate));
        if ($bdate && cleanPhoneAndTaz($bid) && cleanPhoneAndTaz($uid) && strlen($bid) <= 20 && strlen($uid) <= 20 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM users WHERE uid = ? AND deleted = 0 AND permission = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '6';
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? AND deleted = 0 LIMIT 1");
                $q->execute(array($bid));
                if($q->rowCount() <= 0)
                {
                    echo '4';
                }
                else
                {
                    $q = $mysql->prepare("SELECT * FROM books b1, borrows b2 WHERE b1.uid = b2.bookid AND b2.status = 0 AND b1.uid = ? LIMIT 1");
                    $q->execute(array($bid));
                    if($q->rowCount() > 0)
                    {
                        echo '1';
                    }
                    else
                    {
                        $q = $mysql->prepare("SELECT * FROM borrows WHERE status = 0 AND userid = ? GROUP BY userid HAVING COUNT(*) >= ANY(SELECT borrow_books FROM lib_settings)");
                        $q->execute(array($uid));
                        if($q->rowCount() > 0)
                        {
                            echo '7';
                        }
                        else
                        {
                            $g = $mysql->prepare("INSERT INTO borrows SET userid = ?, bookid = ?, borrowdate = NOW(), returndate = ?, comments = ?");
                            $g->execute(array($uid,$bid,$bdate,$comments));
                            echo '2';
                        }
                    }

                }
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['getborrows']) && $_POST['getborrows']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['searchstr']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $searchstr = $_POST['searchstr'];
        $users = array();
        if(strlen($searchstr) >= 1 && strlen($searchstr) <= 20)
        {
            $q = $mysql->prepare("SELECT *, b1.uid AS borrowuid, b1.comments AS borrowcomments FROM borrows b1, users u, books b2, books_isbn b3 WHERE b1.uid = ? AND b1.bookid = b2.uid AND b1.userid = u.uid AND b2.isbn = b3.isbn");
            $q->execute(array($searchstr));
            while($row = $q->fetch())
            {
                $curuser = new stdClass();
                $curuser->uid = $row['borrowuid'];
                $curuser->borrowdate = $row['borrowdate'];
                $curuser->returndate = $row['returndate'];
                $curuser->comments = $row['borrowcomments'];
                $curuser->username = $row['username'];
                $curuser->uname = $row['uname'];
                $curuser->userid = $row['userid'];
                $curuser->libisbn = $row['libisbn'];
                $curuser->bname = $row['bookname'];
                $curuser->author = $row['author'];
                $curuser->status = $row['status'];

                $curuserJSON = json_encode($curuser);

                array_push($users,$curuserJSON);
            }
            $q = $mysql->prepare("SELECT *, b1.uid AS borrowuid, b1.comments AS borrowcomments FROM borrows b1, users u, books b2, books_isbn b3 WHERE b1.uid<>:term3 AND b1.bookid = b2.uid AND b1.userid = u.uid AND b2.isbn = b3.isbn AND (u.username LIKE :term1 OR u.uname LIKE :term1 OR u.userid LIKE :term1 OR b2.libisbn LIKE :term1 OR b3.bookname LIKE :term1 OR b3.author LIKE :term1) ORDER BY status LIMIT 10");
            $q->execute(array(":term3" => $searchstr,":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $curuser = new stdClass();
                $curuser->uid = $row['borrowuid'];
                $curuser->borrowdate = $row['borrowdate'];
                $curuser->returndate = $row['returndate'];
                $curuser->comments = $row['borrowcomments'];
                $curuser->username = $row['username'];
                $curuser->uname = $row['uname'];
                $curuser->userid = $row['userid'];
                $curuser->libisbn = $row['libisbn'];
                $curuser->bname = $row['bookname'];
                $curuser->author = $row['author'];
                $curuser->status = $row['status'];

                $curuserJSON = json_encode($curuser);

                array_push($users,$curuserJSON);
            }
            $q = $mysql->prepare("SELECT *, b1.uid AS borrowuid, b1.comments AS borrowcomments FROM borrows b1, users u, books b2, books_isbn b3 WHERE b1.uid<>:term3 AND b1.bookid = b2.uid AND b1.userid = u.uid AND b2.isbn = b3.isbn AND (u.username LIKE :term2 OR u.uname LIKE :term2 OR u.userid LIKE :term2 OR b2.libisbn LIKE :term2 OR b3.bookname LIKE :term2 OR b3.author LIKE :term2) AND b1.uid<>ALL(SELECT b1.uid FROM borrows b1, users u, books b2, books_isbn b3 WHERE b1.bookid = b2.uid AND b1.userid = u.uid AND b2.isbn = b3.isbn AND (u.username LIKE :term1 OR u.uname LIKE :term1 OR u.userid LIKE :term1 OR b2.libisbn LIKE :term1 OR b3.bookname LIKE :term1 OR b3.author LIKE :term1)) ORDER BY status LIMIT 10");
            $q->execute(array(":term3" => $searchstr,":term2" => "%".$searchstr."%",":term1" => $searchstr."%"));
            while($row = $q->fetch())
            {
                $curuser = new stdClass();
                $curuser->uid = $row['borrowuid'];
                $curuser->borrowdate = $row['borrowdate'];
                $curuser->returndate = $row['returndate'];
                $curuser->comments = $row['borrowcomments'];
                $curuser->username = $row['username'];
                $curuser->uname = $row['uname'];
                $curuser->userid = $row['userid'];
                $curuser->libisbn = $row['libisbn'];
                $curuser->bname = $row['bookname'];
                $curuser->author = $row['author'];
                $curuser->status = $row['status'];

                $curuserJSON = json_encode($curuser);

                array_push($users,$curuserJSON);
            }

            $response = json_encode($users);
            echo $response;
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['editborrow']) && $_POST['editborrow']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM borrows WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE borrows SET comments = ? WHERE uid = ?");
                $q->execute(array($comments,$uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['cancelborrow']) && $_POST['cancelborrow']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        if (cleanPhoneAndTaz($uid) &&  strlen($uid)<=20)
        {
            $q = $mysql->prepare("SELECT * FROM borrows WHERE uid = ? AND status = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE borrows SET status = 2 WHERE uid = ?");
                $q->execute(array($uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['extendborrow']) && $_POST['extendborrow']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        if (strlen($uid)<=20 && cleanPhoneAndTaz($uid))
        {
            $q = $mysql->prepare("SELECT * FROM borrows WHERE uid = ? AND status = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE borrows SET returndate = DATE_ADD(returndate, INTERVAL 1 MONTH) WHERE uid = ?");
                $q->execute(array($uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['returnborrow']) && $_POST['returnborrow']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        if (cleanPhoneAndTaz($uid) &&  strlen($uid)<=20)
        {
            $q = $mysql->prepare("SELECT * FROM borrows WHERE uid = ? AND status = 0 LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE borrows SET status = 1, returndate = NOW() WHERE uid = ?");
                $q->execute(array($uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['getgenres']) && $_POST['getgenres']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['action'])&& isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $uid = $_POST['uid'];
        $action = $_POST['action'];
        $genres = array();
        if(cleanPhoneAndTaz($uid)&&cleanPhoneAndTaz($action)&&  strlen($uid)<=20 && strlen($action)==1)
        {

            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                if($action == '0')
                {
                    $row = $q->fetch();
                    $isbn = $row['isbn'];
                    $q = $mysql->prepare("SELECT * FROM books_genres b1,genres b2 WHERE b1.bookid = ? AND b1.genre = b2.gname ORDER BY genre");
                    $q->execute(array($isbn));
                    while($row = $q->fetch())
                    {
                        $cursample = new stdClass();
                        $cursample->genre = $row['genre'];
                        $cursample->description = $row['description'];

                        $curuserJSON = json_encode($cursample);

                        array_push($genres,$curuserJSON);
                    }
                    $response = json_encode($genres);
                    echo $response;
                }
                else if($action == '1')
                {
                    $row = $q->fetch();
                    $isbn = $row['isbn'];
                    $q = $mysql->prepare("SELECT * FROM genres WHERE gname<> ALL(SELECT genre FROM books_genres WHERE bookid = ?) ORDER BY gname");
                    $q->execute(array($isbn));
                    while($row = $q->fetch())
                    {
                        $cursample = new stdClass();
                        $cursample->genre = $row['gname'];
                        $cursample->description = $row['description'];

                        $curuserJSON = json_encode($cursample);

                        array_push($genres,$curuserJSON);
                    }
                    $response = json_encode($genres);
                    echo $response;
                }
                else
                {
                    echo '1';
                }
            }
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['addgenre']) && $_POST['addgenre']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['genre']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $genre = $_POST['genre'];
        $comments = '';
        if (isset($_POST['comments']))
        {
            $comments = $_POST['comments'];
        }
        if (strlen($genre) >= 2 && strlen($genre) <= 50 && strlen($comments) <= 5000)
        {
            $q = $mysql->prepare("SELECT * FROM genres WHERE gname = ? LIMIT 1");
            $q->execute(array($genre));
            if($q->rowCount() > 0)
            {
                echo '4';
            }
            else
            {
                $g = $mysql->prepare("INSERT INTO genres SET gname = ?, description = ?");
                $g->execute(array($genre,$comments));

                echo '2';
            }
        }
        else
        {
            echo '6';
        }
    }
    else if(isset($_POST['deletegenre']) && $_POST['deletegenre']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['genre']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $genre = $_POST['genre'];
        if (strlen($genre) >= 2 && strlen($genre) <= 50)
        {
            $q = $mysql->prepare("SELECT * FROM genres WHERE gname = ? LIMIT 1");
            $q->execute(array($genre));
            if($q->rowCount() > 0)
            {
                $g = $mysql->prepare("SELECT * FROM books_genres b1, genres b2 WHERE b1.genre=b2.gname AND b2.gname = ?");
                $g->execute(array($genre));
                if($g->rowCount() <= 0)
                {
                    $g = $mysql->prepare("DELETE FROM genres WHERE gname = ?");
                    $g->execute(array($genre));
                    echo '2';
                }
                else
                {
                    echo '1';
                }
            }
            else
            {
                echo '4';
            }
        }
        else
        {
            echo '6';
        }
    }
    else if(isset($_POST['addgenreforbook']) && $_POST['addgenreforbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['genre']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $genre = $_POST['genre'];
        $uid = $_POST['uid'];
        if (strlen($genre) >= 2 && strlen($genre) <= 50 && strlen($uid) <= 20)
        {
            $q = $mysql->prepare("SELECT * FROM genres WHERE gname = ? LIMIT 1");
            $q->execute(array($genre));
            if($q->rowCount() <= 0)
            {
                echo '4';
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
                $q->execute(array($uid));
                if($q->rowCount() <= 0)
                {
                    echo '4';
                }
                else
                {
                    $row = $q->fetch();
                    $isbn = $row['isbn'];
                    $q = $mysql->prepare("SELECT * FROM books_genres WHERE bookid = ? AND genre = ? LIMIT 1");
                    $q->execute(array($isbn,$genre));
                    if($q->rowCount() > 0)
                    {
                        echo '4';
                    }
                    else
                    {
                        $g = $mysql->prepare("INSERT INTO books_genres SET genre = ?, bookid = ?");
                        $g->execute(array($genre,$isbn));

                        echo '2';
                    }
                }
            }
        }
        else
        {
            echo '6';
        }
    }
    else if(isset($_POST['deletegenreforbook']) && $_POST['deletegenreforbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['genre']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $genre = $_POST['genre'];
        $uid = $_POST['uid'];
        if (strlen($genre) >= 2 && strlen($genre) <= 50 && strlen($uid) <= 20)
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '4';
            }
            else
            {
                $row = $q->fetch();
                $isbn = $row['isbn'];
                $q = $mysql->prepare("SELECT * FROM books_genres WHERE bookid = ? AND genre = ? LIMIT 1");
                $q->execute(array($isbn,$genre));
                if($q->rowCount() <= 0)
                {
                    echo '4';
                }
                else
                {
                    $g = $mysql->prepare("DELETE FROM books_genres WHERE genre = ? AND bookid = ?");
                    $g->execute(array($genre,$isbn));

                    echo '2';
                }
            }


        }
        else
        {
            echo '6';
        }
    }
    else if(isset($_POST['getpicture']) && $_POST['getpicture']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        $uid = $_POST['uid'];
        $image = array();
        if(cleanPhoneAndTaz($uid)&&  strlen($uid)<=20)
        {

            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                    $q = $mysql->prepare("SELECT picture FROM books WHERE uid = ?");
                    $q->execute(array($uid));
                    while($row = $q->fetch())
                    {
                        $curuserJSON = json_encode($row['picture']);

                        array_push($image,$curuserJSON);
                        break;
                    }
                    $response = json_encode($image);
                    echo $response;

            }
        }
        else
        {
            echo '1';
        }
    }
    else if(isset($_POST['addpictureforbook']) && $_POST['addpictureforbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']) && isset($_POST['picture']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        $picture = $_POST['picture'];
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20 && strlen($picture) <= 40000000)
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE books SET picture = ? WHERE uid = ?");
                $q->execute(array($picture,$uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else if(isset($_POST['deletepictureforbook']) && $_POST['deletepictureforbook']=='1' && isset($_SESSION['login'])&&isset($_SESSION['librarian'])&&isset($_SESSION['ip']) && isset($_POST['uid']))
    {
        if($clientip!=$_SESSION['ip'])
        {
            exit;
        }
        include('config/settings.php');
        $uid = $_POST['uid'];
        if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20)
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() <= 0)
            {
                echo '1';
            }
            else
            {
                $q = $mysql->prepare("UPDATE books SET picture = 0 WHERE uid = ?");
                $q->execute(array($uid));
                echo '2';
            }
        }
        else
        {
            echo '4';
        }
    }
    else
    {
        echo '3';
    }
}
else
{
    echo '0';
}
exit;
?>