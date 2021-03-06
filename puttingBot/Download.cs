using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.Encodings;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace puttingBot
{
    static class Download
    {
        public static string downloadPic(string url, string path)
        {
            WebClient wc = new WebClient();
            MD5 md5 = new MD5CryptoServiceProvider();

            Console.WriteLine("downloading");
            try
            {
                byte[] data = wc.DownloadData(url);
                Console.WriteLine("saving");
                string hash = Encoding.UTF8.GetString(md5.ComputeHash(data));
                MemoryStream ms = new MemoryStream(data);
                Image image = Image.FromStream(ms);
                Console.WriteLine(image.RawFormat.ToString());
                if (image.RawFormat.ToString() == "Gif")
                    return "false";
                string filename = hash + ".jpg";
                string filepath = path + filename;

                image.Save(filepath);
                return filepath;
            }
            catch (Exception e)
            {
                Console.WriteLine("failed." + e.Message);
                return "false";
            }
        }

        public static T downloadJson<T>(string url, string cookie = "", string bearer = "")
        {
            return JsonConvert.DeserializeObject<T>(downloadText(url, cookie, bearer));
        }

        public static string downloadText(string url,string cookie = "",string bearer = "",string usragt="")
        {

            WebClient wc = new WebClient();
            if (cookie != "") wc.Headers.Add("cookie", cookie);
            if (usragt != "") wc.Headers.Add("User-Agent", usragt);
            if (bearer != "") wc.Headers.Add("authorization", "Bearer " + bearer);

            Console.WriteLine("downloading " + url);
            try
            {
                byte[] data = wc.DownloadData(url);
                Console.WriteLine("saving");
                string text = Encoding.UTF8.GetString(data);
                return text;
            }
            catch (Exception e)
            {
                Console.WriteLine("failed." + e.Message);
                return default;
            }
        }
    }
}
