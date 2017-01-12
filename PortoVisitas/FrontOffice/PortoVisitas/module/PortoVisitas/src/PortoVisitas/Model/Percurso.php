<?php
namespace PortoVisitas\Model;


use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

class Percurso
{
    public $id;
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
        $this->id = (! empty($data['ID'])) ? $data['ID'] : null;
        $this->name = (! empty($data['Name'])) ? $data['Name'] : null;
        $this->description = (! empty($data['Description'])) ? $data['Description'] : null;
        $this->creator = (! empty($data['Creator'])) ? $data['Creator'] : null;
        $this->startHour = (! empty($data['StartHour'])) ? $data['StartHour'] : null;
        $this->finishHour = (! empty($data['FinishHour'])) ? $data['FinishHour'] : null;
        $this->percursoPoisOrder = (! empty($data['PercursoPoisOrder'])) ? $data['PercursoPoisOrder'] : null;
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
                'name' => 'startHour',
                'required' => true,
            ));
    
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

