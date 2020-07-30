﻿using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PPTRANControlesWebApp.Models.Usuario
{
    public class UsuarioEditViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public List<UsuarioFuncaoViewModel> Funcoes { get; set; }

        public UsuarioEditViewModel(AppIdentityUser usuario, RoleManager<IdentityRole> roleManager)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
            UserName = usuario.UserName;

            Funcoes = 
                roleManager.Roles.ToList()
                .Select(funcao => new UsuarioFuncaoViewModel{ Nome = funcao.Name, Id = funcao.Id }).ToList();

            foreach (var funcao in Funcoes)
            {
                //var funcaoUser  = roleManager.GetRoleIdAsync()
            }
        }
    }
}
