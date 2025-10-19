using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public sealed class ChatMessage
    {
        public string Role { get; set; } = "user";      // "system" | "user" | "assistant"
        public string Content { get; set; } = "";
    }

    public sealed class ChatRequest
    {
        public string Model { get; set; } = "gpt-5";
        public List<ChatMessage> Messages { get; set; } = new();
        public bool Stream { get; set; } = false;
    }

    // Response for non-streaming
    public sealed class ChatChoice
    {
        public int Index { get; set; }
        public ChatMessage Message { get; set; } = new();
        public object? Logprobs { get; set; }
        public string? Finish_Reason { get; set; }
    }

    public sealed class ChatResponse
    {
        public string Id { get; set; } = "";
        public string Object { get; set; } = "";
        public long Created { get; set; }
        public List<ChatChoice> Choices { get; set; } = new();
    }

    // Streaming delta payload (stream = true)
    public sealed class ChatDelta
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }

    public sealed class ChatStreamChoice
    {
        public int Index { get; set; }
        public ChatDelta Delta { get; set; } = new();
        public string? Finish_Reason { get; set; }
    }

    public sealed class ChatStreamEvent
    {
        public string Id { get; set; } = "";
        public string @Object { get; set; } = "chat.completion.chunk";
        public long Created { get; set; }
        public string? Model { get; set; }
        public List<ChatStreamChoice> Choices { get; set; } = new();
    }
}
