using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ContextInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Enderecos.Any())
            {
                return;
            }

            /************************ Endereço clinica *********************************/
            var enderecos = new Endereco[]
            {
                new Endereco{EnderecoId=1, CPF="12.321.654/6565-46", Cep="01019-020", Rua="Rua do Carmo", Bairro="Sé", Cidade="São Paulo", Estado="SP", Numero=44, Complto="1° ANDAR"},
                new Endereco{EnderecoId=2, CPF="64.646.546/0001-11", Cep="02034-030", Rua="Rua Darzan", Bairro="Santana", Cidade="São Paulo", Estado="SP", Numero=144, Complto=""},
                new Endereco{EnderecoId=3, CPF="222.245.678-97", Cep="01319-001",Rua="Maria Paula"},
                new Endereco{EnderecoId=4, CPF="222.245.678-97", Cep="01319-001",Rua="Maria Paula"}
            };

            foreach (Endereco e in enderecos)
            {
                context.Enderecos.Add(e);
            }
            context.SaveChanges();
            /**********************************************************************************/

            /************************ Endereço clinica *********************************/
            if (context.Clinicas.Any())
            {
                return;
            }

            var clinicas = new Clinica[]
            {
                new Clinica{ClinicaId=1, Nome="POUPETRAN SÉ", CNPJ="12.321.654/6565-46", Alias="SÉ", Email="se@poupetran.com.br", Tel1="(11) 64645-6546", Tel2="(11) 97997-9879", Status=EnumHelper.Status.Ativo, EnderecoId=1},
                new Clinica{ClinicaId=2, Nome="POUPETRAN DARZAN", CNPJ="64.646.546/0001-11", Alias="DARZAN", Email="darzan@poupetran.com.br", Tel1="(45) 45454-5454", Tel2="(65) 66565-6565", Status=EnumHelper.Status.Ativo, EnderecoId=2}
            };

            foreach (Clinica c in clinicas)
            {
                context.Add(c);
                context.SaveChanges();
            }
            /**********************************************************************************/
            if (context.Clientes.Any())
            {
                return;
            }

            var clientes = new Cliente[]
            {
                new Cliente{
                    ClienteId = 1,
                    DtCadastro = DateTime.Today,
                    Nome = "Everton Oliveira",
                    NumRenach = "123456",
                    Categoria = EnumHelper.Categoria.B,
                    Telefone = "(11)7699-4991",
                    ClinicaId = 1,
                    CPF = "222.245.678-97",
                    EnderecoId = 3}
            };

            foreach (Cliente c in clientes)
            {
                context.Add(c);
                context.SaveChanges();
            }

            /**********************************************************************************/
            if (context.Colaboradores.Any())
            {
                return;
            }

            var colaborador = new Colaborador[]
            {
                new Colaborador{
                    ColaboradorId =1,
                    Nome = "Everton Oliveira",
                    CPF = "222.245.678-97",
                    Funcao = "Analista",
                    ClinicaId = 1,
                    EnderecoId = 4}
            };

            foreach (Colaborador c in colaborador)
            {
                context.Add(c);
                context.SaveChanges();
            }

            /**********************************************************************************/
            if (context.Agendas.Any())
            {
                return;
            }

            if (context.Caixas.Any())
            {
                return;
            }
        }
    }
}
