using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats
{
    public partial class Unisdvx
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("detail")]
        public string? Detail { get; set; }

        [JsonProperty("retCode")]
        public long? RetCode { get; set; }

        [JsonProperty("retMsg")]
        public string? RetMsg { get; set; }

        [JsonProperty("pageNo")]
        public long? PageNo { get; set; }

        [JsonProperty("pageSize")]
        public long? PageSize { get; set; }

        [JsonProperty("totalPage")]
        public long? TotalPage { get; set; }

        [JsonProperty("totalSize")]
        public long? TotalSize { get; set; }

        [JsonProperty("data")]
        public PlayData?[] Data { get; set; }
    }

    public partial class PlayData
    {
        [JsonProperty("comboCount")]
        public long ComboCount { get; set; }

        [JsonProperty("gameDate")]
        public DateTimeOffset GameDate { get; set; }

        [JsonProperty("highestScore")]
        public long HighestScore { get; set; }

        [JsonProperty("recordPage")]
        public string RecordPage { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("machineName")]
        public string MachineName { get; set; }

        [JsonProperty("score")]
        public long Score { get; set; }

        [JsonProperty("musicId")]
        public long MusicId { get; set; }

        [JsonProperty("gaugeCount")]
        public long GaugeCount { get; set; }

        [JsonProperty("btnRate")]
        public long BtnRate { get; set; }

        [JsonProperty("clearType")]
        public long ClearType { get; set; }

        [JsonProperty("storeName")]
        public string StoreName { get; set; }

        [JsonProperty("musicGrade")]
        public long MusicGrade { get; set; }

        [JsonProperty("criticalCount")]
        public long CriticalCount { get; set; }

        [JsonProperty("errorCount")]
        public long ErrorCount { get; set; }

        [JsonProperty("scoreId")]
        public long ScoreId { get; set; }

        [JsonProperty("nearCount")]
        public long NearCount { get; set; }

        [JsonProperty("statisPage")]
        public string StatisPage { get; set; }

        [JsonProperty("productId")]
        public long ProductId { get; set; }

        [JsonProperty("longRate")]
        public long LongRate { get; set; }

        [JsonProperty("storeId")]
        public long StoreId { get; set; }

        [JsonProperty("musicName")]
        public string MusicName { get; set; }

        [JsonProperty("scoreGrade")]
        public long ScoreGrade { get; set; }

        [JsonProperty("volRate")]
        public long VolRate { get; set; }

        [JsonProperty("machineId")]
        public long MachineId { get; set; }

        [JsonProperty("musicGradeName")]
        public string MusicGradeName { get; set; }

        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [JsonProperty("clearTypeName")]
        public string ClearTypeName { get; set; }

        [JsonProperty("musicImage")]
        public Uri MusicImage { get; set; }

        [JsonProperty("maxChain")]
        public long MaxChain { get; set; }
    }
}
