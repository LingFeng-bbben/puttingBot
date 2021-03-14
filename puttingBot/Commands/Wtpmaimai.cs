using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using puttingBot.Formats;

namespace puttingBot.Commands
{
    
    class Wtpmaimai:Command
    {
        public Wtpmaimai()
        {
            nameToCall = "maimai";
            helpComment = "舞萌DX玩啥";
        }
        static string jsonpath = "maimai.json";
        static public string getSong(string lv = "")
        {
            try
            {
                string json = File.ReadAllText(jsonpath);
                MaimaiJson songdb = JsonConvert.DeserializeObject<MaimaiJson>(json);
                if (lv != "")
                {
                    songdb.songs = songdb.songs.FindAll(o => o.diffs.Any(d => d.lv == lv));
                }
                int songid = new Random((int)DateTime.Now.Ticks).Next(0, songdb.songs.Count);
                string message = songdb.songs[songid].name + "\n";
                foreach (var a in songdb.songs[songid].diffs)
                {
                    message += "[" + a.lv;
                    message += a.isDX ? "DX" : "";
                    message += "] ";
                }
                return message;
            }
            catch
            {
                return "他说他要打" + lv;
            }
        }
    }
}
