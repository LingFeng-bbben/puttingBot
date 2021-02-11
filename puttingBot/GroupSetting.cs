using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using puttingBot.Formats.GroupSetting;
using Newtonsoft.Json;

namespace puttingBot
{
    static class GetGSetting
    {
        const string jsonFile = "GroupSetting.json";
        public static GroupSetting GetSetting(long groupId)
        {
            string json = File.ReadAllText(jsonFile);
            GroupSettingJson root = JsonConvert.DeserializeObject<GroupSettingJson>(json);
            GroupSetting setting = root.data.Find(o => o.groupNum == groupId);
            if (setting != null)
                return setting;
            else
                throw new Exception("No record");
        }
        public static void SetSetting(GroupSetting setting)
        {
            GroupSettingJson root = new GroupSettingJson();
            if (File.Exists(jsonFile))
            {
                string json = File.ReadAllText(jsonFile);
                root = JsonConvert.DeserializeObject<GroupSettingJson>(json);
            }
            else
            {
                root.data = new List<GroupSetting>();
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
