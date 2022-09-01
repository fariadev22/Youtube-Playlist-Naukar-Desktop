using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VideoFilter
    {
        [EnumMember(Value = "None")]
        None,

        [EnumMember(Value = "Duplicate")]
        Duplicate,

        [EnumMember(Value = "Private")]
        Private
    }
}
