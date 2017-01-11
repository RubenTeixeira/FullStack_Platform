<?php 
namespace PortoVisitas\Form;

use Zend\Form\Form;

class PercursoForm extends Form
{

    public function __construct($name = null)
    {
        // we want to ignore the name passed
        parent::__construct('percurso');
        
        $this->setAttribute('class', 'form-horizontal');
        $this->add(array(
            'name' => 'percursoid',
            'type' => 'Hidden'
        ));

        $this->add(array(
            'name' => 'submit',
            'type' => 'Submit',
            'attributes' => array(
                'value' => 'Submeter',
                'id' => 'submitbutton',
                'class' => 'btn btn-primary'
            )
        ));
    }
}


