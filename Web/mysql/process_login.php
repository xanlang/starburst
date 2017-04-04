<?php

/**
date created: 14/07/25
description: login processing page. 
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
**/

include_once '../../wwwinclude/db_connect.php';
include_once '../../wwwinclude/functions.php';

sec_session_start(); // our custom secure way of starting a php session.

if (isset($_POST['email'], $_POST['p'])) {
	$email = $_POST['email'];

	$password = $_POST['p']; // the hashed password
	
	if (login($email, $password, $mysqli) == true) {
		// login success
		header("Location: ../protected_page.php");
	} else {
		// login failed
		echo "login failed.<BR><BR>";

		header("Location: ../index.php?error=1");
	}
} else {
	// The correct POST variables were not sent to this page
	echo 'Invalid Request';
}