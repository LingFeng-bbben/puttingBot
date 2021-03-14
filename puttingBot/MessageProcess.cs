using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Mirai_CSharp;
using System.Linq;
using puttingBot.Formats.GroupSetting;
using puttingBot.Formats.Dydy;

namespace puttingBot
{
    public class MessageProcrss : IGroupMessage
    {
        List<string> lastmessages = new List<string>();
        IMessageBase[] say(string a) {
            Console.WriteLine("REPLY: " + a);
            return new IMessageBase[] { new PlainMessage(a) };
        }
        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            try {
                string message = "";
                string imgurl = "";
                string types = "";
                foreach (IMessageBase messageBase in e.Chain)
                {
                    if (messageBase.Type == "Plain")
                        message = message + messageBase.ToString();
                    if (messageBase.Type == "At") {
                        AtMessage at = (AtMessage)messageBase;
                        if (at.Target == Program.selfQQnum)
                        {
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say("不要艾特我哟！！！"));
                            return false;
                        }
                    }
                    if (messageBase.Type == "Image") {
                        ImageMessage img = (ImageMessage)messageBase;
                        imgurl = img.Url;
                    }
                    types = types + ',' + messageBase.Type.Trim();
                }
                Console.WriteLine("{2}:[{0}]({3}):{1}", types, message, e.Sender.Id, imgurl);
                //
                GroupSetting groupsetting = new GroupSetting();
                groupsetting.groupNum = e.Sender.Group.Id;
                try
                {
                    groupsetting = GetGSetting.GetSetting(e.Sender.Group.Id);
                }
                catch
                {
                    Console.WriteLine("未找到设定,正在创建设定档");
                    GetGSetting.SetSetting(groupsetting);
                }
                //各种命令
                if (message.StartsWith('#'))
                {
                    message = message.Remove(0, 1);
                    List<string> messages = message.Split(' ').ToList();
                    messages[0] = messages[0].ToLower();
                    messages[0] = messages[0].Trim();
                    switch (messages[0])
                    {
                        case "maimai":
                            if (messages.Count > 1)
                            {
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Wtpmaimai.getSong(messages[1])));
                                return false;
                            }
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Wtpmaimai.getSong()));
                            return false;
                        case "jrrp":
                            int rp = Commands.Jrrp.getJrrp(e.Sender.Id);
                            var reply = new IMessageBase[] { new AtMessage(e.Sender.Id), new PlainMessage("今日人品是：" + rp + " 哟") };
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, reply);
                            return false;
                        case "help":
                            string helpMenu = "~~~帮助菜单哟~~~\n";
                            foreach (Commands.Command o in Commands.Command.commandsList)
                            {
                                helpMenu += String.Format("#{0}:{1}\n", o.nameToCall, o.helpComment);
                            }
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(helpMenu));
                            return false;
                        case "sdvx":
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlaySdvx.getSdvx()));
                            return false;
                        case "arcsdvx":
                            if (messages.Count > 2)
                            {
                                if (messages[1] == "bind")
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.ArcScore.BindUser(messages[2], e.Sender.Id)));
                                if (messages[1] == "bindid")
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.ArcScore.Bindid(messages[2], e.Sender.Id)));
                                return false;
                            }
                            if (messages.Count > 1)
                            {
                                int index = int.Parse(messages[1]);
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.ArcScore.GetRecent(e.Sender.Id, index)));
                                return false;
                            }
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.ArcScore.GetRecent(e.Sender.Id)));
                            return false;
                        case "chuni":
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlayChuni.getChuni()));
                            return false;
                        case "rbdx":
                            if (messages.Count > 1)
                            {
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlayRBDX.getRBDX(int.Parse(messages[1]))));
                                return false;
                            }
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlayRBDX.getRBDX()));
                            return false;
                        case "dydy":
                            if (messages.Count > 1)
                            {
                                if (messages[1] == "add")
                                {
                                    string dydy="";
                                    if (messages.Count > 2)
                                        dydy = message.Remove(0, 9);
                                    if (imgurl == "") {
                                        Commands.Dydy.add(new DyItem(e.Sender.Id, dydy));
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                                    }
                                    else
                                    {
                                        DyItem dyItem = new DyItem(e.Sender.Id, dydy);
                                        dyItem.pic = imgurl;
                                        Commands.Dydy.add(dyItem);
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加(含一张图片)"));
                                    }
                                }
                                if (messages[1] == "disable" && e.Sender.Permission > GroupPermission.Member)
                                {
                                    groupsetting.disableDydy = true;
                                    GetGSetting.SetSetting(groupsetting);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已停止dydy在本群的响应"));
                                }
                                if (messages[1] == "enable" && e.Sender.Permission > GroupPermission.Member)
                                {
                                    groupsetting.disableDydy = false;
                                    GetGSetting.SetSetting(groupsetting);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, Commands.Dydy.read());
                                }
                                if (messages[1] == "archive" && e.Sender.Permission > GroupPermission.Member)
                                {
                                    Commands.Dydy.archive();
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已归档30天前的dydy"));
                                }
                                if (messages[1] == "old")
                                {
                                    if (!groupsetting.disableDydy)
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, Commands.Dydy.read(true));
                                }
                                if (messages[1] != "add" && messages[1].StartsWith("add"))
                                {
                                    string dydy = message.Remove(0, 8);
                                    if (imgurl == "")
                                    {
                                        Commands.Dydy.add(new DyItem(e.Sender.Id, dydy));
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                                    }
                                    else
                                    {
                                        DyItem dyItem = new DyItem(e.Sender.Id, dydy);
                                        dyItem.pic = imgurl;
                                        Commands.Dydy.add(dyItem);
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加(含一张图片)"));
                                    }
                                }
                            }
                            else
                            {
                                if (!groupsetting.disableDydy)
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, Commands.Dydy.read());
                            }
                            return false;
                        case "csm":
                            if (messages.Count > 1)
                            {
                                if (messages[1] == "add")
                                {
                                    string dydy = message.Remove(0, 8);
                                    Commands.Csm.add(e.Sender.Id, dydy);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                                }
                                if (messages[1] != "add" && messages[1].StartsWith("add"))
                                {
                                    string dydy = message.Remove(0, 7);
                                    Commands.Csm.add(e.Sender.Id, dydy);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                                }
                            }
                            else
                            {
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Csm.read()));
                            }
                            return false;
                        case "long":
                            string path = Commands.Long.cacLong(imgurl);
                            Console.WriteLine("UPloading:" + path);
                            if (path != "false")
                            {
                                try
                                {
                                    ImageMessage msg = await session.UploadPictureAsync(UploadTarget.Group, path);
                                    IMessageBase[] chain = new IMessageBase[] { msg };
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, chain);
                                    System.IO.File.Delete(path);
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee.Message);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("本布丁认为：本图无龙哟"));
                                }
                            }
                            else
                            {
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("解析出错了哟"));
                            }
                            return false;
                        case "tq":
                            if (messages.Count > 1)
                            {
                                if (messages[1] == "mt")
                                {
                                    string tq = Commands.Weather.GetWeather(groupsetting.bindingCity, 1);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(tq));
                                }
                                if (messages[1] == "ld")
                                {
                                    string picpath = Commands.Weather.GetRadar(groupsetting.bindingCity);
                                    try
                                    {
                                        ImageMessage msg = await session.UploadPictureAsync(UploadTarget.Group, picpath);
                                        IMessageBase[] chain = new IMessageBase[] { msg };
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, chain);
                                        System.IO.File.Delete(picpath);
                                    }
                                    catch (Exception ee)
                                    {
                                        Console.WriteLine(ee.Message);
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("出错了哟"));
                                    }
                                }
                                if (messages[1] == "prvlist")
                                {
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Weather.GetPrv()));
                                }
                                if (messages[1] == "citylist")
                                {
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Weather.GetCity(messages[2])));
                                }
                                if (messages[1] == "bind")
                                {
                                    groupsetting.bindingCity = int.Parse(messages[2]);
                                    string tq = Commands.Weather.GetWeatherNow(groupsetting.bindingCity);
                                    GetGSetting.SetSetting(groupsetting);
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("成功绑定！！\n" + tq));
                                }
                                if (messages[1] == "ana")
                                {
                                    string picpath = Commands.Weather.GetAnalyze();
                                    try
                                    {
                                        ImageMessage msg = await session.UploadPictureAsync(UploadTarget.Group, picpath);
                                        IMessageBase[] chain = new IMessageBase[] { msg };
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, chain);
                                        System.IO.File.Delete(picpath);
                                    }
                                    catch (Exception ee)
                                    {
                                        Console.WriteLine(ee.Message);
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("出错了哟"));
                                    }
                                }
                            }
                            else
                            {
                                string tq = Commands.Weather.GetWeatherNow(groupsetting.bindingCity);
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say(tq));
                            }
                            return false;
                        default:
                            Console.WriteLine("Unknown command:" + messages[0]);
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say("什么东西哟"));
                            return false;
                    }
                }
                //关键词触发
                if (message.Contains("njmlp"))
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("にじゃまれぴ！"));
                if (message.Contains("🍮"))
                {
                    if (message.Contains("💩"))
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("味道有点怪哟"));
                    else
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("全部吃掉了哟"));
                }
                if (message.Contains("布")&& message.Contains("丁"))
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("喊你祖宗干嘛"));
                if (message.StartsWith("我要出勤"))
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Bus.chuqing()));
                }
                if (message.StartsWith("观前回家"))
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Bus.huijia_gq()));
                }
                if (message.StartsWith("苏中回家"))
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Bus.huijia_sz()));
                }

                //复读
                if (repeatedmessage == "") repeatedmessage = message;
                if (lastmessages.All(o => o == message) && repeatedmessage != message)
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(message));
                    repeatedmessage = message;
                }
                lastmessages.Add(message);
                if (lastmessages.Count > 2)
                {
                    lastmessages.RemoveAt(0);
                }


                return false;
            }catch(Exception r)
            {
                Console.WriteLine(r.Message + "\n" + r.StackTrace);
                return false;
            }
        }
        string repeatedmessage = "";
    }
}
