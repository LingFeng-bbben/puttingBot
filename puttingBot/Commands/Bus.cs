using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HtmlAgilityPack;

namespace puttingBot.Commands
{
    class BusInfo
    {
        public Dictionary<string, string> positionInfo = new Dictionary<string, string>();
        public string lineName;
        public string dirction;
        public BusInfo(string _name,string _dirc)
        {
            lineName = _name;
            dirction = _dirc;
        }
    }
    class Bus
    {
        static string url = "http://szxing-fwc.icitymobile.com/line/";
        static string schoolBusStop = "西交大";
        static string gqBusStop = "乐桥";
        static string getFormatedInfo(long busID,string stop)
        {
            BusInfo info = downloadLineData(busID);
            int dis = caculateDistance(info, stop);
            return dis == 999 ? String.Format("{0}还未发车哟", info.lineName) : String.Format("{0}离{1}还有{3}站", info.lineName, schoolBusStop, dis);
        }
        public static string chuqing()
        {
            string message = "打机去咯!!!!\n";
            message += getFormatedInfo(10000831,schoolBusStop);
            message += "\n";
            message += getFormatedInfo(10000375, schoolBusStop);
            message += "\n";
            message += getFormatedInfo(10000566, schoolBusStop);
            return message;
        }
        public static string huijia_gq()
        {
            string message = "从观前回家咯!!!!\n";
            message += getFormatedInfo(10000830, gqBusStop);
            message += "\n";
            message += getFormatedInfo(10000036, gqBusStop);
            message += "\n";
            message += getFormatedInfo(10000554, gqBusStop);
            return message;
        }
        public static string huijia_sz()
        {
            string message = "从苏中回家咯!!!!\n";
            message += getFormatedInfo(10000036, "国际大厦西");
            message += "\n";
            message += getFormatedInfo(10000554, "四季新家园");
            return message;
        }
        public static BusInfo downloadLineData(long lineId)
        {
            string html = Download.downloadText(url+lineId,"","", "Chrome/88.0.4324.190");
            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);
            HtmlNodeCollection chartNodes = htmldoc.DocumentNode.SelectNodes("//div[@class=\"am-list-body\"]/a");
            string lineName = htmldoc.DocumentNode.SelectSingleNode("//h3[@class=\"am-card-item-title am-ft-orange\"]").InnerText;
            string to = htmldoc.DocumentNode.SelectSingleNode("//div[@class=\"am-card-item-content\"]/div[4]").InnerText;
            //Console.WriteLine(lineName + " "+to );
            BusInfo busInfo = new BusInfo(lineName, to);

            for (int i = 1; i < chartNodes.Count+1; i++)
            {
                HtmlNodeCollection stationNodes = htmldoc.DocumentNode.SelectNodes("//div[@class=\"am-list-body\"]/a["+i+"]/span");
                string stopname = "" ;
                string timeEnter = "";
                if (stationNodes.Count < 2)
                {
                    stopname = stationNodes[0].InnerText;
                }
                else
                {
                    stopname = stationNodes[0].InnerText;
                    timeEnter = stationNodes[1].InnerText;
                }
                busInfo.positionInfo.Add(stopname, timeEnter);
                //Console.WriteLine(stopname + " " + timeEnter);
            }
            return busInfo;
        }
        public static int caculateDistance(BusInfo info, string station)
        {
            var infoarr = info.positionInfo.ToArray();
            int startPos = 0;
            List<int> busPos = new List<int>();
            for (int i = 0; i < infoarr.Length; i++)
            {
                if (infoarr[i].Key == station) startPos = i;
                if (infoarr[i].Value != "") busPos.Add(i);
            }
            int mindistance = 999;
            foreach(int pos in busPos)
            {
                if ((pos < startPos) && ((startPos - pos) < mindistance)) mindistance = startPos - pos;
            }
            return mindistance;
        }
    }
}
