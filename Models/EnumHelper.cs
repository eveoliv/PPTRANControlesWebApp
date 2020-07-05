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

        public enum Clinica
        {
            Clinica1 = 1,
            Clinica2
        };

        public enum Tipo
        {
            Credito = 1,
            Debito
        };

        public enum FormaPgto
        {
            Cartao = 1,            
            Dinheiro
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
    }
}
