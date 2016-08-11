using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUERION.Common.Helper
{
    public class ResponseData<T>
    {
        public bool Status { get; set; }
        public T Model { get; set; }
        public string Errormessage { get; set; }
    }
}
