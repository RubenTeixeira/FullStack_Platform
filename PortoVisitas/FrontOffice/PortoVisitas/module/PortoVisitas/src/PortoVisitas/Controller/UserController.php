<?php
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;
use PortoVisitas\Form\LoginForm;
use PortoVisitas\Service\WebApiService;
use PortoVisitas\Form\RegisterForm;
use PortoVisitas\Model\User;
use PortoVisitas\Model\Poi;

/**
 * UserController
 *
 * @author
 *
 * @version
 *
 */
class UserController extends AbstractActionController
{
    public function loginAction()
    {
        $request = $this->getRequest();
        if (! $request->isPost()) {
            $form = new LoginForm();
            $form->get('submit')->setValue('Entrar');
            return array(
                'form' => $form
            );
        } else {
            $name = $request->getPost('email');
            $pass = $request->getPost('password');
            
            $token = WebApiService::Login($name, $pass);
            
            if ($token != null) {
                $this->startSession($name, $token);
            }
            
            return $this->redirect()->toRoute('user');
        }
    }

    public function logoutAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        $_SESSION['user'] = null;
        $_SESSION['token'] = null;
        return $this->redirect()->toRoute('home');
    }

    public function infoAction()
    {
    if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }

        $poiDTOs = WebApiService::getUserPois($_SESSION['user']);
        $pois = array();
        foreach ($poiDTOs as $poiDTO) {
            $poi = new Poi();
            $poi->exchangeDTO($poiDTO);
            $pois[] = $poi;
        }
        return new ViewModel(array(
            'mail' => $_SESSION['user'],
            'pois' => $pois
        ));
    }

    public function registerAction()
    {
        $form = new RegisterForm();
        $form->get('submit')->setValue('Registar');
        
        $request = $this->getRequest();
        if ($request->isPost()) {
            $user = new User();
            $form->setInputFilter($user->getInputFilter());
            $form->setData($request->getPost());
            
            if ($form->isValid()) {
                $user->exchangeArray($form->getData());
                $error = WebApiService::Register($user->email, $user->password);
                if ($error != null) {
                    echo $error;
                } else {
                    // start session with newly created user
                    $this->startSession($user->email, $user->password);
                }
            } else {
                echo "Invalid Form values!";
            }
        }
        
        return array(
            'form' => $form
        );
    }
    
    private function startSession($name, $token)
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        $_SESSION['user'] = $name;
        $_SESSION['token'] = $token;
        return $this->redirect()->toRoute('home');
    }
}
