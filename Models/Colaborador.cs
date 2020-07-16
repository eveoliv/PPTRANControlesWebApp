using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Colaborador : BaseModel
    {
        public Colaborador() { }      

        [DisplayName("Data de Início")]       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DtCadastro { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("CRP")]
        public string CRP { get; set; }

        [DisplayName("CRM")]
        public string CRM { get; set; }
    
        [DisplayName("Telefone 1")]
        public string Telefone1 { get; set; }

        [DisplayName("Telefone 2")]
        public string Telefone2 { get; set; }

        [DisplayName("Função")]
        public EnumHelper.Funcao Funcao { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }
     
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }
       
        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
