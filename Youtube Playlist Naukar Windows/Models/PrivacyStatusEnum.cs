using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrivacyStatusEnum
    {
        [EnumMember(Value = "Private")]
        Private,

        [EnumMember(Value = "Public")]
        Public,

        [EnumMember(Value = "Unlisted")]
        Unlisted
    }
}
