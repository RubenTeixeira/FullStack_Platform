﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class Algav1DTO
    {
        public int poiOrigem { get; set; }
        public List<int> poiList { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime horaInicialVisita { get; set; }
        public int inclinacaoMax { get; set; }
        public string tipoVeiculo { get; set; }
        public int kilometrosMax { get; set; }
    }
}
