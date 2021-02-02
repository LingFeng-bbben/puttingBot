using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Commands
{
    class Jrrp : Command
    {
        public Jrrp()
        {
            nameToCall = "jrrp";
            helpComment = "今天的人品值是多少呢？宫子来告诉你哟";
        }
        public static int getJrrp(long qqNum)
        {
            Random random = new Random(System.DateTime.Today.DayOfYear + (int)qqNum);
            return random.Next(0, 100);
        }
    }
}
