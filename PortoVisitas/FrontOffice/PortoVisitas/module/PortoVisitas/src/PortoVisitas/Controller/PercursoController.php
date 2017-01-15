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
                
                if ($error != null) {
                   echo "Por favor tente novamente.";
                   echo '<script type="application/javascript">alert("Erro no envio: ' . $error->Message . '");</script>';
                   return;
                } else {
                    return $this->redirect()->toRoute('percurso');
                }
            } else {
                echo "Dados inválidos.";
                return;
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
    
    public function editAction()
    {
        $id = (int) $this->params()->fromRoute('id', 0);
        if (! $id) {
            return $this->redirect()->toRoute('percurso', array(
                'action' => 'index'
            ));
        }
        
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $percurso = new Percurso();
        $dto = WebApiService::getPercursoById($id);
        $percurso->exchangeDTO($dto);
        
        if ($_SESSION['user'] != $percurso->creator) {
            return $this->redirect()->toRoute('user');
        }
    
        $form = new PercursoForm();
        $form->bind($percurso);
        $form->get('id')->setValue($percurso->percursoid);
        $form->get('horaInicialVisita')->setValue($percurso->startHour);
        $form->get('poiList')->setValue(implode(',',array_column($percurso->percursoPOIs, 'ID')));
        $form->get('percursoPoisOrder')->setValue($percurso->percursoPoisOrder);
        $form->get('finishHour')->setValue($percurso->finishHour);
        $pois = WebApiService::getPois();
        $options = $this->getPoiOptions($pois);
        $form->get('poiOrigem')->setValueOptions($options);
        $form->get('percursoPOIs')->setValueOptions($options);
        $form->get('submitPercurso')->setAttribute('value', 'Guardar');
        $request = $this->getRequest();
        if ($request->isPost()) {
            $form->setInputFilter($percurso->getInputFilter());
            $form->setData($request->getPost());
            if ($form->isValid()) {
                //$edited = $percurso->exchangeArray($form->getData());
                //var_dump($edited);
                WebApiService::savePercurso($percurso);
                //return $this->redirect()->toRoute('percurso');
            } else {
                return new JsonModel((array) $form->getMessages());
            }
        }
        return array(
            'id' => $id,
            'form' => $form
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
    
    public function deleteAction()
    {
        $id = (int) $this->params()->fromRoute('id', 0);
        if (!$id) {
            return $this->redirect()->toRoute('percurso');
        }
    
        $request = $this->getRequest();
        if ($request->isPost()) {
            $del = $request->getPost('del', 'Nao');
    
            if ($del == 'Sim') {
                $id = (int) $request->getPost('id');
                WebApiService::deletePercurso($id);
            }
            return $this->redirect()->toRoute('percurso');
        }
    
        return array(
            'id'    => $id,
            'percurso' => WebApiService::getPercursoById($id),
        );
    }

    public function generatePoisAction()
    {
        $request = $this->getRequest();
        
        if ($request->isPost()) {
            $dto = new PassaPorPoisPercursoRequestDTO();
            $result = $this->generate($request, $dto, 'Algav1');
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
            $result = $this->generate($request, $dto, 'Algav2');
            if (null == $result)
                return new JsonModel(array(
                    "httpStatus" => '404',
                    "title" => 'No results'
                ));
            return new JsonModel((array) $result);
        }
    }

    private function generate($request, $dto, $uri)
    {
        $form = new PercursoForm();
        $form->setInputFilter($dto->getInputFilter());
        $form->setData($request->getPost());
        if ($form->isValid()) {
            $dto->exchangeArray($form->getData());
            $percursoGerado = WebApiService::getPercurso($dto,$uri);
            return $percursoGerado;
        }
        return array('ERROR' => 'INVALID', 'Messages' => $form->getMessages(), 'dto' => $dto);
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
