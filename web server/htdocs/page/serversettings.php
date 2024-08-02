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
    function deleteSiteIP(ip)
    {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processdeletesiteip", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('ipaddr='+ip);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 alert(data);
             }

         else if(data.indexOf("SUCCESS") >= 0) {
                 window.location.reload();
             }
         else
         {
             alert(data);
         }
     }
    }
    function addSiteIP()
    {
        var iptoadd = document.getElementById("siteipmask").value;
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processaddsiteip", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('ipaddr='+iptoadd);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 document.getElementById("error_box").innerHTML = "פורמט של כתובת לא נכון. ניתן לכתוב * בבלוק שבו יכול להיות כל מספר";
             }

         else if(data.indexOf("SUCCESS") >= 0) {
                 window.location.reload();
             }
             else if(data.indexOf("ERRO1") >= 0) {
                 document.getElementById("error_box").innerHTML = "IP כבר קיים";
             }
         else
         {
             alert(data);
         }
     }
    }
    function deleteAppIP(ip)
    {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processdeleteappip", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('ipaddr='+ip);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 alert(data);
             }

         else if(data.indexOf("SUCCESS") >= 0) {
                 window.location.reload();
             }
         else
         {
             alert(data);
         }
     }
    }
    function addAppIP()
    {
        var iptoadd = document.getElementById("appipmask").value;
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processaddappip", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('ipaddr='+iptoadd);
    xhr.onload = function() {
         var data = this.responseText;
         if (data.indexOf("ERROR") >= 0) {
                 document.getElementById("error_box1").innerHTML = "פורמט של כתובת לא נכון. ניתן לכתוב * בבלוק שבו יכול להיות כל מספר";
             }

         else if(data.indexOf("SUCCESS") >= 0) {
                 window.location.reload();
             }
             else if(data.indexOf("ERRO1") >= 0) {
                 document.getElementById("error_box1").innerHTML = "IP כבר קיים";
             }
         else
         {
             alert(data);
         }
     }
    }
    function saveSettings()
    {
        var numofbooks = document.getElementById("numofbooks").value;
        var borrowtime = document.getElementById("borrowtime").value;
        var message = document.getElementById("message").value;
        var phone = document.getElementById("phone").value;
        var address = document.getElementById("address").value;
        var email = document.getElementById("email").value;
        
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/?page=processsavelibsettings", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send('numofbooks='+numofbooks+'&borrowtime='+borrowtime+'&message='+message+'&phone='+phone+'&address='+address+'&email='+email);
    xhr.onload = function() {
         var data = this.responseText;

         if(data.indexOf("SUCCESS") >= 0) {
                 document.getElementById("error_box2").innerHTML = "נשמר בהצלחה";
             }
             else{
                 document.getElementById("error_box2").innerHTML = "טעות באחד הפרטים, בדוק שוב את הפרטים שהוזנו";
             }
     }
    }
    </script>
    <div class="box111" id="box111">
    <table style="table-layout: fixed;width: 98%;">
        <colgroup>
            <col style="width:50%;">
            <col style="width:50%;">
        </colgroup>
        <tr>
            <td>
                <div class="box11">
                    <h2>ניהול גישה לאתר</h2>
                    <table border="1" style="table-layout: fixed;width: 98%; text-align: center; direction: ltr;">
                        <colgroup>
                            <col style="width:80%;">
                            <col style="width:20%;">
                        </colgroup>
                        <tr><td><b>IP</b></td><td>מחיקה</td></tr>
                        <?php
                        $q = $mysql->prepare("SELECT * FROM site_ip");
                        $q->execute();
                        while($row = $q->fetch())
                        {
                            ?>
                            <tr>
                                <td>
                                    <?php echo $row['ipmask']; ?>
                                </td>
                                <td>
                                    <button class="extend-button" type="button" onclick="deleteSiteIP('<?php echo $row['ipmask']; ?>')">מחק</button>
                                </td>
                            </tr>
                            <?php
                        }
                        ?>
                        
                    </table>
                    <table>
                    <colgroup>
                        <col style="width:80%;">
                        <col style="width:20%;">
                    </colgroup>
                    <tr>
                    <td>
                        <input id="siteipmask" dir="ltr" class="form__input" maxlength="15" type="text" placeholder="IP" name="siteipmask">
                    </td><td>
                    <button class="extend-button" type="button" onclick="addSiteIP()">הוסף</button>
                    </td>
                    </tr>
                    </table>
                    <div id="error_box"></div>
                </div>
            </td>
            <td>
                <div class="box11">
                    <h2>ניהול גישה מאפליקציית Windows</h2>
                    <table border="1" style="table-layout: fixed;width: 98%; text-align: center; direction: ltr;">
                        <colgroup>
                            <col style="width:80%;">
                            <col style="width:20%;">
                        </colgroup>
                        <tr><td><b>IP</b></td><td>מחיקה</td></tr>
                        <?php
                        $q = $mysql->prepare("SELECT * FROM app_ip");
                        $q->execute();
                        while($row = $q->fetch())
                        {
                            ?>
                            <tr>
                                <td>
                                    <?php echo $row['ipmask']; ?>
                                </td>
                                <td>
                                    <button class="extend-button" type="button" onclick="deleteAppIP('<?php echo $row['ipmask']; ?>')">מחק</button>
                                </td>
                            </tr>
                            <?php
                        }
                        ?>
                        
                    </table>
                    <table>
                    <colgroup>
                        <col style="width:80%;">
                        <col style="width:20%;">
                    </colgroup>
                    <tr>
                    <td>
                        <input id="appipmask" dir="ltr" class="form__input" maxlength="15" type="text" placeholder="IP" name="appipmask">
                    </td><td>
                    <button class="extend-button" type="button" onclick="addAppIP()">הוסף</button>
                    </td>
                    </tr>
                    </table>
                    <div id="error_box1"></div>
                </div>
            </td>
        </tr>
    </table>
    <div class="box11">
        <h2>הגדרות הספרייה</h2>
        <?php 
        $q = $mysql->prepare("SELECT * FROM lib_settings");
        $q->execute();
        while($row = $q->fetch())
        {
            ?>
            <table border="1" style="table-layout: fixed;width: 98%; text-align: center; direction: ltr;">
                <colgroup>
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                    <col style="width:14.285%;">
                </colgroup>
                <tr><td>מספר ספרים שאפשר להשאיל בו זמנית</td><td>זמן השאלה לפי ברירת מחדל בימים</td><td>הודעה על עמוד הבית</td><td>טלפון הספרייה</td><td>כתובת הספרייה</td><td>דואר אלקטרוני של הספרייה</td><td>שמירה</td></tr>
                <tr>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="numofbooks" dir="ltr" class="form__input" maxlength="3" type="text" value="<?php echo $row['borrow_books']; ?>" name="numofbooks"></td>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="borrowtime" dir="ltr" class="form__input" maxlength="3" type="text" value="<?php echo $row['borrow_time']; ?>" name="borrowtime"></td>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="message" dir="ltr" class="form__input" maxlength="5000" type="text" value="<?php echo $row['message']; ?>" name="message"></td>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="phone" dir="ltr" class="form__input" maxlength="12" type="text" value="<?php echo $row['phone']; ?>" name="phone"></td>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="address" dir="ltr" class="form__input" maxlength="50" type="text" value="<?php echo $row['address']; ?>" name="address"></td>
                    <td><input style="font-size: 0.8rem; padding: 5px; text-align: center;" id="email" dir="ltr" class="form__input" maxlength="50" type="text" value="<?php echo $row['email']; ?>" name="email"></td>
                    <td><button class="extend-button" type="button" onclick="saveSettings()">שמור</button></td>
                </tr>

            </table>
            <div id="error_box2"></div>
            <?php
            break;
        }
        ?>
    </div>
    </div>
    <?php   
}
else
{
    echo "הגישה ממחשב זה אינה מאושרת";
}