<?php

/**
date created: 14/07/25
description: registration include file
	based off the site http://www.wikihow.com/Create-a-Secure-Login-Script-in-PHP-and-MySQL
	
If there is no POST data passed into the form, the registration form is displayed. 
The form's submit button calls the JavaScript function regformhash(). This function 
does the necessary validation checks and submits the form when all is well. 

If the POST data exists, some server side checks are done to sanitise and validate it. 
NOTE that these checks are not complete at the time of writing. Some of the issues are
 mentioned in the comments in the file. At present, we just check that the email address 
 is in the correct format, that the hashed password is the correct length and that the 
 user is not trying to register an email that has already been registered.	
**/

include_once 'psl-config.php';
include_once 'db_connect.php';

$error_msg = "";

if (isset($_POST['username'], $_POST['email'], $_POST['p'])) {
	// Sanitize and validate the data passed in
	$username = filter_input(INPUT_POST, 'username', FILTER_SANITIZE_STRING);
	$email = filter_input(INPUT_POST, 'email', FILTER_SANITIZE_EMAIL);
	$email = filter_var($email, FILTER_VALIDATE_EMAIL);
	if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
		// Not a valid email
		$error_msg .= '<p class="error">The email address you entered is not valid </p>';
	}
	
	$password= filter_input(INPUT_POST, 'p', FILTER_SANITIZE_STRING);
	if (strlen($password) !=128) {
		// the hashed pwd should be 128 characters long
		// if not, something really odd has happened
		$error_message .= '<p class="error">Invalid password configuration.</p>';
	}
	
	//Username validity and password validity have been checked client side.
	//This should be adequate as nobody gains any advantage from
	//breaking these rules
	
	$prep_stmt = "SELECT id FROM userinfo WHERE email = ? LIMIT 1";
	$stmt = $mysqli->prepare($prep_stmt);
	
	// check existing email
	if ($stmt) {
	
		$stmt->bind_param('s', $email);
		$stmt->execute();
		$stmt->store_result();
		
		if ($stmt->num_rows == 1) {
			// a user with this email address already exists
			$error_msg .= '<p class="error">A user with this email address already exists.</p>';
			$stmt->close();
		}
		$stmt->close();
	} else {
		$error_msg .= '<p class="error">Database error Line 51</p>';
		if($stmt) { 
			$stmt->close();
		}
	}
		
	// check existing username
	$prep_stmt = "SELECT id FROM userinfo WHERE username = ? LIMIT 1";
	$stmt = $mysqli->prepare($prep_stmt);
	
	if ($stmt) {
		$stmt->bind_param('s', $username);
		$stmt->execute();
		$stmt->store_result();
			
		if ($stmt->num_rows == 1) {
			// user with this username already exists
			$error_msg .= '<p class="error">A user with this username already exists"</p>';
			$stmt->close();
		} 
		$stmt->close();
	} else {
		$error_msg .= '<p class="error">Database error line 71</p>';
		if ($stmt) {
			$stmt->close();
		}
	}
	
    // TODO: 
    // We'll also have to account for the situation where the user doesn't have
    // rights to do registration, by checking what type of user is attempting to
    // perform the operation.
	
	if (empty($error_msg)) {
	// create a random salt
	$random_salt = hash('sha512', uniqid(mt_rand(1, mt_getrandmax()), true));
	
	// create salted password
	$password = hash('sha512', $password . $random_salt);
	
		// Insert the new user into the database
		if ($insert_stmt = $mysqli->prepare("INSERT INTO userinfo (username, email, password, salt) 
											VALUES (?, ?, ?, ?)")) {
											echo "got here too. <BR><BR>";
			$insert_stmt->bind_param('ssss', $username, $email, $password, $random_salt);
			//execute
			if (! $insert_stmt->execute()) {
				header('Location: ../public_html/error.php?err=Registration failure: INSERT');
			}
			header('Location: ../public_html/register_success.php');
		}
	}

}

?>