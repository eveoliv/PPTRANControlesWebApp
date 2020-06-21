using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Formulario
    {
        public long? FormularioId { get; set; }

        public EnumHelper.OptForm Tabagismo { get; set; }
        public string TabagismoObs { get; set; }  
        public EnumHelper.OptForm Cafe { get; set; }        
        public EnumHelper.OptForm Bebida { get; set; }     
        public EnumHelper.OptForm Drogas { get; set; }
        public string DrogasObs { get; set; }
        public EnumHelper.OptForm Medicamento { get; set; }
        public string MedicamentoObs { get; set; }
        public EnumHelper.OptForm ProcMedido { get; set; }      
        public EnumHelper.OptForm TratPsicologico { get; set; }
        public EnumHelper.OptForm Acidente { get; set; }        
        public EnumHelper.OptForm DorCabeca { get; set; }    
        public EnumHelper.OptForm Tonturas { get; set; }
        public EnumHelper.OptForm Insonia { get; set; }
        public EnumHelper.OptForm Desmaios { get; set; }
        public EnumHelper.OptForm Doencas { get; set; }
        public string DoencasObs { get; set; }        
        public EnumHelper.OptForm Alimentacao { get; set; }
        public EnumHelper.OptForm Dominante { get; set; }
        public EnumHelper.OptForm RelFamiliar { get; set; }
        public EnumHelper.OptForm Pcd { get; set; }
        public string PcdObs { get; set; }              
        public EnumHelper.OptForm Habilitado { get; set; }
        public EnumHelper.OptForm AtividadeRem { get; set; }
        public string Declaracao { get; set; }
        public string Parecer { get; set; }
        public DateTime DataForm { get; set; }
        public long? PsicologoId { get; set; }

    }
}
