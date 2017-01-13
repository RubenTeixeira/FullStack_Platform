<?php
namespace PortoVisitas\Form;

use Zend\Form\Form;
use Zend\Form\Element\Time;
use Zend\Form\Element\MultiCheckbox;
use Zend\Form\Element\Select;

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
        
        $time = new Time('horaInicialVisita');
        $time->setAttributes(array(
            'step' => '60',
            'class' => 'form-control'
        ))->setOptions(array(
            'format' => 'H:i'
        ));
        $this->add($time);
        
        $this->add(array(
            'type' => 'Zend\Form\Element\Select',
            'name' => 'tipoVeiculo',
            'attributes' => array(
                'class' => 'form-control'
            ),
            'options' => array(
                'value_options' => array(
                    'pe' => 'Pé',
                    'carro' => 'Carro',
                    'autocarro' => 'Autocarro',
                    'tuk' => 'Tuk'
                )
            )
        ));
        
        $this->add(array(
            'name' => 'inclinacaoMax',
            'type' => 'Number',
            'attributes' => array(
                'min' => '0',
                'max' => '60',
                'step' => '1',
                'class' => 'form-control'
            )
        ));
        
        $this->add(array(
            'name' => 'kilometrosMax',
            'type' => 'Number',
            'attributes' => array(
                'min' => '0',
                'max' => '1000',
                'step' => '1',
                'class' => 'form-control'
            )
        ));
        
        $this->add(array(
            'type' => 'Zend\Form\Element\Radio',
            'name' => 'typeOfSuggestion',
            'options' => array(
                // 'label' => 'Como pretende definir o percurso?',
                'value_options' => array(
                    array(
                        'value' => '0',
                        'label' => 'Escolhendo os pontos de interesse',
                        'label_attributes' => array(
                            'class' => 'radio'
                        ),
                        'attributes' => array(
                            'id' => 'choosePois'
                        )
                    ),
                    
                    array(
                        'value' => '1',
                        'label' => 'Definindo a sua duração máxima',
                        'label_attributes' => array(
                            'class' => 'radio'
                        ),
                        'attributes' => array(
                            'id' => 'chooseDuration'
                        )
                    )
                )
            )
        ));
        
        $this->add(array(
            'type' => 'Zend\Form\Element\Radio',
            'name' => 'maxHorasVisita',
            'options' => array(
                'value_options' => array(
                    array(
                        'value' => '0',
                        'label' => '4',
                        'label_attributes' => array(
                            'class' => 'radio'
                        )
                    ),
                    
                    array(
                        'value' => '1',
                        'label' => '8',
                        'label_attributes' => array(
                            'class' => 'radio'
                        )
                    )
                )
            ),
            'attributes' => [
                'value' => '0' // This set the opt 1 as selected when form is rendered
            ]
        ));
        
        $selectPoiOrig = new Select('poiOrigem');
        $selectPoiOrig->setAttributes(array('class' => 'form-control'));
        $selectPoiOrig->setDisableInArrayValidator(true);
        $this->add($selectPoiOrig);
        
        $multiCheck = new MultiCheckbox('poiList');
        $multiCheck->setDisableInArrayValidator(true);
        $multiCheck->setOption('style', 'display: none');
        $multiCheck->setLabelAttributes(array(
            'class' => 'checkbox'
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
                'value' => 'Gerar Percurso',
                'id' => 'submitbutton',
                'class' => 'btn btn-primary'
            )
        ));
    }
}


