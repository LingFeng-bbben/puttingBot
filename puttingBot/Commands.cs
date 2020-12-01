using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Diagnostics;
using HtmlAgilityPack;

namespace puttingBot.Commands
{
    class Command
    {
        public string nameToCall = "";
        public string helpComment = "";
        public static Object[] commandsList = { new Help(),new Jrrp(),new Dydy(),new Weather() ,new WhatToPlaySdvx(),new WhatToPlayChuni(),new Long()};
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
            try{
                string resultpath = Download.downloadPic(url, longRepoPath + "imgreco/");
                resultpath = resultpath.Replace("imgreco", "imgresult");
                Process p = new Process();
                p.StartInfo.FileName = "bash";
                p.StartInfo.Arguments = "/usr/torch/run.sh";
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                Console.WriteLine(p.StandardOutput.ReadToEnd());

                //System.Diagnostics.Process.Start("bash /usr/torch/run.sh"); //this do not work
                return resultpath;
            }
            catch(Exception e) {
                Console.WriteLine("failed."+e.Message);
                return "false";
            }
        }
    }

    class Weather : Command
    {
        public Weather()
        {
            nameToCall = "tq";
            helpComment = "查询天气";
        }

        public static string GetWeatherNow(int citycode = 58349)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);

            string weatherString = "布丁布丁，看看"+ weather.data.predict.station.city+ "现在天气怎么样哟！\n";

            WeatherJson.Weather wea = weather.data.real.weather;

            weatherString += wea.info + ", 温度" + wea.temperature + "℃, 湿度" + wea.humidity + "%\n";
            weatherString += "降水量:" + wea.rain + "mm";


            return weatherString;
        }

        public static string GetWeather(int citycode = 58349, int day = 0)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);
            WeatherJson.Detail detail = weather.data.predict.detail[day];

            string weatherString = "布丁布丁，播报天气哟！\n";
            weatherString += weather.data.predict.station.city + " " + detail.date + "\n";

            WeatherJson.Weather wea = new WeatherJson.Weather();
            if(detail.day.weather.info != "9999")
            {
                wea = detail.day.weather;
                weatherString += "白天\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃\n";
                wea = detail.night.weather;
                weatherString += "晚上\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃";
            }
            else
            {
                wea = detail.night.weather;
                weatherString += "晚上\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃";
            }
            
            return weatherString;
        }

        public static string GetRadar(int citycode = 58349)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);
            string url = weather.data.radar.image;
            string picpath = Download.downloadPic("http://image.nmc.cn" + url,"/usr/weather/");
            return picpath;
        }

        public static string GetPrv()
        {
            PrvJson.Root[] roots = Download.downloadJson<PrvJson.Root[]>("http://www.nmc.cn/rest/province/all");
            string list = "tx夹我放不出名字，缩写自个儿看吧";
            foreach(PrvJson.Root prv in roots)
            {
                list += prv.Code + "，";
            }
            return list;
        }

        public static string GetCity(string prv)
        {
            CityJson.Root[] roots = Download.downloadJson<CityJson.Root[]>("http://www.nmc.cn/rest/province/"+prv);
            string list = "";
            foreach (CityJson.Root city in roots)
            {
                list += city.Code + " " + city.City + "\n";
            }
            return list;
        }

        public static string GetAnalyze() {
            string html = Download.downloadText("http://www.nmc.cn/publish/observations/china/dm/radar-h000.htm");
            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);

            HtmlNode imgNode = htmldoc.DocumentNode.SelectSingleNode("//div[@class=\"container\"]/div[@class=\"row\"]/div[@class=\"col-xs-10\"]/div[@class=\"bgwhite\"]/div[@class=\"row\"]/div[@class=\"col-xs-10\"]/div[@class=\"imgblock\"]/img[@id=\"imgpath\"]");
            string url = imgNode.Attributes.First(o => o.Name == "src").Value;
            Console.WriteLine(url);
            string picpath = Download.downloadPic(url, "/usr/weather/");
            return picpath;
        }
    }
}
