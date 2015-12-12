using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.Model
{
    class FormatCode
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public FormatCode(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }
    }
}
