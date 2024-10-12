using System.Collections.Generic;

namespace ScriptHelper
{
    public class OpenRouterModels
    {
        public List<OpenRouterModel> data { get; set; }
    }

    public class OpenRouterModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public int context_length { get; set; }
        public OpenRouterModelPerRequestLimits per_request_limits { get; set; }
    }

    public class OpenRouterModelPerRequestLimits
    {
        public string prompt_tokens { get; set; }
        public string completion_tokens { get; set; }
    }
}