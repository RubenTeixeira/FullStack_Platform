<?php
namespace PortoVisitas\DTO;

class PassaPorPoisPercursoRequestDTO extends PercursoRequestDTO
{

    /*
     * List of int (id)
     */
    public $poiList;
    

    public function exchangeArray($data)
    {
        parent::exchangeArray($data);
        if (! empty($data['poiList'])) {
            $pois = array();
            foreach ($data['poiList'] as $poi)
                $pois[] = intval($poi);
            $this->poiList = $pois;
        } else {
            $this->poiList = null;
        }
        
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
            
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

