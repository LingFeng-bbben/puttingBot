using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace puttingBot.Commands
{
    class Command
    {
        public string nameToCall = "";
        public string helpComment = "";
        public static Object[] commandsList = { new Help(),new Jrrp(),new WhatToPlaySdvx(),new Gamble()};
    }
    class Jrrp:Command
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
    class Help : Command
    {
        public Help() {
            nameToCall = "help";
            helpComment = "宫子bot的帮助菜单哟"; 
        }
    }
    class WhatToPlaySdvx : Command
    {
        static XElement songDb = XElement.Load("sdvx_music_db.xml");
        public WhatToPlaySdvx()
        {
            nameToCall = "sdvx";
            helpComment = "宫子帮你决定sdvx该玩什么歌哟";
        }
        public static string getSdvx()
        {
            int songCount = songDb.Elements("music").Count();
            int songid = new Random().Next(0,songCount);
            XElement TheSong = songDb.Elements("music").ToArray()[songid];
            string songName = TheSong.Element("info").Element("title_name").Value;
            string[] songDiffs = TheSong.Element("difficulty").Elements().Select(o=>o.Element("difnum").Value).ToArray();
            string songDiff = "";
            foreach(var e in songDiffs)
            {
                if(int.Parse(e)!=0)
                    songDiff += $"[{e}]";
            }
            return songDiff+"\n"+songName;
        }
    }
    partial class Gamble : Command
    {
        public Gamble()
        {
            nameToCall = "gb";
            helpComment = "赌狗游戏相关哟,具体请#gb help哟";
        }
    }
}
