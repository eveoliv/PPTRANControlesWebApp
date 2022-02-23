using PPTRANControlesWebApp.Areas.Identity.Data;
using System;

namespace PPTRANControlesWebApp.Models.Usuario
{
    public class UsuarioViewModel
    {      
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ClinicaId { get; set; }
        public string Pwd { get; set; }

        public UsuarioViewModel() {}
        public UsuarioViewModel(AppIdentityUser usuario)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
            UserName = usuario.UserName;           
        }
    }
}
