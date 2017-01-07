<?php
namespace PortoVisitas\Model;

use Zend\InputFilter\InputFilter;
use Zend\InputFilter\InputFilterInterface;
use Zend\InputFilter\Input;
use Zend\Validator;

class User
{

    //public $id;

    public $email;

    public $password;

    protected $inputFilter;

    public function exchangeArray($data)
    {
        //$this->id = (! empty($data['id'])) ? $data['id'] : null;
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
            
            $password = new Input('password');
            $password->getValidatorChain()->attach(new Validator\StringLength(5));
            
            $inputFilter->add($email)->add($password);
            $this->inputFilter = $inputFilter;
        }
        return $this->inputFilter;
    }
}

