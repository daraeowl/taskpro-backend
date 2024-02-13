using System.Text.Json.Serialization;

namespace taskpro_api.Models
{
    public class Task
    {
        public int Id { get; set; }


        [JsonPropertyName("title")]
        public string? Title { get; set; } // Make the Title property nullable
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
