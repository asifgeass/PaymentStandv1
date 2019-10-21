using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML
{
    public class AttrRecord
    {
        private float Code { get; set; }
        private string Name { get; set; }
        private string Value { get; set; } = string.Empty;
        private short View { get; set; } = 1;
        private short Edit { get; set; }
        private short Change { get; set; }
        private float CodeOut { get; set; }
        private string Type { get; set; }
        private string Mandatory { get; set; }
        private string MinLength { get; set; }
        private string MaxLength { get; set; }
        private string Min { get; set; }
        private string Max { get; set; }
        private Lookup Lookup { get; set; }
        private string Hint { get; set; }
        private string Print { get; set; }
        private string Format { get; set; }
    }
}
