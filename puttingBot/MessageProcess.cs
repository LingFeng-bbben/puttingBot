using Mirai_CSharp.Models;
using Mirai_CSharp.Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Mirai_CSharp;
using System.Linq;

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
            Console.WriteLine("{2}:[{0}]({3}):{1}", types, message, e.Sender.Id,imgurl);
            //各种命令
            if (message.StartsWith('#') && e.Sender.Id != 1780202038)
            {
                message = message.Remove(0, 1);
                List<string> messages = message.Split(' ').ToList();
                messages[0] = messages[0].ToLower();
                messages[0] = messages[0].Trim();
                //Console.WriteLine(commands[0]);
                switch (messages[0])
                {
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
                    case "chuni":
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlayChuni.getChuni()));
                        return false;
                    case "gb":
                        if (messages.Count > 1) {
                            switch (messages[1])
                            {
                                case "help":
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Gamble.Help()));
                                    return false;
                                case "gwq":
                                    if (Commands.Gamble.GiveMeMoney(e.Sender.Id))
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("没有布丁了吗？勉为其难给你100🍮哟"));
                                    else
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("你明明自己有🍮哟！！"));
                                    return false;
                                case "q":
                                    long money = Commands.Gamble.CheckMyMoney(e.Sender.Id);
                                    Console.WriteLine(money);
                                    if (money > 0)
                                    {
                                        var replys = new IMessageBase[] { new AtMessage(e.Sender.Id), new PlainMessage($"现在有{money}🍮哟") };
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, replys);
                                    }
                                    else
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("看来你没有布丁哟？"));
                                    return false;
                                default:
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("什么东西哟"));
                                    return false;
                            }
                        }
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Gamble.Help()));
                        return false;
                    case "dydy":
                        if (messages.Count > 1)
                        {
                            if (messages[1] == "add")
                            {
                                string dydy = message.Remove(0,9); 
                                Commands.Dydy.add(e.Sender.Id,dydy);
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                            }
                            if (messages[1] != "add" && messages[1].StartsWith("add"))
                            {
                                string dydy = message.Remove(0,8);
                                Commands.Dydy.add(e.Sender.Id, dydy);
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("已添加"));
                            }
                        }
                        else
                        {
                            await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Dydy.read()));
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
                                IMessageBase[] chain = new IMessageBase[] { msg }; // 数组里边可以加上更多的 IMessageBase, 以此达到例如图文并发的情况
                                await session.SendGroupMessageAsync(e.Sender.Group.Id, chain); // 自己填群号, 一般由 IGroupMessageEventArgs 提供
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
                    default:
                        Console.WriteLine("Unknown command:" + messages[0]);
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("什么东西哟"));
                        return false;
                }
            }
            if (message.Contains("樱语"))
                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("嗯的是的哟"));
            if (message.Contains("🍮") && e.Sender.Id != 1780202038)
            {
                if (message.Contains("💩"))
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("味道有点怪哟"));
                else
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say("全部吃掉了哟"));
            }
            if (message.Contains("脚本"))
                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("脚本哥，差不多得了哟"));
            if (message.Contains("/mxh") && e.Sender.Id == 1780202038)
            {
                var cps = message.Split(" ");
                try
                {
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(cps[1] + "和" + cps[2] + "希望你不要再当脚本哥了"));
                }
                catch { }
            }

            if (repeatedmessage == "") repeatedmessage = message;
            if (lastmessages.All(o => o == message)&&repeatedmessage != message)
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
        }
        string repeatedmessage = "";
    }
}
