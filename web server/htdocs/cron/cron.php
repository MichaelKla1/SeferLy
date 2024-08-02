<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;
$q = $mysql->prepare("SELECT * FROM crondates");
$q->execute();
if($q->rowCount() <= 0)
{
    $g = $mysql->prepare("DELETE FROM login_logs WHERE logdate < DATE_SUB(NOW(),INTERVAL 1 MONTH)");
    $g->execute();
    $g = $mysql->prepare("DELETE FROM pass_chng_log WHERE logdate < DATE_SUB(NOW(),INTERVAL 1 MONTH)");
    $g->execute();
    $g = $mysql->prepare("DELETE FROM temp_codes WHERE code_created < DATE_SUB(NOW(),INTERVAL 30 MINUTE)");
    $g->execute();
    $q = $mysql->prepare("SELECT * FROM borrows b, users u, books_isbn b1, books b2 WHERE b.status = 0 AND b.mailsent = 0 AND b.userid = u.uid AND b.bookid = b2.uid AND b2.isbn = b1.isbn AND b.returndate < DATE_ADD(NOW(),INTERVAL 7 DAY)");
    $q->execute();
    while($row = $q->fetch())
    {
        $mail = $row['email'];
        $bookr = $row['bookid'];
        $bookn = $row['bookname'];
        $booka = $row['author'];
        
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
        $mail->addAddress($mail);
        $mail->Subject = "Less than one week remain until return of book ".$bookn;
        $mail->Body = "Hello! Less than one week remain until return of book ".$bookn." that was written by ".$booka." book number ".$bookr; // Send the email
        if (!$mail->send()) {

        }
    }
    $q = $mysql->prepare("UPDATE borrows SET mailsent = 1 WHERE status = 0 AND mailsent = 0 AND b.returndate < DATE_ADD(NOW(),INTERVAL 7 DAY)");
    $q->execute();
    $q = $mysql->prepare("INSERT INTO crondates VALUES (NOW(),NOW(),NOW(),NOW())");
    $q->execute();
}
else
{
    $row = $q->fetch();
    $loginlogsdelete = $row['loginlogsdelete'];
    $tempcodesdelete = $row['tempcodesdelete'];
    $passrestoresdelete = $row['passrestoresdelete'];
    $borrowsreturndate = $row['borrowsreturndate'];
    if($tempcodesdelete-1800>0)
    {
        $g = $mysql->prepare("DELETE FROM temp_codes WHERE code_created < DATE_SUB(NOW(),INTERVAL 30 MINUTE)");
        $g->execute();
        $q = $mysql->prepare("UPDATE crondates SET tempcodesdelete = NOW()");
        $q->execute();
    }
    if($passrestoresdelete-86400>0)
    {
        $g = $mysql->prepare("DELETE FROM pass_chng_log WHERE logdate < DATE_SUB(NOW(),INTERVAL 1 MONTH)");
        $g->execute();
        $q = $mysql->prepare("UPDATE crondates SET passrestoresdelete = NOW()");
        $q->execute();
    }
    if($loginlogsdelete-86400>0)
    {
        $g = $mysql->prepare("DELETE FROM login_logs WHERE logdate < DATE_SUB(NOW(),INTERVAL 1 MONTH)");
        $g->execute();
        $q = $mysql->prepare("UPDATE crondates SET loginlogsdelete = NOW()");
        $q->execute();
    }
    if($borrowsreturndate-86400>0)
    {
        $q = $mysql->prepare("SELECT * FROM borrows b, users u, books_isbn b1, books b2 WHERE b.status = 0 AND b.mailsent = 0 AND b.userid = u.uid AND b.bookid = b2.uid AND b2.isbn = b1.isbn AND b.returndate < DATE_ADD(NOW(),INTERVAL 7 DAY)");
        $q->execute();
        while($row = $q->fetch())
        {
            echo '123';
            $mail = $row['email'];
            $bookr = $row['bookid'];
            $bookn = $row['bookname'];
            $booka = $row['author'];
            
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
            $mail->addAddress($mail);
            $mail->Subject = "Less than one week remain until return of book ".$bookn;
            $mail->Body = "Hello! Less than one week remain until return of book ".$bookn." that was written by ".$booka." book number ".$bookr; // Send the email
            if (!$mail->send()) {

            }
        }
        $q = $mysql->prepare("UPDATE borrows SET mailsent = 1 WHERE status = 0 AND mailsent = 0 AND returndate < DATE_ADD(NOW(),INTERVAL 7 DAY)");
        $q->execute();
        $q = $mysql->prepare("UPDATE crondates SET borrowsreturndate = NOW()");
        $q->execute();
    }
}
