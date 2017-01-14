<?php
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;
use PortoVisitas\Service\WebApiService;
use PortoVisitas\Form\PoiForm;
use PortoVisitas\Model\Poi;

/**
 * PoiController
 *
 * @author
 *
 * @version
 *
 */
class PoiController extends AbstractActionController
{

    /**
     * The default action - show the home page
     */
    public function indexAction()
    {
        $poiDTOs = WebApiService::getPois();
        $pois = array();
        foreach ($poiDTOs as $poiDTO) {
            $poi = new Poi();
            $poi->exchangeDTO($poiDTO);
            $pois[] = $poi;
        }
        
        return new ViewModel(array(
            'pois' => $pois
        ));
    }

    public function addAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        $form = new PoiForm();
        $request = $this->getRequest();
        
        if ($request->isPost()) {
            $poi = new Poi();
            $form->setInputFilter($poi->getInputFilter());
            $form->setData($request->getPost());
            if ($form->isValid()) {
                $poi->exchangeArray($form->getData());
                if (session_status() == PHP_SESSION_NONE) {
                    session_start();
                }
                $poi->creator = $_SESSION['user'];
                
                // var_dump($poi);
                $this->formatConnectedPois($poi);
                $this->formatHashtags($poi);
                $error = WebApiService::savePoi($poi);
                
                if ($error == null) {
                    $redirectUrl = $this->url()->fromRoute('poi');
                    
                    echo '<script type="application/javascript" charset="utf-8">alert("Ponto de Interesse enviado"); window.location.href = "' . $redirectUrl . '";</script>';
                } else {
                    echo "Por favor tente novamente.";
                    echo '<script type="application/javascript">alert("Erro no envio: ' . $error->Message . '");</script>';
                }
            }
        }
        
        $pois = WebApiService::getPois();
        $options = $this->getPoiOptions($pois);
        $poiCheckBox = $form->get('connectedPois');
        $poiCheckBox->setValueOptions($options);
        
        return new ViewModel(array(
            'form' => $form
        ));
    }

    private function getPoiOptions($pois)
    {
        $options = array();
        foreach ($pois as $poi) {
            $options[$poi['ID']] = $poi['Name'];
        }
        return $options;
    }

    private function formatConnectedPois($poi)
    {
        $connectedPois = array();
        foreach ($poi->connectedPois as $connected) {
            $poiObj = new Poi();
            $poiObj->poiid = intval($connected);
            $poiObj->name = "Dummy";
            $poiObj->openHour = "2017-01-11T08:00:00";
            $poiObj->closeHour = "2017-01-11T18:00:00";
            $poiObj->gps_lat = 41.14015;
            $poiObj->gps_long = - 8.6;
            $poiObj->altitude = 15;
            $poiObj->visitDuration = 60;
            $poiObj->connectedPois = [];
            $connectedPois[] = $poiObj->getArrayCopy();
        }
        $poi->connectedPois = $connectedPois;
    }

    private function formatHashtags($poi)
    {
        $oldHashtagArr = explode(',', $poi->hashtags);
        $newHashtagsArr = array();
        foreach ($oldHashtagArr as $tag) {
            $hashtag = array();
            $hashtag['Text'] = $tag;
            $newHashtagsArr[] = $hashtag;
        }
        $poi->hashtags = $newHashtagsArr;
    }
}