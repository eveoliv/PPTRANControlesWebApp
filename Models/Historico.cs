using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Historico : BaseModel
    {
        [DisplayName("Nome Historico")]
        public string Nome { get; set; }
    }
}
