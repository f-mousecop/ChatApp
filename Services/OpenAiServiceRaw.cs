using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public sealed class OpenAiServiceRaw
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private const string BaseUrl = "https://api.openai.com/v1/responses";

        public OpenAiServiceRaw()
        {
            var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("Environment variable OPENAI_API_KEY not set");

            _http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);
        }

        public async Task<string> GetReplyAsync(string userText, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userText))
                return string.Empty;

            var req = new ResponsesRequest
            {
                Model = "gpt-5",
                Stream = false,
                Input =
                {
                    new ResponsesInputItem
                    {
                        Role = "system",
                        Content = { new ResponsesTextPart { Text = "You are a helpful assistant." } }
                    },
                    new ResponsesInputItem
                    {
                        Role = "user",
                        Content = { new ResponsesTextPart { Text = userText } }
                    }
                }
            };

            var body = JsonSerializer.Serialize(req, _jsonSerializerOptions);
            using var resp = await _http.PostAsync(BaseUrl,
                new StringContent(body, Encoding.UTF8, "application/json"), ct);

            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync(ct);
            var parsed = JsonSerializer.Deserialize<ResponsesResponse>(json, _jsonSerializerOptions)
                ?? throw new InvalidOperationException("Invalid Responses API payload");

            // Prefer convenience field 
            if (!string.IsNullOrWhiteSpace(parsed.Output_text))
                return parsed.Output_text!.Trim();

            // Fallback: concatenate any output text parts
            var text = parsed.Output?
                .Where(o => o.Content != null)
                .SelectMany(o => o.Content!)
                .Where(p => string.Equals(p.Type, "text", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Text)
                .Aggregate(new StringBuilder(), (sb, t) => sb.Append(t), sb => sb.ToString());

            return text?.Trim() ?? string.Empty;
        }


        /// <summary>
        /// Streams assistant tokens using the Responses API semantic SSE
        /// Invokes onToken for every Output_text.delta event
        /// </summary>
        /// <param name="userText"></param>
        /// <param name="onToken"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task StreamReplyAsync(string userText, Action<string> onToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userText)) return;

            var req = new ResponsesRequest
            {
                Model = "gpt-5",
                Stream = true,
                Input =
                {
                    new ResponsesInputItem
                    {
                        Role = "system",
                        Content = { new ResponsesTextPart { Text = "You are a helpful assistant." } }
                    },
                    new ResponsesInputItem
                    {
                        Role = "user",
                        Content = { new ResponsesTextPart { Text = userText } }
                    }
                }
            };

            using var msg = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(req, _jsonSerializerOptions), Encoding.UTF8, "application/json")
            };

            // IMPORTANT stream the response don't buffer
            using var resp = await _http.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead, ct);
            resp.EnsureSuccessStatusCode();

            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false);

            // Responses API emits **semantic** SSE:
            //   event: response.output_text.delta
            //   data: {"type":"response.output_text.delta","delta":"..."}
            //   ...
            //   event: response.completed
            string? currentEvent = null;

            while (!reader.EndOfStream && !ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (line is null) break;
                if (line.Length == 0) continue;

                if (line.StartsWith("event:", StringComparison.Ordinal))
                {
                    currentEvent = line["event:".Length..].Trim();
                    continue;
                }

                if (line.StartsWith("data:", StringComparison.Ordinal))
                {
                    var payload = line["data:".Length..].Trim();
                    if (string.IsNullOrWhiteSpace(payload)) continue;

                    // Some servers may send "[DONE]" in legacy modes — ignore safely
                    if (payload == "[DONE]") break;

                    ResponsesStreamEvent? ev = null;
                    try { ev = JsonSerializer.Deserialize<ResponsesStreamEvent>(payload, _jsonSerializerOptions); }
                    catch { /* ignore malformed chunks */ }

                    if (ev == null) continue;

                    var type = ev.Type ?? currentEvent ?? "";

                    if (string.Equals(type, "response.output_text.delta", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(ev.Delta))
                            onToken(ev.Delta);
                    }
                    else if (string.Equals(type, "response.completed", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    else if (string.Equals(type, "response.error", StringComparison.OrdinalIgnoreCase))
                    {
                        // Best effort: surface message (optional)
                        var msgText = ev.Message ?? "Unknown streaming error.";
                        throw new InvalidOperationException(msgText);
                    }
                }
            }
        }
        public void Dispose() => _http.Dispose();
    }
}
