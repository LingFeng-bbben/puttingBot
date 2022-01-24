using Mirai_CSharp;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace puttingBot.Commands
{
    class WhatToPlayRBDX : Command
    {
        const string filename = "RBDX.txt";
        const string jacketPath = "rbdxjacket/";
        const string apiPath = "https://chilundui.com/data/rbdx/image/song/500";//+id.png
        static Random random = new Random(new Guid().GetHashCode() + (int)DateTime.Now.Ticks);
        public WhatToPlayRBDX()
        {
            nameToCall = "rbdx";
            helpComment = "宫子帮你决定日本大学该玩什么歌哟";
        }
        public static async Task<IMessageBase[]> getRBDXAsync(MiraiHttpSession session)
        {
            string[] songStr = File.ReadAllLines(filename);
            Formats.RBDX.RbdxCsv songDB = new Formats.RBDX.RbdxCsv(songStr);
            List<Formats.RBDX.RbdxSong> levelList = songDB.list.FindAll(o => o.fumens.Max(p => p.level) <= 11);
            Console.WriteLine(levelList);
            int songCount = levelList.Count();
            int songid = random.Next(0, songCount);
            string text = levelList[songid].ToString();
            string pngpath = await cacheJacket(levelList[songid].id);
            //ImageMessage jkt = await session.UploadPictureAsync(UploadTarget.Group, pngpath);
            IMessageBase[] message = { new PlainMessage(text) };
            return message;
        }
        public static async Task<IMessageBase[]> getRBDXAsync(MiraiHttpSession session,int level)
        {
            try
            {
                if (level <= 0)
                    throw new Exception();
                string[] songStr = File.ReadAllLines(filename);
                Formats.RBDX.RbdxCsv songDB = new Formats.RBDX.RbdxCsv(songStr);
                List<Formats.RBDX.RbdxSong> levelList = songDB.list.FindAll(o => o.fumens.Any(p => p.level == level));
                Console.WriteLine(levelList);
                int songCount = levelList.Count();
                int songid = random.Next(0, songCount);
                string text = levelList[songid].ToString();
                string pngpath = await cacheJacket(levelList[songid].id);
                //ImageMessage jkt = await session.UploadPictureAsync(UploadTarget.Group, pngpath);
                IMessageBase[] message = { new PlainMessage(text) };
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return new IMessageBase[]{ new PlainMessage("你打一个" + level + "给我看看哟？")};
            }
        }
        public static async Task<string> cacheJacket(string id)
        {
            string filename = id + ".png";
            string filepath = jacketPath + filename;
            if (!File.Exists(filepath))
                await Task.Run(() => Download.downloadPic(apiPath + filename, filepath, false));
            return filepath;

        }
    }
}
