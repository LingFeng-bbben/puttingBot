using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.dataStructure
{
    class DydyJson
    {
        public List<DyItem> dyItems = new List<DyItem>();
        public DydyJson()
        {
            dyItems = new List<DyItem>();
        }
    }
    class DyItem
    {
        public long qid = 0;
        public DateTime timeAdded = new DateTime();
        public string dyData = "";
        public DyItem(long _qid,string _dydy)
        {
            qid = _qid;
            timeAdded = DateTime.Now;
            dyData = _dydy;
        }
    }
}
