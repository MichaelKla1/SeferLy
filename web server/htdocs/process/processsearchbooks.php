<?php
if(isset($_POST['sorttype']) && isset($_POST['ppage']) && isset($_POST['sortcriteria']) && isset($_SESSION['login'])&&isset($_SESSION['ip'])&&!isset($_SESSION['admin']))
{
    $sorttype = $_POST['sorttype'];
    $sortcriteria = $_POST['sortcriteria'];
    $ppage = $_POST['ppage'];
    if (cleanPhoneAndTaz($sorttype) && cleanPhoneAndTaz($ppage)&& cleanPhoneAndTaz($sortcriteria) && ($sorttype=='0' || $sorttype=='1') && ($sortcriteria=='0' || $sortcriteria=='1' || $sortcriteria=='2' || $sortcriteria=='3'))
    {
        $bname = '';
        $bauthor = '';
        if(isset($_POST['bname']))
        {
            $bname = $_POST['bname'];
        }
        if(isset($_POST['bauthor']))
        {
            $bauthor = $_POST['bauthor'];
        }
        $genresArr = array();
        $genresStr = 'b3.uid IN (';
        if(isset($_POST['genres']))
        {
            $genres1 = explode(",",$_POST['genres']);
            for($i=0;$i<count($genres1);$i++)
            {
                if(cleanPhoneAndTaz($genres1[$i])&& strlen($genres1[$i])<=20)
                {
                    array_push($genresArr, $genres1[$i]);
                    $genresStr .= $genres1[$i].',';
                }
                else
                {
                    exit;
                }
            }
            $genresStr = rtrim($genresStr, ",");
            $genresStr .= ')';
        }
        else
        {
            $genresStr = '1=1';
        }
        $responsestr = '';
        if($sorttype == '1')
        {
            $sorttype = 'DESC';
        }
        else
        {
            $sorttype = '';
        }
        if($sortcriteria == '1')
        {
            $sortcriteria = 'author';
        }
        else if($sortcriteria == '2')
        {
            $sortcriteria = 'bookyear';
        }
        else if($sortcriteria == '3')
        {
            $sortcriteria = 'b1.bookadded';
        }
        else
        {
            $sortcriteria = 'bookname';
        }
        $startitem = intval($ppage)*50;
        $addedelements = 0;
        $q = $mysql->prepare("SELECT *, MIN(b1.bookadded) AS mindate FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b1.deleted = 0 AND b.author LIKE :term2 AND b.bookname LIKE :term1 AND ".$genresStr." GROUP BY b.isbn ORDER BY ".$sortcriteria." ".$sorttype." LIMIT ".$startitem.",50");
        $q->execute(array(":term2" => $bauthor."%",":term1" => $bname."%"));
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
            $addedelements++;
        }
        $q = $mysql->prepare("SELECT COUNT(*) AS cc FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b1.deleted = 0 AND b.author LIKE :term2 AND b.bookname LIKE :term1 AND ".$genresStr." GROUP BY b.isbn ORDER BY ".$sortcriteria." ".$sorttype);
        $q->execute(array(":term2" => $bauthor."%",":term1" => $bname."%"));
        $elapsed = 0;
        while($row = $q->fetch())
        {
            $elapsed = min(max(0,$startitem+2-$row['cc']),50);
            $startitem = max(0,$startitem-$row['cc']);
            break;
        }
        
        $q = $mysql->prepare("SELECT *, MIN(b1.bookadded) AS mindate FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b1.deleted = 0 AND b.author LIKE :term2 AND b.bookname LIKE :term1 AND ".$genresStr." AND b.isbn<>ALL(SELECT b.isbn FROM books_genres b2, genres b3, books_isbn b, books b1 WHERE b2.bookid=b.isbn AND b2.genre=b3.gname AND b.isbn = b1.isbn AND b1.deleted = 0 AND b.author LIKE :term2 AND b.bookname LIKE :term1 AND ".$genresStr." GROUP BY b.isbn ORDER BY ".$sortcriteria." ".$sorttype.") GROUP BY b.isbn ORDER BY ".$sortcriteria." ".$sorttype." LIMIT ".$startitem.",".$elapsed);
        $q->execute(array(":term2" => "%".$bauthor."%",":term1" => "%".$bname."%",":term22" => "%".$bauthor."%",":term11" => "%".$bname."%"));
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
            $addedelements;
        }
        if($responsestr != '')
        {
            echo $responsestr;
            if($addedelements>=50)
            {
                echo '<div class="brand" style="margin:auto;cursor:pointer;color:dodgerblue;" id="extendbooklist" onclick="searchBooks(1);">עוד תוצאות...</div>';
            }
        }
        else
        {
            if(intval($ppage)==0)
            {
                echo '<h1>'.'אין ספרים התואמות לקריטריונים שנבחרו'.'</h1>';
            }
        }
    }
    else
    {
        exit;
    }
}
else
{
    exit;
}
