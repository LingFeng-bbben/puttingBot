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
            helpComment = "a网查分系统";
        }

        public static string BindUser(string username,long QQid)
        {
            AUserRecord record =Download.downloadJson<AUserRecord>("https://arcana.nu/api/v1/sdvx/5/profiles/?name=" + username.ToUpper(), cookie, auth);
            if (record.Items.Length <= 0)
                return "未在A网查找到用户.请检查用户名或网络服务提供者.";
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
            if (record.Items.Length > 1)
                return "存在多个用户.现已绑定" + record.Items[0].Name + "\n最后登录时间:" + record.Items[0].AccessTime.LocalDateTime+"\n如果这不是您的账户,请考虑使用id绑定.";
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
        public static string GetRecent(long QQid,int index=0)
        {
            try
            {
                BindList bindList = new BindList();
                if (File.Exists(bindfile))
                    bindList = JsonConvert.DeserializeObject<BindList>(File.ReadAllText(bindfile));
                if (!bindList.list.Exists(o => o.qqid == QQid)) return "未绑定.使用 #arcsdvx bind 【游戏用户名】，或 #arcsdvx bindid 【a网后台url内id】";
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
                IEnumerable<AMusicRecord> OrderRecords = records.OrderByDescending(o => o.Timestamp.Ticks);
                AMusicRecord recent = OrderRecords.ToArray()[index];
                Music music = musicData.Find(o => o.Id == recent.MusicId);
                Chart chart = chartData.Find(o => o.Id == recent.ChartId);
                string result = String.Format(
                    "『{0}』-{1}-\n" +
                    "「{2}」  {3}  {4}\n" +
                    "𝐂{7} 𝐍{8} 𝐄{9}\n" +
                    "{5} at {6:g}",
                    music.Title, emojiLevel(chart.Difficulty, chart.Rating),
                    styleGrade(recent.Grade), styleDigit(recent.Score), recent.Lamp,
                    aPlays[0].Related.Profiles[0].Name, recent.Timestamp.LocalDateTime,
                    recent.Critical, recent.Near, recent.Error);
                return result;
            }
            catch(Exception e)
            {
                return(e.Message + e.StackTrace);
            }
        }
        static string emojiLevel(string diff,long rati)
        {
            string emoji = "";
            switch (diff)
            {
                case "NOV":
                    emoji = "🟪";
                    break;
                case "ADV":
                    emoji = "🟨";
                    break;
                case "EXH":
                    emoji = "🟥";
                    break;
                case "INF":
                    emoji = "🟧";
                    break;
                case "MXM":
                    emoji = "⬜";
                    break;
            }
            return diff + emoji + rati;
        }
        static string styleDigit(long score)
        {
            string digit = string.Format("{0:D8}",score);
            digit = digit.Replace("0", "𝟎");
            digit = digit.Replace("1", "𝟏");
            digit = digit.Replace("2", "𝟐");
            digit = digit.Replace("3", "𝟑");
            digit = digit.Replace("4", "𝟒");
            digit = digit.Replace("5", "𝟓");
            digit = digit.Replace("6", "𝟔");
            digit = digit.Replace("7", "𝟕");
            digit = digit.Replace("8", "𝟖");
            digit = digit.Replace("9", "𝟗");
            return digit;
        }
        static string styleGrade(string grade)
        {
            switch (grade)
            {
                case "S":
                    return "𝓢";
                case "AAA_PLUS":
                    return "𝘼𝘼𝘼+";
                case "AAA":
                    return "𝘼𝘼𝘼";
                case "AA_PLUS":
                    return "𝘼𝘼+";
                case "AA":
                    return "𝘼𝘼";
                case "A_PLUS":
                    return "𝘼+";
                case "A":
                    return "𝘼";
                case "B":
                    return "𝔹";
                case "C":
                    return "𝑪";
                case "D":
                    return "𝐃";
                default:
                    return "";
            }
        }
    }
}
