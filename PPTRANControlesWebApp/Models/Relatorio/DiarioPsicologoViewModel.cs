using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PPTRANControlesWebApp.Models.Relatorio
{
    public class DiarioPsicologoViewModel
    {
        public long? Id { get; set; }
        public long? ClinicaId { get; set; }
        [DisplayName("Psicologo")]
        public string Nome { get; set; }
        [DisplayName("Candidato")]
        public string Cliente { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
    }
}
