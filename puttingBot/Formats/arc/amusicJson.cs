using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace puttingBot.Formats.arc
{
    class amusicJson
    {
        public partial class Amusic
        {
            [JsonProperty("_items")]
            public Item[] Items { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("_links")]
            public Links Links { get; set; }

            [JsonProperty("_id")]
            public string Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("artist")]
            public string Artist { get; set; }
        }

        public partial class Links
        {
            [JsonProperty("_self")]
            public Uri Self { get; set; }
        }

    }
}
