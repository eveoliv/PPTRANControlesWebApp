﻿using Models;
using System;
using System.Linq;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ContextInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            /********************************* ENDERECOS *************************************************/
            if (context.Enderecos.Any())
            {
                return;
            }

            var enderecos = new Endereco[]
            {
                //clinicas
                new Endereco{ Id = 1, Cep="01019-020", Rua="Rua do Carmo", Bairro="Sé", Cidade="São Paulo", Estado="SP", Numero=44, Complto="1° ANDAR"},
                new Endereco{ Id = 2, Cep="02034-030", Rua="Rua Darzan", Bairro="Santana", Cidade="São Paulo", Estado="SP", Numero=144, Complto=""},
            };

            foreach (Endereco e in enderecos)
            {
                context.Enderecos.Add(e);
            }
            context.SaveChanges();

            /*********************************** CLINICAS ************************************************/           
            if (context.Clinicas.Any())
            {
                return;
            }

            var clinicas = new Clinica[]
            {
                new Clinica{
                    Id = 1,
                    Nome ="POUPETRAN SÉ",
                    CNPJ ="12.321.654/6565-46",
                    Alias ="SÉ", Email="se@poupetran.com.br",
                    Telefone1 ="(11) 3115-0442",
                    Telefone2 ="(11) 97496-2877",
                    Status =EnumHelper.Status.Ativo,
                    Endereco = new Endereco() { Id = 1}
                },
                new Clinica{Id = 2,
                    Nome ="POUPETRAN DARZAN",
                    CNPJ ="64.646.546/0001-11",
                    Alias ="DARZAN",
                    Email ="darzan@poupetran.com.br",
                    Telefone1 ="(11) 2768-7708",
                    Telefone2 ="",
                    Status =EnumHelper.Status.Ativo,
                    Endereco = new Endereco(){ Id = 2} }
            };

            foreach (Clinica c in clinicas)
            {
                context.Clinicas.Add(c);
            }
            context.SaveChanges();

            /************************************ HISTORICO **********************************************/
            if (context.Historicos.Any())
            {
                return;
            }

            var historico = new Historico[]
            {
                new Historico{ Nome = "Valor Fracionado Exame", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Renovação Ex.", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "2ª Via CNH", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Inscrição CNH", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Licença Estr.", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Registro CNH", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Baixa Condutor", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Alteração/Inclusão de Dados", Status = EnumHelper.Status.Ativo },
                new Historico{ Nome = "Laudo Pessoa com Deficiência", Status = EnumHelper.Status.Ativo }
            };

            foreach (Historico h in historico)
            {
                context.Historicos.Add(h);
            }
            context.SaveChanges();

            /*********************************** CLIENTES ************************************************/
            //if (context.Clientes.Any())
            //{
            //    return;
            //}

            //var clientes = new Cliente[]
            //{
            //    new Cliente{
            //        Id = 1,
            //        DtCadastro = DateTime.Today,
            //        Nome = "Everton Oliveira",
            //        NumRenach = "123456",
            //        Categoria = EnumHelper.Categoria.B,
            //        Telefone1 = "(11)97699-4991",                     
            //        Status = EnumHelper.Status.Ativo,
            //        Clinica = new Clinica(){ Id = 1 },
            //        CPF = "222.205.648-90",
            //        Historico = new Historico(){ Id = 1 },
            //        PsicologoId = 2,
            //        MedicoId = 3,
            //        Endereco =  new Endereco(){ Id = 3 }
            //    }
            //};

            //foreach (Cliente c in clientes)
            //{
            //    context.Clientes.Add(c);
            //}
            //context.SaveChanges();

            /************************************ COLABORADORES ******************************************/
            if (context.Colaboradores.Any())
            {
                return;
            }

            var colaborador = new Colaborador[]
            {
                new Colaborador{
                    Id =1,
                    DtCadastro = DateTime.Today,
                    Nome = "Administrador",
                    CPF = "222.205.648-91",
                    Funcao = EnumHelper.Funcao.Administrador,
                    Email = "Admin@email",
                    Status = EnumHelper.Status.Ativo,
                    Clinica = new Clinica(){Id = 1},
                    Endereco = new Endereco(){Id = 1}
                }

                // new Colaborador{
                //    Id =2,
                //    Nome = "Barbara Hansen",
                //    CPF = "222.205.648-92",
                //    Funcao = EnumHelper.Funcao.Psicologo,
                //    Status = EnumHelper.Status.Ativo,
                //    Clinica = new Clinica(){Id = 1},
                //    Endereco = new Endereco(){Id = 5}
                // },

                //  new Colaborador{
                //    Id =3,
                //    Nome = "Doutor Estranho",
                //    CPF = "222.205.648-93",
                //    Funcao = EnumHelper.Funcao.Medico,
                //    Status = EnumHelper.Status.Ativo,
                //    Clinica = new Clinica(){Id = 1},
                //    Endereco = new Endereco(){Id = 6}
                //  },

                //  new Colaborador{
                //    Id =4,
                //    Nome = "Everton Oliveira",
                //    CPF = "222.205.648-94",
                //    Funcao = EnumHelper.Funcao.Operador,
                //    Status = EnumHelper.Status.Ativo,
                //    Clinica = new Clinica(){Id = 1},
                //    Endereco = new Endereco(){Id = 7}
                //},
            };         

            foreach (Colaborador c in colaborador)
            {
                context.Colaboradores.Add(c);
            }
            context.SaveChanges();

            /************************************ PRODUTOS ***********************************************/
            if (context.Produtos.Any())
            {
                return;
            }

            var produto = new Produto[]
            {
                new Produto{ Nome = "Exame Médico", Valor = 91.11M, Status = EnumHelper.Status.Ativo },
                new Produto{ Nome = "Exame Psicotécnico", Valor = 106.30M, Status = EnumHelper.Status.Ativo },
                new Produto{ Nome = "Exame Médico com Psicotécnico", Valor = 197.41M, Status = EnumHelper.Status.Ativo },
                new Produto{ Nome = "Laudo Pessoa com Deficiência", Valor = 600.00M, Status = EnumHelper.Status.Ativo },
                new Produto{ Nome = "Exame Médico Pessoa com Deficiência", Valor = 66.82M, Status = EnumHelper.Status.Ativo },
                new Produto{ Nome = "Outros", Valor = 0M, Status = EnumHelper.Status.Ativo }
            };

            foreach (Produto c in produto)
            {
                context.Produtos.Add(c);
            }
            context.SaveChanges();        
          
        }
    }
}

