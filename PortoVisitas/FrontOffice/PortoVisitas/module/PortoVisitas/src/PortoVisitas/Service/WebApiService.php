<?php
namespace PortoVisitas\Service;

use Zend\Http\Client;
use Zend\Http\Request;
use Zend\Json\Json;

class WebApiService
{

    public static $enderecoBase = 'http://10.8.11.86/PVAPI';

    public static function Login($username, $password)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/api/Account/Login');
        $client->setMethod(Request::METHOD_POST);
        $data = "email=$username&password=$password&rememberme=false";
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/x-www-form-urlencoded',
            'Content-Length' => $len
        ));
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
        $data = "email=$mail&password=$password&confirmpassword=$password&role=User";
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/x-www-form-urlencoded',
            'Content-Length' => $len
        ));
        $client->setRawBody($data);
        $response = $client->send();
        if (! empty($response->getBody())) {
            $body = Json::decode($response->getBody(), false);
            if ($body->IsSuccessStatusCode)
                return null;
            else
                return $body;
        }
    }

    public static function getPois()
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/POI');
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $pois = Json::decode($response->getBody(), true);
        //var_dump($pois);
        return $pois;
    }
    
    public static function getUserPois($user)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/UserPOI?email='.$user);
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $pois = Json::decode($response->getBody(), true);
        return $pois;
    }

    public static function savePoi($poi)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/api/POI');
        $client->setMethod(Request::METHOD_POST);
        $data = json_encode($poi);
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/json',
            'Content-Length' => $len
        ));
        $client->setRawBody($data);
        //var_dump($data);
        $response = $client->send();
        if (! empty($response->getBody())) {
            $body = Json::decode($response->getBody(), false);
            if (! empty($body->ID)) // success
                return null;
            else
                return $body;
        }
    }

    public static function getPercursos()
    {
        return null;
    }

    public static function getPercurso($id)
    {}

    public static function savePercurso($percurso)
    {
        return false;
    }

    public static function deletePercurso($id)
    {
        return false;
    }
    
    // public static function getPoi($id){
    // if (! isset($_SESSION)) {
    // session_start();
    // }
    // if (! isset($_SESSION['access_token'])) {
    // WebApiServices::Login();
    // }
    
    // $client = new Client(WebApiServices::$enderecoBase . '/api/POIs/' . $id);
    // $client->setMethod(Request::METHOD_GET);
    // $bearer_token = 'Bearer ' . $_SESSION['access_token'];
    // $client->setHeaders(array(
    // 'Authorization' => $bearer_token
    // ));
    // $client->setOptions([
    // 'sslverifypeer' => false
    // ]);
    // $response = $client->send();
    // $body = $response->getBody();
    // $poi = Json::decode($response->getBody(), true);
    // return $poi;
    // }
}

