<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

class Visita
{
    public $visitaID;
    public $creator;
    public $date;
    public $percurso;
    protected $inputFilter;
    
    
    public function exchangeArray($data)
    {
        $this->visitaID = (! empty($data['visitaID'])) ? $data['visitaID'] : 0;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->date = (! empty($data['date']))  ? $data['date'] : null;
        $this->percurso = (! empty($data['percurso'])) ? array('ID' => $data['percurso']) : null;
    }
    
    public function exchangeDTO($data)
    {
        $this->visitaID = (! empty($data['VisitaID'])) ? $data['VisitaID'] : null;
        $this->creator = (! empty($data['Creator'])) ? $data['Creator'] : null;
        $this->date = (! empty($data['Date']))  ? $data['Date'] : null;
        $this->percurso = (! empty($data['Percurso'])) ? $data['Percurso'] : null;
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
                'name' => 'date',
                'required' => true,
            ));
    
    
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
    
}

