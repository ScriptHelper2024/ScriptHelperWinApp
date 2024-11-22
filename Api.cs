using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHelper
{
    public class ChatAIRequest
    {
        public string Model { get; set; }
        public bool JsonMode { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
        public double Temperature { get; set; }
        public string OperationName { get; set; }
    }

    public class ChatMessageModel
    {
        // valid roles: "user", "assistant", "system"
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class Api
    {

        public static string openRouterKey = string.Empty;

        public static async Task<string> OpenRouterModels()
        {
            //if (openRouterKey == null)
            //{
            //    throw new Exception("OpenRouter Key Missing");
            //}
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openRouterKey);
                var apiResponse = await httpClient.GetAsync("https://openrouter.ai/api/v1/models");
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        public static async Task<string> OpenRouterAPI(ChatAIRequest chatAiRequest)
        {
            if (openRouterKey == null)
            {
                throw new Exception("OpenRouter Key Missing");
            }
            // create api
            string model = chatAiRequest.Model.ToString() ?? "openrouter/auto";
            // replace or/ with empty string
            model = model.Replace("or/", "");
            List<object> messages = new List<object>();
            foreach (var message in chatAiRequest.Messages)
            {
                messages.Add(new { role = message.Role, content = message.Content });
            }
            float temperature = (float)chatAiRequest.Temperature;

            // create request
            using (var httpClient = new HttpClient())
            {
                var openRouterRequest = new
                {
                    model,
                    messages,
                    temperature
                };

                var requestContent = new StringContent(
                    JsonConvert.SerializeObject(openRouterRequest),
                    Encoding.UTF8,
                    "application/json");

                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openRouterKey);
                var chatResponse = await httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", requestContent);

                // get response
                var responseContent = await chatResponse.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseContent);
                var responseText = responseObj["choices"]?[0]?["message"]?["content"]?.ToString();
                if (responseText ==  null)
                {
                    throw new Exception("Bad Response: " + responseContent);
                }
                return responseText;
            }
        }
    }

}