using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.RBDX
{
    class RbdxCsv
    {
        public List<RbdxSong> list = new List<RbdxSong>();
        public RbdxCsv(string[] csv)
        {
            foreach(string a in csv)
            {
                list.Add(new RbdxSong(a));
            }
        }
    }

    class RbdxSong {
        public string id;
        public string name;
        public string artist;
        public string source;
        public string bpm;
        public string length;
        public RbdxFumen[] fumens = new RbdxFumen[4];
        internal int Parse(string a)
        {
            try
            {
                if (a.Contains("?"))
                    return 999;
                return a != "-" ? int.Parse(a) : 0;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("AT:"+a);
                return 0;
            }
        }
        public RbdxSong(string csv,string div = "	")
        {
            string[] TheSong = csv.Split(div);
            id = TheSong[0];
            name = TheSong[1];
            artist = TheSong[2];
            source = TheSong[3];
            bpm = TheSong[4];
            length = TheSong[6];
            fumens[0] = TheSong[8] != "-" ? new RbdxFumen(Parse(TheSong[8]),Parse(TheSong[9]), TheSong[12]) : new RbdxFumen();
            fumens[1] = TheSong[13] != "-" ? new RbdxFumen(Parse(TheSong[13]), Parse(TheSong[14]), TheSong[17]) : new RbdxFumen();
            fumens[2] = TheSong[18] != "-" ? new RbdxFumen(Parse(TheSong[18]),Parse(TheSong[19]), TheSong[22]) : new RbdxFumen();
            fumens[3] = TheSong[23] != "-" ? new RbdxFumen(Parse(TheSong[23]), Parse(TheSong[24]), TheSong[27]) : new RbdxFumen();
        }
        public override string ToString()
        {
            string message = name + "\n";//6 11 16
            foreach (RbdxFumen a in fumens)
            {
                message += a.level != -1 ? a.ToString() : "";
            }
            return message;
        }
    }
    class RbdxFumen {
        public string auther;
        public int noteCount;
        public float density;
        public int level;
        public int level2;

        public RbdxFumen(int lv,int lv2,string auth)
        {
            auther = auth;
            level = lv;
            level2 = lv2;
        }
        public RbdxFumen()
        {
            level = -1;
            level2 = 0;
        }
        public override string ToString()
        {
            string message = "[" + level.ToString() + (level2 != 0 ? "." + level2 : "") + ":" + auther + "]";
            return message;
        }
    }
}
