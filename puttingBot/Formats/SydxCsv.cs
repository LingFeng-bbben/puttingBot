using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats
{
    class SydxCsv
    {
        public List<SydxSong> songlist = new List<SydxSong>();
        public SydxCsv(string path)
        {
            string[] file = System.IO.File.ReadAllLines(path);
            foreach(string a in file)
            {
                songlist.Add(new SydxSong(a));
            }
        }
    }

    class SydxSong
    {
        public string id;
        public string name;
        public string artist;
        public int[] diff= new int[4];
        public SydxSong(string line)
        {
            try
            {
                string[] splited = line.Split("	");
                id = splited[0];
                name = splited[1];
                artist = splited[2];
                diff[0] = int.Parse(splited[3]);
                diff[1] = int.Parse(splited[4]);
                diff[2] = int.Parse(splited[5]);
                if (splited[6] != "") diff[3] = int.Parse(splited[6]);
                else if (splited[7] != "") diff[3] = int.Parse(splited[7]);
                else diff[3] = 0;
            }catch(Exception e)
            {
                //Console.WriteLine("SYDX convertion error.at"+line + e.Message + e.StackTrace);
            }
        }
    }
}
