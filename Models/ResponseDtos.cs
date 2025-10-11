using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public sealed class ResponsesInputItem
    {
        public string Role { get; set; } = "user";

        public List<ResponsesTextPart> Content { get; set; } = new() { new ResponsesTextPart() };
    }

    public sealed class ResponsesTextPart
    {
        public string Type { get; set; } = "text";
        public string Text { get; set; } = "";
    }

    public sealed class ResponsesRequest
    {
        public string Model { get; set; } = "gpt-5";
        public List<ResponsesInputItem> Input { get; set; } = new();
        public bool Stream { get; set; } = false;
    }

    // Non-streaming response: field output_text if present
    public sealed class ResponsesResponse
    {
        public string Id { get; set; } = "";
        public long Created { get; set; }
        public string Model { get; set; } = "";

        // convenience concatenated text (if present)
        [JsonPropertyName("output_text")]
        public string? Output_text { get; set; }

        // full output for fallback
        public List<ResponsesOutputItem>? Output { get; set; }
    }

    public sealed class ResponsesOutputItem
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "message"; // message or other types
        public string Role { get; set; } = "assistant";
        public List<ResponsesTextPart>? Content { get; set; }
    }

    // Streaming events (semantic SSE)
    public sealed class ResponsesStreamEvent
    {
        public string Type { get; set; } = "";     // e.g., "response.output_text.delta", "response.completed"
        public string? Delta { get; set; }         // present on *.delta events
        public string? Message { get; set; }       // present on error events
    }
}
