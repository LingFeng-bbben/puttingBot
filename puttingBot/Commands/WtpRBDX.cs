using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Commands
{
    class WhatToPlayRBDX : Command
    {
        const string filename = "RBDX.txt";
        static Random random = new Random(new Guid().GetHashCode() + (int)DateTime.Now.Ticks);
        public WhatToPlayRBDX()
        {
            nameToCall = "rbdx";
            helpComment = "宫子帮你决定日本大学该玩什么歌哟";
        }
        public static string getRBDX()
        {
            string[] songStr = File.ReadAllLines(filename);
            Formats.RbdxCsv songDB = new Formats.RbdxCsv(songStr);
            int songCount = songDB.list.Count();
            int songid = random.Next(0, songCount);
            return songDB.list[songid].ToString();
        }
        public static string getRBDX(int level)
        {
            try
            {
                string[] songStr = File.ReadAllLines(filename);
                Formats.RbdxCsv songDB = new Formats.RbdxCsv(songStr);
                List<Formats.RbdxSong> levelList = songDB.list.FindAll(o => o.fumens.Any(p => p.level == level));
                Console.WriteLine(levelList);
                int songCount = levelList.Count();
                int songid = random.Next(0, songCount);
                return levelList[songid].ToString();
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }
        }
    }
}
