<?php
if(isset($_POST['uid']) && isset($_POST['rating']) && isset($_SESSION['login']) && isset($_SESSION['ip'])&& !isset($_SESSION['admin']))
{
    $uid = $_POST['uid'];
    $rating = $_POST['rating'];
    if (cleanPhoneAndTaz($uid)&&  strlen($uid)<=20 && strlen($rating) == 1 && ($rating == '1' || $rating == '2' ||$rating == '3' ||$rating == '4' ||$rating == '5'))
    {
        $q = $mysql->prepare("SELECT * FROM borrows WHERE bookid = ? AND userid = ? AND status = 1 LIMIT 1");
        $q->execute(array($uid,$_SESSION['login']));
        if($q->rowCount() <= 0)
        {
            exit;
        }
        else
        {
            $q = $mysql->prepare("SELECT * FROM books WHERE uid = ? LIMIT 1");
            $q->execute(array($uid));
            if($q->rowCount() > 0)
            {
                $g = $mysql->prepare("DELETE FROM books_ratings WHERE bookid = ? AND userid = ?");
                $g->execute(array($uid,$_SESSION['login']));
                $g = $mysql->prepare("INSERT INTO books_ratings SET bookid = ?, userid = ?, rating = ?");
                $g->execute(array($uid,$_SESSION['login'],$rating));
                echo 'SUCCESS';
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
}
