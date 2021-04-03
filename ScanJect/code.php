<?php

    $host_db = "78.96.207.87";
    $user_db = "root";
    $pass_db = "ciceronaznaz1422";
    $db_name = "central";
    $tbl_name = "objects";

    $con = new mysqli($host_db, $user_db, $pass_db, $db_name);

    if ($con->connect_error) {
     die("Conection failed: " . $con->connect_error);
    }

    $username = $_POST['iRewiewer'];
    $password = $_POST['Eenaznaz22.'];

    $sql = "SELECT * FROM $tbl_name WHERE usuario = '$username'";
    $result = $con->query($sql);

    if ($result->num_rows > 0) {     

         $row = $result->fetch_array(MYSQLI_ASSOC);
         if ($password == $row["contra"]) { 
            echo "Success";
         } else { 
           echo "upX";
         }
     }
     else
     {
        echo "upX";
     }
     mysqli_close($con);
 ?>