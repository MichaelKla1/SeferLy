<?php
if(!isset($_SESSION['login']) || !isset($_SESSION['admin']) || !isset($_SESSION['ip']) || $_SESSION['admin'] != 1)
{
    Header('Location: ?page=login');
    exit;
}
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
?>
<script>
function check()
{
    var email = document.getElementById("mail").value;
    document.getElementById("error_container").innerHTML = "";
    if (document.getElementById("username").value.length < 3)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של שם משתמש הוא 3";
        return false;
    }
    else if (document.getElementById("uname").value.length < 2)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של שם מלא הוא 2";
        return false;
    }
    else if (email.length < 2)
    {
        document.getElementById("error_container").innerHTML = "דואר אלקטרוני בפורמט לא נכון";
        return false;
    }
    else if (document.getElementById("phone").value.length < 8)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של טלפון הוא 8";
        return false;
    }
    else if (document.getElementById("address").value.length < 3)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של כתובת הוא 3";
        return false;
    }
    else if (document.getElementById("userid").value.length < 8)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של ת''ז הוא 8";
        return false;
    }
    if(email.indexOf('@')===-1  || email.indexOf('.')===-1 || email.indexOf('.') < email.indexOf('@') || email.indexOf('@') !== email.lastIndexOf('@') ||  email.indexOf('.')===email.length-1)
    {
        document.getElementById("error_container").innerHTML = "דואר אלקטרוני בפורמט לא נכון";
        return false;
    }
    return true;
    
}
function saveUser(uid)
{
    var email = document.getElementById("mail"+uid).value;
    var allok = true;
    if (document.getElementById("uname"+uid).value.length < 2)
    {
        alert("אורך מינימלי של שם מלא הוא 2");
        allok = false;
    }
    else if (email.length < 2)
    {
        alert("דואר אלקטרוני בפורמט לא נכון");
        allok = false;
    }
    else if (document.getElementById("phone"+uid).value.length < 8)
    {
        alert("אורך מינימלי של טלפון הוא 8");
        allok = false;
    }
    else if (document.getElementById("address"+uid).value.length < 3)
    {
        alert("אורך מינימלי של כתובת הוא 3");
        allok = false;
    }
    if(email.indexOf('@')===-1  || email.indexOf('.')===-1 || email.indexOf('.') < email.indexOf('@') || email.indexOf('@') !== email.lastIndexOf('@') ||  email.indexOf('.')===email.length-1)
    {
        alert("דואר אלקטרוני בפורמט לא נכון");
        allok = false;
    }
    if(allok)
    {
        var uname = document.getElementById("uname"+uid).value;
        var phone = document.getElementById("phone"+uid).value;
        var address = document.getElementById("address"+uid).value;
        var email = document.getElementById("mail"+uid).value;
        var comments = document.getElementById("comments"+uid).value;
        
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/?page=processseditlibrarian", true);
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        xhr.send('uid='+uid+'&uname='+uname+'&phone='+phone+'&address='+address+'&mail='+email+'&comments='+comments);
        xhr.onload = function() {
             var data = this.responseText;

             if(data.indexOf("SUCCESS") >= 0) {
                     alert("נשמר בהצלחה");
                 }
                 else{
                     alert(data);
                 }
         }
    }
}
function resetPass(uid)
{
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processlibrarianresetpass", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('uid='+uid);
    xhr.onload = function() {
         var data = this.responseText;

         if(data.indexOf("SUCCESS") >= 0) {
                 alert("איפוס סיסמה בוצע בהצלחה. הסיסמה החדשה נשלחה לדואר אלקטרוני של המשתמש");
             }
             else{
                 alert(data);
             }
     }
}
function deleteUser(uid,action)
{
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processdeletelibrarian", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('uid='+uid+'&action='+action);
    xhr.onload = function() {
         var data = this.responseText;

         if(data.indexOf("SUCCESS") >= 0) {
                 alert("נשמר בהצלחה");
                 if(action == '1')
                 {
                    document.getElementById("deletebutton"+uid).innerHTML = 'שחזר';
                    document.getElementById("deletebutton"+uid).setAttribute( "onClick", "javascript: deleteUser("+uid+",0);" );
                 }
                 else
                 {
                     document.getElementById("deletebutton"+uid).innerHTML = 'מחק';
                     document.getElementById("deletebutton"+uid).setAttribute( "onClick", "javascript: deleteUser("+uid+",1);" );
                 }
             }
             else{
                 alert(data);
             }
     }
}
</script>
<div class="box11">
    <h1>הוספת ספרן חדש</h1>
    <form action="?page=processaddlibrarian" method="POST" onsubmit="return check()">
    <table style="table-layout: fixed;width: 98%; text-align: center; direction: ltr;">
    <colgroup>
        <col style="width:33.333%;">
        <col style="width:33.333%;">
        <col style="width:33.333%;">
    </colgroup>
    <tr>
    <td><input dir="ltr" id="username" class="form__input" maxlength="20" type="text" placeholder="שם משתמש" name="username" required></td>
    <td><input dir="ltr" id="uname" class="form__input" maxlength="50" type="text" placeholder="שם מלא" name="uname" required></td>
    <td><input dir="ltr" id="mail" class="form__input" maxlength="70" type="text" placeholder="דואר אלקטרוני" name="mail" required></td>
    </tr>
    <tr>
    <td><input dir="ltr" id="phone" class="form__input" maxlength="12" type="text" placeholder="טלפון" name="phone" required></td>
    <td><input dir="ltr" id="address" class="form__input" maxlength="50" type="text" placeholder="כתובת" name="address" required></td>
    <td><input dir="ltr" id="userid" class="form__input" maxlength="10" type="text" placeholder="ת''ז" name="userid" required></td>
    </tr>
    </table>
        <textarea style="margin-right: 4vw;" dir="ltr" id="comments" class="form__input" maxlength="5000" rows="4" cols="50" placeholder="הערות" name="comments"></textarea>
    <input class="form__button" type="submit" value="הוסף">
    
    <div id="error_container">
    <?php 
        if(isset($_SESSION['error']))
        {
            echo '<br />'.$_SESSION['error'].'<br />';
            unset($_SESSION['error']);
        }
    ?>
    </div>
    </form>
</div>
<div class="box11">
    <h1>עריכת פרטי ספרנים קיימים</h1>
    <table border="1" style="table-layout: fixed;width: 98%; text-align: center; direction: ltr; word-wrap: break-word;">
    <colgroup>
        <col style="width:10%;">
        <col style="width:10%;">
        <col style="width:10%;">
        <col style="width:10%;">
        <col style="width:10%;">
        <col style="width:10%;">
        <col style="width:19%;">
        <col style="width:7%;">
        <col style="width:7%;">
        <col style="width:7%;">
    </colgroup>
    <tr><td>שם משתמש</td><td>שם מלא</td><td>דואר אלקטרוני</td><td>טלפון</td><td>כתובת</td><td>ת''ז</td><td>הערות</td><td>שמור</td><td>איפוס סיסמה</td><td>מחק\שחזר</td></tr>
    <?php
        $q = $mysql->prepare("SELECT * FROM users WHERE permission = 1 ORDER BY deleted, uname");
        $q->execute(array());
        if($q->rowCount() <= 0)
        {
            echo "<h1>"."אין ספרנים קיימים"."</h1>";
        }
        while($row = $q->fetch())
        {
    ?>
        <tr>
            <td><?php echo $row['username']; ?></td>
            <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="uname<?php echo $row['uid']; ?>" dir="ltr" class="form__input" maxlength="50" type="text" value="<?php echo $row['uname']; ?>" name="uname"></td>
            <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="mail<?php echo $row['uid']; ?>" dir="ltr" class="form__input" maxlength="70" type="text" value="<?php echo $row['email']; ?>" name="mail"></td>
            <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="phone<?php echo $row['uid']; ?>" dir="ltr" class="form__input" maxlength="12" type="text" value="<?php echo $row['phone']; ?>" name="phone"></td>
            <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="address<?php echo $row['uid']; ?>" dir="ltr" class="form__input" maxlength="50" type="text" value="<?php echo $row['addr']; ?>" name="address"></td>
            <td><?php echo $row['userid']; ?></td>
            <td><textarea rows="2" cols="10" style="font-size: 0.8rem; padding: 5px; text-align: center;" id="comments<?php echo $row['uid']; ?>" dir="ltr" class="form__input" maxlength="5000" name="comments"><?php echo $row['comments']; ?></textarea></td>
            <td><button class="extend-button" type="button" onclick="saveUser(<?php echo $row['uid']; ?>)">שמור</button></td>
            <td><button class="extend-button" type="button" onclick="resetPass(<?php echo $row['uid']; ?>)">איפוס</button></td>
            <?php
            if ($row['deleted']==0)
            {
            ?>
                <td><button id="deletebutton<?php echo $row['uid']; ?>" class="extend-button" type="button" onclick="deleteUser(<?php echo $row['uid']; ?>,'1')">מחק</button></td>
            <?php
            }
            else
            {
            ?>
                <td><button id="deletebutton<?php echo $row['uid']; ?>" class="extend-button" type="button" onclick="deleteUser(<?php echo $row['uid']; ?>,'0')">שחזר</button></td>
            <?php
            }
            ?>
        </tr>
    <?php
        }
    ?>
</table>
</div>
<?php   
}
else
{
    echo "הגישה ממחשב זה אינה מאושרת";
}