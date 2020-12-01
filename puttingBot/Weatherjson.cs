using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherJson
{
    public class Station
    {
        public string? code { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? url { get; set; }
    }

    public class Weather
    {
        public double? temperature { get; set; }
        public double? temperatureDiff { get; set; }
        public double? airpressure { get; set; }
        public double? humidity { get; set; }
        public double? rain { get; set; }
        public int? rcomfort { get; set; }
        public int? icomfort { get; set; }
        public string? info { get; set; }
        public string? img { get; set; }
        public double? feelst { get; set; }
    }

    public class Wind
    {
        public string? direct { get; set; }
        public string? power { get; set; }
        public string? speed { get; set; }
    }

    public class Warn
    {
        public string? alert { get; set; }
        public string? pic { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? url { get; set; }
        public string? issuecontent { get; set; }
        public string? fmeans { get; set; }
        public string? signaltype { get; set; }
        public string? signallevel { get; set; }
        public string? pic2 { get; set; }
    }

    public class Real
    {
        public Station? station { get; set; }
        public string? publish_time { get; set; }
        public Weather? weather { get; set; }
        public Wind? wind { get; set; }
        public Warn? warn { get; set; }
    }



    public class Day
    {
        public Weather? weather { get; set; }
        public Wind? wind { get; set; }
    }

    public class Night
    {
        public Weather? weather { get; set; }
        public Wind? wind { get; set; }
    }

    public class Detail
    {
        public string? date { get; set; }
        public string? pt { get; set; }
        public Day? day { get; set; }
        public Night? night { get; set; }
    }

    public class Predict
    {
        public Station? station { get; set; }
        public string? publish_time { get; set; }
        public List<Detail> detail { get; set; }
    }

    public class Air
    {
        public string? forecasttime { get; set; }
        public int? aqi { get; set; }
        public int? aq { get; set; }
        public string? text { get; set; }
        public string? aqiCode { get; set; }
    }

    public class Tempchart
    {
        public string? time { get; set; }
        public double? max_temp { get; set; }
        public double? min_temp { get; set; }
        public string? day_img { get; set; }
        public string? day_text { get; set; }
        public string? night_img { get; set; }
        public string? night_text { get; set; }
    }

    public class Passedchart
    {
        public double? rain1h { get; set; }
        public double? rain24h { get; set; }
        public double? rain12h { get; set; }
        public double? rain6h { get; set; }
        public double? temperature { get; set; }
        public string? tempDiff { get; set; }
        public double? humidity { get; set; }
        public double? pressure { get; set; }
        public double? windDirection { get; set; }
        public double? windSpeed { get; set; }
        public string? time { get; set; }
    }

    public class Month
    {
        public int? month { get; set; }
        public double? maxTemp { get; set; }
        public double? mintemp { get; set; }
        public double? precipitation { get; set; }
    }

    public class Climate
    {
        public string? time { get; set; }
        public List<Month> month { get; set; }
    }

    public class Radar
    {
        public string? title { get; set; }
        public string? image { get; set; }
        public string? url { get; set; }
    }

    public class Data
    {
        public Real real { get; set; }
        public Predict predict { get; set; }
        public Air air { get; set; }
        public List<Tempchart> tempchart { get; set; }
        public List<Passedchart> passedchart { get; set; }
        public Climate climate { get; set; }
        public Radar radar { get; set; }
    }

    public class Root
    {
        public string? msg { get; set; }
        public int? code { get; set; }
        public Data data { get; set; }
    }
}
