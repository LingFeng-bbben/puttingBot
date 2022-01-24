using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using puttingBot.Formats.Dydy;
using Mirai_CSharp.Models;

namespace puttingBot.Commands
{
    class Dydy : Command
    {
        static string jsFilePath = "dydy.json";
        static string jsArchivePath = "dydyArchve.json";
        static Random random = new Random(new Guid().GetHashCode() + (int)DateTime.Now.Ticks);
        public static int archiveDate = 10;
        public Dydy()
        {
            nameToCall = "dydy";
            helpComment = "吊言吊语哟, #dydy add 添加新的语录";
        }
        public static void add(DyItem a)
        {
            string text = File.ReadAllText(jsFilePath);
            DydyJson dydyJson = JsonConvert.DeserializeObject<DydyJson>(text);
            if (!dydyJson.dyItems.Any(o => o.dyData == a.dyData && o.pic == a.pic))
                dydyJson.dyItems.Add(a);
            File.WriteAllText(jsFilePath, JsonConvert.SerializeObject(dydyJson, Formatting.Indented));
        }
        public static IMessageBase[] read(bool isArchive = false)
        {

            string text = "";
            if (isArchive)
                text = File.ReadAllText(jsArchivePath);
            else
                text = File.ReadAllText(jsFilePath);
            DydyJson dydyJson = JsonConvert.DeserializeObject<DydyJson>(text);
            int radIndex = 0;
            for (int i = 0; i < random.Next(5, random.Next(5, 88)); i++)
                radIndex = random.Next(0, dydyJson.dyItems.Count);
            try
            {
                List<IMessageBase> message = new List<IMessageBase>();
                if (dydyJson.dyItems[radIndex].dyData != "")
                    message.Add(new PlainMessage(dydyJson.dyItems[radIndex].dyData));
                if (dydyJson.dyItems[radIndex].pic != "")
                {
                    message.Add(new ImageMessage(null, dydyJson.dyItems[radIndex].pic, null));
                    var result = Download.downloadPic(dydyJson.dyItems[radIndex].pic, "dydyCache/" + dydyJson.dyItems[radIndex].timeAdded.Ticks+".jpg",false);
                    if (result == "false") throw new Exception("corroped");
                }
                return message.ToArray();
            }
            catch
            {
                return new IMessageBase[] { new PlainMessage("肥肠抱歉，此DYDY档已经毁损\nTime:" + dydyJson.dyItems[radIndex].timeAdded) };
            }
        }
        public static void archive()
        {
            string text = File.ReadAllText(jsFilePath);
            DydyJson dydyJson = JsonConvert.DeserializeObject<DydyJson>(text);
            DydyJson dydyJsonA = new DydyJson();
            if (File.Exists(jsArchivePath))
            {
                string textA = File.ReadAllText(jsArchivePath);
                dydyJsonA = JsonConvert.DeserializeObject<DydyJson>(textA);
            }
            dydyJsonA.dyItems.AddRange(dydyJson.dyItems.Where(o => o.timeAdded.AddDays(archiveDate) < DateTime.Now));
            dydyJson.dyItems.RemoveAll(o => o.timeAdded.AddDays(archiveDate) < DateTime.Now);
            File.WriteAllText(jsFilePath, JsonConvert.SerializeObject(dydyJson, Formatting.Indented));
            File.WriteAllText(jsArchivePath, JsonConvert.SerializeObject(dydyJsonA, Formatting.Indented));
        }
    }
}
