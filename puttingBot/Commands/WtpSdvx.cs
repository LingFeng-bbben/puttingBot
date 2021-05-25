using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using puttingBot.Formats;
using System.IO;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.Threading.Tasks;

namespace puttingBot.Commands
{
    class WhatToPlaySdvx : Command
    {
        static string csvPath = "sydx.txt";
        static string csvPathHDD = "sdvx.txt";
        static string marsLanConv = "sdvx_mars.txt";
        public WhatToPlaySdvx()
        {
            nameToCall = "sdvx/sydx";
            helpComment = "宫子帮你决定sdvx该玩什么歌哟";
            convertMdb();
        }
        public static string convertMarthan(string mars)
        {
            string[] file = File.ReadAllLines(marsLanConv);
            foreach (string line in file)
            {
                string marorg = line.Split(" ")[0];
                string trans = line.Split(" ")[1];
                mars = mars.Replace(marorg, trans);
            }
            return mars;
        }
        public static string[] convertMarthan(string[] mars)
        {
            string[] file = File.ReadAllLines(marsLanConv);
            foreach (string line in file)
            {
                string marorg = line.Split(" ")[0];
                string trans = line.Split(" ")[1];
                for(int i=0;i<mars.Length;i++)
                {
                    mars[i] = mars[i].Replace(marorg, trans);
                }
            }
            return mars;
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
                    string id = TheSong.Attribute("id").Value;
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
                System.IO.File.WriteAllLines(csvPathHDD, convertMarthan(lines.ToArray()));
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }
        public static async Task<IMessageBase[]> getSydxAsync(MiraiHttpSession session,bool isHDD = false)
        {

            SydxCsv csv = new SydxCsv(isHDD ? csvPathHDD : csvPath);
            int songid = new Random().Next(0, csv.songlist.Count);
            string text = csv.songlist[songid].name+"\n";
            text += csv.songlist[songid].artist + "\n";
            foreach (int i in csv.songlist[songid].diff){
                if (i != 0)
                    text += "[" + i + "]";
            }
            string jacketPath = Jacket.getJkPathById(csv.songlist[songid].id);
            ImageMessage jkt = await session.UploadPictureAsync(UploadTarget.Group, jacketPath);
            IMessageBase[] message = { jkt, new PlainMessage(text) };

            return message;
        }
        public static async Task<IMessageBase[]> getSydxAsync(MiraiHttpSession session, string lv, bool isHDD = false)
        {
            try
            {
                SydxCsv csv = new SydxCsv(isHDD ? csvPathHDD : csvPath);
                int level = int.Parse(lv);
                List<SydxSong> songlist = csv.songlist.FindAll(o => o.diff.Any(a => a == level));
                int songid = new Random().Next(0, songlist.Count);
                string text = songlist[songid].name + "\n";
                text += songlist[songid].artist + "\n";
                foreach (int i in songlist[songid].diff)
                {
                    if (i != 0)
                        text += "[" + i + "]";
                }
                string jacketPath = Jacket.getJkPathById(songlist[songid].id);
                ImageMessage jkt = await session.UploadPictureAsync(UploadTarget.Group, jacketPath);
                IMessageBase[] message = { jkt, new PlainMessage(text) };

                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                IMessageBase[] message = { new PlainMessage("你打一个" + lv + "给我看看哟？") };
                return message;
            }
        }
    }
}
