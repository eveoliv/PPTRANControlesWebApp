using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Models
{
    public class CaixaViewModel
    {
        public Caixa Caixa { get; set; }       
        public Cliente Cliente { get; set; }       
        public Clinica Clinica { get; set; }
        public Colaborador Colaborador { get; set; }
        public Produto Produto { get; set; }
    }
}
