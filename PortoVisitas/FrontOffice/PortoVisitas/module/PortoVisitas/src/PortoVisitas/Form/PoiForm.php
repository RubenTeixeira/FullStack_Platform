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
            'options' => array(
                'label' => 'Nome:'
            )
        ));
        
        $this->add(array(
            'name' => 'description',
            'type' => 'Textarea',
            'options' => array(
                'label' => 'Descricao:'
            )
        ));
        
        $this->add(array(
            'name' => 'gps_lat',
            'type' => 'Number',
            'options' => array(
                'label' => 'Latitude:'
            ),
            'attributes' => array(
                'min' => '-120.976200',
                'max' => '41.250000',
                'step' => '0.000001'
            )
        ));
        
        $this->add(array(
            'name' => 'gps_long',
            'type' => 'Number',
            'options' => array(
                'label' => 'Longitude:'
            ),
            'attributes' => array(
                'min' => '-31.960000',
                'max' => '115.840000',
                'step' => '0.000001'
            )
        ));
        
        $time = new Time('openHour');
        $time
        ->setLabel('Hora abertura: ')
        ->setAttributes(array(
//             'min'  => '00:00:00',
//             'max'  => '23:59:59',
            'step' => '60', // seconds; default step interval is 60 seconds
        ))
        ->setOptions(array(
            'format' => 'H:i'
        ));
        
        $time2 = new Time('closeHour');
        $time2
        ->setLabel('Hora encerramento: ')
        ->setAttributes(array(
//             'min'  => '00:00:00',
//             'max'  => '23:59:59',
            'step' => '60', // seconds; default step interval is 60 seconds
        ))
        ->setOptions(array(
            'format' => 'H:i'
        ));
        
        $this->add($time);
        $this->add($time2);
        
//         $this->add(array(
//             'name' => 'openHour',
//             'type' => 'Time',
//             'options' => array(
//                 'label' => 'Hora abertura:',
//                 'format' => 'H:i:s',
//             ),
//             'attributes' => array(
//                 'min' => '00:00:00',
//                 'max' => '23:59:59',
//                 'step' => '60'
//             )
//         ));
        
//         $this->add(array(
//             'name' => 'closeHour',
//             'type' => 'Time',
//             'options' => array(
//                 'label' => 'Hora encerramento:'
//             )
//         ));
        
        $this->add(array(
            'name' => 'altitude',
            'type' => 'Number',
            'options' => array(
                'label' => 'Altitude:'
            ),
            'attributes' => array(
                'min' => '0',
                'max' => '3000',
                'step' => '1'
            )
        ));
        
        $multiCheck = new MultiCheckbox('connectedPoi');
        $multiCheck->setLabel('Ligacoes:');
        $multiCheck->setDisableInArrayValidator(true);
        $this->add($multiCheck);
        
        $this->add(array(
            'name' => 'hashtags',
            'type' => 'Textarea',
            'options' => array(
                'label' => 'Hashtags:'
            ),
            'attributes' => array(
                'id' => 'hashtags'
            )
        ));
        
        $this->add(array(
            'name' => 'submit',
            'type' => 'Submit',
            'attributes' => array(
                'value' => 'Submeter',
                'id' => 'submitbutton'
            )
        ));
    }
}

