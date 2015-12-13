using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoCeHealthLive.Model
{
    class DocumentReference
    {
        public string UUID { get; set; }
        public DocumentReference(string uuid)
        {
            this.UUID = uuid;
        }
    }
}
