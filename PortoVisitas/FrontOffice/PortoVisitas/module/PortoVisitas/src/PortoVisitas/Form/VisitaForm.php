<?php
namespace PortoVisitas\Form;

use Zend\Form\Form;

class VisitaForm extends Form
{

    public function __construct($name = null)
    {
        // we want to ignore the name passed
        parent::__construct('visita');
        
        $this->setAttribute('class', 'form-horizontal');
        $this->setAttribute('id', 'visitaForm');
        
        $this->add(array(
            'name' => 'visitaID',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'creator',
            'id' => 'formCreator',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'percurso',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'type' => 'Zend\Form\Element\Date',
            'name' => 'date',
            'options' => array(
                'format' => 'Y-m-d'
            ),
            'attributes' => array(
                'min' => '2017-01-15',
                'max' => '2020-01-01',
                'step' => '1'
            ) // days; default step interval is 1 day

        ));
        
        
        $this->add(array(
            'name' => 'cancel',
            'attributes' => array(
                'value' => 'Cancelar',
                'class' => 'btn btn-default',
                'onclick' => 'window.history.back()'
            )
        ));
        
        $this->add(array(
            'name' => 'submit',
            'type' => 'Submit',
            'attributes' => array(
                'value' => 'Agendar',
                'class' => 'btn btn-primary',
            )
        ));
    }
}

