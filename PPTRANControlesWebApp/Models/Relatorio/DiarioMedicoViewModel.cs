using System;
using System.ComponentModel;

namespace PPTRANControlesWebApp.Models.Relatorio
{
    public class DiarioMedicoViewModel
    {
        public long? Id { get; set; }

        [DisplayName("Medico Nome")]
        public string Nome { get; set; }

        public long? ClinicaId { get; set; }

        [DisplayName("Candidato")]
        public string Cliente { get; set; }

        public DateTime Data { get; set; }       
    }
}
