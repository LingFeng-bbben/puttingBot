using System;
using System.Collections.Generic;
using Windows.Data.Text;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.IO;

namespace puttingBot.Commands
{
    class Command
    {
        public string nameToCall = "";
        public string helpComment = "";
        public static Object[] commandsList = { new Help(),new Jrrp(), new PlasticJpn(),new WhatToPlaySdvx(),new WhatToPlayChuni()};
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
            int songCount =songs.Count();
            int songid = new Random().Next(0, songCount);
            XElement TheSong = songs.ToArray()[songid];
            string songName = TheSong.Element("str").Value;
            return songName;
        }
    }
    class PlasticJpn : Command
    {
        static Dictionary<char, List<string>> dic =
            JsonConvert.DeserializeObject<Dictionary<char, List<string>>>(
                File.ReadAllText("JpnDic.json"));
        public PlasticJpn()
        {
            nameToCall = "slry";
            helpComment = "这句热语，用腻红锅怎么说哟";
        }
        public static async Task<string> getPlaJpn(string ipt)
        {
            var trcg = new TextReverseConversionGenerator("ja");
            IReadOnlyList<TextPhoneme> tpl = await trcg.GetPhonemesAsync(ipt);
            string resultReading = "";
            string resultKanji = "";
            foreach (var a in tpl)
            {
                resultReading += a.ReadingText;
                foreach (var b in a.ReadingText)
                {
                    try
                    {
                        resultKanji += dic[b][0];
                    }
                    catch {
                        resultKanji += b;
                    }
                }
                resultKanji += " ";
            }
            return resultKanji;
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
