<?php
if(!defined('DBHOST'))
{
    define( 'DBHOST', 'sql7.freemysqlhosting.net' );
}
if(!defined('DBUSER'))
{
    define( 'DBUSER', 'sql7726571' );
}
if(!defined('DBPASS'))
{
    define( 'DBPASS', 'k5hSV73f9a' );
}
if(!defined('DBNAME'))
{
    define( 'DBNAME', 'sql7726571' );
}


$dsn = "mysql:host=" . DBHOST . ";dbname=" . DBNAME . "";



try {
    $mysql = new PDO($dsn, DBUSER, DBPASS, array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8"));
} catch (PDOException $e) {
    die('Cant connect: ' . $e->getMessage());
}


?>
