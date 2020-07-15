using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Cliente : BaseModel
    {              
        public Cliente() { }

        [DisplayName("Data Cadastro")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DtCadastro { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }

        [DisplayName("CNH")]
        public string CNH { get; set; }

        [DisplayName("Categoria")]
        public EnumHelper.Categoria Categoria { get; set; }

        [DisplayName("Numero do Renach")]
        public string NumRenach { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }
    
        [DisplayName("Numero do Laudo")]
        public string NumLaudo { get; set; }

        [DisplayName("Data 1ª Habilitacao")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DtHabHum { get; set; }

        [DisplayName("Nome do Pai")]
        public string Pai { get; set; }

        [DisplayName("Nome da Mãe")]
        public string Mae { get; set; }     

        [DisplayName("Nacionalidade")]
        public string Nacionalidade { get; set; }

        [DisplayName("Naturalidade")]
        public string Naturalidade { get; set; }

        [DisplayName("Estado Civil")]
        public string EstadoCivil { get; set; }

        [DisplayName("Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DtNascimento { get; set; }

        [DisplayName("Profissão")]
        public string Profissao { get; set; }

        [DisplayName("Escolaridade")]
        public string Escolaridade { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Status Cliente")]
        public EnumHelper.Status Status { get; set; }

        [DisplayName("Pagto Realizado")]
        public EnumHelper.YesNo StatusPgto { get; set; }

        [DisplayName("Medico")]
        public long? MedicoId { get; set; }       

        [DisplayName("Psicologo")]
        public long? PsicologoId { get; set; }       

        [DisplayName("Clinica")]
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        [DisplayName("Historico")]
        public long? HistoricoId { get; set; }
        public Historico Historico { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
