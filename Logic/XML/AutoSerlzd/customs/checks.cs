using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.XML.AutoSerlzd.customs
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PS_ERIP
    {

        private PS_ERIPRunOperationResponse runOperationResponseField;

        /// <remarks/>
        public PS_ERIPRunOperationResponse RunOperationResponse
        {
            get
            {
                return this.runOperationResponseField;
            }
            set
            {
                this.runOperationResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponse
    {

        private ulong kioskReceiptField;

        private byte errorCodeField;

        private PS_ERIPRunOperationResponsePayRecord payRecordField;

        /// <remarks/>
        public ulong KioskReceipt
        {
            get
            {
                return this.kioskReceiptField;
            }
            set
            {
                this.kioskReceiptField = value;
            }
        }

        /// <remarks/>
        public byte ErrorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        public PS_ERIPRunOperationResponsePayRecord PayRecord
        {
            get
            {
                return this.payRecordField;
            }
            set
            {
                this.payRecordField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecord
    {

        private PS_ERIPRunOperationResponsePayRecordAttrRecord attrRecordField;

        private PS_ERIPRunOperationResponsePayRecordCheck checkField;

        private ushort codeField;

        private byte recordIDField;

        private uint paymentIDField;

        private decimal payAllField;

        private decimal summaField;

        private decimal payCommissionField;

        private ushort currencyField;

        private string dateField;

        /// <remarks/>
        public PS_ERIPRunOperationResponsePayRecordAttrRecord AttrRecord
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
        public PS_ERIPRunOperationResponsePayRecordCheck Check
        {
            get
            {
                return this.checkField;
            }
            set
            {
                this.checkField = value;
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
        public byte RecordID
        {
            get
            {
                return this.recordIDField;
            }
            set
            {
                this.recordIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint PaymentID
        {
            get
            {
                return this.paymentIDField;
            }
            set
            {
                this.paymentIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal PayAll
        {
            get
            {
                return this.payAllField;
            }
            set
            {
                this.payAllField = value;
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
        public ushort Currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordAttrRecord
    {

        private ushort codeField;

        private byte codeOutField;

        private string nameField;

        private byte printField;

        private string valueField;

        private byte viewField;

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
        public byte CodeOut
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
        public byte Print
        {
            get
            {
                return this.printField;
            }
            set
            {
                this.printField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordCheck
    {

        private PS_ERIPRunOperationResponsePayRecordCheckCheckHeader checkHeaderField;

        private PS_ERIPRunOperationResponsePayRecordCheckCheckFooter checkFooterField;

        /// <remarks/>
        public PS_ERIPRunOperationResponsePayRecordCheckCheckHeader CheckHeader
        {
            get
            {
                return this.checkHeaderField;
            }
            set
            {
                this.checkHeaderField = value;
            }
        }

        /// <remarks/>
        public PS_ERIPRunOperationResponsePayRecordCheckCheckFooter CheckFooter
        {
            get
            {
                return this.checkFooterField;
            }
            set
            {
                this.checkFooterField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordCheckCheckHeader
    {

        private PS_ERIPRunOperationResponsePayRecordCheckCheckHeaderCheckLine[] checkLineField;

        private byte countField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CheckLine")]
        public PS_ERIPRunOperationResponsePayRecordCheckCheckHeaderCheckLine[] CheckLine
        {
            get
            {
                return this.checkLineField;
            }
            set
            {
                this.checkLineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordCheckCheckHeaderCheckLine
    {

        private byte idxField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordCheckCheckFooter
    {

        private PS_ERIPRunOperationResponsePayRecordCheckCheckFooterCheckLine checkLineField;

        private byte countField;

        /// <remarks/>
        public PS_ERIPRunOperationResponsePayRecordCheckCheckFooterCheckLine CheckLine
        {
            get
            {
                return this.checkLineField;
            }
            set
            {
                this.checkLineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PS_ERIPRunOperationResponsePayRecordCheckCheckFooterCheckLine
    {

        private byte idxField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Idx
        {
            get
            {
                return this.idxField;
            }
            set
            {
                this.idxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
