using System;

namespace Models
{
    public class DataBuilder
    {
        public static string PtBr_Data()
        {
            var mes = DateTime.Now.Month;
            string nomeMes = null;

            if (mes == 1)
                nomeMes = "Janeiro";

            if (mes == 2)
                nomeMes = "Fevereiro";

            if (mes == 3)
                nomeMes = "Março";

            if (mes == 4)
                nomeMes = "Abril";

            if (mes == 5)
                nomeMes = "Maio";

            if (mes == 6)
                nomeMes = "Junho";

            if (mes == 7)
                nomeMes = "Julho";

            if (mes == 8)
                nomeMes = "Agosto";

            if (mes == 9)
                nomeMes = "Setembro";

            if (mes == 10)
                nomeMes = "Outubro";

            if (mes == 11)
                nomeMes = "Novembro";

            if (mes == 12)
                nomeMes = "Dezembro";

            string dataPorExtenso 
                = $"São Paulo, {DateTime.Now.Day} de {nomeMes} de {DateTime.Now.Year}.";

            return dataPorExtenso;
        }
    }
}
