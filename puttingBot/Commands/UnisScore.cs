using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using puttingBot.Formats.Unis;
using Newtonsoft.Json;
using System.Linq;
using puttingBot.Formats;
using Mirai_CSharp.Models;
using Mirai_CSharp;
using System.Threading.Tasks;

namespace puttingBot.Commands
{
    class UnisScore : Command
    {
        const string bindfile = "unisdvx.json";
        const string authfile = "auth.txt";
        static string csvPathHDD = "sdvx.txt";

        public UnisScore()
        {
            nameToCall = "arcsdvx";
            helpComment = "世宇网查分系统";
        }

        public static string SendText(string phone, long QQid)
        {
            string response = Download.RequestPOST("https://iot.universal-space.cn/api/sms/captcha/get/" + phone);
            if (response.Contains("success"))
            {
                QUBind bind = new QUBind(phone, QQid);
                UBindList bindList = new UBindList();
                if (File.Exists(bindfile))
                    bindList = JsonConvert.DeserializeObject<UBindList>(File.ReadAllText(bindfile));
                if (bindList.list.Exists(o => o.qqid == QQid))
                {
                    bindList.list.Remove(bindList.list.Find(o => o.qqid == QQid));
                }
                bindList.list.Add(bind);
                File.WriteAllText(bindfile, JsonConvert.SerializeObject(bindList));
                return "发送成功, 登录：#unis login 验证码 (请注意本布丁和世宇没有任何关系)";
            }
            return "发送失败：" + response;
        }

        public static string Login(string captcha, long QQid) 
        {
            var bindList = JsonConvert.DeserializeObject<UBindList>(File.ReadAllText(bindfile));
            string phone = "";
            if (bindList.list.Exists(o => o.qqid == QQid))
            {
                phone = bindList.list.Find(o => o.qqid == QQid).phoneNumber;
            }
            else
            {
                return "请先发送验证码：#unis send 手机号";
            }
            var response = JsonConvert.DeserializeObject<LoginStatus>( 
                Download.RequestPOST("https://iot.universal-space.cn/api/unis/Myself/loginUser?mobile=" + phone + "&captcha=" + captcha));
            if (response.Code != 1) return response.Message;
            string token = response.Data.Token;
            bindList.list.Find(o => o.qqid == QQid).token = response.Data.Token;
            File.WriteAllText(bindfile, JsonConvert.SerializeObject(bindList));
            var status = Download.downloadJson<UnisStatus>(
                "https://iot.universal-space.cn/api/mns/mnsGameStatis/getUserRecordStatis?productId=3084", "", "", token);
            return "成功绑定" + status.Data.PlayerName;
        }

        public static string Status(long QQid)
        {
            var bindList = JsonConvert.DeserializeObject<UBindList>(File.ReadAllText(bindfile));
            if (!bindList.list.Exists(o => o.qqid == QQid))
            {
                return "请先发送验证码：#unis send 手机号";
            }
            string token = "";
            if(bindList.list.Find(o => o.qqid == QQid).token == "")
                return "登录：#unis login 验证码";
            token = bindList.list.Find(o => o.qqid == QQid).token;
            var status = Download.downloadJson<UnisStatus>(
                    "https://iot.universal-space.cn/api/mns/mnsGameStatis/getUserRecordStatis?productId=3084", "", "", token);
            if (status.Code == 1)
                return status.Data.PlayerName + " PC:"+status.Data.PlayCount + " PCB:"+status.Data.GamecoinBlock;
            else
                return status.Message+status.Detail;
        }

        
        public static async Task<IMessageBase[]> GetRecentAsync(long QQid, MiraiHttpSession session,int index=0)
        {
            try
            {
                var bindList = JsonConvert.DeserializeObject<UBindList>(File.ReadAllText(bindfile));
                if (!bindList.list.Exists(o => o.qqid == QQid))
                {
                    return new IMessageBase[] { new PlainMessage("请先发送验证码：#unis send 手机号") };
                }
                string token = "";
                if (bindList.list.Find(o => o.qqid == QQid).token == "")
                    return new IMessageBase[] { new PlainMessage("登录：#unis login 验证码") };
                token = bindList.list.Find(o => o.qqid == QQid).token;
                var status = Download.downloadJson<UnisStatus>(
                        "https://iot.universal-space.cn/api/mns/mnsGameStatis/getUserRecordStatis?productId=3084", "", "", token);
                if (status.Code != 1)
                    return new IMessageBase[] { new PlainMessage(status.Message + status.Detail) };
                index++;
                var playdata = Download.downloadJson<Unisdvx>(
                        "https://iot.universal-space.cn/api/mns/mnsGame/recordList?productId=3084&pageNo="+index+"&pageSize=1&orderBy=gameDate", "", "", token);
                var record = playdata.Data[0];
                string ingameID = record.MusicId.ToString();
                int ingameLv = (int)record.MusicGrade;
                //寻找图片:
                string jacketPath = Jacket.getJkPathById(ingameID, ingameLv+1);

                SydxCsv csv = new SydxCsv(csvPathHDD);
                int ingameRating = csv.songlist.Find(o => o.id == ingameID).diff[ingameLv>3?3:ingameLv];

                Console.WriteLine(jacketPath);

                string result = String.Format(
                    "『{0}』\n" +
                    "-{1}-\n" +
                    "「{2}」  {3}  \n" +
                    "{4}\n" +
                    "𝐂{7} 𝐍{8} 𝐄{9}\n" +
                    "{5} at {10} {6:g}",
                    record.MusicName, 
                    record.MusicGradeName+" "+ingameRating,
                    styleGrade(record.Score), styleDigit(record.Score), 
                    record.ClearTypeName.ToUpper(),
                    status.Data.PlayerName, record.GameDate.LocalDateTime,
                    record.CriticalCount, record.NearCount, record.ErrorCount,record.StoreName);
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
        static string styleGrade(long score)
        {
            string grade = "D";
            if (score > 0) grade = "打的什么臭狗屎";
            if (score > 8700000) grade = "𝔹";
            if (score > 9000000) grade = "𝘼+";
            if (score > 9300000) grade = "𝘼𝘼";
            if (score > 9500000) grade = "𝘼𝘼+";
            if (score > 9700000) grade = "𝘼𝘼𝘼";
            if (score > 9800000) grade = "𝘼𝘼𝘼+";
            if (score > 9900000) grade = "𝓢";
            return grade;
        }
    }
}
