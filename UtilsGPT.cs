using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ScriptHelper
{
    internal class UtilsGPT
    {
        public static async Task<string> doGPT(string operationName, string model, int tokenMax, double temperature, string userPrompt, string systemPrompt, string clue, FormApp1 myForm, string statusMessage, bool jsonMode = false)
        {
            // convert to a doGPTChat call
            List<ChatMessageModel> messages = new List<ChatMessageModel>
            {
                new ChatMessageModel() { Role = "system", Content = systemPrompt },
                new ChatMessageModel() { Role = "user", Content = userPrompt },
            };
            return await doGPTChat(operationName, model, temperature, messages, myForm, statusMessage, jsonMode);
        }

        public static async Task<string> doGPTChat(string operationName, string model, double temperature, List<ChatMessageModel> messages, FormApp1 myForm, string statusMessage = "", bool jsonMode = false)
        {
            string qryResponse = "no repsonse";
            int errorKount = 0;
            DateTime startTime;
            DateTime endTime;
            TimeSpan timeSpan;
            double seconds = 0;
            bool looper = true;
            myForm.updateGPTErrorMsg("", "");

            // get the userpropmt and systemprompt from the messages
            string userPrompt = "";
            string systemPrompt = "";
            foreach (ChatMessageModel message in messages)
            {
                if (message.Role == "user")
                {
                    userPrompt += message.Content + "\r\n";
                }
                else if (message.Role == "system")
                {
                    systemPrompt += message.Content + "\r\n";
                }
            }
            postPrompts(model, userPrompt, systemPrompt, statusMessage, myForm);

            int tokenMax = -1;
            try
            {
                tokenMax = Utils.getMaxTokens(model, userPrompt + systemPrompt);
            } 
            catch (Exception ex)
            {
                myForm.updateGPTErrorMsg("Request Failed: Are you missing your OpenRouter Key?", "Request Failed: Are you missing your OpenRouter Key?");
                myForm.updateGPTStatus("Request Failed: Are you missing your OpenRouter Key?", Color.Red);

                return $"#Request Failed: Are you missing your OpenRouter Key?";
            }

            if (tokenMax < 0) // less than 500 tokens available for return 
            {
                myForm.updateGPTErrorMsg("Prompts too big to process", "From UtilsGPT.doGPT: userPrompt + systemPrompt are too large.  They leave less than 500 tokens for the maxTokens parameter that sets the max return size");
                myForm.updateGPTStatus("Request aborted.  Prompts too large", Color.Red);

                return $"#ErrorPromptSize: Prompts too big to process.  Edit prompts and try again";
            }

            while (looper)
            {
                startTime = DateTime.Now;
                if (statusMessage.Length > 0)
                    myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + model + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

                //new chatRequest
                var request = new ChatAIRequest()
                {
                    Model = model,
                    Messages = messages,
                    Temperature = temperature,
                    JsonMode = jsonMode,
                    OperationName = operationName,
                };

                try
                {
                    qryResponse = await Api.OpenRouterAPI(request);
                    if (qryResponse.EndsWith("\r\n"))
                    {
                        qryResponse = qryResponse.Substring(0, qryResponse.Length - 2);
                    }
                    looper = false;
                } 
                catch (Exception ex)
                {
                    errorKount += 1;
                    DateTime currentDateTime = DateTime.Now;

                    string errorMsg = $"#Error: {ex.Message} Count: {errorKount.ToString()} at {currentDateTime}";

                    myForm.updateGPTErrorMsg(errorMsg, ex.Message);

                    Console.WriteLine(errorMsg);

                    Thread.Sleep(750);
                    if (errorKount > 2)
                    {
                        return $"#ErrorCountExceeded";
                    }
                }

                endTime = DateTime.Now;
                timeSpan = endTime - startTime;
                seconds = timeSpan.TotalSeconds;

                    
            } // while looper

            if (statusMessage.Length > 0)
                myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + model + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);
            return qryResponse;
        }

        private static void postPrompts(string model, string userPrompt, string systemPrompt, string statusMessage, FormApp1 myForm)
        {
            if (Utils.MagicalMysteryTour)
            {
                DateTime startTime = DateTime.Now;
                string output = $"***New Prompt*** At {startTime}\r\n***Type***:{statusMessage} using {model}\r\n***SystemPrompt***:\r\n{systemPrompt}\r\n*** End System Prompt ***\r\n\r\n***UserPrompt***:\r\n{userPrompt}\r\n*** End User Prompt***\r\n";
                myForm.postPrompts(output, false);  // boolean will clear RTB if set to true
            }
            else
            {
                return;
            }
        }
    }
}
