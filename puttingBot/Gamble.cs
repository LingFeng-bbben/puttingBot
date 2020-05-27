using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Gambleing;
using System.Linq;

namespace puttingBot.Commands
{
    partial class Gamble:Command
    {
        static DataStorge data = new DataStorge(DataStorge.fileName);
        public static string Help()
        {
            return "gwq:给你钱\nq:查看余额";
        }
        public static bool GiveMeMoney(long qqNum)
        {
            if (data.usrData.Any(o => o.qqNum == qqNum))
            {
                if (data.usrData.Any(o => o.qqNum == qqNum && o.money == 0))
                {
                    data.usrData.Where(o => o.qqNum == qqNum && o.money == 0).First().money = 100;
                    data.SaveData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                data.usrData.Add(new User(qqNum, 100));
                data.SaveData();
                return true;
            }
        }
        public static long CheckMyMoney(long qqNum)
        {
            if (data.usrData.Any(o => o.qqNum == qqNum))
            {
                return data.usrData.Where(o => o.qqNum == qqNum).First().money;
            }
            else
            {
                return -1;
            }
        }
    }
}

namespace Gambleing
{
    class User
    {
        public long qqNum;
        public long money;
        public User(long _qqNum,long _money)
        {
            qqNum = _qqNum;
            money = _money;
        }
    }
    class DataStorge
    {
        public List<User> usrData = new List<User>();
        public static string fileName = Environment.CurrentDirectory+ @"\gamble.json";
        public DataStorge(string _fileName)
        {
            if (File.Exists(_fileName))
                usrData = JsonConvert.DeserializeObject<DataStorge>(File.ReadAllText(_fileName)).usrData;
            else
            {
                Console.WriteLine($"Warning:{_fileName} does not exist");
            }
        }
        public void SaveData()
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }
    }
}