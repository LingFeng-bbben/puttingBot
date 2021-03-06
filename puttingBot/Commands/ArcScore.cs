﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using puttingBot.Formats.arca_a;
using Newtonsoft.Json;
using System.Linq;
using puttingBot.Formats;
using Mirai_CSharp.Models;
using Mirai_CSharp;
using System.Threading.Tasks;

namespace puttingBot.Commands
{
    class ArcScore : Command
    {
        static string cookie = File.ReadAllLines(authfile)[0];
        static string auth = File.ReadAllLines(authfile)[1];
        const string bindfile = "arcsdvx.json";
        const string authfile = "auth.txt";
        static string csvPathHDD = "sdvx.txt";

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
            ABindList bindList = new ABindList();
            if (File.Exists(bindfile))
                bindList = JsonConvert.DeserializeObject<ABindList>(File.ReadAllText(bindfile));
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
            ABindList bindList = new ABindList();
            if (File.Exists(bindfile))
                bindList = JsonConvert.DeserializeObject<ABindList>(File.ReadAllText(bindfile));
            if (bindList.list.Exists(o => o.qqid == QQid))
            {
                bindList.list.Remove(bindList.list.Find(o => o.qqid == QQid));
            }
            bindList.list.Add(bind);
            File.WriteAllText(bindfile, JsonConvert.SerializeObject(bindList));
            return "成功绑定" + record.Items[0].Name;
        }
        public static async Task<IMessageBase[]> GetRecentAsync(long QQid, MiraiHttpSession session,int index=0)
        {
            try
            {
                ABindList bindList = new ABindList();
                if (File.Exists(bindfile))
                    bindList = JsonConvert.DeserializeObject<ABindList>(File.ReadAllText(bindfile));
                if (!bindList.list.Exists(o => o.qqid == QQid)) {
                    IMessageBase[] errmessage = { new PlainMessage("未绑定.使用 #arcsdvx bind 【游戏用户名】，或 #arcsdvx bindid 【a网后台url内id】") };
                    return errmessage;
                }
                string arcid = bindList.list.Find(o => o.qqid == QQid).arcid;
                List<APlayRecord> aPlays = new List<APlayRecord>();
                await Task.Run(()=>aPlays.Add(Download.downloadJson<APlayRecord>("http://arcana.nu/api/v1/sdvx/5/player_bests/?profile_id=" + arcid, cookie, auth)));
                while (aPlays.Last().Links.Next != null)
                {
                    Console.WriteLine("Next:" + aPlays.Last().Links.Next.OriginalString);
                    await Task.Run(() => aPlays.Add(Download.downloadJson<APlayRecord>(aPlays.Last().Links.Next.OriginalString, cookie, auth)));
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
                //寻找chartid：
                SydxCsv csv = new SydxCsv(csvPathHDD);
                string ingameID = csv.songlist.Find(o => o.name == music.Title).id;
                int ingameLv = digitLevel(chart.Difficulty);
                //寻找图片:
                string jacketPath = Jacket.getJkPathById(ingameID, ingameLv);

                Console.WriteLine(jacketPath);

                string result = String.Format(
                    "『{0}』-{1}-\n" +
                    "「{2}」  {3}  {4}\n" +
                    "𝐂{7} 𝐍{8} 𝐄{9}\n" +
                    "{5} at {6:g}",
                    music.Title, emojiLevel(chart.Difficulty, chart.Rating),
                    styleGrade(recent.Grade), styleDigit(recent.Score), recent.Lamp,
                    aPlays[0].Related.Profiles[0].Name, recent.Timestamp.LocalDateTime,
                    recent.Critical, recent.Near, recent.Error);
                /*
                 * dump score
                APlayRecord playRecord = new APlayRecord();
                playRecord.Items = OrderRecords.ToArray();
                Related related = new Related();
                related.Charts = chartData.ToArray();
                related.Music = musicData.ToArray();
                playRecord.Related = related;
                string record = JsonConvert.SerializeObject(playRecord);
                File.WriteAllText(QQid + "_APlayRecord.json", record);
                */
                ImageMessage jkt = await session.UploadPictureAsync(UploadTarget.Group, jacketPath);
                IMessageBase[] message = { jkt,new PlainMessage(result) };
                return message;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return null;
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
        static int digitLevel(string diff)
        {
            int digit = 1;
            switch (diff)
            {
                case "NOV":
                    digit = 1;
                    break;
                case "ADV":
                    digit = 2;
                    break;
                case "EXH":
                    digit = 3;
                    break;
                case "INF":
                    digit = 4;
                    break;
                case "MXM":
                    digit = 5;
                    break;
            }
            return digit;
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
