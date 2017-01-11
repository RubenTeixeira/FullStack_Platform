<?php
namespace PortoVisitas\Form;

use Zend\Form\Form;
use Zend\Form\Element\Time;
use Zend\Form\Element\MultiCheckbox;

class PoiForm extends Form
{

    public function __construct($name = null)
    {
        // we want to ignore the name passed
        parent::__construct('poi');
        
        $this->setAttribute('class', 'form-horizontal');
        
        $this->add(array(
            'name' => 'poiid',
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
        
        $this->add(array(
            'name' => 'gps_lat',
            'type' => 'Number',
            'attributes' => array(
                'min' => '-120.976200',
                'max' => '41.250000',
                'step' => '0.000001',
                'class' => 'form-control'
            )
        ));
        
        $this->add(array(
            'name' => 'gps_long',
            'type' => 'Number',
            'attributes' => array(
                'min' => '-31.960000',
                'max' => '115.840000',
                'step' => '0.000001',
                'class' => 'form-control'
            )
        ));
        
        $time = new Time('openHour');
        $time->setAttributes(array(
            'step' => '60',
            'class' => 'form-control'
        ))->setOptions(array(
            'format' => 'H:i'
        ));
        
        $time2 = new Time('closeHour');
        $time2->setAttributes(array(
            'step' => '60',
            'class' => 'form-control'
        ))->setOptions(array(
            'format' => 'H:i'
        ));
        
        $this->add($time);
        $this->add($time2);
        
        $this->add(array(
            'name' => 'altitude',
            'type' => 'Number',
            'attributes' => array(
                'min' => '15',
                'max' => '269',
                'step' => '1',
                'class' => 'form-control'
            )
        ));
        
        $multiCheck = new MultiCheckbox('connectedPois');
        $multiCheck->setDisableInArrayValidator(true);
//         $multiCheck->setAttributes(array(
//             //'class' => 'checkbox'
//         ));
        $multiCheck->setLabelAttributes(array(
            'class' => 'checkbox',
        ));
        $this->add($multiCheck);
        
        $this->add(array(
            'name' => 'hashtags',
            'type' => 'Textarea',
            'attributes' => array(
                'id' => 'hashtags',
                'class' => 'form-control'
            )
        ));
        
        $this->add(array(
            'name' => 'visitDuration',
            'type' => 'Number',
            'attributes' => array(
                'min' => '1',
                'max' => '480',
                'step' => '1',
                'class' => 'form-control'
            )
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

