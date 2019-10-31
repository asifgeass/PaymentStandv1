using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class XmlHistory
    {
        #region fields
        private List<EripRequest> list = new List<EripRequest>();
        private int currentIndex = -1;
        #endregion
        #region Public Methods
        public void Add(EripRequest item)
        {
            currentIndex++;
            list.Add(item);
        }
        #endregion
        #region Properties
        public int Count { get => list.Count; }
        public EripRequest Page
            => (currentIndex < 0 || currentIndex >= list.Count)
            ? null : list[currentIndex];

        public EripRequest PrevPage
            => (currentIndex < 1 || currentIndex >= list.Count)
            ? null : list[currentIndex - 1];
        #endregion
    }
}
