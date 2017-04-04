<?php
/**
date created: 14/07/25
description: db connection page
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
**/

include_once 'psl-config.php';
$mysqli = new mysqli(HOST, USER, PASSWORD, DATABASE);


?>