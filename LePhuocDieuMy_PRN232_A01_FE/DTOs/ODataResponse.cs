using System.Text.Json.Serialization;

namespace LePhuocDieuMy_PRN232_A01_FE.DTOs
{

    public class ODataResponse<T>
    {
        [JsonPropertyName("@odata.context")]
        public string Context { get; set; }

        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
    }
}
