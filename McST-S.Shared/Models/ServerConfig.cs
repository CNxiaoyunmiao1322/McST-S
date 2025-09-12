using System.Text.Json.Serialization;

namespace McST_S.Shared.Models
{
    public class ServerConfig
    {
        [JsonPropertyName("serverUrl")]
        public string ServerUrl { get; set; } = string.Empty;

        [JsonPropertyName("currentVersion")]
        public string CurrentVersion { get; set; } = "1.0.0.0"; // 默认版本号
    }
}