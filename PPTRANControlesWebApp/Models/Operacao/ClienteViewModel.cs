using Models;

namespace PPTRANControlesWebApp.Models
{
    public class ClienteViewModel
    {
        public Clinica Clinica { get; set; }
        public Cliente Cliente { get; set; }
        public Endereco Endereco { get; set; }  
        public Historico Historico { get; set; }
        public Colaborador Colaborador { get; set; }
    }
}
