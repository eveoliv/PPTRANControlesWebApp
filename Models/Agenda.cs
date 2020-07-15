﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Agenda : BaseModel
    {
        public Agenda() { }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [DisplayName("Clinica")]
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        [DisplayName("Medico")]
        public long? MedicoId { get; set; }

        [DisplayName("Psicologo")]
        public long? PsicologoId { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
