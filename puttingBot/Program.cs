using Mirai_CSharp.Models;
using puttingBot;
using Mirai_CSharp;
using System;
using System.Threading.Tasks;

namespace puttingBot
{
    public static class Program // 前提: nuget Mirai-CSharp, 版本需要 >= 1.0.1.0
    {
        public const long selfQQnum = 2674713993;
        public static async Task Main()
        {
            //Console.WriteLine(Commands.WhatToPlaySdvx.getSdvx());
            MiraiHttpSessionOptions options = new MiraiHttpSessionOptions("127.0.0.1", 7866, "114451441999");
            await using MiraiHttpSession session = new MiraiHttpSession();
            MessageProcrss mp = new MessageProcrss();
            session.AddPlugin(mp);
            while (true)
            {
                await session.ConnectAsync(options, selfQQnum); 
                while (session.Connected)
                {
                    if (await Console.In.ReadLineAsync() == "exit")
                    {
                        return;
                    }
                }
            }
        }
    }
}
