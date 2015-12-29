using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.Model
{
    //rename
    class CustomCode
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public CustomCode(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }
    }
}
