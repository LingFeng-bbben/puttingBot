using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.Unis
{
    public partial class UnisStatus
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("retCode")]
        public long RetCode { get; set; }

        [JsonProperty("retMsg")]
        public string RetMsg { get; set; }

        [JsonProperty("data")]
        public Status Data { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("statisPage")]
        public string StatisPage { get; set; }

        [JsonProperty("dayCount")]
        public long DayCount { get; set; }

        [JsonProperty("productId")]
        public long ProductId { get; set; }

        [JsonProperty("playerName")]
        public string PlayerName { get; set; }

        [JsonProperty("recordPage")]
        public string RecordPage { get; set; }

        [JsonProperty("weekCount")]
        public long WeekCount { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("playCount")]
        public long PlayCount { get; set; }

        [JsonProperty("productImage")]
        public Uri ProductImage { get; set; }

        [JsonProperty("maxWeekChain")]
        public long MaxWeekChain { get; set; }

        [JsonProperty("blasterEnergy")]
        public long BlasterEnergy { get; set; }

        [JsonProperty("maxPlayChain")]
        public long MaxPlayChain { get; set; }

        [JsonProperty("gamecoinBlock")]
        public long GamecoinBlock { get; set; }
    }
}
