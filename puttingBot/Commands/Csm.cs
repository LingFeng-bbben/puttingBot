using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace puttingBot.Commands
{
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
}
