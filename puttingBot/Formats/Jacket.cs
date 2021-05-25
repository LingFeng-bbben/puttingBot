using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace puttingBot.Formats
{
    static class Jacket
    {
        static string jacketDir = "jacket/";
        public static string getJkPathById(string id,int lv=5)
        {
            string jacketPath = "jacket/jk_dummy_s.png";
            id = string.Format("{0:0000}", int.Parse(id));
            try
            {
                DirectoryInfo jkDir = new DirectoryInfo(jacketDir);
                DirectoryInfo[] jks = jkDir.GetDirectories();
                foreach (var jktd in jks)
                {
                    if (jktd.Name.StartsWith(id))
                    {
                        jacketPath = jacketDir + jktd.Name + "/jk_" + id + "_" + lv + "_s.png";
                        while (!File.Exists(jacketPath)&&lv>0)
                        {
                            lv--;
                            jacketPath = jacketDir + jktd.Name + "/jk_" + id + "_" + lv + "_s.png";
                        }
                        break;
                    }
                }
                return jacketPath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return jacketPath;
            }
        }
    }
}
