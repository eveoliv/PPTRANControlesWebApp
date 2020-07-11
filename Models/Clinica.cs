using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Clinica
    {
        [DisplayName("ID")]
        public long? ClinicaId { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        [DisplayName("Clinica")]
        public string Alias { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string Tel1 { get; set; }
       
        [DisplayName("Telefone")]
        public string Tel2 { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }

        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        public string CpfUser { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<Colaborador> Colaborador { get; set; }
    }
}
