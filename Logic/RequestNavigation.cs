using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class RequestNavigation : EripRequest
    {
        public void SetPrevIndex(int arg)
        {
            _prevIndex = arg;
        }
    }
}
