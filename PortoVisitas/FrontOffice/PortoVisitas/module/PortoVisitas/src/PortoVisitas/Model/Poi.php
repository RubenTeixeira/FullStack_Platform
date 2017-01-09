<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

class Poi
{

    public $poiid;

    public $name;

    public $description;

    public $gps_lat;

    public $gps_long;
    
    public $openHour;
    
    public $closeHour;

    public $creator;

    public $approved;

    public $hashtags;

    public $connectedPoi;

    public $altitude;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->poiid = (! empty($data['poiid'])) ? $data['poiid'] : null;
        $this->name = (! empty($data['name'])) ? $data['name'] : null;
        $this->description = (! empty($data['description'])) ? $data['description'] : null;
        $this->gps_lat = (! empty($data['gps_lat'])) ? $data['gps_lat'] : null;
        $this->gps_long = (! empty($data['gps_long'])) ? $data['gps_long'] : null;
        $this->openHour = (! empty($data['openHour'])) ? $data['openHour'] : null;
        $this->closeHour = (! empty($data['closeHour'])) ? $data['closeHour'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->approved = (! empty($data['approved'])) ? $data['approved'] : null;
        $this->hashtags = (! empty($data['hashtags'])) ? $data['hashtags'] : null;
        $this->connectedPoi = (! empty($data['connectedPoi'])) ? $data['connectedPoi'] : null;
        $this->altitude = (! empty($data['altitude'])) ? $data['altitude'] : null;
    }

    public function getArrayCopy()
    {
        return get_object_vars($this);
    }

    public function setInputFilter(InputFilterInterface $inputFilter)
    {
        throw new \Exception("Not used");
    }

    public function getInputFilter()
    {
        if (! $this->inputFilter) {
            $inputFilter = new InputFilter();
            
            $inputFilter->add(array(
                'name' => 'name',
                'required' => true,
                'filters' => array(
                    array(
                        'name' => 'StripTags'
                    ),
                    array(
                        'name' => 'StringTrim'
                    )
                ),
                'validators' => array(
                    array(
                        'name' => 'StringLength',
                        'options' => array(
                            'encoding' => 'UTF-8',
                            'min' => 3,
                            'max' => 200
                        )
                    )
                )
            ));
            
            $inputFilter->add(array(
                'name' => 'description',
                'required' => true,
                'filters' => array(
                    array(
                        'name' => 'StripTags'
                    ),
                    array(
                        'name' => 'StringTrim'
                    )
                ),
                'validators' => array(
                    array(
                        'name' => 'StringLength',
                        'options' => array(
                            'encoding' => 'UTF-8',
                            'min' => 6,
                            'max' => 1000
                        )
                    )
                )
            ));
            
            $inputFilter->add(array(
                'name' => 'gps_lat',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'gps_long',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'openHour',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'closeHour',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'altitude',
                'required' => true,
            ));
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

