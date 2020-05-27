﻿using Mirai_CSharp.Models;
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
        IMessageBase[] say(string a) {
            return new IMessageBase[]{new PlainMessage(a)};
        }
        public async Task<bool> GroupMessage(MiraiHttpSession session, IGroupMessageEventArgs e)
        {
            string message = "";
            string types = "";
            foreach(IMessageBase messageBase in e.Chain)
            {
                if (messageBase.Type == "Plain")
                    message = message + messageBase.ToString().Trim();
                if (messageBase.Type == "At") {
                    AtMessage at = (AtMessage)messageBase;
                    if (at.Target == Program.selfQQnum)
                    {
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("不要艾特我哟！！！"));
                        return false;
                    }
                }
                types = types+',' + messageBase.Type.Trim();
            }
            Console.WriteLine("[{0}]:{1}",types,message);
            if (message.StartsWith('#'))
            {
                message = message.Remove(0, 1).ToLower();
                List<string> messages = message.Split(' ').ToList();
                //Console.WriteLine(commands[0]);
                switch(messages[0])
                {
                    case "jrrp":
                        int rp = Commands.Jrrp.getJrrp(e.Sender.Id);
                        var reply = new IMessageBase[] { new AtMessage(e.Sender.Id, ""), new PlainMessage("今日人品是：" + rp+" 哟") };
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, reply);
                        return false;
                    case "help":
                        string helpMenu = "~~~帮助菜单哟~~~\n";
                        foreach(Commands.Command o in Commands.Command.commandsList)
                        {
                            helpMenu += String.Format("#{0}:{1}\n", o.nameToCall, o.helpComment);
                        }
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say(helpMenu));
                        return false;
                    case "sdvx":
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.WhatToPlaySdvx.getSdvx()));
                        return false;
                    case "gb":
                        if (messages.Count > 1) {
                            switch (messages[1])
                            {
                                case "help":
                                    await session.SendGroupMessageAsync(e.Sender.Group.Id, say(Commands.Gamble.Help()));
                                    return false;
                                case "gwq":
                                    if(Commands.Gamble.GiveMeMoney(e.Sender.Id))
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("没有布丁了吗？勉为其难给你100🍮哟"));
                                    else
                                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("你明明自己有🍮哟！！"));
                                    return false;
                                case "q":
                                    long money = Commands.Gamble.CheckMyMoney(e.Sender.Id);
                                    Console.WriteLine(money);
                                    if (money > 0)
                                    {
                                        var replys = new IMessageBase[] { new AtMessage(e.Sender.Id, ""), new PlainMessage($"现在有{money}🍮哟") };
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
                    default:
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, say("什么东西哟"));
                        return false;
                }
            }
            if (message.StartsWith("🍮"))
                await session.SendGroupMessageAsync(e.Sender.Group.Id, say("全部吃掉了哟"));
            return false;
        }
    }
}