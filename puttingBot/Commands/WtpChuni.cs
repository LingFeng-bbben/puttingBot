using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace puttingBot.Commands
{
    class WhatToPlayChuni : Command
    {
        static XElement songDb = XElement.Load("Chuni_MusicSort.xml");
        public WhatToPlayChuni()
        {
            nameToCall = "chuni";
            helpComment = "宫子帮你决定中二该玩什么歌哟";
        }
        public static string getChuni()
        {
            var songs = songDb.Element("SortList").Elements("StringID");
            int songCount = songs.Count();
            int songid = new Random().Next(0, songCount);
            XElement TheSong = songs.ToArray()[songid];
            string songName = TheSong.Element("str").Value;
            return songName;
        }
    }
}
