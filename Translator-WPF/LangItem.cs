using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_WPF
{
    public class LangItem
    {
        //private string lang_;
        //private string lang_name_;
        public string Lang {  get; set; } = String.Empty;
        public string Lang_Name { get; set; } = String.Empty;

        public LangItem(string lang, string lang_name) {
            Lang = lang;
            Lang_Name = lang_name;
        }
    }
}
