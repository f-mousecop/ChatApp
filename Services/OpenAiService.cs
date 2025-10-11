using OpenAI;
using OpenAI.Responses;


namespace ChatApp.Services
{
    public class OpenAiService
    {
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        private readonly OpenAIResponseClient _responses;

        private readonly string _model;

        public OpenAiService(string? model = null)
        {
            var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("Environment variable OPENAI_API_KEY is not set.");

            // Pick default model; override via ctor if desired
            _model = string.IsNullOrWhiteSpace(model) ? "gpt-5-chat-latest" : model;

            // The SDK exposes a dedicated client for the Responses API
            _responses = new OpenAIResponseClient(model: _model, apiKey: key);
        }

        public async Task<string> GetReplyAsync(string userText, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userText)) return string.Empty;

            // Simplest path pass a system instruction + user text
            OpenAIResponse response = await _responses.CreateResponseAsync(
                userInputText: userText,
                new ResponseCreationOptions
                {
                    Instructions = "You are a helpful assistant."
                },
                ct);

            return (response.GetOutputText() ?? string.Empty).Trim();
        }


        /// <summary>
        /// Streams assistant tokens. Invoks onToken(textDelta) for each delta
        /// </summary>
        /// <param name="userText"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task StreamReplyAsync(string userText, Action<string> onToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userText)) return;

            // Create a streaming response and iterate semantic streaming updates
            await foreach (StreamingResponseUpdate update in _responses.CreateResponseStreamingAsync(
                userInputText: userText,
                new ResponseCreationOptions
                {
                    Instructions = "You are a helpful assistant."
                },
                ct))
            {
                // Text deltas arrive as StreamingResponseOutputTextDeltaUpdate
                if (update is StreamingResponseOutputTextDeltaUpdate textDelta &&
                    !string.IsNullOrEmpty(textDelta.Delta))
                {
                    onToken(textDelta.Delta);
                }

                // Options: watch for reasoning/status items:
                // if (update is StreamingResponseOutputItemAddedUpdate item && item.Item is ReasoningResponseItem r) { ... }
                // and stop on StreamingResponseCompletedUpdate for explicit completion
            }
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        }
    }
}
