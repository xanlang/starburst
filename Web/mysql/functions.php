<?php
/**
date created: 14/07/25
description: db session functions
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
**/

include_once 'psl-config.php';

function sec_session_start(){
//securely starts a session

	$session_name = "sec_session_id"; // set a custom session name
	$secure = SECURE; // stops Java from being able to access the session ID
	$httponly = true; // forces sessions to only use cookies
	if (ini_set('session.use_only_cookies', 1) === FALSE) {
		header("Location: error.php?err=Could not initiate a safe session (ini_set)");
		exit();
	}
	// Gets current cookies params
	$cookieParams = session_get_cookie_params();
	session_set_cookie_params($cookieParams["lifetime"],
		$cookieParams["path"],
		$cookieParams["domain"],
		$secure,
		$httponly);
	session_name($session_name); // Sets the session name to the one set above
	session_start(); // Start the PHP session
	session_regenerate_id(); // regenerates the session, deletes the old one
}


function login($email, $password, $mysqli) {
	// Using prepared statements means that SQL injection is not possible

	if ($stmt = $mysqli->prepare("SELECT id, username, password, salt FROM userinfo WHERE email = ? LIMIT 1")) {

		$stmt->bind_param('s', $email); // bind "$email" to parameter
		$stmt->execute(); // execute the prepared query
		$stmt->store_result();
		
		// Get system variables from result
		$stmt->bind_result($user_id, $username, $db_password, $salt);
		$stmt->fetch();

		$password = hash('sha512', $password . $salt);
		// hash the password with the unique salt
		if ($stmt->num_rows == 1) {
			//if the user exists we check if the account is locked
			//from too many login attempts
			
			if (checkbrute($user_id, $mysqli) == true) {
				// Account is locked
				// Send an email to user saying their account is locked
				echo "account locked. Too many failed login attempts.";
				return false;
			} else {
				// Check if the password in the database matches
				// the password the user submitted.
				if ($db_password == $password) {
					// password is correct
					// get the user-agent string of the user
					$user_browser = $_SERVER['HTTP_USER_AGENT'];
					// XSS protection as we might print this value
					$user_id = preg_replace("/[^0-9]+/", "", $user_id);
					$_SESSION['user_id'] = $user_id;
					// XSS protection as we might print this value
					$username = preg_replace("/[^a-zA-Z0-9_\-]+/", "", $username);
					$_SESSION['username'] = $username;
					$_SESSION['login_string'] = hash('sha512', $password . $user_browser );
					// login successful
					return true;
				} else {
					// password is not correct
					// record attempt in database
					$now = time();
					$mysqli->query("INSERT INTO login_attempts(user_id, time)
						VALUES ('$user_id', '$now')");
					return false;
				}
			}
		} else {
			// no user exists
			return false;
		}
	} 

		// prepared statement failed.

}

function checkbrute($user_id, $mysqli) {
	// TODO:  add captcha after 2 failed attempts to further impede brute force
	// Get timestamp of current time
	$now = time();
	
	// all login attempts are counted from the past two hours.
	$valid_attempts = $now - (2 * 60 * 60);
	
	if ($stmt = $mysqli->prepare("SELECT time
								FROM login_attempts
								WHERE user_id = ?
								AND time > '$valid_attempts'")) {
		$stmt->bind_param('i', $user_id);
		
		// execute the prepared query
		$stmt->execute();
		$stmt->store_result();
		
		// if there have been more than 5 failed logins
		if ($stmt->num_rows > 5) {
			return true;
		} else {
			return false;
		}
	}
}

function login_check($mysqli) {
	// check if all session variables are set
	
	if (isset($_SESSION['user_id'],
				$_SESSION['username'],
				$_SESSION['login_string'])) {
				
		$user_id = $_SESSION['user_id'];
		$login_string = $_SESSION['login_string'];
		$username = $_SESSION['username'];
		
	// get the user-agent string of the user.
	$user_browser = $_SERVER['HTTP_USER_AGENT'];
	
		if ($stmt = $mysqli->prepare("SELECT password
										FROM userinfo
										WHERE id = ? LIMIT 1")) {
			$stmt->bind_param('i', $user_id); // Bind "user_id" to parameter
			$stmt->execute(); // execute the prepared query
			$stmt->store_result();
			
			if ($stmt->num_rows == 1) {
				// if the user exists get variables from result
				
				$stmt->bind_result($password);
				$stmt->fetch();
				$login_check = hash('sha512', $password . $user_browser);
				
				if ($login_check == $login_string) {
					// logged in!
					return true;
				} else {
					// not logged in
					return false;
				}
			} else {
				// not logged in
				return false;
			}
		} else {
			// not logged in
			return false;
			
		}
	} else {
		// not logged in
		return false;
	}
}

function esc_url($url) {
	/** This next function sanitizes the output from the PHP_SELF 
		server variable. It is a modificaton of a function of the 
		same name used by the WordPress Content Management System
	**/

    if ('' == $url) {
        return $url;
    }
 
    $url = preg_replace('|[^a-z0-9-~+_.?#=!&;,/:%@$\|*\'()\\x80-\\xff]|i', '', $url);
 
    $strip = array('%0d', '%0a', '%0D', '%0A');
    $url = (string) $url;
 
    $count = 1;
    while ($count) {
        $url = str_replace($strip, '', $url, $count);
    }
 
    $url = str_replace(';//', '://', $url);
 
    $url = htmlentities($url);
 
    $url = str_replace('&amp;', '&#038;', $url);
    $url = str_replace("'", '&#039;', $url);
 
    if ($url[0] !== '/') {
        // We're only interested in relative links from $_SERVER['PHP_SELF']
        return '';
    } else {
        return $url;
    }
}

?>