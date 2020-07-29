using System;
using System.Text.Json.Serialization;
namespace ARINC834WebSocketsSample
{
    public class A834DataParameter
    {
        [JsonPropertyName("k")]
        public string ParameterName { get; set; }

        [JsonPropertyName("v")]
        public string Value { get; set; }

        [JsonPropertyName("t")]
        public DateTime TimeStamp { get; set; }
    }
}
