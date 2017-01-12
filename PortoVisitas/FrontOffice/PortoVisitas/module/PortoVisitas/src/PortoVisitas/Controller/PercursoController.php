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


class PercursoController extends AbstractActionController
{
    public function indexAction()
    {
        $percursos = WebApiService::getPercursos();
        return array(
            'percursos' => $percursos,
        );
    }
    
    public function addAction()
    {
        $form = new PercursoForm();
        $request = $this->getRequest();

        if ($request->isPost()) {
            echo "IT WOOOOOOORKS...!!!!";
            //return $this->redirect()->toRoute('percurso');
        }
//             $pois = WebApiService::getPois();
//             $options = $this->getPoiOptions($pois);
//             $poiCheckBox = $form->get('connectedPois');
//             $poiCheckBox->setValueOptions($options);

        $pois = WebApiService::getPois();
        $options = $this->getPoiOptions($pois);
        $poiCheckBox = $form->get('poisMultiCheck');
        $poiCheckBox->setValueOptions($options);
        
        return array(
            'form' => $form,
            'pois' => $pois,
        );
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
