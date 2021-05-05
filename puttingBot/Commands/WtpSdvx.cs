using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using puttingBot.Formats;

namespace puttingBot.Commands
{
    class WhatToPlaySdvx : Command
    {
        static string csvPath = "sydx.txt";
        static string csvPathHDD = "sdvx.txt";
        public WhatToPlaySdvx()
        {
            nameToCall = "sdvx/sydx";
            helpComment = "宫子帮你决定sdvx该玩什么歌哟";
            convertMdb();
        }
        public static void convertMdb()
        {
            try
            {
                XElement songDb = XElement.Load("sdvx_music_db.xml");
                int songCount = songDb.Elements("music").Count();
                List<string> lines = new List<string>();
                for (int songid = 0; songid < songCount; songid++)
                {
                    XElement TheSong = songDb.Elements("music").ToArray()[songid];
                    string id = TheSong.Element("info").Element("label").Value;
                    string songName = TheSong.Element("info").Element("title_name").Value;
                    string songArtist = TheSong.Element("info").Element("artist_name").Value;
                    string[] songDiffs = TheSong.Element("difficulty").Elements().Select(o => o.Element("difnum").Value).ToArray();
                    string line = string.Format("{0}	{1}	{2}	", id, songName, songArtist);
                    foreach(string songdiff in songDiffs)
                    {
                        if (songdiff != "0")
                        line += songdiff + "	";
                    }
                    line += "				";
                    lines.Add(line);
                }
                System.IO.File.WriteAllLines(csvPathHDD, lines.ToArray());
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }
        public static string getSydx(bool isHDD = false)
        {

            SydxCsv csv = new SydxCsv(isHDD ? csvPathHDD : csvPath);
            int songid = new Random().Next(0, csv.songlist.Count);
            string message = csv.songlist[songid].name+"\n";
            message += csv.songlist[songid].artist + "\n";
            foreach (int i in csv.songlist[songid].diff){
                if (i != 0)
                    message += "[" + i + "]";
            }
            return message;
        }
        public static string getSydx(string lv, bool isHDD = false)
        {
            try
            {
                SydxCsv csv = new SydxCsv(isHDD ? csvPathHDD : csvPath);
                int level = int.Parse(lv);
                List<SydxSong> songlist = csv.songlist.FindAll(o => o.diff.Any(a => a == level));
                int songid = new Random().Next(0, songlist.Count);
                string message = songlist[songid].name + "\n";
                message += songlist[songid].artist + "\n";
                foreach (int i in songlist[songid].diff)
                {
                    if (i != 0)
                        message += "[" + i + "]";
                }
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return "你打一个" + lv + "给我看看哟？";
            }
        }
    }
}
