using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Historico : BaseModel
    {
        public Historico() { }

        [DisplayName("Histórico")]
        public string Nome { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
