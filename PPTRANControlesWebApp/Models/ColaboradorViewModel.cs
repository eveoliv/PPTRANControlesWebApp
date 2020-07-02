using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Models
{
    public class ColaboradorViewModel
    {
        public Colaborador Colaborador { get; set; }
        public Endereco Endereco { get; set; }        
        public Clinica Clinica { get; set; }
    }
}
