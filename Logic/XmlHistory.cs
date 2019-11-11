using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlStructureComplat;

namespace Logic
{
    public class XmlHistory
    {
        #region fields
        private List<RequestNavigation> list = new List<RequestNavigation>();
        private int currentIndex = -1;
        #endregion
        #region Properties
        public int Count => list.Count;
        public int Index => currentIndex;
        public EripRequest Current
            => (currentIndex < 0 || currentIndex >= list.Count)
            ? null : list[currentIndex];

        public EripRequest PrevTransaction
            => (currentIndex < 1 || currentIndex >= list.Count)
            ? null : list[currentIndex - 1];
        #endregion
        #region Public Methods
        public EripRequest HomePage()
        {
            if (list.Count <= 0) return null;
            if (currentIndex > 0)
            {
                list[0].SetPrevIndex(currentIndex);
                currentIndex = 0;
            }
            Ex.Log($"{nameof(XmlHistory)}.{nameof(HomePage)}(): curr={currentIndex} prev={list[currentIndex].PrevIndex};");
            return list[0];
        }

        public void Next(PS_ERIP item)
        {
            var page = new RequestNavigation();
            page.Request = item;
            page.SetPrevIndex (currentIndex);
            this.Add(page);
            Ex.Log($"{nameof(XmlHistory)}.{nameof(Next)}(): curr={currentIndex} prev={list[currentIndex].PrevIndex};");
        }

        public EripRequest Previos()
        {
            Ex.Log($"{nameof(XmlHistory)}.{nameof(Previos)}() before: curr={currentIndex} prev={list[currentIndex].PrevIndex};");
            int prevIndex = list[currentIndex].PrevIndex;
            currentIndex = prevIndex;
            var prevPage = list[currentIndex];
            if (currentIndex == 0) 
            { prevPage.SetPrevIndex(0); }
            Ex.Log($"{nameof(XmlHistory)}.{nameof(Previos)}() after: curr={currentIndex} prev={list[currentIndex].PrevIndex};");
            Ex.Log($"{nameof(XmlHistory)}.{nameof(Previos)}() after: page={prevPage.Response.EnumType}, ErrText={prevPage.Response.ResponseReq.ErrorText};");
            return prevPage;
        }
        public XmlHistory SetResponse(PS_ERIP argResponse)
        {
            this.Current.SetResponse(argResponse);
            return this;
        }
        #endregion
        #region Private Methods
        private void Add(RequestNavigation item)
        {
            currentIndex++;
            if (list.Count > currentIndex)
            {
                list.RemoveRange(currentIndex, list.Count - currentIndex);
                list[0].SetPrevIndex(0);
            }
            list.Add(item);
        }
        #endregion

    }
}
