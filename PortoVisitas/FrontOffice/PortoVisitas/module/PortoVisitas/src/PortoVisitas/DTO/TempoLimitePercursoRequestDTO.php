<?php
namespace PortoVisitas\DTO;

class TempoLimitePercursoRequestDTO extends PercursoRequestDTO
{

    /*
     * Int 4/8
     */
    public $maxHorasVisita;
    
    
    public function exchangeArray($data)
    {
        parent::exchangeArray($data);
        
        $this->maxHorasVisita = ($data['maxHorasVisita'] == "1") ? 8 : 4;
        
    }

    public function getArrayCopy()
    {
        return get_object_vars($this);
    }

    public function getInputFilter()
    {
        if (! $this->inputFilter) {
            $inputFilter = parent::getInputFilter();
            
            $inputFilter->add(array(
                'name' => 'maxHorasVisita',
                'required' => true
            ));
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

