﻿
namespace Models
{
    public class EnumHelper
    {
        public enum YesNo
        {
            Não, Sim
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
