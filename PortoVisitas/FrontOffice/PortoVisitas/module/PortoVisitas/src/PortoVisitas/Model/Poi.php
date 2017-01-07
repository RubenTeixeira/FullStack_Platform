<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

class Poi
{

    public $id;

    public $name;

    public $description;

    public $gps_lat;

    public $gps_long;

    public $creator;

    public $approved;
    
    public $hashtags;

    public $connectedPoi;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->id = (! empty($data['id'])) ? $data['id'] : null;
        $this->name = (! empty($data['name'])) ? $data['name'] : null;
        $this->description = (! empty($data['description'])) ? $data['description'] : null;
        $this->gps_lat = (! empty($data['gps_lat'])) ? $data['gps_lat'] : null;
        $this->gps_long = (! empty($data['gps_long'])) ? $data['gps_long'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->approved = (! empty($data['approved'])) ? $data['approved'] : null;
        $this->hashtags = (! empty($data['hashtags'])) ? $data['hashtags'] : null;
        $this->connectedPoi = (! empty($data['connectedPoi'])) ? $data['connectedPoi'] : null;
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
                'filters' => array(
                    array(
                        'name' => 'Float'
                    )
                )
            ));
            
            $inputFilter->add(array(
                'name' => 'gps_long',
                'required' => true,
                'filters' => array(
                    array(
                        'name' => 'Float'
                    )
                )
            ));
        }
        return $this->inputFilter;
    }
}

