using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class OpenAiService
    {
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        private readonly OpenAIResponseClient _responses;
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        private readonly string _model;

        public OpenAiService(string? model = null)
        {
            var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("Environment variable OPENAI_API_KEY is not set.");

            // Pick your default model; override via ctor if desired
            _model = string.IsNullOrWhiteSpace(model) ? "gpt-5" : model;

            // The SDK exposes a dedicated client for the Responses API
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            _responses = new OpenAIResponseClient(model: _model, apiKey: key);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }

        public async Task<string> GetReplyAsync(string userText, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userText)) return string.Empty;

            // Simplest path pass a system instruction + user text
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            OpenAIResponse response = await _responses.CreateResponseAsync(
                userInputText: userText,
                new ResponseCreationOptions
                {
                    Instructions = "You are a helpful assistant."
                },
                ct);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            return (response.GetOutputText() ?? string.Empty).Trim();
        }
    }
}
