using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data
{
    public class ContextInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            if (context.Cliente.Any())
            {
                return;
            }

            var clientes = new Cliente[]
            {
                new Cliente(){ Id = 1, Nome = "Tiger Nixon", CNH = "123456789", CPF = "98765432199" },
                new Cliente(){ Id = 2, Nome = "Garrett Winters", CNH = "111222333",CPF = "22223333444" },
                new Cliente(){ Id = 3, Nome = "Ashton Cox", CNH = "1112228883", CPF = "22223666644" },
                new Cliente(){ Id = 4, Nome = "Cedric Kelly", CNH = "1112228883", CPF = "22223666644" },
                new Cliente(){ Id = 5, Nome = "Airi Satou", CNH = "1112228883", CPF = "22223666644" }
            };

            foreach (Cliente c in clientes)
            {
                context.Cliente.Add(c);
            }
            context.SaveChanges();
        }
    }
}
