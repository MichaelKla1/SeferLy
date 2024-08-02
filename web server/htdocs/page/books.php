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
?>
<script>
function goToBook(isbn)
{
    window.location.href = "/?page=books&book="+isbn;
}
function leaveRating(bid,r)
{
var xhr = new XMLHttpRequest();
xhr.open("POST", "/?page=processleaverating", true);
xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
xhr.send('uid='+bid+'&rating='+r);
xhr.onload = function() {
     var data = this.responseText;
     if (data.indexOf("ERROR") >= 0) {
             alert(data);
         }

     else if(data.indexOf("SUCCESS") >= 0) {
             
         }
     else
     {
         alert(data);
     }
 }
}
</script>
<?php
if(isset($_GET['book']))
{
    $uid = $_GET['book'];
    if(cleanPhoneAndTaz($uid)&&  strlen($uid)<=20)
    {
        $q = $mysql->prepare("SELECT * FROM books b1, books_isbn b2 WHERE uid = ? AND b1.isbn = b2.isbn LIMIT 1");
        $q->execute(array($uid));
        if($q->rowCount() <= 0)
        {
            echo '<h1>404 - העמוד לא נימצא</h1>';
        }
        else
        {
            $row = $q->fetch();
            $bname = $row['bookname'];
            $author = $row['author'];
            $byear = $row['bookyear'];
            $binfo = $row['infobook'];
            $img = $row['picture'];
            $isbn = $row['isbn'];
            $genrearr = array();
            $q = $mysql->prepare("SELECT * FROM books b1, books_genres b2 WHERE b1.uid = ? AND b1.isbn = b2.bookid");
            $q->execute(array($uid));
            while($row = $q->fetch())
            {
                array_push($genrearr,$row['genre']);
            }
            ?>
            <div class="box11">
                <h1><?php echo $bname;?></h1>
                <table style="table-layout: fixed;width: 100%;">
                    <colgroup>
                        <col style="width:63%;">
                        <col style="width:37%;">
                    </colgroup>
                    <tr>
                        <td>
                            <?php echo nl2br($binfo);?>
                        </td>
                        <td><div style="float: left;">
                            <?php 
                                if($img != 0)
                                {
                                    echo '<img src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br />';
                                }
                                else
                                {
                                    echo '<img src="img/no-image.jpg" width="90" height="144"/><br />';
                                }
                                $q = $mysql->prepare("SELECT * FROM borrows WHERE status = 1 AND bookid = ? AND userid = ?");
                                $q->execute(array($uid, $_SESSION['login']));
                                if($q->rowCount() > 0)
                                {
                                    $userrating = 0;
                                    $q = $mysql->prepare("SELECT * FROM books_ratings WHERE bookid = ? AND userid = ?");
                                    $q->execute(array($uid, $_SESSION['login']));
                                    if($q->rowCount() > 0)
                                    {
                                        $row = $q->fetch();
                                        $userrating = $row['rating'];
                                    }
                            ?>
                            <br />
                            <div class="rate">
                                <?php
                                    $i = 5;
                                    while($i > 0)
                                    {
                                        if($i != $userrating)
                                        {
                                            echo '<input type="radio" id="star'.strval($i).'" name="rate" value="'.strval($i).'" />';
                                            echo '<label for="star'.strval($i).'" title="text" onclick="leaveRating('.strval($uid).','.strval($i).')">★ </label>';
                                        }
                                        else
                                        {
                                            echo '<input type="radio" id="star'.strval($i).'" name="rate" value="'.strval($i).'" checked />';
                                            echo '<label for="star'.strval($i).'" title="text" onclick="leaveRating('.strval($uid).','.strval($i).')">★ </label>';
                                        }
                                        $i -= 1;
                                    }
                                 ?>
                            </div>
                            <br />
                            <br />
                            <?php
                                }
                                $q = $mysql->prepare("SELECT AVG(rating) AS average FROM books_ratings WHERE bookid = ?");
                                $q->execute(array($uid));
                                $rating = -1;
                                if($q->rowCount() > 0)
                                {
                                    $row = $q->fetch();
                                    if($row['average'] != null)
                                    {
                                        $rating = $row['average'];
                                    }
                                }
                                if($rating != -1)
                                {
                                    echo "<b>".'ציון ספר ממוצע: '."</b>".$rating."<br />";
                                }
                                $q = $mysql->prepare("SELECT COUNT(*) AS cc FROM books b1 WHERE b1.isbn = ? AND b1.deleted = 0 AND 0=ALL(SELECT COUNT(*) FROM borrows WHERE bookid=b1.uid AND status = 0);");
                                $q->execute(array($isbn));
                                $copies = 0;
                                if($q->rowCount() > 0)
                                {
                                    $row = $q->fetch();
                                    if($row['cc'] != null)
                                    {
                                        $copies = $row['cc'];
                                    }
                                }
                                echo "<b>".'במלאי: '."</b>".$copies."<br />";
                                echo "<b>".'נכתב על ידי: '."</b>".$author."<br />";
                                echo "<b>".'שנת הוצאה: '."</b>".$byear."<br />";
                                echo "<b>"."סוג הספר:"."</b><br />";
                                foreach ($genrearr as $genre) 
                                {
                                    echo $genre."<br />";
                                }
                            ?>
                        </div></td>
                    </tr>
                </table>
            </div>
            <?php
        }
    }
    else
    {
        echo '<h1>404 - העמוד לא נימצא</h1>';
    }
}
else
{
?>
<script>
    var page = 0;
    var sorttype = "";
    var sortcriteria = "";
    var bname = "";
    var bauthor = "";
    var genresString = "";
    function searchBooks(action)
    {
        if(action === 0)
        {
            page = 0;
            sorttype = "0";
            sortcriteria = "0";
            bname = document.getElementById("bname").value.replace("&", "%26").replace("+", "%2B").replace("?", "%3F");
            bauthor = document.getElementById("bauthor").value.replace("&", "%26").replace("+", "%2B").replace("?", "%3F");
            genresString = "";
            if(document.getElementById('des').checked)
            {
                sorttype = "1";
            }
            if(document.getElementById('bookauthor').checked)
            {
                sortcriteria = "1";
            }
            else if(document.getElementById('publicyear').checked)
            {
                sortcriteria = "2";
            }
            else if(document.getElementById('adddate').checked)
            {
                sortcriteria = "3";
            }
            var genres = document.getElementsByClassName("genresofbooks");
            for (let i = 0; i < genres.length; i++) 
            {
                if(genres[i].checked)
                {
                    genresString += genres[i].value + ",";
                }
            }
            if(genresString.length > 0)
            {
                genresString = genresString.slice(0,-1);
            }
        }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/?page=processsearchbooks", true);
        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        var strToSend = 'sorttype='+sorttype+'&sortcriteria='+sortcriteria+'&ppage='+page;
        if(bname.length > 0)
        {
            strToSend += '&bname='+bname;
        }
        if(bauthor.length > 0)
        {
            strToSend += '&bauthor='+bauthor;
        }
        if(genresString.length > 0)
        {
            strToSend += "&genres=" + genresString;
        }
        xhr.send(strToSend);
        xhr.onload = function() {
            var data = this.responseText;
            if(page===0)
            {
             document.getElementById("brands").innerHTML = "";
             document.getElementById("brands").innerHTML = data;
             document.body.style.left = '0px';
             document.body.style.top = '0px';
         }
         else
         {
             document.getElementById("extendbooklist").outerHTML = "";
             document.getElementById("brands").innerHTML += data;
         }
         page++;
         };
    }
</script>
<div class="box111" id="box111">
<table style="table-layout: fixed;width: 98%;">
    <colgroup>
        <col style="width:70%;">
        <col style="width:30%;">
    </colgroup>
    <tr>
        <td>
            <div class="box11">
                <h2>חיפוש ספרים</h2>
                <table style="table-layout: fixed;width: 98%;">
                    <colgroup>
                        <col style="width:50%;">
                        <col style="width:50%;">
                    </colgroup>
                    <tr>
                        <td>
                            <input id="bname" class="form__input" maxlength="50" type="text" placeholder="שם הספר" name="bname">
                        </td>
                        <td>
                            <input id="bauthor" class="form__input" maxlength="50" type="text" placeholder="שם המחבר" name="bauthor">
                        </td>
                    </tr>
                </table>
                <div id="error-box"></div>
                <div style="margin-right: 10px; text-decoration:underline; font-weight: bold;">קטגוריה:</div>
                <br />
                <?php
                    $q = $mysql->prepare("SELECT * FROM genres ORDER BY gname");
                    $q->execute(array());
                    while($row = $q->fetch())
                    {
                        ?>
                        <div style="margin:10px; display:inline;">
                            <input class="genresofbooks" type="checkbox" id="<?php echo $row['gname'];?>" name="<?php echo $row['gname'];?>" value="<?php echo $row['uid'];?>" checked>
                            <label for="<?php echo $row['gname'];?>"><?php echo $row['gname'];?></label>
                        </div>
                        <?php
                    }
                ?>
                <br />
                <br />
                
            </div>
        </td>
        <td>
            <div class="box11">
                <div style="margin:10px; display:inline;">
                    <h2>מיין לפי</h2>
                    <div style="margin-right: 10px; text-decoration:underline; font-weight: bold;">סדר:</div>
                    <input type="radio" id="asc" name="sorttype" value="asc" checked>
                    <label for="html">מ-א עד ת</label><br>
                    <input type="radio" id="des" name="sorttype" value="des">
                    <label for="css">מ-ת עד א</label><br>
                    <div style="margin-right: 10px; text-decoration:underline; font-weight: bold;">קריטריון:</div>
                    <input type="radio" id="bookname" name="sortcriteria" value="bookname" checked>
                    <label for="html">שם הספר</label><br>
                    <input type="radio" id="bookauthor" name="sortcriteria" value="bookauthor">
                    <label for="css">שם המחבר</label><br>
                    <input type="radio" id="publicyear" name="sortcriteria" value="publicyear">
                    <label for="html">שנת הוצאה לאור</label><br>
                    <input type="radio" id="adddate" name="sortcriteria" value="adddate">
                    <label for="css">תאריך הוספה לספרייה</label><br>
                </div>
            </div>
        </td>
    </tr>
</table>
<div class="box11">
    <input class="form__button" type="button" onclick="searchBooks(0);" value="חפש">
    <div id="error_container">
    </div>
</div>
<div class="box11">
    <div class="brands" id="brands">
       
    </div>
</div>
</div>
<?php
}