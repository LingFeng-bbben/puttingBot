using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.Unis
{
    public class UBindList
    {
        public List<QUBind> list = new List<QUBind>();
    }
    public class QUBind
    {
        public long qqid;
        public string phoneNumber;
        public string token;
        public QUBind(string phone,long _q)
        {
            qqid = _q;
            phoneNumber = phone;
        }
    }
}
