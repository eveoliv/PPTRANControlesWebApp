using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EnumHelper
    {
        public enum OptForm
        {
            SIM, NAO,
            MUITO, POUCO,
            GRAVE, NGRAVE, FREQUENTE,
            SOCIAL, BOM, REGULAR, RUIM,
            DESTRO, CANHOTO, AMBIDESTRO
        };      

        public enum Tipo
        {
            Credito, Debito
        };

        public enum FormaPgto
        {
            Cartao, Dinheiro, Cheque
        };

        public enum Categoria
        {
            A, B, C, D, E, AB, AC, AD, AE
        };

        public enum Status
        {
            Ativo, Inativo    
        };       

        public enum Funcao
        {
            Administrador, Atendente, Gerente, Medico, Psicologo           
        }
    }
}
