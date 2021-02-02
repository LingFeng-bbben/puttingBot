using System;
using System.Collections.Generic;
using System.Text;

namespace GroupSettting
{
    class Setting
    {
        public long groupNum { get; set; }
        public int bindingCity = 0;
        public bool disableDydy = false;
        //todo:增加黑名单
    }

    class Root
    {
        public List<Setting> data { get; set; }
    }
}
