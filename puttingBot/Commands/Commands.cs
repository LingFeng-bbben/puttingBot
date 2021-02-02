using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Diagnostics;
using HtmlAgilityPack;

namespace puttingBot.Commands
{
    class Command
    {
        public string nameToCall = "";
        public string helpComment = "";
        public static Object[] commandsList = { 
            new Help(),
            new Jrrp(),
            new Dydy(),
            new Weather(),
            new WhatToPlaySdvx(),
            new WhatToPlayChuni(),
            new WhatToPlayRBDX(),
            new Long()
        };
    }
}
