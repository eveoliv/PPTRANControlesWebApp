
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
            Dinheiro, Cartao, Cheque, Tansferencia, Selecionar
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

        public enum UF
        {
            SP, RJ, AC, AM, RR, PA, AP, TO, MA, PI, CE, RN, PB, PE,
            AL, SE, BA, MG, ES, RO, PR, SC, RS, MS, MT, GO, DF
        }

        public enum EstadoCivil
        {
            Solteiro_a,
            Casado_a,
            Divorciado_a,
            Viuvo_a,
            Separado_a,
            Uniao_Estavel
        }

        public enum Escolaridade
        {
            Fundamental,
            Medio,
            Superior,
            Pos_graduacao,
            Mestrado,
            Doutorado
        }
    }
}
