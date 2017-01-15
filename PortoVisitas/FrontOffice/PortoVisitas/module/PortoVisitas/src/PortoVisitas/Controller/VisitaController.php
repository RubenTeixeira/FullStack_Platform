<?php
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;
use PortoVisitas\Service\WebApiService;
use PortoVisitas\Model\Visita;
use PortoVisitas\Form\VisitaForm;

/**
 * VisitaController
 *
 * @author
 *
 * @version
 *
 */
class VisitaController extends AbstractActionController
{

    /**
     * The default action - show the home page
     */
    public function indexAction()
    {
        $visitaDTOs = WebApiService::getVisitas();
        $visitas = array();
        foreach ($visitaDTOs as $visitaDTO) {
            $visita = new Visita();
            $visita->exchangeDTO($visitaDTO);
            $visitas[] = $visita;
        }
        
        return new ViewModel(array(
            'visitas' => $visitas
        ));
    }
    
    public function addAction()
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
        $form = new VisitaForm();
        $request = $this->getRequest();
    
        if ($request->isPost()) {
            $visita = new Visita();
            $form->setInputFilter($visita->getInputFilter());
            $form->setData($request->getPost());
    
            if ($form->isValid()) {
                $visita->exchangeArray($form->getData());
                $visita->creator = $_SESSION['user'];
                $error = WebApiService::saveVisita($visita);
    
                if ($error != null) {
                    echo "Por favor tente novamente.";
                    echo '<script type="application/javascript">alert("Erro no envio: ' . $error->Message . '");</script>';
                    return;
                } else {
                    return $this->redirect()->toRoute('visita');
                }
            } else {
                echo "Dados inválidos.";
                return;
            }
    
        }
    
        $percurso = WebApiService::getPercursoById($id);
        $form->get('percurso')->setValue($percurso['ID']);
        return array(
            'form' => $form,
            'percurso' => $percurso
        );
    }
    
    public function editAction()
    {
        $id = (int) $this->params()->fromRoute('id', 0);
        if (! $id) {
            return $this->redirect()->toRoute('visita', array(
                'action' => 'index'
            ));
        }
    
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $visita = new Visita();
        $dto = WebApiService::getVisitaById($id);
        $visita->exchangeDTO($dto);
    
        if ($_SESSION['user'] != $visita->creator) {
            return $this->redirect()->toRoute('user');
        }
    
        $form = new VisitaForm();
        $form->bind($visita);
//         $form->get('id')->setValue($percurso->percursoid);
//         $form->get('horaInicialVisita')->setValue($percurso->startHour);
//         $form->get('poiList')->setValue(implode(',',array_column($percurso->percursoPOIs, 'ID')));
//         $form->get('percursoPoisOrder')->setValue($percurso->percursoPoisOrder);
//         $form->get('finishHour')->setValue($percurso->finishHour);
//         $pois = WebApiService::getPois();
//         $options = $this->getPoiOptions($pois);
//         $form->get('poiOrigem')->setValueOptions($options);
//         $form->get('percursoPOIs')->setValueOptions($options);
//         $form->get('submitPercurso')->setAttribute('value', 'Guardar');
        $request = $this->getRequest();
        if ($request->isPost()) {
            $form->setInputFilter($visita->getInputFilter());
            $form->setData($request->getPost());
            if ($form->isValid()) {
                //$edited = $percurso->exchangeArray($form->getData());
                //var_dump($edited);
                WebApiService::saveVisita($visita);
                return $this->redirect()->toRoute('visita');
//             } else {
//                 return new JsonModel((array) $form->getMessages());
            }
        }
        $form->get('percurso')->setValue($visita->percurso['ID']);
        $date = explode('T',$visita->date);
        $form->get('date')->setValue($date[0]);
        $form->get('submit')->setValue('Guardar');
        return array(
            'id' => $visita->visitaID,
            'percurso' => $visita->percurso,
            'form' => $form
        );
    }
    
    public function deleteAction()
    {
        $id = (int) $this->params()->fromRoute('id', 0);
        if (!$id) {
            return $this->redirect()->toRoute('visita');
        }
    
        $request = $this->getRequest();
        if ($request->isPost()) {
            $del = $request->getPost('del', 'Nao');
    
            if ($del == 'Sim') {
                $id = (int) $request->getPost('id');
                WebApiService::deleteVisita($id);
            }
            return $this->redirect()->toRoute('visita');
        }
        $visita = WebApiService::getVisitaById($id);
        return array(
            'id'    => $id,
            'visita' => $visita,
        );
    }
}