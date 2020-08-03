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
            Dinheiro, Cartao, Cheque, Selecionar
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
            Administrador, Operador, Gestor, Medico, Psicologo
        }
    }
}
