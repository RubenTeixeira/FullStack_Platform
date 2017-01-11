<?php
namespace PortoVisitas\DTO;

class PassaPorPoisPercursoRequestDTO extends PercursoRequestDTO
{

    /*
     * List of int (id)
     */
    public $poiList;

    /*
     * Time string (HH:mm)
     */
    public $startingMinute;
    
    
    
    

    public function exchangeArray($data)
    {
        parent::exchangeArray($data);
        $this->poiList = (! empty($data['poiList'])) ? $data['poiList'] : null;
        $this->startingMinute = (! empty($data['startingMinute'])) ? $data['startingMinute'] : null;
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
                'name' => 'poiList',
                'required' => true
            ));
            
            $inputFilter->add(array(
                'name' => 'startingMinute',
                'required' => true
            ));
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

