<?php
if(!isset($_SESSION['login']))
{
    Header('Location: ?page=login');
    exit;
}
if(isset($_SESSION['admin']))
{
    Header('Location: ?page=serversettings');
    exit;
}
include('config/connect.php');
?>
<script>
function check()
{
    document.getElementById("error_container").innerHTML = "";
    if (document.getElementById("newpass1").value.length < 6 || document.getElementById("oldpass").value.length < 6 || document.getElementById("newpass2").value.length < 6)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של סיסמה הוא 6";
        return false;
    }
	else if(document.getElementById("newpass1").value != document.getElementById("newpass2").value)
	{
		document.getElementById("error_container").innerHTML = "סיסמאות חדשות צריכות להיות זהות";
        return false;
	}
    else
    {
        return true;
    }
}
</script>
<?php
$q = $mysql->prepare("SELECT * FROM users WHERE uid = ?");
$q->execute(array($_SESSION['login']));
if($q->rowCount() > 0)
{
    $row = $q->fetch();
    $uname = $row['uname'];
    $email = $row['email'];
    $phone = $row['phone'];
    $addr = $row['addr'];
    $userid = $row['userid'];
}
else
{
	exit;
}
?>
<div class="box1111">
<div class="box11">
	<h1>הגדרות חשבון</h1>
	<u>שינוי סיסמה</u><br />
	<form action="?page=processaccount" method="POST" onsubmit="return check()">
		<input dir="ltr" id="oldpass" class="form__input" maxlength="30" type="password" placeholder="סיסמה ישנה" name="oldpass" required>
		<input dir="ltr" id="newpass1" class="form__input" maxlength="30" type="password" placeholder="סיסמה חדשה" name="newpass1" required>
		<input dir="ltr" id="newpass2" class="form__input" maxlength="30" type="password" placeholder="סיסמה חדשה" name="newpass2" required>
		<input class="form__button" type="submit" value="שמור">
		<div id="error_container">
		<?php 
            if(isset($_SESSION['error']))
            {
                echo '<br />'.$_SESSION['error'].'<br />';
                unset($_SESSION['error']);
            }
        ?>
		</div>
	</form><br />
	<?php
		echo "<u>".'שם מלא:'."</u> ".$uname.'<br />';
		echo "<u>".'דוא אלקטרוני:'."</u> ".$email.'<br />';
		echo "<u>".'מספר טלפון:'."</u> ".$phone.'<br />';
		echo "<u>".'כתובת:'."</u> ".$addr.'<br />';
		echo "<u>"."ת''ז:"."</u> ".$userid.'<br />';
		echo "<b>"."לשינוי פריט כלשהו ניתן לפנות לספרנית"."</b>";
	?>
</div>
</div>