using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Agenda : BaseModel
    {
        public Agenda() { }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        [DisplayName("Hora")]
        public string Hora { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Tel. Celular")]
        public string Telefone1 { get; set; }

        [DisplayName("Tel. Fixo")]
        public string Telefone2 { get; set; }
    
        [DisplayName("Medico")]
        public string Medico { get; set; }

        [DisplayName("Psicologo")]
        public string Psicologo { get; set; }
        
        [DisplayName("Clinica")]
        public string Clinica { get; set; }      

        [DisplayName("Observação")]
        public string Obs { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
