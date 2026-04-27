using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyWebApplication.Domain.Entities.Flyweight;

namespace MyWebApplication.BusinessLogic.Flyweight
{
    public class LanguageFactory
    {
        private static Dictionary<string, Language> _languages = new();

        public static Language GetLanguage(string code)
        {
            if (!_languages.ContainsKey(code)) //verfica daca e limba 
            {
                _languages[code] = new Language(code); 
            }

            return _languages[code];
        }
    }
}
