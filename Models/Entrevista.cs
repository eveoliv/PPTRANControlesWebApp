using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Entrevista
    {
        [DisplayName("EntrevistaId")]
        public long? EntrevistaId { get; set; }

        [DisplayName("CPF Entrevistado")]
        public string CPF { get; set; }

        [DisplayName("Fumante?")]
        public EnumHelper.OptForm Tabagismo { get; set; }

        [DisplayName("Frequência.")]
        public string TabagismoObs { get; set; }

        [DisplayName("Consome Café Puro?")]
        public EnumHelper.OptForm Cafe { get; set; }

        [DisplayName("Consumo de bebida alcoólica?")]
        public EnumHelper.OptForm Bebida { get; set; }

        [DisplayName("Utiliza algum tipo de droga?")]
        public EnumHelper.OptForm Drogas { get; set; }

        [DisplayName("Caso utilize, informar qual tipo.")]
        public string DrogasObs { get; set; }

        [DisplayName("Utiliza medicamentos?")]
        public EnumHelper.OptForm Medicamento { get; set; }

        [DisplayName("Caso utilize, informar qual medicamento.")]
        public string MedicamentoObs { get; set; }

        [DisplayName("Internações ou cirurgias recentes?")]
        public EnumHelper.OptForm ProcMedido { get; set; }

        [DisplayName("Caso positivo informar tipo.")]
        public string ProcMedicoObs { get; set; }

        [DisplayName("Esta sob tratamento psiquiatrico?")]
        public EnumHelper.OptForm TratPsicologico { get; set; }

        [DisplayName("Sofreu algum tipo de acidente?")]
        public EnumHelper.OptForm Acidente { get; set; }

        [DisplayName("Sente dores de cabeça?")]
        public EnumHelper.OptForm DorCabeca { get; set; }

        [DisplayName("Sente tontura?")]
        public EnumHelper.OptForm Tonturas { get; set; }

        [DisplayName("Sofre de insônia?")]
        public EnumHelper.OptForm Insonia { get; set; }

        [DisplayName("Sofre de desmaios?")]
        public EnumHelper.OptForm Desmaios { get; set; }

        [DisplayName("Tem Diabetes, epilepsia, doença cardíaca, doença neurológica, doença pulmonar ou outra?")]
        public EnumHelper.OptForm Doencas { get; set; }

        [DisplayName("Especificar qual doença")]
        public string DoencasObs { get; set; }

        [DisplayName("Como é sua alimentação?")]
        public EnumHelper.OptForm Alimentacao { get; set; }

        [DisplayName("Qual é sua mão dominante?")]
        public EnumHelper.OptForm Dominante { get; set; }

        [DisplayName("Como é seu relacionamento familiar?")]
        public EnumHelper.OptForm RelFamiliar { get; set; }

        [DisplayName("Pessoa com deficiência física?")]
        public EnumHelper.OptForm Pcd { get; set; }

        [DisplayName("Caso afirmativo, informar qual deficiência.")]
        public string PcdObs { get; set; }

        [DisplayName("Possui habilitação?")]
        public EnumHelper.OptForm Habilitado { get; set; }

        [DisplayName("Exerce atividade remunerada vinculada a habilitação?")]
        public EnumHelper.OptForm AtividadeRem { get; set; }

        [DisplayName("Declaração")]
        public string Declaracao { get; set; }

        [DisplayName("Parecer")]
        public string Parecer { get; set; }

        [DisplayName("Data da entrevista")]
        public DateTime DataForm { get; set; }

        public long? PsicologoId { get; set; }

        //fk
        public virtual Cliente Cliente { get; set; }

    }
}
