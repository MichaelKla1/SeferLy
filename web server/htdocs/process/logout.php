<?php
if(isset($_SESSION['login']))
{
    session_unset();
    session_destroy();
    Header('Location: /?page=home');
}
?>