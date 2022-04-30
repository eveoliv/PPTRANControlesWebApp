using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPTRANControlesWebApp.Models.Relatorio
{
    public class SemanalMedicoViewModel
    {
        public long? Id { get; set; }

        public long? ClinicaId { get; set; }

        [DisplayName("Medico")]
        public string Nome { get; set; }

        [DisplayName("Candidato")]
        public string Cliente { get; set; }

        [DisplayName("Data Atendimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataCx { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicio { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataFim { get; set; }
    }
}
