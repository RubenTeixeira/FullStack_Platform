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
        $pois = WebApiService::getPois();
        return array(
            'pois' => $pois
        );
    }

    public function addAction()
    {
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
        
        return array(
            'form' => $form
        );
    }

    private function getPoiOptions($pois)
    {
        $options = array();
        foreach ($pois as $poi) {
            $options[$poi->ID] = $poi->Name;
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
            $poiObj->gps_lat = 1.1;
            $poiObj->gps_long = 1.1;
            $poiObj->altitude = 15;
            $connectedPois[] = $poiObj->getArrayCopy();
        }
        $poi->connectedPois = $connectedPois;
    }

    private function formatHashtags($poi)
    {
        $oldHashtagArr = explode(',', $poi->hashtags);
        var_dump($oldHashtagArr);
        $newHashtagsArr = array();
        foreach ($oldHashtagArr as $tag) {
            $hashtag = array();
            $hashtag['Text'] = $tag;
            $newHashtagsArr[] = $hashtag;
        }
        $poi->hashtags = $newHashtagsArr;
    }
}