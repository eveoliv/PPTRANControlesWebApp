using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Clinica : BaseModel
    {        
        public Clinica() { }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        [DisplayName("Clinica")]
        public string Alias { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Tel. Celular")]
        public string Telefone1 { get; set; }
       
        [DisplayName("Tel. Fixo")]
        public string Telefone2 { get; set; }

        [DisplayName("Observação")]
        public string Obs { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }

        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
     
        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<Colaborador> Colaborador { get; set; }
    }
}
