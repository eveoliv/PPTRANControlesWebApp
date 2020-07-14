using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ContextInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            /********************************* Endereços ************************************************/
            if (context.Enderecos.Any())
            {
                return;
            }

            var enderecos = new Endereco[]
            {
                //clinicas
                new Endereco{ Id=1, CPF="12.321.654/6565-46", Cep="01019-020", Rua="Rua do Carmo", Bairro="Sé", Cidade="São Paulo", Estado="SP", Numero=44, Complto="1° ANDAR"},
                new Endereco{ Id=2, CPF="64.646.546/0001-11", Cep="02034-030", Rua="Rua Darzan", Bairro="Santana", Cidade="São Paulo", Estado="SP", Numero=144, Complto=""},

                //cliente
                new Endereco{ Id=3, CPF="222.205.648-90", Cep="01319-001",Rua="Maria Paula"},
                //atendente
                new Endereco{ Id=4, CPF="222.205.648-91", Cep="01319-001",Rua="Maria Paula"},
                //psicologo
                new Endereco{ Id=5, CPF="222.205.648-92", Cep="01319-001",Rua="Maria Paula"},
                //medico
                new Endereco{ Id=6, CPF="222.205.648-93", Cep="01319-001",Rua="Maria Paula"}
            };

            foreach (Endereco e in enderecos)
            {
                context.Enderecos.Add(e);
            }
            context.SaveChanges();

            /********************************** CLINICAS ************************************************/           
            if (context.Clinicas.Any())
            {
                return;
            }

            var clinicas = new Clinica[]
            {
                new Clinica{Id=1, Nome="POUPETRAN SÉ", CNPJ="12.321.654/6565-46", Alias="SÉ", Email="se@poupetran.com.br", Tel1="(11) 64645-6546", Tel2="(11) 97997-9879", Status=EnumHelper.Status.Ativo, EnderecoId=1},
                new Clinica{Id=2, Nome="POUPETRAN DARZAN", CNPJ="64.646.546/0001-11", Alias="DARZAN", Email="darzan@poupetran.com.br", Tel1="(45) 45454-5454", Tel2="(65) 66565-6565", Status=EnumHelper.Status.Ativo, EnderecoId=2}
            };

            foreach (Clinica c in clinicas)
            {
                context.Add(c);
            }
            context.SaveChanges();

            /********************************** CLIENTES ************************************************/
            if (context.Clientes.Any())
            {
                return;
            }

            var clientes = new Cliente[]
            {
                new Cliente{
                    Id = 1,
                    DtCadastro = DateTime.Today,
                    Nome = "Everton Oliveira",
                    NumRenach = "123456",
                    Categoria = EnumHelper.Categoria.B,
                    Telefone = "(11)97699-4991",
                    ClinicaId = 1,
                    CPF = "222.205.648-90",
                    PsicologoId = 2,
                    MedicoId = 3,
                    EnderecoId = 3}
            };

            foreach (Cliente c in clientes)
            {
                context.Add(c);
            }
            context.SaveChanges();

            /*********************************** COLABORADORES ******************************************/
            if (context.Colaboradores.Any())
            {
                return;
            }

            var colaborador = new Colaborador[]
            {
                new Colaborador{
                    Id =1,
                    Nome = "Everton Oliveira",
                    CPF = "222.205.648-91",
                    Funcao = EnumHelper.Funcao.Atendente,
                    ClinicaId = 1,
                    EnderecoId = 4 },

                 new Colaborador{
                    Id =2,
                    Nome = "Barbara Hansen",
                    CPF = "222.205.648-92",
                    Funcao = EnumHelper.Funcao.Psicologo,
                    ClinicaId = 1,
                    EnderecoId = 5 },

                  new Colaborador{
                    Id =3,
                    Nome = "Doutor Estranho",
                    CPF = "222.205.648-93",
                    Funcao = EnumHelper.Funcao.Medico,
                    ClinicaId = 1,
                    EnderecoId = 6}
            };          

            foreach (Colaborador c in colaborador)
            {
                context.Add(c);
            }
            context.SaveChanges();

            /************************************ AGENDAS ***********************************************/
            if (context.Agendas.Any())
            {
                return;
            }

            /************************************ CAIXAS ************************************************/
            if (context.Caixas.Any())
            {
                return;
            }

            /************************************ PRODUTOS ********************************************/
            if (context.Produtos.Any())
            {
                return;
            }

            var produto = new Produto[]
            {
                new Produto{ Nome = "Exame Médico", Valor = 91.11M},
                new Produto{ Nome = "Exame Psicotécnico", Valor = 106.30M},
                new Produto{ Nome = "Laudo Pessoa com Deficiência", Valor = 600.00M},
                new Produto{ Nome = "Exame Médico Pessoa com Deficiência", Valor = 66.82M}
            };

            foreach (Produto c in produto)
            {
                context.Add(c);
            }
            context.SaveChanges();

            /************************************ PRODUTOS ********************************************/
            if (context.Historicos.Any())
            {
                return;
            }

            var historico = new Historico[]
            {
                new Historico{ Nome = "Inscrição CNH"},
                new Historico{ Nome = "Renovação Ex."},
                new Historico{ Nome = "2ª Via CNH"},
                new Historico{ Nome = "Mudança/Adição"},
                new Historico{ Nome = "Licença Estr."},
                new Historico{ Nome = "Registro CNH"},
                new Historico{ Nome = "Baixa Condutor"},
                new Historico{ Nome = "Alteração de Dados"},
                new Historico{ Nome = "Laudo Pessoa com Deficiência"}                                
            };

            foreach (Historico i in historico)
            {
                context.Add(i);
            }
            context.SaveChanges();
        }
    }
}

