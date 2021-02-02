using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace puttingBot.Commands
{
    class Long : Command
    {
        static string longRepoPath = "/usr/torch/";
        public Long()
        {
            nameToCall = "long";
            helpComment = "加上带人脸图片，计算龙度";
        }
        public static string cacLong(string url)
        {
            try
            {
                string resultpath = Download.downloadPic(url, longRepoPath + "imgreco/");
                resultpath = resultpath.Replace("imgreco", "imgresult");
                Process p = new Process();
                p.StartInfo.FileName = "bash";
                p.StartInfo.Arguments = "/usr/torch/run.sh";
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                Console.WriteLine(p.StandardOutput.ReadToEnd());

                //System.Diagnostics.Process.Start("bash /usr/torch/run.sh"); //this do not work
                return resultpath;
            }
            catch (Exception e)
            {
                Console.WriteLine("failed." + e.Message);
                return "false";
            }
        }
    }
}
