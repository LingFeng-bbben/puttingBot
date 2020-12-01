using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using GroupSettting;
using Newtonsoft.Json;

namespace puttingBot
{
    static class GetGSetting
    {
        const string jsonFile = "GroupSetting.json";
        public static Setting GetSetting(long groupId)
        {
            string json = File.ReadAllText(jsonFile);
            Root root = JsonConvert.DeserializeObject<Root>(json);
            Setting setting = root.data.Find(o => o.groupNum == groupId);
            if (setting != null)
                return setting;
            else
                throw new Exception("No record");
        }
        public static void SetSetting(Setting setting)
        {
            Root root = new Root();
            if (File.Exists(jsonFile))
            {
                string json = File.ReadAllText(jsonFile);
                root = JsonConvert.DeserializeObject<Root>(json);
            }
            else
            {
                root.data = new List<Setting>();
            }
            if(root.data.Any(o => o.groupNum == setting.groupNum))
            {
                root.data.Remove(root.data.Find(o => o.groupNum == setting.groupNum));
            }
            root.data.Add(setting);
            string newJson = JsonConvert.SerializeObject(root);
            File.WriteAllText(jsonFile,newJson);
        }
    }
}
