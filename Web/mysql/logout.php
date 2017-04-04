<?php

/**
date created: 14/07/25
description: logout script
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
**/

include_once '../../wwwinclude/functions.php';
sec_session_start();

// Unset all session values
$_SESSION = array();

// get session parameters
$params = session_get_cookie_params();

// delete the actual cookie
setcookie(session_name(),
	'', time() - 42000,
	$params["path"],
	$params["domain"],
	$params["secure"],
	$params["httponly"]);
	
// Destroy session
session_destroy();
header('Location: ../index.php');