using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using puttingBot.Formats.arca_a;
using Newtonsoft.Json;
using System.Linq;

namespace puttingBot.Commands
{
    class ArcScore : Command
    {
        static string cookie = File.ReadAllLines(authfile)[0];
        static string auth = File.ReadAllLines(authfile)[1];
        const string bindfile = "arcsdvx.json";
        const string authfile = "auth.txt";
        public ArcScore()
        {
            nameToCall = "arcsdvx";
            helpComment = "";
        }

        public static string BindUser(string username,long QQid)
        {
            AUserRecord record =Download.downloadJson<AUserRecord>("https://arcana.nu/api/v1/sdvx/5/profiles/?name=" + username.ToUpper(), cookie, auth);
            QABind bind = new QABind(record.Items[0].ProfileId, QQid);
            BindList bindList = new BindList();
            if (File.Exists(bindfile))
                bindList = JsonConvert.DeserializeObject<BindList>(File.ReadAllText(bindfile));
            if (bindList.list.Exists(o => o.qqid == QQid))
            {
                bindList.list.Remove(bindList.list.Find(o => o.qqid == QQid));
            }
            bindList.list.Add(bind);
            File.WriteAllText(bindfile, JsonConvert.SerializeObject(bindList));
            return "成功绑定" + record.Items[0].Name;
        }
        public static string Bindid(string arcid, long QQid)
        {
            AUserRecord record = Download.downloadJson<AUserRecord>("https://arcana.nu/api/v1/sdvx/5/profiles/?_id=" + arcid, cookie, auth);
            QABind bind = new QABind(record.Items[0].ProfileId, QQid);
            BindList bindList = new BindList();
            if (File.Exists(bindfile))
                bindList = JsonConvert.DeserializeObject<BindList>(File.ReadAllText(bindfile));
            if (bindList.list.Exists(o => o.qqid == QQid))
            {
                bindList.list.Remove(bindList.list.Find(o => o.qqid == QQid));
            }
            bindList.list.Add(bind);
            File.WriteAllText(bindfile, JsonConvert.SerializeObject(bindList));
            return "成功绑定" + record.Items[0].Name;
        }
        public static string GetRecent(long QQid)
        {
            try
            {
                BindList bindList = new BindList();
                if (File.Exists(bindfile))
                    bindList = JsonConvert.DeserializeObject<BindList>(File.ReadAllText(bindfile));
                if (!bindList.list.Exists(o => o.qqid == QQid)) return "未绑定";
                string arcid = bindList.list.Find(o => o.qqid == QQid).arcid;
                List<APlayRecord> aPlays = new List<APlayRecord>();
                aPlays.Add(Download.downloadJson<APlayRecord>("http://arcana.nu/api/v1/sdvx/5/player_bests/?profile_id=" + arcid, cookie, auth));
                while (aPlays.Last().Links.Next != null)
                {
                    Console.WriteLine("Next:" + aPlays.Last().Links.Next.OriginalString);
                    aPlays.Add(Download.downloadJson<APlayRecord>(aPlays.Last().Links.Next.OriginalString, cookie, auth));
                }
                List<AMusicRecord> records = new List<AMusicRecord>();
                List<Music> musicData = new List<Music>();
                List<Chart> chartData = new List<Chart>();
                foreach(APlayRecord a in aPlays)
                {
                    records.AddRange(a.Items);
                    musicData.AddRange(a.Related.Music);
                    chartData.AddRange(a.Related.Charts);
                }
                long maxTime = records.Max(o => o.Timestamp.Ticks);
                AMusicRecord recent = records.Find(o => o.Timestamp.Ticks == maxTime);
                Music music = musicData.Find(o => o.Id == recent.MusicId);
                Chart chart = chartData.Find(o => o.Id == recent.ChartId);

                return music.Title + " " + chart.Difficulty + chart.Rating + " " + recent.Grade + "\n" + recent.Lamp +"\n"+ recent.Score;
            }
            catch(Exception e)
            {
                return(e.Message + e.StackTrace);
            }
        }
    }
}
