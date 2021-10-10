using Mirai_CSharp.Models;
using puttingBot;
using Mirai_CSharp;
using System;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace puttingBot
{
    public static class Program // 前提: nuget Mirai-CSharp, 版本需要 >= 1.0.1.0
    {
        public const long selfQQnum = 2674713993;
        static MiraiHttpSessionOptions options;
        public static async Task Main()
        {
            //Console.WriteLine(Commands.WhatToPlaySdvx.getSdvx());
            options = new MiraiHttpSessionOptions("127.0.0.1", 7866, "114451441999");
            await connect();
            await Task.Delay(1);
        }
        
        public static async Task<bool> OnBotDropped(MiraiHttpSession session,IBotDroppedEventArgs e)
        {
            await connect();
            Console.WriteLine("Connection Failed:Reconnect");
            return true;
        }

        static MiraiHttpSession session;

        static Timer pso2timer = new Timer(checkpso2EQ);
        public static async Task connect()
        {
            if (session!= null)
                await session.DisposeAsync();
            session = new MiraiHttpSession(); 
            session.BotDroppedEvt += OnBotDropped;
            MessageProcrssGroup mp = new MessageProcrssGroup();
            MessageProcessFriend mpf = new MessageProcessFriend();
            session.AddPlugin(mpf);
            session.AddPlugin(mp);
            pso2timer.Change(0, 500000);
            while (true)
            {
                Console.WriteLine("Connect");
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
        static string lastEQ = "";
        static void checkpso2EQ(object state)
        {
            var html = Download.downloadText("https://acf.me.uk/");
            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);

            HtmlNode Node = htmldoc.DocumentNode.SelectSingleNode("//div[@class=\"inner\"]/div[@class=\"highlights\"]/section/div[@class=\"content\"]/header/p");
            var text = Node.InnerText.Replace("JST", "JST\n");
            var eqName = text.Split('\n')[1];
            if (lastEQ != eqName)
            {
                lastEQ = eqName;
                session.SendGroupMessageAsync(145424987, new PlainMessage(text));
                session.SendGroupMessageAsync(980879330, new PlainMessage(text));
            }
        }
    }
}
