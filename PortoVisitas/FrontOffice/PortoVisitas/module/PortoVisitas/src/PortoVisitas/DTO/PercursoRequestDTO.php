<?php
namespace PortoVisitas\DTO;

use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

abstract class PercursoRequestDTO
{

    /*
     * Int
     */
    public $poiOrigem;

    /*
     * Int
     */
    public $inclinacaoMax;

    /*
     * String
     */
    public $tipoVeiculo;

    /*
     * Int
     */
    public $kilometrosMax;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->poiOrigem = (! empty($data['poiOrigem'])) ? $data['poiOrigem'] : null;
        $this->inclinacaoMax = (! empty($data['inclinacaoMax'])) ? $data['inclinacaoMax'] : null;
        $this->tipoVeiculo = (! empty($data['tipoVeiculo'])) ? $data['tipoVeiculo'] : null;
        $this->kilometrosMax = (! empty($data['kilometrosMax'])) ? $data['kilometrosMax'] : null;
    }

    public abstract function getArrayCopy();

    public function setInputFilter(InputFilterInterface $inputFilter)
    {
        throw new \Exception("Not used");
    }

    public function getInputFilter()
    {
        $inputFilter = new InputFilter();
        
        $inputFilter->add(array(
            'name' => 'poiOrigem',
            'required' => true
        ));
        
        $inputFilter->add(array(
            'name' => 'inclinacaoMax',
            'required' => true
        ));
        
        $inputFilter->add(array(
            'name' => 'inclinacaoMax',
            'required' => true
        ));
        
        return $inputFilter;
    }
}

