using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Channels;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace puttingBot.Commands
{
    class Command
    {
        public string nameToCall = "";
        public string helpComment = "";
        public static Object[] commandsList = { new Help(),new Jrrp(),new Dydy() ,new WhatToPlaySdvx(),new WhatToPlayChuni(),new Long()};
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
    partial class Gamble : Command
    {
        public Gamble()
        {
            nameToCall = "gb";
            helpComment = "赌狗游戏相关哟,具体请#gb help哟";
        }
    }

    class Dydy : Command
    {
        static string jsFilePath = "dydy.json";
        static Random random = new Random(new Guid().GetHashCode());
        public Dydy()
        {
            nameToCall = "dydy";
            helpComment = "吊言吊语哟, #dydy add 添加新的语录";
        }
        public static void add(long qid,string a)
        {
            string text = File.ReadAllText(jsFilePath);
            dataStructure.DydyJson dydyJson = JsonConvert.DeserializeObject<dataStructure.DydyJson>(text);
            if (!dydyJson.dyItems.Any(o => o.dyData == a))
                dydyJson.dyItems.Add(new dataStructure.DyItem(qid,a));
            File.WriteAllText(jsFilePath,JsonConvert.SerializeObject(dydyJson, Formatting.Indented));
        }
        public static string read()
        {
            string text = File.ReadAllText(jsFilePath);
            dataStructure.DydyJson dydyJson = JsonConvert.DeserializeObject<dataStructure.DydyJson>(text);
            int radIndex = 0;
            for (int i = 0; i < random.Next(5, random.Next(5, 88));i++)
                radIndex = random.Next(0, dydyJson.dyItems.Count);
            return dydyJson.dyItems[radIndex].dyData;
        }
    }
    class Csm : Command
    {
        static string jsFilePath = "csm.json";
        static Random random = new Random(new Guid().GetHashCode());
        public Csm()
        {
            nameToCall = "csm";
            helpComment = "随机生成吃什么, #csm add 添加新的吃饭地方";
        }
        public static void add(long qid, string a)
        {
            string text = File.ReadAllText(jsFilePath);
            dataStructure.DydyJson dydyJson = JsonConvert.DeserializeObject<dataStructure.DydyJson>(text);
            if (!dydyJson.dyItems.Any(o => o.dyData == a))
                dydyJson.dyItems.Add(new dataStructure.DyItem(qid, a));
            File.WriteAllText(jsFilePath, JsonConvert.SerializeObject(dydyJson, Formatting.Indented));
        }
        public static string read()
        {
            string text = File.ReadAllText(jsFilePath);
            dataStructure.DydyJson dydyJson = JsonConvert.DeserializeObject<dataStructure.DydyJson>(text);
            int radIndex = 0;
            for (int i = 0; i < random.Next(5, random.Next(5, 88)); i++)
                radIndex = random.Next(0, dydyJson.dyItems.Count);
            return dydyJson.dyItems[radIndex].dyData;
        }
    }

    class Long : Command
    {
        static string longRepoPath = "/usr/torch/";
        public Long()
        {
            nameToCall = "long";
            helpComment = "加上带人脸图片，计算龙度";
        }
        public static string cacLong(string url)
        {
            WebClient wc = new WebClient();
            
            Console.WriteLine("downloading");
            try
            {
                byte[] data = wc.DownloadData(url);
                Console.WriteLine("saving");
                MemoryStream ms = new MemoryStream(data);
                Image image = Image.FromStream(ms);
                Console.WriteLine(image.RawFormat.ToString());
                if (image.RawFormat.ToString() == "Gif")
                    return "false";
                string filename = image.GetHashCode().ToString();
                string filepath = longRepoPath + "imgreco/" + filename + ".jpg";
                
                image.Save(filepath);
                Process p = new Process();
                p.StartInfo.FileName = "bash";
                p.StartInfo.Arguments = "/usr/torch/run.sh";
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                Console.WriteLine(p.StandardOutput.ReadToEnd());
                string resultpath = longRepoPath + "imgresult/" + filename + ".jpg";
                //System.Diagnostics.Process.Start("bash /usr/torch/run.sh"); //this do not work
                return resultpath;
            }
            catch(Exception e) {
                Console.WriteLine("failed."+e.Message);
                return "false";
            }
        }
    }
}
