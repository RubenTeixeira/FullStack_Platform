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
            'pois' => $pois,
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
                echo var_dump($error);
                // Redirect to list of pois
                //return $this->redirect()->toRoute('poi');
            }
        }
        
        $pois = WebApiService::getPois();
        $options = $this->getPoiOptions($pois);
        $poiCheckBox = $form->get('connectedPoi');
        $poiCheckBox->setValueOptions($options);
        
        return array('form' => $form);      
    }
    
    private function getPoiOptions($pois)
    {
        $options = array();
        foreach ($pois as $poi)
        {
            $options[$poi->ID] = $poi->Name;
        }
        return $options;
    }
    
    private function formatConnectedPois($poi)
    {
        $connectedPois = array();
        foreach ($poi->connectedPoi as $connected)
        {
            $poiObj = array();
            $poiObj['POIID'] = $connected;
            $poiObj['Name'] = "Dummy";
            $poiObj['GPS_Lat0'] = 1.0;
            $poiObj['GPS_Long'] = 1.0;
            $poiObj['Altitude'] = 15;
            $connectedPois[] = $poiObj;
        }
        $poi->connectedPoi = $connectedPois;
    }
    
    private function formatHashtags($poi)
    {
        $oldHashtagArr = explode(',', $poi->hashtags);
        var_dump($oldHashtagArr);
        $newHashtagsArr = array();
        foreach ($oldHashtagArr as $tag)
        {
            $hashtag = array();
            $hashtag['Text'] = $tag;
            $newHashtagsArr[] = $hashtag;
        }
        $poi->hashtags = $newHashtagsArr;
    }
}