<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\InputFilter;

class Hashtag
{

    public $hashtagId;

    public $text;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->hashtagId = (! empty($data['hashtagId'])) ? $data['hashtagId'] : null;
        $this->text = (! empty($data['text'])) ? $data['text'] : null;
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
                'name' => 'text',
                'required' => true,
                'filters' => array(
                    array(
                        'name' => 'StripTags'
                    ),
                    array(
                        'name' => 'StringTrim'
                    )
                ),
                'validators' => array(
                    array(
                        'name' => 'StringLength',
                        'options' => array(
                            'encoding' => 'UTF-8',
                            'min' => 3,
                            'max' => 50
                        )
                    )
                )
            ));
        }
        return $this->inputFilter;
    }
}

