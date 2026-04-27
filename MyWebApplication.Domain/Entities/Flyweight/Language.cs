using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Domain.Entities.Flyweight
{
    public class Language //obiect reutilizaubil 
    {
        public string Code { get; private set; }

        public Language(string code)
        {
            Code = code; //en/fr/
        }
    }
}