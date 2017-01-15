<?php
namespace PortoVisitas\Model;


use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;
use Zend\Db\Sql\Ddl\Column\Date;
use Zend\Db\Sql\Ddl\Column\Time;

class Percurso
{
    public $percursoid;
    public $name;
    public $description;
    public $creator;
    public $startHour;
    public $finishHour;
    public $percursoPoisOrder;
    public $percursoPOIs;
    protected $inputFilter;
    
    public function exchangeArray($data)
    {
        $this->percursoid = (! empty($data['id'])) ? $data['id'] : null;
        $this->name = (! empty($data['name'])) ? $data['name'] : null;
        $this->description = (! empty($data['description'])) ? $data['description'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->startHour = (! empty($data['horaInicialVisita']))  ? $data['horaInicialVisita'] : null;
        $this->finishHour = (! empty($data['finishHour'])) ? $data['finishHour'] : null;
        $this->percursoPoisOrder = (! empty($data['percursoPoisOrder'])) ? $data['percursoPoisOrder'] : null;
        $this->percursoPOIs = (! empty($data['poiList'])) ? explode(',',$data['poiList']) : null;
        $this->exchangePercursoPOIs();
    }
    
    public function exchangeDTO($data)
    {
        $this->percursoid = (! empty($data['ID'])) ? $data['ID'] : null;
        $this->name = (! empty($data['Name'])) ? $data['Name'] : null;
        $this->description = (! empty($data['Description'])) ? $data['Description'] : null;
        $this->creator = (! empty($data['Creator'])) ? $data['Creator'] : null;
        $this->startHour = (! empty($data['StartHour']))  ? $this->getTime($data['StartHour']) : null;
        $this->finishHour = (! empty($data['FinishHour'])) ? $data['FinishHour'] : null;
        $this->percursoPoisOrder = (! empty($data['PercursoPOIsOrder'])) ? $data['PercursoPOIsOrder'] : null;
        $this->percursoPOIs = (! empty($data['PercursoPOIs'])) ? $data['PercursoPOIs'] : null;
    }
    
    public function getArrayCopy()
    {
        return get_object_vars($this);
    }
    
    public function setInputFilter(InputFilterInterface $inputFilter)
    {
        throw new \Exception("Not used");
    }
    
    private function exchangePercursoPOIs()
    {
        $connectedPois = array();
        if (null != $this->percursoPOIs) {
            foreach ($this->percursoPOIs as $connected) {
                $poiObj = new Poi();
                $poiObj->poiid = intval($connected);
                $poiObj->name = "Dummy";
                $poiObj->openHour = "2017-01-11T08:00:00";
                $poiObj->closeHour = "2017-01-11T18:00:00";
                $poiObj->gps_lat = 41.14015;
                $poiObj->gps_long = -8.6;
                $poiObj->altitude = 15;
                $poiObj->visitDuration = 60;
                $poiObj->connectedPois = [];
                $connectedPois[] = $poiObj->getArrayCopy();
            }
        }
        $this->percursoPOIs = $connectedPois;
    }
    
//     private function exchangePercursoPOIsDTO()
//     {
//         $connectedPois = array();
//         if (null != $this->percursoPOIs) {
//             foreach ($this->percursoPOIs as $connected) {
//                 $poiObj = new Poi();
//                 $poiObj->poiid = $connected['ID'];
//                 $poiObj->name = $connected['Name'];
//                 $poiObj->openHour = "2017-01-11T08:00:00";
//                 $poiObj->closeHour = "2017-01-11T18:00:00";
//                 $poiObj->gps_lat = 41.14015;
//                 $poiObj->gps_long = -8.6;
//                 $poiObj->altitude = 15;
//                 $poiObj->visitDuration = 60;
//                 $poiObj->connectedPois = [];
//                 $connectedPois[] = $poiObj->getArrayCopy();
//             }
//         }
//         $this->percursoPOIs = $connectedPois;
//     }
    
    private function stringify($poiOrder)
    {
        $this->percursoPoisOrder = implode(',', $poiOrder);
    }
    
    public function getInputFilter()
    {
        if (! $this->inputFilter) {
            $inputFilter = new InputFilter();
   
            $inputFilter->add(array(
                'name' => 'name',
                'required' => true,
            ));
    
            $inputFilter->add(array(
                'name' => 'description',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'horaInicialVisita',
                'required' => true,
            ));
            
            $inputFilter->add(array(
                'name' => 'finishHour',
                'required' => true,
            ));
    
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
    
    
    private function getTime($date)
    {
        $arr = explode('T',$date);
        $time = explode(':',$arr[1]);
        return $time[0].":".$time[1];
    }
}

