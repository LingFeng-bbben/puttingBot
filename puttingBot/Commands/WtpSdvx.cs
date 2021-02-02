using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace puttingBot.Commands
{
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
            int songid = new Random().Next(0, songCount);
            XElement TheSong = songDb.Elements("music").ToArray()[songid];
            string songName = TheSong.Element("info").Element("title_name").Value;
            string[] songDiffs = TheSong.Element("difficulty").Elements().Select(o => o.Element("difnum").Value).ToArray();
            string songDiff = "";
            foreach (var e in songDiffs)
            {
                if (int.Parse(e) != 0)
                    songDiff += $"[{e}]";
            }
            return songDiff + "\n" + songName;
        }
    }
}
