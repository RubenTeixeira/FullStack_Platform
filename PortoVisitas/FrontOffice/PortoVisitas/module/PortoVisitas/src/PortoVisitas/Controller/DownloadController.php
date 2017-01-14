<?php
namespace PortoVisitas\Controller;

use Zend\Mvc\Controller\AbstractActionController;
use Zend\View\Model\ViewModel;

/**
 * DownloadController
 *
 * @author
 *
 * @version
 *
 */
class DownloadController extends AbstractActionController
{

    /**
     * The default action - show the home page
     */
    public function indexAction()
    {
        if (session_status() == PHP_SESSION_NONE) {
            session_start();
        }
        if (!isset($_SESSION['user'])) {
            return $this->redirect()->toRoute('user', array('action'=>'login'));
        }
        return new ViewModel();
    }
}