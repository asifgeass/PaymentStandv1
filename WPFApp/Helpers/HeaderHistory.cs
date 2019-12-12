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
            Ex.Log($"HeaderHistory.Add({argHeader}) old={this.CurrentHeader}");
            list.Add(argHeader);
            isHomeLast = false;
            return this;
        }

        public HeaderHistory Home(string argHeader)
        {
            Ex.Log($"HeaderHistory.Home() old={this.CurrentHeader}");
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

        public HeaderHistory BackTo(string argHeader=null, bool isRemoveAfter=true)
        {
            bool isHomeLocal = isHomeLast;
            isHomeLast = false;
            try
            {
                Ex.Log($"HeaderHistory.BackTo({argHeader}) old={this.CurrentHeader}");
                if (isHomeLocal)
                {
                    if (prevHeader != null)
                    {
                        if(argHeader==null)
                        {
                            prevHeader.RemoveRange(prevHeader.Count-1,1);
                            list.Clear();
                            prevHeader.ForEach(x => list.Add(x));
                        }
                        else
                        {
                            var homeindex = prevHeader.LastIndexOf(argHeader);
                            bool isPrev = RemoveAfterIndex(homeindex, prevHeader, isRemoveAfter);
                            //if (isPrev)
                            list.Clear();
                            prevHeader.ForEach(x => list.Add(x));                            
                        }
                    }
                }
                else
                {
                    int index = list.LastIndexOf(argHeader);
                    if (index == -1)
                    {
                        Ex.Log($"HeaderHistory.BackTo(): list.LastIndexOf({argHeader}) = -1 NOT FOUND;");
                        return this;
                        //if (prevHeader != null)
                        //{
                        //    var homeindex = prevHeader.LastIndexOf(argHeader);
                        //    bool isPrev = RemoveAfterIndex(homeindex, prevHeader, isRemoveAfter);
                        //    if (isPrev)
                        //    {
                        //        list.Clear();
                        //        prevHeader.ForEach(x => list.Add(x));
                        //        return this;
                        //    }
                        //}
                        //else { return this; }
                    }
                    RemoveAfterIndex(index, list, isRemoveAfter);
                }
            }
            catch (Exception ex)
            { ex.Log($"HeaderHistory.BackTo({argHeader})"); }
            
            Ex.Log($"HeaderHistory.BackTo({argHeader}) new={this.CurrentHeader}");
            return this;
        }

        private bool RemoveAfterIndex(int index, List<string> list, bool isRemoveAfter)
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
