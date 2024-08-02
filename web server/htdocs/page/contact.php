<?php
include('config/connect.php');
$q = $mysql->prepare("SELECT * FROM lib_settings");
$q->execute(array());
if($q->rowCount() > 0)
{
    $row = $q->fetch();
    $phone = $row['phone'];
    $address = $row['address'];
    $email = $row['email'];
    echo '<div style="text-align:center;">';
    echo "<h1>יצירת קשר</h1><h4>ניתן ליצור קשר עם נציגי הספרייה בדרכים הבאים:<br>";
    echo "מספר טלפון: ".$phone."<br>";
    echo "כתובת הספרייה: ".$address."<br>";
    echo "דואר אלקטרוני: ".$email."</h4></div>";
}


