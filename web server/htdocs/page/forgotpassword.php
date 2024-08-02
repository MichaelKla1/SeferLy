<?php
if (!isset($_SESSION['login']))
{
?>
<script>
var sess = 0;
var oncode = false;
function restorePass()
{
    var capchar = '';
    if($("#g-recaptcha-response").val()!="") 
    {
        capchar = $("#g-recaptcha-response").val();
    }
    else
    {
        document.getElementById("error_container").innerHTML = 'צריך לעבור בדיקה "אני לא רובוט"';
        return;
    }
    document.getElementById("error_container").innerHTML = '';
    if(!oncode)
    {
        var username = document.getElementById("username").value;
        var userid = document.getElementById("userid").value;
        var email = document.getElementById("email").value;
    if(username.length < 3)
    {
        document.getElementById("error_container").innerHTML = 'אורך מינינמלי של שם משתמש הוא 3';
        return;
    }
    else if(userid.length < 8)
    {
        document.getElementById("error_container").innerHTML = 'אורך מינימלי של תעודת זהות הוא 8';
        return;
    }
    else if(email.length < 3)
    {
        document.getElementById("error_container").innerHTML = 'אורך מינימלי של דואר אלקטרוני הוא 3';
        return;
    }
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processrestorepass", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('username='+username+'&userid='+userid+'&email='+email+'&g-recaptcha-response='+capchar);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 document.getElementById("error_container").innerHTML = data;
                 grecaptcha.reset(0);
         }
         else if(data.indexOf("SUCCESS") >= 0) {
                 sess = data.substr(data.indexOf("SUCCESS")+8,data.length-data.indexOf("SUCCESS")-8-1);
                 document.getElementById("fields").innerHTML = '<input dir="ltr" id="code" class="form__input" maxlength="8" type="text" placeholder="הזן קוד שקיבלתה במאיל" name="code" required>';
                 oncode = true;
             }
         else
         {
             document.getElementById("error_container").innerHTML = data;
             grecaptcha.reset(0);

         }
     }
 }
 else
 {
    var code = document.getElementById("code").value;
    if(code.length < 3)
    {
        document.getElementById("error_container").innerHTML = 'אורך מינימלי שך קוד הוא 3';
        return;
    }
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processrestorepasscode", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('sess='+sess+'&code='+code);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 document.getElementById("error_container").innerHTML = data;
             }
         else if(data.indexOf("SUCCESS") >= 0) {
                 document.getElementById("formrestore").innerHTML = '<h1>הסיסמה החדשה נשלחה לדואר אלקטרוני שלך</h1>';
             }
         else
         {
             document.getElementById("error_container").innerHTML = data;
         }
     }
 }
}
</script>
<div style="text-align: center">
            <table align="center">
                <tr>
                    <td>
                        <div id="formrestore">
                        <form class="form1" id="f1" action="?page=processlogin" method="POST" onsubmit="return false;">
                            <h3>שחזור סיסמה</h3>
                            <div style="text-align: right;">מלא את הפרטים הבאים ולחץ המשך:</div>
                            <div id ="fields">
                            <input dir="ltr" id="username" class="form__input" maxlength="20" type="text" placeholder="שם משתמש" name="username" required>
                            <input dir="ltr" id="userid" class="form__input" maxlength="12" type="text" placeholder="ת''ז" name="userid" required>
                            <input dir="ltr" id="email" class="form__input" maxlength="50" type="text" placeholder="דואר אלקטרוני" name="email" required>
                            <div class="g-recaptcha" id="r1" data-sitekey="6LeaoxkqAAAAANlzZSsWLbU0Bp2b4_BNGVfZUaSt" data-action="RPASS"></div>
                            
                            </div>
                            <div id="error_container">
                            </div>
                            
                            <button id="buttonR" class="extend-button" type="button" onclick="restorePass()">המשך</button>
                        </form>
                        </div>
                    </td></tr>
            </table>
        </div>

<?php
}
else
{
    Header('Location: ?page=account');
    exit;
}