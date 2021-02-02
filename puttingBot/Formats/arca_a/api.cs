using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.arca_a
{
    public partial class AUserRecord
    {
        [JsonProperty("_links")]
        public PageLinks Links { get; set; }

        [JsonProperty("_items")]
        public Profile[] Items { get; set; }

        [JsonProperty("_related")]
        public Related Related { get; set; }
    }
    public partial class APlayRecord
    {
        [JsonProperty("_links")]
        public PageLinks Links { get; set; }

        [JsonProperty("_items")]
        public AMusicRecord[] Items { get; set; }

        [JsonProperty("_related")]
        public Related Related { get; set; }
    }

    public partial class AMusicRecord
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("chart_id")]
        public string ChartId { get; set; }

        [JsonProperty("music_id")]
        public string MusicId { get; set; }

        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("lamp")]
        public string Lamp { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; set; }

        [JsonProperty("btn_rate")]
        public double BtnRate { get; set; }

        [JsonProperty("long_rate")]
        public double LongRate { get; set; }

        [JsonProperty("vol_rate")]
        public double VolRate { get; set; }

        [JsonProperty("critical")]
        public long Critical { get; set; }

        [JsonProperty("near")]
        public long Near { get; set; }

        [JsonProperty("error")]
        public long Error { get; set; }

        [JsonProperty("max_chain")]
        public long MaxChain { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class PageLinks
    {
        [JsonProperty("_self")]
        public Uri Self { get; set; }

        [JsonProperty("_next")]
        public Uri Next { get; set; }
    }

    public partial class Related
    {
        [JsonProperty("charts")]
        public Chart[] Charts { get; set; }

        [JsonProperty("music")]
        public Music[] Music { get; set; }

        [JsonProperty("profiles")]
        public Profile[] Profiles { get; set; }
    }

    public partial class Chart
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("music_id")]
        public string MusicId { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("bpm_min")]
        public double BpmMin { get; set; }

        [JsonProperty("bpm_max")]
        public double BpmMax { get; set; }
    }


    public partial class Music
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }
    }

    public partial class Profile
    {
        [JsonProperty("_id")]
        public string ProfileId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sdvx_id")]
        public string SdvxId { get; set; }

        [JsonProperty("skill_level")]
        public object SkillLevel { get; set; }

        [JsonProperty("register_time")]
        public DateTimeOffset RegisterTime { get; set; }

        [JsonProperty("access_time")]
        public DateTimeOffset AccessTime { get; set; }
    }
    /*
    public enum Grade { A, APlus, Aa, AaPlus, Aaa, AaaPlus, B, C, D, S };

    public enum Lamp { Clear, Hc, Play, Puc, Uc };

    public enum Difficulty { Adv, Exh, Inf, Mxm, Nov };
    */

}
