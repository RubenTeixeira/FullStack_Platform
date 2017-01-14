<?php
/**
 * Zend Framework (http://framework.zend.com/)
 *
 * @link      http://github.com/zendframework/PortoVisitas for the canonical source repository
 * @copyright Copyright (c) 2005-2015 Zend Technologies USA Inc. (http://www.zend.com)
 * @license   http://framework.zend.com/license/new-bsd New BSD License
 */
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use PortoVisitas\Service\WebApiService;
use PortoVisitas\Form\PercursoForm;
use Zend\View\Model\JsonModel;
use PortoVisitas\DTO\PassaPorPoisPercursoRequestDTO;
use PortoVisitas\DTO\TempoLimitePercursoRequestDTO;
use Zend\Http\Request;
use PortoVisitas\Model\Percurso;

class PercursoController extends AbstractActionController
{

    public function indexAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $percursos = WebApiService::getPercursos();
        return array(
            'percursos' => $percursos
        );
    }

    public function addAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $form = new PercursoForm();
        $request = $this->getRequest();
        
        if ($request->isPost()) {
            $percurso = new Percurso();
            $form->setInputFilter($percurso->getInputFilter());
            $form->setData($request->getPost());
            
            if ($form->isValid()) {
                $percurso->exchangeArray($form->getData());
                $percurso->creator = $_SESSION['user'];
                $error = WebApiService::savePercurso($percurso);
                return new JsonModel((array) $error);
//                 if ($error != null) {
//                    return new JsonModel((array) $error);
//                 } else {
//                     return $this->redirect()->toRoute('percurso');
//                 }
            } else {
                return new JsonModel(array('message' => 'Form is invalid', 'errors' => $form->getMessages(), 'data' => $form->getData()));
            }
            
        }
        
        $pois = WebApiService::getPois();
        $options = $this->getPoiOptions($pois);
        $poiCheckBox = $form->get('percursoPOIs');
        $poiCheckBox->setValueOptions($options);
        $selectPoiOrig = $form->get('poiOrigem');
        $selectPoiOrig->setValueOptions($options);
        $form->get('maxHorasVisita')->setDisableInArrayValidator(true);
        
        return array(
            'form' => $form,
            'pois' => $pois
        );
    }
    
    public function detailsAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $id = (int) $this->params()->fromRoute('id', 0);
        if (!$id) {
            return $this->redirect()->toRoute('percurso');
        }
        $percurso = WebApiService::getPercursoById($id);
        return array(
            'percurso' => $percurso
        );
    }

    public function generatePoisAction()
    {
        $request = $this->getRequest();
        
        if ($request->isPost()) {
            $dto = new PassaPorPoisPercursoRequestDTO();
            $result = $this->generate($request, $dto);
            if (null == $result)
                return new JsonModel(array(
                    "httpStatus" => '404',
                    "title" => 'No results'
                ));
            return new JsonModel((array) $result);
        }
    }

    public function generateTimeAction()
    {
        $request = $this->getRequest();
        
        if ($request->isPost()) {
            $dto = new TempoLimitePercursoRequestDTO();
            $result = $this->generate($request, $dto);
            if (null == $result)
                return new JsonModel(array(
                    "httpStatus" => '404',
                    "title" => 'No results'
                ));
            return new JsonModel((array) $result);
        }
    }

    private function generate($request, $dto)
    {
        $form = new PercursoForm();
        $form->setInputFilter($dto->getInputFilter());
        $form->setData($request->getPost());
        if ($form->isValid()) {
            $dto->exchangeArray($form->getData());
//             return $dto;
            $percursoGerado = WebApiService::getPercurso($dto);
            return $percursoGerado;
            // return array(
            // "percurso" => [
            // 2,
            // 10,
            // 2,
            // 47,
            // 9
            // ],
            // "duracao" => "350",
            // "kilometros" => "0.12302205728278467"
            // );
        }
    }

    private function getPoiOptions($pois)
    {
        $options = array();
        foreach ($pois as $poi) {
            $options[$poi['ID']] = $poi['Name'];
        }
        return $options;
    }
}
