<?php
    if(!isset($_SESSION['login']) || isset($_SESSION['admin']))
    {
        Header('Location: ?page=login');
        exit;
    }
?>
<script>
function goToBook(isbn)
{
    window.location.href = "/?page=books&book="+isbn;
}
function extendBorrow(e,bid)
{
    if (!e) e = window.event;
   e.stopPropagation();
var xhr = new XMLHttpRequest();
xhr.open("POST", "/?page=processextendborrow", true);
xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
xhr.send('uid='+bid);
xhr.onload = function() {
     var data = this.responseText;
     if (data.indexOf("ERROR") >= 0) {
             alert(data);
         }

     else if(data.indexOf("SUCCESS") >= 0) {
             window.location.reload();
         }
     else if(data.indexOf("ALREADY_EXTENDED")>=0)
     {
         document.getElementById("mess"+bid).innerHTML = "לא ניתן להעריך יותר מפעם אחד";
     }
     else
     {
         alert(data);
     }
 }
}
</script>
<div class="box11">
    <h1>ספרים בהשאלה</h1>
    <?php
        $q = $mysql->prepare("SELECT *, b1.uid AS borrowuid, b2.uid AS bookuid FROM borrows b1, books b2, books_isbn b3 WHERE b1.userid = ? AND b1.bookid = b2.uid AND b2.isbn = b3.isbn AND b1.status = 0 ORDER BY returndate");
        $q->execute(array($_SESSION['login']));
        if($q->rowCount() <= 0)
        {
            echo "<h1>"."אין ספרים בהשאלה"."</h1>";
        }
        while($row = $q->fetch())
        {
            $isbn = $row['isbn'];
            $bookuid = $row['bookuid'];
            echo '<div class="box2" onclick="goToBook('.$bookuid.')">';
            $img = $row['picture'];
            $bname = $row['bookname'];
            $bauthor = $row['author'];
            $rdate = $row['returndate'];
            $libisbn = $row['libisbn'];
            $borrowuid = $row['borrowuid'];
            $extended = $row['extended'];
            
            if($img != 0)
            {
                echo '<img src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/>';
            }
            else
            {
                echo '<img src="img/no-image.jpg" width="90" height="144"/>';
            }
            echo '<div class="padding-box">';
            echo '<font color="RebeccaPurple">'.$bname.'</font> '.'נכתב על ידי '.'<font color="RebeccaPurple">'.$bauthor.'</font>';
            echo "&nbsp&nbsp&nbsp&nbsp&nbsp";
            echo 'מספר ספר: '.'<font color="RebeccaPurple">'.$libisbn.'</font> ';
            echo "</div>";
            $dat = date("Y-m-d");
            $daysleft = intval(max((strtotime($rdate)-strtotime($dat))/86400,0));
            if($daysleft>7)
            {
                echo '<div class="return-date-box-high">';
            }
            else
            {
                echo '<div class="return-date-box-low">';
            }
            echo 'ימים עד להחזרה:'.' ';
            
            echo $daysleft;
            echo "</div>";
            if($daysleft>0 && $extended == 0)
            {
                echo '<button class="extend-button" type="button" onclick="extendBorrow(event,'.$borrowuid.')">בקשת הערכה בחודש</button><div id="mess'.$borrowuid.'" style="display:inline-block;padding:2px;"></div>';
            }
            echo "</div><br />";
        }
    ?>
</div>

<div class="box11">
    <h1>היסטוריית קריאות</h1>
    <?php
        $q = $mysql->prepare("SELECT *, b1.uid AS borrowuid, b2.uid AS bookuid FROM borrows b1, books b2, books_isbn b3 WHERE b1.userid = ? AND b1.bookid = b2.uid AND b2.isbn = b3.isbn AND b1.status = 1 ORDER BY returndate");
        $q->execute(array($_SESSION['login']));
        if($q->rowCount() <= 0)
        {
            echo "<h1>"."אין ספרים שניקראו"."</h1>";
        }
        while($row = $q->fetch())
        {
            $isbn = $row['isbn'];
            $bookuid = $row['bookuid'];
            echo '<div class="box2" onclick="goToBook('.$bookuid.')">';
            $img = $row['picture'];
            $bname = $row['bookname'];
            $bauthor = $row['author'];
            $rdate = $row['returndate'];
            $libisbn = $row['libisbn'];
            $borrowuid = $row['borrowuid'];
            
            if($img != 0)
            {
                echo '<img src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/>';
            }
            else
            {
                echo '<img src="img/no-image.jpg" width="90" height="144"/>';
            }
            echo '<div class="padding-box">';
            echo '<font color="RebeccaPurple">'.$bname.'</font> '.'נכתב על ידי '.'<font color="RebeccaPurple">'.$bauthor.'</font>';
            echo "&nbsp&nbsp&nbsp&nbsp&nbsp";
            echo 'מספר ספר: '.'<font color="RebeccaPurple">'.$libisbn.'</font> ';
            echo "</div>";

            echo 'הוחזר בתאריך:'.' ';
            
            echo '<font color="RebeccaPurple">'.$rdate.'</font>';
            echo "</div><br />";
        }
        echo "</div><br />";
    ?>
</div>