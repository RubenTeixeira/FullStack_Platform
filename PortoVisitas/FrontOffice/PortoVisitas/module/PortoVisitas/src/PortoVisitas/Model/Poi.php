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

    public $connectedPois;

    public $altitude;

    public $visitDuration;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        if (! empty($data['poiid']))
            $this->poiid = $data['poiid'];
        else 
            if (! empty($data['id']))
                $this->poiid = $data['id'];
            else
                $this->poiid = null;
        $this->name = (! empty($data['name'])) ? $data['name'] : null;
        $this->description = (! empty($data['description'])) ? $data['description'] : null;
        $this->gps_lat = (! empty($data['gps_lat'])) ? $data['gps_lat'] + 0.0 : null;
        $this->gps_long = (! empty($data['gps_long'])) ? $data['gps_long'] + 0.0 : null;
        $this->openHour = (! empty($data['openHour'])) ? $data['openHour'] : null;
        $this->closeHour = (! empty($data['closeHour'])) ? $data['closeHour'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->approved = (! empty($data['approved'])) ? $data['approved'] : null;
        $this->hashtags = (! empty($data['hashtags'])) ? $data['hashtags'] : null;
        if (! empty($data['connectedPois']))
            $this->connectedPois = $data['connectedPois'];
        else 
            if (! empty($data['connectedPoi']))
                $this->connectedPois = $data['connectedPoi'];
            else
                $this->connectedPois = null;
        $this->altitude = (! empty($data['altitude'])) ? $data['altitude'] : null;
        $this->visitDuration = (! empty($data['visitDuration'])) ? $data['visitDuration'] : null;
    }

    public function exchangeDTO($data)
    {
        $this->poiid = (! empty($data['ID'])) ? $data['ID'] : null;
        $this->name = (! empty($data['Name'])) ? $data['Name'] : null;
        $this->description = (! empty($data['Description'])) ? $data['Description'] : null;
        $this->gps_lat = (! empty($data['GPS_Lat'])) ? $data['GPS_Lat'] + 0.0 : null;
        $this->gps_long = (! empty($data['GPS_Long'])) ? $data['GPS_Long'] + 0.0 : null;
        $this->openHour = (! empty($data['OpenHour'])) ? $data['OpenHour'] : null;
        $this->closeHour = (! empty($data['CloseHour'])) ? $data['CloseHour'] : null;
        $this->creator = (! empty($data['Creator'])) ? $data['Creator'] : null;
        $this->approved = (! empty($data['Approved'])) ? $data['Approved'] : null;
        $this->visitDuration = (! empty($data['VisitDuration']) ? $data['VisitDuration'] : null);
        $this->hashtags = (! empty($data['Hashtags'])) ? $data['Hashtags'] : null;
        $this->connectedPois = (! empty($data['ConnectedPOI']) ? $data['ConnectedPOI'] : null);
        $this->altitude = (! empty($data['Altitude'])) ? $data['Altitude'] : null;
        $this->exchangeConnectedDTO();
    }

    private function exchangeConnectedDTO()
    {

        $connectedPois = array();
        if (null != $this->connectedPois) {
            foreach ($this->connectedPois as $connected) {
                $poiObj = new Poi();
                $poiObj->poiid = (! empty($connected['poiid'])) ? $connected['poiid'] : (! empty($connected['id'])) ? $connected['id'] : (! empty($connected['ID'])) ? $connected['ID'] : null;
                $poiObj->name = "Dummy";
                $poiObj->openHour = "2017-01-11T08:00:00";
                $poiObj->closeHour = "2017-01-11T18:00:00";
                $poiObj->gps_lat = 41.14015;
                $poiObj->gps_long = - 8.6;
                $poiObj->altitude = 15;
                $poiObj->visitDuration = 60;
                $poiObj->connectedPois = [];
                $connectedPois[] = $poiObj->getArrayCopy();
            }
        }
        $this->connectedPois = $connectedPois;
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
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'gps_long',
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'openHour',
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'closeHour',
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'altitude',
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'visitDuration',
                'required' => true
            ));
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

