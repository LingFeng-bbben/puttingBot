using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats
{
    class MaimaiJson
    {
        public List<Song> songs = new List<Song>();
    }
    class Song
    {
        public string name;
        public List<Difficulty> diffs = new List<Difficulty>();
        public Song(string _name)
        {
            name = _name;
        }
    }
    class Difficulty
    {
        public string lv;
        public bool isDX;
    }
}
