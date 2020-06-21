using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Colaborador
    {
        public long? ColaboradorId { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string CRP { get; set; }
        public string CRM { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Funcao { get; set; }
        public EnumHelper.Clinica Clinica { get; set; }

    }
}
