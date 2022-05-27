<?php

	// Servidor Active Directory
	$phpAD["ldap_server"] = "10.14.0.251";

	// Usuario e senha necess�rio dominio
	$phpAD["auth_user"] = "ramais";
	$phpAD["auth_pass"] = "unimed@18";

	// Unidade organizacional
	$phpAD["ldap_dn"] = "OU=UNIMED,DC=unimed014,DC=com,DC=br";
 
	// $phpAD["ldap_default_ou"] = "Matriz";
	$phpAD["ldap_default_ou"] = "OU=UNIMED";
 
	// Dominio Active directory
	$phpAD["ad_domain_name"] = "empresa";
 
	set_time_limit(0);
 
	// Base do dominio para procura.
	$base_dn = $phpAD["ldap_dn"];
 
	// Conectando ao servidor
	if (!($connect=@ldap_connect($phpAD["ldap_server"])))
	die("Could not connect to ldap server");
 
	// Autenticando
	if (!($bind=@ldap_bind($connect, $phpAD["auth_user"], $phpAD["auth_pass"])))
	die("Unable to bind to server");
 
	$filtro = "(&(objectClass=user)(objectCategory=person)(displayname=*))";
 
	$mostrar = array("displayname","department","telephonenumber","mail","manager","physicalDeliveryOfficeName");
 
	// Busca no active directory $busca = ldap_search($ds, $ldap_dn, $filtro/*, $attributes*/);
	if (!($busca=@ldap_search($connect, $base_dn, $filtro, $mostrar)))
	die("Nao foi possivel realizar busca no Active Directory");
 
	$info = ldap_get_entries($connect, $busca);
	// echo "<pre>";
	// print_r($info[2]);
	// echo "</pre>";
	// exit;
 
	//Salva todos os usuarios em um vetor
	foreach ($info as $Key => $Value){
    $nome      	 = $info[$Key]["displayname"][0];
    $departamento = $info[$Key]["department"][0];
    $telefone     = $info[$Key]["telephonenumber"][0];
    $email        = $info[$Key]["mail"][0];
    $coordenador  = $info[$Key]["manager"][0];
    $unidade  	  = $info[$Key]["physicaldeliveryofficename"][0];
   
   // $State     = dechex($State);
   // $State      = substr($State,-1,1);//verifica contas desabilitadas
 
    $Value = $nome."^".$departamento."^".$telefone."^".$email."^".$coordenador."^".$unidade;
		if ( $nome != ""/* && $State != 2*/)
			$USERs[]=$Value;
    }
   
    if ( count($USERs) > 0 )
    sort($USERs);
   
	if ( count($USERs) == 0 ){
		echo "Nao foi econtrado nenhum usuario";
	}
	
 
  // Verifica todos departamentos na OU como financeiro, RH, TI...
	for ($i=0;$i<=count($USERs)-1;$i++){
		$Value    = $USERs[$i];
		$Splitted = explode("^",$Value);

		$nome          = $Splitted[0];
		$departamento  = $Splitted[1];
		$telefone      = $Splitted[2];
		$email         = $Splitted[3];
		$coordenador   = $Splitted[4];
		$unidade   	   = $Splitted[5];

		$org_array = explode(",",$coordenador);
		$coordenador = substr($org_array[0],3,(strlen($org_array[0])));
		$temp[$i] = $coordenador;
	}
   
    $org2 = array_unique($temp);
	
	require_once("class/conexao.php");
	conexao();	
	$sql = mysql_query("DELETE FROM bdunimed.ti_ramais");
	
	foreach( $org2 as $mostra ){
		 for ($i=0;$i<=count($USERs)-1;$i++)
		  {
		   $Value    = $USERs[$i];
		   $Splitted = explode("^",$Value);

		   $nome         = $Splitted[0];
		   $departamento = $Splitted[1];
		   $telefone     = $Splitted[2];
		   $email        = $Splitted[3];
		   $coordenador  = $Splitted[4];
		   $unidade  	 = $Splitted[5];
		   
		   $org_array = explode(",",$coordenador);
		   $coordenador = substr($org_array[0],3,(strlen($org_array[0])));
		   $temp[$i] = $coordenador;
		   if($coordenador == $mostra && !empty($nome) && !empty($coordenador) && $nome != "coord"){			   
			   $sql = mysql_query("INSERT INTO bdunimed.ti_ramais(nome, departamento, telefone, email, coordenador, unidade)
								  VALUES ('".$nome."', '".$departamento."', '".$telefone."', '".$email."', '".$coordenador."','".$unidade."')");
		   }
		}
	}

	echo "Processo finalizado!";
	
?>

