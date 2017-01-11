<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilter;
use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\Input;
use Zend\Validator;

class User
{

    public $email;

    public $password;
    
    private $role = "User";

    protected $inputFilter;

    public function exchangeArray($data)
    {
        $this->email = (! empty($data['email'])) ? $data['email'] : null;
        $this->password = (! empty($data['password'])) ? $data['password'] : null;
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
            
            $email = new Input('email');
            $email->getValidatorChain()->attach(new Validator\EmailAddress());
            $inputFilter->add($email);
            
            $inputFilter->add(array(
                'name' => 'password',
                'required' => true,
                'filters' => array(
                    array('name' => 'StripTags'),
                    array('name' => 'StringTrim')
                ),
                'validators' => array(
                    array(
                        'name' => 'StringLength',
                        'options' => array(
                            'encoding' => 'UTF-8',
                            'min' => 3,
                            'max' => 32
                        )
                    )
                )
            ));
            
            $inputFilter->add(array(
                'name' => 'passwordConfirm',
                'required' => true,
                'filters' => array(
                    array('name' => 'StripTags'),
                    array('name' => 'StringTrim')
                ),
                'validators' => array(
                    array(
                        'name' => 'StringLength',
                        'options' => array(
                            'encoding' => 'UTF-8',
                            'min' => 3,
                            'max' => 32
                        ),
                    ),
                    array(
                        'name' => 'Identical',
                        'options' => array(
                            'token' => 'password'
                        )
                    )
                )
            ));
            
            
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

