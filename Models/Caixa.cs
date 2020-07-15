﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Caixa : BaseModel
    {       
        public Caixa() { }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        //cartao, dinheiro
        [DisplayName("Tipo")]
        public EnumHelper.Tipo Tipo { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        //deb, cred
        [DisplayName("Pagamento")]
        public EnumHelper.FormaPgto FormaPgto { get; set; }

        //nome cliente
        [DisplayName("Referência")]
        public string Ref { get; set; }


        [DisplayName("Histórico")]
        public long? HistoricoId { get; set; }
        public Historico Historico { get; set; }

        [DisplayName("Produto")]
        public long? ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }       

        [DisplayName("Clinica")]
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
