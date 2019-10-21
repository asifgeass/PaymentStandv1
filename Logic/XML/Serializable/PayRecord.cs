using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML.Serializable
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PayRecord
    {

        private GroupRecord groupRecordField;

        private Lookup[] lookupsField;

        private AttrRecord[] attrRecordField;

        private ushort codeField;

        private string nameField;

        private string stornoModeField;

        private decimal commissionField;

        private decimal summaField;

        private byte viewField;

        private decimal fineField;

        private decimal payCommissionField;

        private string sessionIdField;

        private byte editField;

        private byte getPayListTypeField;

        /// <remarks/>
        public GroupRecord GroupRecord
        {
            get
            {
                return this.groupRecordField;
            }
            set
            {
                this.groupRecordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Lookup", IsNullable = false)]
        public Lookup[] Lookups
        {
            get
            {
                return this.lookupsField;
            }
            set
            {
                this.lookupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AttrRecord")]
        public AttrRecord[] AttrRecord
        {
            get
            {
                return this.attrRecordField;
            }
            set
            {
                this.attrRecordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string StornoMode
        {
            get
            {
                return this.stornoModeField;
            }
            set
            {
                this.stornoModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Commission
        {
            get
            {
                return this.commissionField;
            }
            set
            {
                this.commissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Summa
        {
            get
            {
                return this.summaField;
            }
            set
            {
                this.summaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte View
        {
            get
            {
                return this.viewField;
            }
            set
            {
                this.viewField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Fine
        {
            get
            {
                return this.fineField;
            }
            set
            {
                this.fineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal PayCommission
        {
            get
            {
                return this.payCommissionField;
            }
            set
            {
                this.payCommissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SessionId
        {
            get
            {
                return this.sessionIdField;
            }
            set
            {
                this.sessionIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Edit
        {
            get
            {
                return this.editField;
            }
            set
            {
                this.editField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte GetPayListType
        {
            get
            {
                return this.getPayListTypeField;
            }
            set
            {
                this.getPayListTypeField = value;
            }
        }
    }
}
