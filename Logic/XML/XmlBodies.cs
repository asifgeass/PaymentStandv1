using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlBodies.XML.AutoSerlzd
{
    public static class XmlBodies
    {
        public static XDocument XmlTest()
        {
            //return x1Get();
            return x31Get();            
        }
        public static XDocument XmlTest2(string param)
        {
            return x32Run(param);
        }
        public static XDocument XmlTest3()
        {
            return x33Conf();
        }
        public static XDocument x1Get(string PayCode = "400")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("GetPayListRequest",
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PayCode", PayCode)
                    )
                ));
            return xdoc;
        }
        public static XDocument x21Get1(string PayCode = "411")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("GetPayListRequest",
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PAN", "BY93AKBB36019010100000000000"),
                        new XElement("TypePAN", "ACCOUNT"),
                        new XElement("PayCode", PayCode)
                    )
                ));
            return xdoc;
        }
        public static XDocument x21Get2(string PayCode = "411")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("GetPayListRequest",
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PAN", "BY93AKBB36019010100000000000"),
                        new XElement("TypePAN", "ACCOUNT"),
                        new XElement("PayCode", PayCode)
                    )
                ));
            return xdoc;
        }
        public static XDocument x22Run(string SessionId)
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                return null;
            }
            XDocument xml = new XDocument(
                new XElement("PS_ERIP",
                  new XElement("RunOperationRequest",
                    new XElement("Key"),
                    new XElement("TerminalID", "TEST_TERMINAL"),
                    new XElement("Version", "3"),                    
                    new XElement("SignCode", "0"),
                    new XElement("PayDate", "17/09/2019 09:00:00"),
                    new XElement("KioskReceipt", "17092019090000"),
                    new XElement("KioskError", "0"),
                    new XElement("PayRecordCount", "0"),
                    new XElement("PayRecord",
                        new XAttribute("Code", "411"),                        
                        new XAttribute("DIType", "9120"),
                        new XAttribute("Name", "Госпошлина за выдачу разрешения"),
                        new XAttribute("SessionId", SessionId),
                        new XAttribute("GetPayListType", "0"),
                        new XAttribute("Commission", "0"),
                        new XAttribute("Summa", "8.00"),
                        new XAttribute("Currency", "933"),
                        new XAttribute("Fine", "1.50"),
                        new XAttribute("PayCommission", "0")
                        ),
                    new XElement("GroupRecord",
                        new XAttribute("Code", "401"),
                        new XAttribute("Name", "Физическое лицо")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "660"),
                        new XAttribute("CodeOut", "1001"),
                        new XAttribute("Name", "Оплачено по услуге"),
                        new XAttribute("Value", "1234AA-7")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "745"),
                        new XAttribute("CodeOut", "1002"),
                        new XAttribute("Name", "Свидетельство о регистрации"),
                        new XAttribute("Value", "AAA123456789")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "873"),
                        new XAttribute("Name", "УНП плательщика"),
                        new XAttribute("Value", "109879310")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "661"),
                        new XAttribute("Name", "Тип транспортного средства"),
                        new XAttribute("Value", "Легковой автомобиль")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "662"),
                        new XAttribute("Name", "Разрешённая максимальная масса авто"),
                        new XAttribute("Value", "Не более 1,5 тонны")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "663"),
                        new XAttribute("Name", "Коэффициенты госпошлины"),
                        new XAttribute("Value", "2")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "861"),
                        new XAttribute("Name", "Счетчик запросов"),
                        new XAttribute("Value", "-1")
                        ))));
            return xml;
        }
        public static XDocument x23Conf(string PayCode = "421")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("ConfirmRequest",
                        new XElement("Key"),
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PayRecord",
                            new XAttribute("RecordID", "1"),
                            new XAttribute("PaymentID", "175124620"),
                            new XAttribute("Date", "17/09/2019 09:00:00"),
                            new XAttribute("KioskReceipt", "17092019090000"),
                            new XAttribute("ConfirmCode", "1")
                            )
                        )
                    )
                );
            return xdoc;
        }
        public static XDocument x31Get(string PayCode = "421")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("GetPayListRequest",
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PAN", "BY93AKBB36019010100000000000"),
                        new XElement("TypePAN", "ACCOUNT"),
                        new XElement("PayCode", PayCode)
                    )
                ));
            return xdoc;
        }
        public static XDocument x32Run(string SessionId)
        {
            XDocument xml = new XDocument(
                new XElement("PS_ERIP",
                  new XElement("RunOperationRequest",
                    new XElement("Key"),
                    new XElement("TerminalID", "TEST_TERMINAL"),
                    new XElement("Version", "3"),
                    new XElement("SignCode", "0"),
                    new XElement("PayDate", "17/09/2019 09:00:06"),
                    new XElement("KioskReceipt", "17092019090006"),
                    new XElement("KioskError", "0"),
                    new XElement("PayRecordCount", "0"),
                    new XElement("PayRecord",
                        new XAttribute("Code", "421"),
                        new XAttribute("DIType", "9120"),
                        new XAttribute("Name", "Гостехосмотр"),
                        new XAttribute("SessionId", SessionId),
                        new XAttribute("CodeOut", "421"),
                        new XAttribute("StornoMode", "S"),
                        new XAttribute("ClaimID", "0"),
                        new XAttribute("Category", "1"),
                        new XAttribute("GetPayListType", "1"),
                        new XAttribute("Summa", "8"),
                        new XAttribute("Edit", "0"),
                        new XAttribute("View", "0"),
                        new XAttribute("Fine", "0"),
                        new XAttribute("PayCommission", "0"),
                        new XAttribute("Commission", "0")
                        ),
                    new XElement("GroupRecord",
                        new XAttribute("Code", "401"),
                        new XAttribute("Name", "Физическое лицо")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "660"),
                        new XAttribute("CodeOut", "1001"),
                        new XAttribute("Name", "Оплачено по услуге"),
                        new XAttribute("Value", "1234AA-7")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "745"),
                        new XAttribute("CodeOut", "1002"),
                        new XAttribute("Name", "Свидетельство о регистрации"),
                        new XAttribute("Value", "AAA123456789")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "873"),
                        new XAttribute("Name", "УНП плательщика"),
                        new XAttribute("Value", "109879310")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "661"),
                        new XAttribute("CodeOut", "1002"),
                        new XAttribute("Name", "Тип транспортного средства"),
                        new XAttribute("Value", "Легковой автомобиль")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "664"),
                        new XAttribute("Name", "Тип двигателя"),
                        new XAttribute("Value", "Гибридный или электромобиль")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "665"),
                        new XAttribute("Name", "Колесная формула"),
                        new XAttribute("Value", "4x2")
                        ),
                    new XElement("AttrRecord",
                        new XAttribute("Code", "861"),
                        new XAttribute("Name", "Счетчик запросов"),
                        new XAttribute("Value", "0")
                        ))));
            return xml;
        }
        public static XDocument x33Conf(string PayCode = "421")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("ConfirmRequest",
                        new XElement("Key"),
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PayRecord",
                            new XAttribute("RecordID", "1"),
                            new XAttribute("PaymentID", "175124622"),
                            new XAttribute("Date", "17/09/2019 09:00:04"),
                            new XAttribute("KioskReceipt", "17092019090004"),
                            new XAttribute("ConfirmCode", "1")
                            )
                        )
                    )
                );
            return xdoc;
        }
        public static XDocument x41Get(string PayCode = "441")
        {
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("PS_ERIP",
                    new XElement("GetPayListRequest",
                        new XElement("TerminalID", "TEST_TERMINAL"),
                        new XElement("Version", "3"),
                        new XElement("PAN", "BY93AKBB36019010100000000000"),
                        new XElement("TypePAN", "ACCOUNT"),
                        new XElement("PayCode", PayCode)
                    )
                ));
            return xdoc;
        }
    }
}
