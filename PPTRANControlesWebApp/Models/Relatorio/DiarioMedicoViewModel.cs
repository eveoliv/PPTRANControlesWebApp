using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PPTRANControlesWebApp.Models.Relatorio
{
    public class DiarioMedicoViewModel
    {
        public long? Id { get; set; }
        public long? ClinicaId { get; set; }
        [DisplayName("Medico")]
        public string Nome { get; set; }
        [DisplayName("Candidato")]
        public string Cliente { get; set; }
        public DateTime Data { get; set; }       
    }
}
