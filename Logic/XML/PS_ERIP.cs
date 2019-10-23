using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PS_ERIP
    {

        private GetPayListResponse getPayListResponseField;
        
        public GetPayListResponse GetPayListResponse
        {
            get
            {
                return this.getPayListResponseField;
            }
            set
            {
                this.getPayListResponseField = value;
            }
        }
    }
}
