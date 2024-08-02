<?php
if(!defined('DBHOST'))
{
    define( 'DBHOST', 'sql12.freemysqlhosting.net' );
}
if(!defined('DBUSER'))
{
    define( 'DBUSER', 'sql12722619' );
}
if(!defined('DBPASS'))
{
    define( 'DBPASS', 'WQ7LPFjd7d' );
}
if(!defined('DBNAME'))
{
    define( 'DBNAME', 'sql12722619' );
}


$dsn = "mysql:host=" . DBHOST . ";dbname=" . DBNAME . "";



try {
    $mysql = new PDO($dsn, DBUSER, DBPASS, array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8"));
} catch (PDOException $e) {
    die('Cant connect: ' . $e->getMessage());
}


?>
