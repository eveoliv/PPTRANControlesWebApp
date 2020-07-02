using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Colaborador
    {
        [DisplayName("ID")]
        public long? ColaboradorId { get; set; }

        [DisplayName("Data de Início")]       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DtCadastro { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("CRP")]
        public string CRP { get; set; }

        [DisplayName("CRM")]
        public string CRM { get; set; }
    
        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Função")]
        public string Funcao { get; set; }
        //public EnumHelper.Clinica Clinica { get; set; }

        public EnumHelper.Status Status { get; set; }

        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

    }
}
