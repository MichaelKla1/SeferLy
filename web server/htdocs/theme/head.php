<html dir="rtl">
    <head>
        <link rel="stylesheet" href="css/style.css">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.3/css/bootstrap.min.css">
        <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet">
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"></script>
        <script type="text/javascript" src="js/js.js"></script>
        <script src="https://www.google.com/recaptcha/enterprise.js" async defer></script>
        <script src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
    </head>
    

<body class="b1"> 
  <div id="navigation-wrap" class="navigation-wrap bg-light start-header">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <nav class="navbar navbar-expand-md navbar-light">
          
            <div class="navbar-brand"><img src="img/logo_new.png" alt=""></div>  
            
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
              <span class="navbar-toggler-icon"></span>
            </button>
            <?php 
            if(!isset($_SESSION['admin']) || $_SESSION['admin'] != 1)
            {
            ?>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
              <ul class="navbar-nav ml-auto py-4 py-md-0">
                <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                if ($page=='home' || $page=='')
                    echo "active";
                ?>">
                  <a class="nav-link" href="?page=home">בית</a>
                </li>
                 <?php 
                if(isset($_SESSION['login']))
                {
                ?>
                <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4">
                  <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">חשבון שלי</a>
                  <div class="dropdown-menu">
                    <a class="dropdown-item" href="?page=account">הגדרות חשבון</a>
                    <a class="dropdown-item" href="?page=processlogout">התנתקות</a>
                  </div>
                </li>
                <?php
                }
                ?>
                <?php 
                if(isset($_SESSION['login']))
                {
                ?>
                <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                if ($page=='books')
                    echo "active";
                ?>">
                  <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">ספרים</a>
                  <div class="dropdown-menu">
                    <a class="dropdown-item" href="?page=mybooks">ספרים שלי</a>
                    <a class="dropdown-item" href="?page=books">קטלוג ספרים</a>
                  </div>
                </li>
                <?php
                }
                ?>
                <?php 
                if(!isset($_SESSION['login']))
                {
                ?>
                <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                if ($page=='login')
                    echo "active";
                ?>">
                  <a class="nav-link" href="?page=login">התחברות</a>
                </li>
                <?php
                }
                ?>
                <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                if ($page=='contact')
                    echo "active";
                ?>">
                  <a class="nav-link" href="?page=contact">יצירת קשר</a>
                </li>
              </ul>
            </div>
            <?php
            }
            else
            {
            ?>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav ml-auto py-4 py-md-0">
                    <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                    if ($page=='serversettings')
                        echo "active";
                    ?>">
                      <a class="nav-link" href="?page=serversettings">הגדרות אתר</a>
                    </li>
                    <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4 <?php 
                    if ($page=='librarians')
                        echo "active";
                    ?>">
                      <a class="nav-link" href="?page=librarians">ניהול ספרנים</a>
                    </li>
                    <li class="nav-item pl-4 pl-md-0 ml-0 ml-md-4">
                      <a class="nav-link" href="?page=processlogout">התנתקות</a>
                    </li>
                </ul>
            </div>
            <?php
            }
            ?>
            
          </nav>    
        </div>
      </div>
    </div>
  </div>
    
    <div id="content-page" class="absolute-center">

        
  
  



