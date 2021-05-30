using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puttingBot.Formats.Unis
{
    public partial class LoginStatus
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
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("avatarUrl")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("inviteUrl")]
        public Uri InviteUrl { get; set; }

        [JsonProperty("inviteCode")]
        public string InviteCode { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("PWDtype")]
        public long PwDtype { get; set; }

        [JsonProperty("spreadDomain")]
        public Uri SpreadDomain { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
