<?php
if(!isset($_SESSION['login']) || isset($_SESSION['admin']))
{
    echo "<h1>".'על מנת לקבל שירות, נדרש להתחבר'.'</h1>';
}
else
{
    include('config/connect.php');
    $q = $mysql->prepare("SELECT * FROM lib_settings");
    $q->execute(array());
    if($q->rowCount() > 0)
    {
        $row = $q->fetch();
        $message = $row['message'];
        if($message!="")
        {
            echo '<div class="box11">';
            echo $message;
            echo "</div>";
        }
    }
    ?>
    <div class="box11">
        <h1>ספרים חדשים</h1>
        <div class="brands" id="brands">
            <?php
            $q = $mysql->prepare("SELECT *, MIN(b1.bookadded) AS mindate FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b1.deleted = 0 AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b1.isbn <> ALL(SELECT b4.isbn FROM borrows b5,books b4 WHERE b5.bookid = b4.uid AND (b5.status = 0 OR b5.status = 1) AND b5.userid = ?) GROUP BY b.isbn ORDER BY b1.bookadded DESC LIMIT 25");
            $q->execute(array($_SESSION['login']));
            $responsestr = '';
            while($row = $q->fetch())
            {
                $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? AND b1.picture != '0' AND b1.uid<>ALL(SELECT bookid FROM borrows WHERE status = 0)");
                $g->execute(array($row['isbn']));
                $imgarr = array();
                $bidarr = array();
                while($row1 = $g->fetch())
                {
                    array_push($imgarr, $row1['picture']);
                    array_push($bidarr, $row1['uid']);
                }
                if(count($imgarr) <= 0)
                {
                    $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? LIMIT 1");
                    $g->execute(array($row['isbn']));
                    if($g->rowCount() <= 0)
                    {
                        exit;
                    }
                    else
                    {
                        $row2 = $g->fetch();
                        $responsestr .= '<div class="brand"><a href="?page=books&book='.$row2['uid'].'"><img style="cursor:pointer;" src="img/no-image.jpg" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                    }
                }
                else
                {
                    $key = array_rand($imgarr);
                    $img = $imgarr[$key];
                    $responsestr .= '<div class="brand"><a href="?page=books&book='.$bidarr[$key].'"><img style="cursor:pointer;" src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                }
            }
            echo $responsestr;
            ?>
        </div>
    </div>
    <div class="box11">
        <h1>ספרים פופולרים</h1>
        <div class="brands" id="brands">
            <?php
            
            $q = $mysql->prepare("SELECT *, COUNT(*) AS cc FROM borrows b2, books b1, books_isbn b WHERE b.isbn = b1.isbn AND b1.uid = b2.bookid AND b1.deleted = 0 AND (b2.status = 0 OR b2.status = 1) AND b1.isbn <> ALL(SELECT b4.isbn FROM borrows b5,books b4 WHERE b5.bookid = b4.uid AND (b5.status = 0 OR b5.status = 1) AND b5.userid = ?) GROUP BY b1.isbn ORDER BY cc DESC LIMIT 25");
            $q->execute(array($_SESSION['login']));
            $responsestr = '';
            while($row = $q->fetch())
            {
                $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? AND b1.picture != '0' AND b1.uid<>ALL(SELECT bookid FROM borrows WHERE status = 0)");
                $g->execute(array($row['isbn']));
                $imgarr = array();
                $bidarr = array();
                while($row1 = $g->fetch())
                {
                    array_push($imgarr, $row1['picture']);
                    array_push($bidarr, $row1['uid']);
                }
                if(count($imgarr) <= 0)
                {
                    $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? LIMIT 1");
                    $g->execute(array($row['isbn']));
                    if($g->rowCount() <= 0)
                    {
                        exit;
                    }
                    else
                    {
                        $row2 = $g->fetch();
                        $responsestr .= '<div class="brand"><a href="?page=books&book='.$row2['uid'].'"><img style="cursor:pointer;" src="img/no-image.jpg" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                    }
                }
                else
                {
                    $key = array_rand($imgarr);
                    $img = $imgarr[$key];
                    $responsestr .= '<div class="brand"><a href="?page=books&book='.$bidarr[$key].'"><img style="cursor:pointer;" src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                }
            }
            echo $responsestr;
            ?>
        </div>
    </div>
    <div class="box11">
        <h1>ספרים בשבילי</h1>
        <div class="brands" id="brands">
            <?php
            #AI start
            $isbndistances = array();
            $q = $mysql->prepare("SELECT * FROM ai_performed WHERE userid = ? AND performdatetime > DATE_SUB(NOW(),INTERVAL 60 MINUTE) LIMIT 1");
            $q->execute(array($_SESSION['login']));
            if($q->rowCount() <= 0)
            {
                $q = $mysql->prepare("SELECT * FROM genres");
                $q->execute();
                $idealgenres = array();
                while($row = $q->fetch())
                {
                    $idealgenres[$row['gname']] = 0;
                }
                $q = $mysql->prepare("SELECT g.genre AS genren, COUNT(*) cc FROM books_isbn b2, books_genres g WHERE b2.isbn = ANY(SELECT isbn FROM books WHERE deleted = 0 AND isbn = b2.isbn) AND b2.isbn = g.bookid AND b2.isbn=ANY(SELECT b4.isbn FROM borrows b5,books b4 WHERE b5.bookid = b4.uid AND (b5.status = 0 OR b5.status = 1) AND b5.userid = ?) GROUP BY g.genre");
                $q->execute(array($_SESSION['login']));
                while($row = $q->fetch())
                {
                    $i = 1;
                    while($i <= intval($row['cc']))
                    {
                        $idealgenres[$row['genren']] += 1/(sqrt($i));
                        $i++;
                    }
                }
                $q = $mysql->prepare("SELECT * FROM books b1, books_genres g WHERE b1.isbn = g.bookid AND b1.isbn <> ALL(SELECT b4.isbn FROM borrows b5,books b4 WHERE b5.bookid = b4.uid AND (b5.status = 0 OR b5.status = 1) AND b5.userid = ?) GROUP BY b1.isbn, g.genre ORDER BY b1.isbn");
                $q->execute(array($_SESSION['login']));
                $isbngenres = array();
                while($row = $q->fetch())
                {
                    $bookisbn = $row['isbn'];
                    if (array_key_exists($row['isbn'],$isbngenres))
                    {
                        $isbngenres[$row['isbn']] = array_merge($isbngenres[$row['isbn']],array($row['genre']));
                    }
                    else
                    {
                        $isbngenres[$row['isbn']] = array($row['genre']);
                    }
                }
                foreach ($isbngenres as $isbn => $genres) 
                {
                    $distance = 0;
                    foreach ($idealgenres as $genre => $count)
                    {
                        if($count == 0)
                        {
                            $distance += 1;
                        }
                        else if(!in_array($genre, $genres))
                        {
                            $distance += $idealgenres[$genre];
                        }
                    }
                    if(count($isbndistances) < 25)
                    {
                        $isbndistances[$isbn] = $distance;
                        asort($isbndistances);
                    }
                    else
                    {
                        $lastKey = key(array_slice($isbndistances, -1, 1, true));
                        if($isbndistances[$lastKey]>$distance)
                        {
                            array_pop($isbndistances);
                            $isbndistances[$isbn] = $distance;
                            asort($isbndistances);
                        }
                    }
                }
                $q = $mysql->prepare("DELETE FROM ai_performed WHERE userid = ?");
                $q->execute(array($_SESSION['login']));
                $q = $mysql->prepare("INSERT INTO ai_performed SET userid = ?, performdatetime = NOW()");
                $q->execute(array($_SESSION['login']));
                $q = $mysql->prepare("DELETE FROM ai_results WHERE userid = ?");
                $q->execute(array($_SESSION['login']));
                $insertquery = '';
                foreach ($isbndistances as $isbn1 => $dis)
                {
                    $insertquery .= '(:term2,'.$isbn1.','.$dis.'),';
                }
                if($insertquery != '')
                {
                    $insertquery = substr($insertquery, 0, -1);
                }
                $q = $mysql->prepare("INSERT INTO ai_results VALUES ".$insertquery);
                $q->execute(array(":term2" => $_SESSION['login']));
            }
            else
            {
                $q = $mysql->prepare("SELECT * FROM ai_results WHERE userid = ? ORDER BY distance");
                $q->execute(array($_SESSION['login']));
                while($row = $q->fetch())
                {
                    $isbndistances[$row['bookisbn']] = $row['distance'];
                }
            }
            #AI end ($isbndistances contains isbns with minimum distances sorted ascending)
            $isbntoselectquery = '';
            foreach ($isbndistances as $isbn1 => $dis)
            {
                $isbntoselectquery .= 'b.isbn = '.$isbn1.' OR ';
            }
            if($isbntoselectquery != '')
            {
                $isbntoselectquery = substr($isbntoselectquery, 0, -4);
            }
            else
            {
                $isbntoselectquery = '1=0';
            }
            $q = $mysql->prepare("SELECT * FROM books_isbn b, books b1 WHERE b1.isbn = b.isbn AND b1.deleted = 0 AND (".$isbntoselectquery.") GROUP BY b1.isbn");
            $q->execute();
            $responsestr = '';
            while($row = $q->fetch())
            {
                $isbndistances[$row['isbn']] = array($row['author'],$row['bookname']);
            }
            foreach ($isbndistances as $isbn1 => $par)
            {
                $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? AND b1.picture != '0' AND b1.uid<>ALL(SELECT bookid FROM borrows WHERE status = 0)");
                $g->execute(array($isbn1));
                $imgarr = array();
                $bidarr = array();
                while($row1 = $g->fetch())
                {
                    array_push($imgarr, $row1['picture']);
                    array_push($bidarr, $row1['uid']);
                }
                if(count($imgarr) <= 0)
                {
                    $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? LIMIT 1");
                    $g->execute(array($isbn1));
                    if($g->rowCount() <= 0)
                    {
                        exit;
                    }
                    else
                    {
                        $row2 = $g->fetch();
                        $responsestr .= '<div class="brand"><a href="?page=books&book='.$row2['uid'].'"><img style="cursor:pointer;" src="img/no-image.jpg" width="90" height="144"/><br><div style="cursor:pointer;">'.$par[1].' '.'על ידי '.$par[0].' </div></a></div>';
                    }
                }
                else
                {
                    $key = array_rand($imgarr);
                    $img = $imgarr[$key];
                    $responsestr .= '<div class="brand"><a href="?page=books&book='.$bidarr[$key].'"><img style="cursor:pointer;" src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br><div style="cursor:pointer;">'.$par[1].' '.'על ידי '.$par[0].' </div></a></div>';
                }
            }
            echo $responsestr;
            ?>
        </div>
    </div>
    <div class="box11">
        <h1>לקרוא שוב</h1>
        <div class="brands" id="brands">
            <?php
            $q = $mysql->prepare("SELECT *, MAX(b6.borrowdate) AS maxdate FROM borrows b6, books_genres b2, genres b3, books_isbn b, books b1 WHERE b6.bookid = b1.uid AND b2.bookid=b.isbn AND b1.deleted = 0 AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b6.userid = ? AND b6.status = 1 GROUP BY b.isbn ORDER BY maxdate DESC LIMIT 40");
            $q->execute(array($_SESSION['login']));
            $responsestr = '';
            while($row = $q->fetch())
            {
                $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? AND b1.picture != '0' AND b1.uid<>ALL(SELECT bookid FROM borrows WHERE status = 0)");
                $g->execute(array($row['isbn']));
                $imgarr = array();
                $bidarr = array();
                while($row1 = $g->fetch())
                {
                    array_push($imgarr, $row1['picture']);
                    array_push($bidarr, $row1['uid']);
                }
                if(count($imgarr) <= 0)
                {
                    $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? LIMIT 1");
                    $g->execute(array($row['isbn']));
                    if($g->rowCount() <= 0)
                    {
                        exit;
                    }
                    else
                    {
                        $row2 = $g->fetch();
                        $responsestr .= '<div class="brand"><a href="?page=books&book='.$row2['uid'].'"><img style="cursor:pointer;" src="img/no-image.jpg" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                    }
                }
                else
                {
                    $key = array_rand($imgarr);
                    $img = $imgarr[$key];
                    $responsestr .= '<div class="brand"><a href="?page=books&book='.$bidarr[$key].'"><img style="cursor:pointer;" src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                }
            }
            echo $responsestr;
            ?>
        </div>
    </div>
    <div class="box11">
        <h1>ספרים לפי קטגוריות שונות</h1>
        <?php
        $q1 = $mysql->prepare("SELECT * FROM genres");
        $q1->execute(array());
        while($row1 = $q1->fetch())
        {
            echo '<div class="box11"><h2>'.$row1['gname'].'</h2>';
        ?>
        <div class="brands" id="brands">
            <?php
            $q = $mysql->prepare("SELECT *, MIN(b1.bookadded) AS mindate FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b1.deleted = 0 AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b3.gname = ? AND b1.isbn <> ALL(SELECT b4.isbn FROM borrows b5,books b4 WHERE b5.bookid = b4.uid AND (b5.status = 0 OR b5.status = 1) AND b5.userid = ?) GROUP BY b.isbn ORDER BY b1.bookadded DESC LIMIT 25");
            $q->execute(array($row1['gname'],$_SESSION['login']));
            $responsestr = '';
            while($row = $q->fetch())
            {
                $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? AND b1.picture != '0' AND b1.uid<>ALL(SELECT bookid FROM borrows WHERE status = 0)");
                $g->execute(array($row['isbn']));
                $imgarr = array();
                $bidarr = array();
                while($row1 = $g->fetch())
                {
                    array_push($imgarr, $row1['picture']);
                    array_push($bidarr, $row1['uid']);
                }
                if(count($imgarr) <= 0)
                {
                    $g = $mysql->prepare("SELECT * FROM books b1 WHERE  b1.isbn = ? LIMIT 1");
                    $g->execute(array($row['isbn']));
                    if($g->rowCount() <= 0)
                    {
                        exit;
                    }
                    else
                    {
                        $row2 = $g->fetch();
                        $responsestr .= '<div class="brand"><a href="?page=books&book='.$row2['uid'].'"><img style="cursor:pointer;" src="img/no-image.jpg" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                    }
                }
                else
                {
                    $key = array_rand($imgarr);
                    $img = $imgarr[$key];
                    $responsestr .= '<div class="brand"><a href="?page=books&book='.$bidarr[$key].'"><img style="cursor:pointer;" src="data:image/jpeg;base64,'.$img.'" width="90" height="144"/><br><div style="cursor:pointer;">'.$row['bookname'].' '.'על ידי '.$row['author'].' </div></a></div>';
                }
            }
            echo $responsestr;
            ?>
        </div>
        <?php
            echo '</div>';
        }
        ?>
    </div>
    <?php
}

