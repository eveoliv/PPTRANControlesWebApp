using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Models.Usuario
{
    public class UsuarioEditViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }       

        public UsuarioEditViewModel() { }
        public UsuarioEditViewModel(AppIdentityUser usuario,
            RoleManager<IdentityRole> roleManager, IList<string> userRole)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
            UserName = usuario.UserName;
            Role = userRole.FirstOrDefault();           

            Roles = new List<SelectListItem>
            {
                new SelectListItem() { Value = "1", Text = RolesNomes.Administrador },
                new SelectListItem() { Value = "2", Text = RolesNomes.Gestor },
                new SelectListItem() { Value = "3", Text = RolesNomes.Operador },
                new SelectListItem() { Value = "4", Text = RolesNomes.Inativo }
            };                                        
        }
    }
}
