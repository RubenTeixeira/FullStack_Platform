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
     * String (pe/carro/autocarro/tuk)
     */
    public $tipoVeiculo;

    /*
     * Int
     */
    public $kilometrosMax;

    /*
     * Time string (HH:mm)
     */
    public $horaInicialVisita;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->poiOrigem = (! empty($data['poiOrigem'])) ? intval($data['poiOrigem']) : null;
        $this->inclinacaoMax = (! empty($data['inclinacaoMax'])) ? intval($data['inclinacaoMax']) : null;
        $this->tipoVeiculo = (! empty($data['tipoVeiculo'])) ? $data['tipoVeiculo'] : null;
        $this->kilometrosMax = (! empty($data['kilometrosMax'])) ? intval($data['kilometrosMax']) : null;
        $this->horaInicialVisita = (! empty($data['horaInicialVisita'])) ? $data['horaInicialVisita'] : null;
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
            'name' => 'tipoVeiculo',
            'required' => true
        ));
        
        $inputFilter->add(array(
            'name' => 'kilometrosMax',
            'required' => true
        ));
        
        $inputFilter->add(array(
            'name' => 'horaInicialVisita',
            'required' => true
        ));
        
        return $inputFilter;
    }
}

