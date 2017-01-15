<?php
namespace PortoVisitas\Model;

class Visita
{
    public $visitaID;
    public $creator;
    public $date;
    public $percurso;
    protected $inputFilter;
    
    
    public function exchangeArray($data)
    {
        $this->visitaID = (! empty($data['visitaID'])) ? $data['visitaID'] : null;
        $this->creator = (! empty($data['creator'])) ? $data['creator'] : null;
        $this->date = (! empty($data['date']))  ? $data['date'] : null;
        $this->percurso = (! empty($data['percurso'])) ? $data['percurso'] : null;
    }
    
    public function exchangeDTO($data)
    {
        $this->visitaID = (! empty($data['VisitaID'])) ? $data['VisitaID'] : null;
        $this->creator = (! empty($data['Creator'])) ? $data['Creator'] : null;
        $this->date = (! empty($data['Date']))  ? $data['Date'] : null;
        $this->percurso = (! empty($data['Percurso'])) ? $data['Percurso'] : null;
    }
}

