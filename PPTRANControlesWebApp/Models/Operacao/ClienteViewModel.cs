using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Models
{
    public class ClienteViewModel
    {
        public Cliente Cliente { get; set; }
        public Endereco Endereco { get; set; }
        public Entrevista Entrevista { get; set; }
        public Clinica Clinica { get; set; }
    }
}
