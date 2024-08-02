<script>
function check()
{
    document.getElementById("error_container").innerHTML = "";
    if (document.getElementById("username").value.length < 3 || document.getElementById("passw").value.length < 6)
    {
        document.getElementById("error_container").innerHTML = "אורך מינימלי של שם משתמש הוא 3 <br>אורך מינימלי של סיסמה הוא 6";
        return false;
    }
    else
    {
        return true;
    }
}
</script>
<?php
if (!isset($_SESSION['login']))
{
?>
<div style="text-align: center">
            <table align="center">
                <tr>
                    <td>
                        <form class="form1" action="?page=processlogin" method="POST" onsubmit="return check()">
                            <h3>התחברות</h3>

                            <input dir="ltr" id="username" class="form__input" maxlength="20" type="text" placeholder="שם משתמש" name="uname" required>
                            <input dir="ltr" id="passw" class="form__input" maxlength="30" type="password" placeholder="סיסמא" name="pass" required>
                            <div class="g-recaptcha" data-sitekey="6LeaoxkqAAAAANlzZSsWLbU0Bp2b4_BNGVfZUaSt" data-action="LOGIN"></div>
                            <a style="float:right;" href="?page=forgotpassword">שכחתי סיסמה</a><br>
                            <input class="form__button" type="submit" value="התחבר">
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
?>
