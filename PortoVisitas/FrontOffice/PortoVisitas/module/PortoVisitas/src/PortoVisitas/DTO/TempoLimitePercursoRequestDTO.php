<?php
namespace PortoVisitas\DTO;

class TempoLimitePercursoRequestDTO extends PercursoRequestDTO
{

    /*
     * Int 4/8
     */
    public $maxHorasVisita;
    
    /*
     * Time string (HH:mm)
     */
    public $horaInicialVisita;
    
    public function exchangeArray($data)
    {
        parent::exchangeArray($data);
        $this->maxHorasVisita = (! empty($data['maxHorasVisita'])) ? $data['maxHorasVisita'] : null;
        $this->horaInicialVisita = (! empty($data['horaInicialVisita'])) ? $data['horaInicialVisita'] : null;
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
            
            $inputFilter->add(array(
                'name' => 'horaInicialVisita',
                'required' => true
            ));
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

