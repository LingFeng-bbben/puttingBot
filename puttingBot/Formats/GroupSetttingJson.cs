using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.GroupSetting
{
    class GroupSetting
    {
        public long groupNum { get; set; }
        public int bindingCity = 0;
        public bool disableDydy = false;
        //todo:增加黑名单
    }

    class GroupSettingJson
    {
        public List<GroupSetting> data { get; set; }
    }
}
