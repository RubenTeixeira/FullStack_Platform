<?php
namespace PortoVisitas\Form;

use Zend\Form\Form;

class LoginForm extends Form
{
    
    public function __construct($name = null) {
        // we want to ignore the name passed
        parent::__construct('user');
        
        $this->add(array(
            'name' => 'email',
            'type' => 'Email',
            'options' => array(
                'label' => 'Email:',
            ),
        ));
        $this->add(array(
            'name' => 'password',
            'type' => 'Password',
            'options' => array(
                'label' => 'Password:',
            ),
        ));
        $this->add(array(
            'name' => 'submit',
            'type' => 'Submit',
            'attributes' => array(
                'value' => 'Go',
                'id' => 'submitbutton',
            ),
        ));
    }
}

