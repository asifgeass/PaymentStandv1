using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML.AutoSerlzd
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AttrRecord
    {

        private byte codeField;

        private ushort codeOutField;

        private string nameField;

        private byte minLengthField;

        private byte maxLengthField;

        private bool maxLengthFieldSpecified;

        private byte mandatoryField;

        private bool mandatoryFieldSpecified;

        private byte editField;

        private byte viewField;

        private string hintField;

        private string typeField;

        private string lookupField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Code
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
        public ushort CodeOut
        {
            get
            {
                return this.codeOutField;
            }
            set
            {
                this.codeOutField = value;
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
        public byte MinLength
        {
            get
            {
                return this.minLengthField;
            }
            set
            {
                this.minLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte MaxLength
        {
            get
            {
                return this.maxLengthField;
            }
            set
            {
                this.maxLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaxLengthSpecified
        {
            get
            {
                return this.maxLengthFieldSpecified;
            }
            set
            {
                this.maxLengthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Mandatory
        {
            get
            {
                return this.mandatoryField;
            }
            set
            {
                this.mandatoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MandatorySpecified
        {
            get
            {
                return this.mandatoryFieldSpecified;
            }
            set
            {
                this.mandatoryFieldSpecified = value;
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
        public string Hint
        {
            get
            {
                return this.hintField;
            }
            set
            {
                this.hintField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Lookup
        {
            get
            {
                return this.lookupField;
            }
            set
            {
                this.lookupField = value;
            }
        }
    }
}
