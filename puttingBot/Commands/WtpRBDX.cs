using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Formats.RBDX.RbdxCsv songDB = new Formats.RBDX.RbdxCsv(songStr);
            int songCount = songDB.list.Count();
            int songid = random.Next(0, songCount);
            return songDB.list[songid].ToString();
        }
        public static string getRBDX(int level)
        {
            try
            {
                string[] songStr = File.ReadAllLines(filename);
                Formats.RBDX.RbdxCsv songDB = new Formats.RBDX.RbdxCsv(songStr);
                List<Formats.RBDX.RbdxSong> levelList = songDB.list.FindAll(o => o.fumens.Any(p => p.level == level));
                Console.WriteLine(levelList);
                int songCount = levelList.Count();
                int songid = random.Next(0, songCount);
                return levelList[songid].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return "你打一个" + level + "给我看看哟？";
            }
        }
    }
}
