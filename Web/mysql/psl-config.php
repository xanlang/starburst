<?php 

/**
date created: 14/07/25
description: global mysql configuration page
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
**/

define("HOST", "localhost");
define("USER", "xanxan");
define("PASSWORD", "4K@K7wtaA!oy");
define("DATABASE", "xanxandb");

define("CAN_REGISTER", "any");
define("DEFAULT_ROLE", "member");

define("SECURE", TRUE); // development only
define("REGISTRATION_OPEN", TRUE); // Change to false to lock registration. Maybe implement this in an admin CPANEL.
?>