using ExceptionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Helpers
{
    public class HeaderHistory
    {
        private List<string> prevHeader;
        private List<string> list = new List<string>();
        private string temp;
        private bool isHomeLast = false;

        public HeaderHistory Add(string argHeader)
        {
            list.Add(argHeader);
            isHomeLast = false;
            return this;
        }

        public HeaderHistory Home(string argHeader)
        {
            isHomeLast = true;
            prevHeader = new List<string>();
            list.ForEach(x => prevHeader.Add(x));
            list.Clear();
            list.Add(argHeader);
            return this;
        }

        public HeaderHistory RemoveLast()
        {
            if (list.Count <= 0) return this;
            list.RemoveRange(list.Count-1, 1);
            return this;
        }

        public HeaderHistory BackTo(string argHeader, bool isRemoveAfter=false)
        {
            try
            {
                int index = list.LastIndexOf(argHeader);
                if (index == -1)
                {
                    if (isHomeLast && prevHeader != null)
                    {
                        var homeindex = prevHeader.LastIndexOf(argHeader);
                        bool isPrev = RemoveAfterIndex(homeindex, prevHeader);
                        if(isPrev)
                        {
                            list.Clear();
                            prevHeader.ForEach(x => list.Add(x));
                        }
                    }
                    else { return this; }
                }
                RemoveAfterIndex(index, list, isRemoveAfter);
            }
            catch (Exception ex)
            { ex.Log($"HeaderHistory.BackTo({argHeader})"); }
            isHomeLast = false;
            return this;
        }

        private bool RemoveAfterIndex(int index, List<string> list, bool isRemoveAfter = false)
        {
            if (index >= 0)
            {
                if(isRemoveAfter) index = index + 1;
                if (index < list.Count)
                {
                    list.RemoveRange(index, list.Count - index);
                    return true;
                }
            }
            return false;
        }

        private string GetHeaderString()
        {
            string temp = string.Empty;
            list.ForEach(x => temp += ($"{x} / "));
            temp = temp.Trim(' ', '/');
            return temp;
        }

        public string CurrentHeader => GetHeaderString();
    }
}
