﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Endereco
    {
        //pk
        public long EnderecoId { get; set; }
    
        [DisplayName("CEP")]
        public string Cep { get; set; }

        [DisplayName("Rua")]
        public string Rua { get; set; }

        [DisplayName("Bairro")]
        public string Bairro { get; set; }

        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Número")]
        public long? Numero { get; set; }

        [DisplayName("Complemento")]
        public string Complto { get; set; }

        //fk
        public virtual IEnumerable<Cliente> Clientes { get; set; }

    }
}