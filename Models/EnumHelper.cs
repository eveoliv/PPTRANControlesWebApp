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

        public enum Lancamento
        {
            Credito = 1,
            Debito
        };

        public enum Tipo
        {
            Cartao = 1,
            Cheque,
            Dinheiro
        };
    }
}
