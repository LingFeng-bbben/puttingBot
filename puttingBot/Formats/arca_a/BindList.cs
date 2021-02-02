using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.arca_a
{
    public class BindList
    {
        public List<QABind> list = new List<QABind>();
    }
    public class QABind
    {
        public long qqid;
        public string arcid;
        public QABind(string _arc,long _q)
        {
            qqid = _q;
            arcid = _arc;
        }
    }
}
