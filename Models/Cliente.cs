using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models
{
    public class Cliente
    {       
        [DisplayName("ID")]
        public long? ClienteId { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }

        [DisplayName("CNH")]
        public string CNH { get; set; }

        [DisplayName("Categoria")]
        public string Categoria { get; set; }

        [DisplayName("Numero do Renach")]
        public string NumRenach { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Perfil Psicologico")] 
        public string PerfilPsicologico { get; set; }

        [DisplayName("Numero do Laudo")]
        public string NumLaudo { get; set; }

        [DisplayName("Data 1ª Habilitacao")]
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
        public DateTime DtNascimento { get; set; }

        [DisplayName("Profissão")]
        public string Profissao { get; set; }

        [DisplayName("Escolaridade")]
        public string Escolaridade { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

    }
}
