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
        $this->id = (! empty($data['id'])) ? $data['id'] : null;
        $this->name = (! empty($data['name'])) ? $data['name'] : null;
        $this->description = (! empty($data['description'])) ? $data['description'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->startHour = (! empty($data['startHour'])) ? $data['startHour'] : null;
        $this->finishHour = (! empty($data['finishHour'])) ? $data['finishHour'] : null;
        $this->percursoPoisOrder = (! empty($data['percursoPoisOrder'])) ? $this->stringify($data['percursoPoisOrder']) : null;
        $this->percursoPOIs = (! empty($data['percursoPOIs'])) ? $data['percursoPOIs'] : null;
    }
    
    public function getArrayCopy()
    {
        return get_object_vars($this);
    }
    
    public function setInputFilter(InputFilterInterface $inputFilter)
    {
        throw new \Exception("Not used");
    }
    
    private function stringify($poiOrder)
    {
        
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
            
            $inputFilter->add(array(
                'name' => 'finishHour',
                'required' => true,
            ));
    
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

