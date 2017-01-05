<?php
namespace PortoVisitas\Service;

use Zend\Http\Client;
use Zend\Http\Request;
use Zend\Json\Json;

class WebApiService
{

    public static $enderecoBase = 'https://localhost:44329';

    public static function Login($username, $password)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/Token');
        $client->setMethod(Request::METHOD_POST);
        $data = "grant_type=password&username=$username&password=$password";
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/x-www-form-urlencoded',
            'Content-Length' => $len
        ));
        $client->setOptions([
            'sslverifypeer' => false
        ]);
        $client->setRawBody($data);
        $response = $client->send();
        $body = Json::decode($response->getBody());
        if (! empty($body->access_token)) {
            return $body->access_token;
        } else
            return null;
    }
    
    public static function Register($mail, $password)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/api/Account/Register');
        $client->setMethod(Request::METHOD_POST);
        $data = "email=$mail&password=$password&confirmpassword=$password";
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/x-www-form-urlencoded',
            'Content-Length' => $len
        ));
        $client->setOptions([
            'sslverifypeer' => false
        ]);
        $client->setRawBody($data);
        $response = $client->send();
        if (! empty($response->getBody()) ) {
            $body = Json::decode($response->getBody(), false);
            return $body;
        } else
            return null;
    }

    public static function getPois()
    {
        
        $client = new Client(WebApiService::$enderecoBase . '/api/POI');
        $client->setMethod(Request::METHOD_GET);
        /*
        $bearer_token = 'Bearer ' . $_SESSION['token'];
        $client->setHeaders(array(
            'Authorization' => $bearer_token
        ));
        */
        $client->setOptions([
            'sslverifypeer' => false
        ]);
        
        $response = $client->send();
        $body = $response->getBody();
        $pois = Json::decode($response->getBody(), false);
        return $pois;
    }
    
    public static function getPercursos()
    {
        return null;
    }
    
    public static function getPercurso($id)
    {
        
    }
    
    public static function savePercurso($percurso)
    {
        return false;
    }
    
    public static function deletePercurso($id)
    {
        return false;
    }
    
//     public static function getPoi($id){
//         if (! isset($_SESSION)) {
//             session_start();
//         }
//         if (! isset($_SESSION['access_token'])) {
//             WebApiServices::Login();
//         }
        
//         $client = new Client(WebApiServices::$enderecoBase . '/api/POIs/' . $id);
//         $client->setMethod(Request::METHOD_GET);
//         $bearer_token = 'Bearer ' . $_SESSION['access_token'];
//         $client->setHeaders(array(
//             'Authorization' => $bearer_token
//         ));
//         $client->setOptions([
//             'sslverifypeer' => false
//         ]);
//         $response = $client->send();
//         $body = $response->getBody();
//         $poi = Json::decode($response->getBody(), true);
//         return $poi;
//     }
}

