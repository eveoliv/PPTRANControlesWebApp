using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EnumHelper
    {
        public enum OptForm
        {
            SIM = 1, NAO,
            MUITO, POUCO,
            GRAVE, NGRAVE,
            FREQUENTE, SOCIAL,
            BOM, REGULAR, RUIM,
            DESTRO, CANHOTO, AMBIDESTRO
        };      

        public enum Tipo
        {
            Credito = 1,
            Debito
        };

        public enum FormaPgto
        {
            Cartao = 1,            
            Dinheiro,
            Cheque
        };

        public enum Categoria
        {
            A = 1,
            B, C, D, E, AB, AC, AD, AE
        };

        public enum Status
        {
            Ativo,
            Inativo    
        };

        public enum Funcao
        {
            Administrador = 1,
            Atendente,
            Gerente,
            Medico,            
            Psicologo,            
        }
    }
}
