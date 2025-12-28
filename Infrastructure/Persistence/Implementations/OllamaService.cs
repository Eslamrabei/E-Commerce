using Microsoft.Extensions.Configuration;
using ServiceAbstraction.Contracts;
using Shared.Dtos.AiSearch;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace Persistence.Implementations
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _client;
        public OllamaService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:11434");
            _client.Timeout = TimeSpan.FromMinutes(2);
        }

        public async Task<string> GenerateRagResponseAsync(string userQuery, List<ProductSearchResponse> products)
        {
            var contextBuilder = new StringBuilder();
            foreach (var product in products)
                contextBuilder.Append($"- {product.Name}: {product.Description} (Price: {product.Price} EGP)");

            var prompt = $@"
            You are a helpful sales assistant for 'FreshBite' online grocery.
            User Question: ""{userQuery}""
            
            Here are the products we found in our stock that match the request:
            {contextBuilder}

            Instructions:
            1. Answer the user's question using ONLY the provided products.
            2. Be friendly and mention the product prices clearly.
            3. Explain WHY these products are good for them.
            4. If the product list is empty, apologize politely.
            
            Your Answer:";

            var request = new
            {
                model = "llama3.2",
                prompt = prompt,
                stream = false
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"
                );

            try
            {
                var response = await _client.PostAsync("/api/generate", jsonContent);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var ollamaResponse = JsonSerializer.Deserialize<OllamaApiResponse>(responseString);

                return ollamaResponse?.Response ?? "Thinking...";
            }
            catch (Exception ex)
            {
                return $"Error connecting to AI brain: {ex.Message}";
            }

        }
        public class OllamaApiResponse
        {
            [JsonPropertyName("response")]
            public string Response { get; set; }
        }
    }


    public class GroqService : IGroqService
    {
        private readonly HttpClient _httpClient;

        public GroqService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration["Groq:ModelUrl"]!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["Groq:ApiKey"]);
        }

        public async Task<string> GenerateRagResponseAsync(string userQuery, List<ProductSearchResponse> products)
        {
            // 1. تجهيز سياق المنتجات
            var contextBuilder = new StringBuilder();
            foreach (var p in products)
            {
                contextBuilder.AppendLine($"- {p.Name}: {p.Description} ({p.Price} EGP)");
            }

            // 2. تجهيز الرسائل (Groq بيستخدم نظام Chat زي ChatGPT)
            var requestBody = new
            {
                model = "llama-3.1-8b-instant", // موديل سريع جداً ومجاني
                messages = new[]
                {
                new { role = "system", content = "You are a helpful sales assistant. Format your response using HTML. Use <b> for product names and prices to make them pop. Use <br> for line breaks. Use bullet points (<ul> <li>) if listing items. Add emojis 🍎." },
                new { role = "user", content = $"User asks: {userQuery}\n\nAvailable Products:\n{contextBuilder}" }
            },
                temperature = 0.5 // عشان الرد يبقى موزون
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("", jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    return $"Groq API Error ({response.StatusCode}): {errorBody}";
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                // Groq بيرجع JSON معقد شوية، بنفكه هنا
                using var doc = JsonDocument.Parse(jsonString);
                return doc.RootElement
                          .GetProperty("choices")[0]
                          .GetProperty("message")
                          .GetProperty("content")
                          .GetString() ?? "No response.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
