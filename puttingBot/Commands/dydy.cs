using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace puttingBot.Commands
{
    class Dydy : Command
    {
        static string jsFilePath = "dydy.json";
        static Random random = new Random(new Guid().GetHashCode() + (int)DateTime.Now.Ticks);
        public Dydy()
        {
            nameToCall = "dydy";
            helpComment = "吊言吊语哟, #dydy add 添加新的语录";
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
}
