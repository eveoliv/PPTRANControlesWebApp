using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Agenda
    {
        [DisplayName("ID")]
        public long? AgendaId { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteId { get; set; }

        [DisplayName("Psicologo")]
        public long? PsicologoId { get; set; }

        [DisplayName("Medico")]
        public long? MedicoId { get; set; }
    }
}
