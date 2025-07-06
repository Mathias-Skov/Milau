using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Milau.Models
{
    public class UserDetails
    {
        public string? Id { get; set; }
        [JsonPropertyName("license")]
        public string? License { get; set; }
        [JsonPropertyName("hwid")]
        public string? Hwid { get; set; }
    }
}
