<?php 
namespace PortoVisitas\Form;

use Zend\Form\Form;
use Zend\Form\Element\Time;
use Zend\Form\Element\MultiCheckbox;

class PercursoForm extends Form
{

    public function __construct($name = null)
    {
        // we want to ignore the name passed
        parent::__construct('percurso');
        
        $this->setAttribute('class', 'form-horizontal');
        $this->setAttribute('id', 'percursoForm');
        
        $this->add(array(
            'name' => 'id',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'creator',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'name',
            'type' => 'Text',
            'attributes' => array(
                'class' => 'form-control'
            )
        ));
        
        $this->add(array(
            'name' => 'description',
            'type' => 'Textarea',
            'attributes' => array(
                'class' => 'form-control'
            )
        ));
        
        $time = new Time('startHour');
        $time->setAttributes(array(
            'step' => '60',
            'class' => 'form-control'
        ))->setOptions(array(
            'format' => 'H:i'
        ));
        $this->add($time);
        
        $this->add(array(
            'type' => 'Zend\Form\Element\Radio',
            'name' => 'typeOfSuggestion',
            'options' => array(
                //'label' => 'Como pretende definir o percurso?',
                'value_options' => array(
                    array(
                        'value' => '0',
                        'label' => 'Escolhendo pontos de interesse',
                        'attributes' => array(
                            'id' => 'choosePois',
                        ),
                    ),
                    
                    array(
                        'value' => '1',
                        'label' => 'Definindo duracao do percurso',
                        'attributes' => array(
                            'id' => 'chooseDuration',
                        ),
                    ),
                ),
            )
        ));
        
        $multiCheck = new MultiCheckbox('poisMultiCheck');
        $multiCheck->setDisableInArrayValidator(true);
        $multiCheck->setOption('style', 'display: none');
        $multiCheck->setLabelAttributes(array(
            'class' => 'checkbox',
        ));
        $this->add($multiCheck);
        
        $this->add(array(
            'name' => 'finishHour',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'percursoPoisOrder',
            'type' => 'Hidden'
        ));
        
        $this->add(array(
            'name' => 'percursoPOIs',
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


