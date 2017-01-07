<?php
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;
use PortoVisitas\Service\WebApiService;

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
        
    }
}