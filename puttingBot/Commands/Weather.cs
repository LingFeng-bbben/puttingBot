using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace puttingBot.Commands
{
    class Weather : Command
    {
        public Weather()
        {
            nameToCall = "tq";
            helpComment = "查询天气";
        }

        public static string GetWeatherNow(int citycode = 58349)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);

            string weatherString = "布丁布丁，看看" + weather.data.predict.station.city + "现在天气怎么样哟！\n";

            WeatherJson.Weather wea = weather.data.real.weather;

            weatherString += wea.info + ", 温度" + wea.temperature + "℃, 湿度" + wea.humidity + "%\n";
            weatherString += "降水量:" + wea.rain + "mm";


            return weatherString;
        }

        public static string GetWeather(int citycode = 58349, int day = 0)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);
            WeatherJson.Detail detail = weather.data.predict.detail[day];

            string weatherString = "布丁布丁，播报天气哟！\n";
            weatherString += weather.data.predict.station.city + " " + detail.date + "\n";

            WeatherJson.Weather wea = new WeatherJson.Weather();
            if (detail.day.weather.info != "9999")
            {
                wea = detail.day.weather;
                weatherString += "白天\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃\n";
                wea = detail.night.weather;
                weatherString += "晚上\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃";
            }
            else
            {
                wea = detail.night.weather;
                weatherString += "晚上\n";
                weatherString += wea.info + ", 温度" + wea.temperature + "℃";
            }

            return weatherString;
        }

        public static string GetRadar(int citycode = 58349)
        {
            WeatherJson.Root weather = Download.downloadJson<WeatherJson.Root>("http://www.nmc.cn/rest/weather?stationid=" + citycode);
            string url = weather.data.radar.image;
            string picpath = Download.downloadPic("http://image.nmc.cn" + url, "/usr/weather/");
            return picpath;
        }

        public static string GetPrv()
        {
            PrvJson.Root[] roots = Download.downloadJson<PrvJson.Root[]>("http://www.nmc.cn/rest/province/all");
            string list = "tx夹我放不出名字，缩写自个儿看吧";
            foreach (PrvJson.Root prv in roots)
            {
                list += prv.Code + "，";
            }
            return list;
        }

        public static string GetCity(string prv)
        {
            CityJson.Root[] roots = Download.downloadJson<CityJson.Root[]>("http://www.nmc.cn/rest/province/" + prv);
            string list = "";
            foreach (CityJson.Root city in roots)
            {
                list += city.Code + " " + city.City + "\n";
            }
            return list;
        }

        public static string GetAnalyze()
        {
            string html = Download.downloadText("http://www.nmc.cn/publish/observations/china/dm/radar-h000.htm");
            var htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);

            HtmlNode imgNode = htmldoc.DocumentNode.SelectSingleNode("//div[@class=\"container\"]/div[@class=\"row\"]/div[@class=\"col-xs-10\"]/div[@class=\"bgwhite\"]/div[@class=\"row\"]/div[@class=\"col-xs-10\"]/div[@class=\"imgblock\"]/img[@id=\"imgpath\"]");
            string url = imgNode.Attributes.First(o => o.Name == "src").Value;
            Console.WriteLine(url);
            string picpath = Download.downloadPic(url, "/usr/weather/");
            return picpath;
        }
    }
}
