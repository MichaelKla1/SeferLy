<?php
function clean($var){
    if(strlen($var)>500)
    {
        return false;
    }
	for ($index = 0; $index < strlen($var); $index++) 
        {
            if ($var[$index]<'a' || $var[$index]>'z')
            {
                if ($var[$index]<'A' || $var[$index]>'Z')
                {
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        return false;
                    }
                }
            }
        }
        return true;
}
function cleanPass($var){
    if(strlen($var)>100)
    {
        return false;
    }
        for ($index = 0; $index < strlen($var); $index++) 
        {
            if ($var[$index]<'a' || $var[$index]>'z')
            {
                if ($var[$index]<'A' || $var[$index]>'Z')
                {
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        if($var[$index]!='!' && $var[$index]!='"' && $var[$index]!='#' && $var[$index]!='$' && $var[$index]!='%' && $var[$index]!='&' && $var[$index]!='\'' && $var[$index]!='(' && $var[$index]!=')' && $var[$index]!='*' && $var[$index]!='+' && $var[$index]!=',' && $var[$index]!='-' && $var[$index]!='.' && $var[$index]!='/' && $var[$index]!=':' && $var[$index]!=';' && $var[$index]!='<' && $var[$index]!='=' && $var[$index]!='>' && $var[$index]!='?' && $var[$index]!='@')
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
}
function cleanName($var)
{
    return preg_match('/[a-zA-Z\x{0590}-\x{05FF}\'-., ]/u', $var);
}
function cleanMail($var)
{
    if(strlen($var)>50)
    {
        return false;
    }
    if(strpos($var,'@')===false || strpos($var,'@')<=0 || strpos($var,'.')===false || strrpos($var,'.') < strpos($var,'@') || strpos($var,'@') != strrpos($var,'@') || strrpos($var,'.')==strlen($var)-1)
    {
        return false;
    }
    for ($index = 0; $index < strlen($var); $index++) 
        {
            if ($var[$index]<'a' || $var[$index]>'z')
            {
                if ($var[$index]<'A' || $var[$index]>'Z')
                {
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        if($var[$index]!='+' && $var[$index]!='.' && $var[$index]!='@')
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
}
function cleanPhoneAndTaz($var)
{
    if(strlen($var)>50)
    {
        return false;
    }
    for ($index = 0; $index < strlen($var); $index++) 
        {
            
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        return false;
                    }
        }
        return true;
}
function cleanYear($var)
{
    if(strlen($var)!=4)
    {
        return false;
    }
    for ($index = 0; $index < strlen($var); $index++) 
        {
            
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        return false;
                    }
        }
        return true;
}
function cleanISBN($var)
{
    if(strlen($var)>25||  strlen($var)<5)
    {
        return false;
    }
    for ($index = 0; $index < strlen($var); $index++) 
        {
            
                    if ($var[$index]<'0' || $var[$index]>'9')
                    {
                        if($var[$index]!='-')
                        {
                            return false;
                        }
                        
                    }
        }
        return true;
}
function cleanAddress($var)
{
    return preg_match('/[a-zA-Z0-9\x{0590}-\x{05FF}\'-., ]/u', $var);
}
function compareIP($ipmask,$iptest)
{
    $index1=0;
    for ($index = 0; $index < strlen($iptest); $index++) 
    {
        if ($ipmask[$index1]==$iptest[$index])
        {
            $index1++;
        }
        elseif($ipmask[$index1]=='*')
        {
            while($index < strlen($iptest) && $iptest[$index]!='.')
            {
                $index++;
            }
            $index--;
            $index1++;
        }
        else
        {
            return false;
        }
    }
    return true;
}
function isSessionActive() {
    session_start();
    $lastActivity=$_SESSION['lastActivity'];
    if ($lastActivity!=null && ($lastActivity+(1*60) > time())) {
        $_SESSION['lastActivity']=time();
        return true;    
    } else {
        deleteSession();
        return false;
    }
}

function deleteSession() {
    session_start();
    setcookie("PHPSESSID", "", time() - 3600, '/');
    session_unset();
    session_destroy();
}
function generateRandomStr($number)
{
    $arr = array('a','b','c','d','e','f',
                 'g','h','i','j','k','l',
                 'm','n','o','p','r','s',
                 't','u','v','x','y','z',
                 'A','B','C','D','E','F',
                 'G','H','I','J','K','L',
                 'M','N','O','P','R','S',
                 'T','U','V','X','Y','Z',
                 '1','2','3','4','5','6',
                 '7','8','9','0');
    $pass = "";
    for($i = 0; $i < $number; $i++)
    {
      $index = rand(0, count($arr) - 1);
      $pass .= $arr[$index];
    }
    return $pass;
 }
 function generateRandomNum($number)
{
    $arr = array('1','2','3','4','5','6',
                 '7','8','9','0');
    $pass = "";
    for($i = 0; $i < $number; $i++)
    {
      $index = rand(0, count($arr) - 1);
      $pass .= $arr[$index];
    }
    return $pass;
 }
 function is_ip($ip = null) 
{

    if( !$ip or strlen(trim($ip)) == 0){
        return false;
    }

    $i=0;
    foreach(explode(".", $ip) as $block)
    {
        $i++;
        if(($block == '*') || (ctype_digit($block) && intval($block) >=0 && intval($block) <= 255))
        {
            continue;
        }
        else
        {
            return false;
        }
    }
    if($i!=4)
    {
        return false;
    }
    return true;
}
