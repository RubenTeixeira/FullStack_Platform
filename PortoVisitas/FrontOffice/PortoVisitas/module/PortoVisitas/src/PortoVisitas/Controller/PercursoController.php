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

class PercursoController extends AbstractActionController
{
    public function indexAction()
    {
        $percursos = WebApiService::getPercursos();
        return array(
            'percursos' => $percursos,
        );
    }
}
