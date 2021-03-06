<?php
namespace PortoVisitas\Service;

use Zend\Http\Client;
use Zend\Http\Request;
use Zend\Json\Json;
use Zend\Json\Server\Error;

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
        // var_dump($pois);
        return $pois;
    }

    public static function getUserPois($user)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/UserPOI?email=' . $user);
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
        // var_dump($data);
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
        $client = new Client(WebApiService::$enderecoBase . '/api/Percurso');
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $percursos = Json::decode($response->getBody(), true);
        // var_dump($pois);
        return $percursos;
    }

    public static function getPercursoPois($data)
    {
        $uri = 'Algav1';
        return $this::getPercurso($data, $uri);
    }

    public static function getPercursoTempo($data)
    {
        $uri = 'Algav2';
        return $this::getPercurso($data, $uri);
    }

    public static function getPercurso($data, $uri)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/' . $uri);
        $client->setOptions(array(
            'maxredirects' => 0,
            'timeout' => 30
        ));
        $client->setMethod(Request::METHOD_POST);
        $data = json_encode($data);
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/json',
            'Content-Length' => $len
        ));
        $client->setRawBody($data);
        $response = $client->send();
        
        // if (!$response->isSuccess())
        // return null;
        
        $body = $response->getBody();
        $percurso = Json::decode($response->getBody(), true);
        return $percurso;
    }

    public static function getPercursoById($id)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/Percurso/' . $id);
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $percurso = Json::decode($response->getBody(), true);
        return $percurso;
    }

    public static function savePercurso($percurso)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        if (null == $percurso->percursoid) {
            $client = new Client($enderecoBase . '/api/Percurso');
            $client->setMethod(Request::METHOD_POST);
        } else {
            $client = new Client($enderecoBase . '/api/Percurso/' . $percurso->percursoid);
            $client->setMethod(Request::METHOD_PUT);
        }
        $data = json_encode($percurso);
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/json',
            'Content-Length' => $len
        ));
        $client->setRawBody($data);
        $response = $client->send();
        if (! empty($response->getBody())) {
            $body = Json::decode($response->getBody(), false);
            if (! empty($body->ID)) // success
                return null;
            else
                return $body;
        }
    }

    public static function deletePercurso($id)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/api/Percurso/' . $id);
        $client->setMethod(Request::METHOD_DELETE);
        $response = $client->send();
        $body = $response->getBody();
    }

    public static function getVisitas()
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/Visitas');
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $visitas = Json::decode($response->getBody(), true);
        return $visitas;
    }
    
    public static function getVisitaById($id)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/Visitas/'.$id);
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $visita = Json::decode($response->getBody(), true);
        return $visita;
    }

    public static function getUserVisitas($email)
    {
        $client = new Client(WebApiService::$enderecoBase . '/api/UserVisitas?email=' . $email);
        $client->setMethod(Request::METHOD_GET);
        $response = $client->send();
        $body = $response->getBody();
        $visitas = Json::decode($response->getBody(), true);
        // var_dump($pois);
        return $visitas;
    }

    public static function saveVisita($visita)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        if (null == $visita->visitaID || 0 == $visita->visitaID) {
            $client = new Client($enderecoBase . '/api/Visitas');
            $client->setMethod(Request::METHOD_POST);
        } else {
            $client = new Client($enderecoBase . '/api/Visitas/' . $visita->visitaID);
            $client->setMethod(Request::METHOD_PUT);
        }
        $data = json_encode($visita);
        $len = strlen($data);
        $client->setHeaders(array(
            'Content-Type' => 'application/json',
            'Content-Length' => $len
        ));
        $client->setRawBody($data);
        $response = $client->send();
        if (! empty($response->getBody())) {
            $body = Json::decode($response->getBody(), false);
            if (! empty($body->VisitaID)) // success
                return null;
            else
                return $body;
        }
        return;
    }

    public static function deleteVisita($id)
    {
        $enderecoBase = WebApiService::$enderecoBase;
        $client = new Client($enderecoBase . '/api/Visitas/' . $id);
        $client->setMethod(Request::METHOD_DELETE);
        $response = $client->send();
        $body = $response->getBody();
    }

    public static function get404()
    {
        $error = new Error();
        $error->setCode(404);
        $error->setMessage('No results');
    }
}

