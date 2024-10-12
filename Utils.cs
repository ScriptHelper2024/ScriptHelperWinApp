using Microsoft.ML.Tokenizers;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpToken;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ScriptHelper
{
    static class Utils

    {
        public static long lastUpdateSceneInScenes = 0;
        public static long lastUpdateSceneNotesList = 0;

        public static Boolean fork = false;
        public static Boolean refactor = false;
        public static Boolean refactorMovieText = true;
        public static int currentSceneCount = 24;
        public static int currentTokenCount = 40;
        public static int currentSceneNumber = 0;
        public static int originalSceneCount = 24;

        public static string forkTitle = "";

        public static string programVersion = "0.95";

        public static string dumpJSONpwd = "phantom";
        public static int dataVersion = 3;

        public static Boolean MagicalMysteryTour = false;
        public static List<DateTime> dateTimes = new List<DateTime>();
        public static string SnakeBelly = "StormyWeather";

        public static string currentMovieFilename = "exit";

        public static string newMovieFilename = "";
        public static Boolean jumpStartFlag = false;
        public static Boolean magicMovieNoteFlag = false;
        public static Boolean magicSceneTextNoteFlag = false;
        public static Boolean magicSceneScriptNoteFlag = false;
        public static Boolean SceneSeedUpdateFlag = true;
        public static Boolean startupFlag = false;

        public static Boolean newScenesFlag = false;

        public static Boolean ExitAfterStart = false;
        public static Boolean updateScriptStyleToDictionaryFlag = false;
        public static Boolean updateMovieStyleToDictionaryFlag = false;
        public static Boolean enlargeScript = false;

        public static Boolean Saving = false;

        public static string originalSceneText = "";
        public static string originalMovieText = "";
        public static string originalScriptText = "";

        public static StyleElements globalStyle = new StyleElements("-none-", "author", "-none-");

        public static Boolean McKeeFlag = true;
        //public static AuthenticatedUser User = null;

        public static Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
        public static string makeScenesRTF(List<SceneObj> myScenes)
        {
            string rtfString = string.Empty;

            foreach (SceneObj scene in myScenes)
            {
               if (scene.splitSceneMakeFlag == false)
                {
                    rtfString += "\\b " + scene.Title + ": \\b0\\par " + scene.Hint + " \\par\\par";
                }
                else
                {
                    rtfString += "\\b " + scene.Title + ": \\b0\\par " + Utils.truncStringAtWord(scene.Hint,200) + "..." + " \\par\\par";
                }
            }




            rtfString = @"{\rtf1\ansi " + rtfString + "}";
            return rtfString;

        }

        public static List<SceneObj> upgradeScenes1to2(List<SceneObj> myScenes)
        {
            List<SceneObj> upScenes = new List<SceneObj>();
            SceneObj newScene;
            string serializedObject;
            int noteCounter = 0;

            foreach (SceneObj scene in myScenes)
            {
                newScene = new SceneObj();
                serializedObject = JsonConvert.SerializeObject(scene);
                newScene = JsonConvert.DeserializeObject<SceneObj>(serializedObject);

                noteCounter = 0;

                foreach (NotesSceneText note in scene.myNoteTextList)
                {
                    if (note.myScript.Trim().Length > 0)
                    {
                        newScene.myNoteTextList[noteCounter].myScripts.Add(new NotesSceneScript(note.myBeatSheet, note.myScript, "", "Base:"));
                    }


                    noteCounter++;
                }
                upScenes.Add(newScene);

            }

            return upScenes;


        }


        public static string makeRTFEmpty()
        {
            string rtfString = string.Empty;
            rtfString = @"{\rtf1\ansi }";

            return rtfString;
        }
        public static string TextBetweenBrackets(string input)
        {
            int startIndex = input.IndexOf('[');
            int endIndex = input.IndexOf(']');

            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
            {
                return input.Substring(startIndex + 1, endIndex - startIndex - 1);
            }

            return string.Empty;
        }
        public static string makePendingMessage(string model)
        {
            return "awaiting reponse from " + model + "\n";


        }

        public static int tokenCount(string text)
        {
            var encoding = GptEncoding.GetEncoding("cl100k_base");  // gpt4 anf gpt3.5

            encoding = GptEncoding.GetEncodingForModel("gpt-4");
            var encoded = encoding.Encode(text);
            int count = 0;
            count = encoded.Count;
            return count;

        }

        public static int getMaxTokens(string model, string prompts)
        {
            int totalTokens = 8000;

            //if (model == "gpt-4-0613") { totalTokens = 8192; }
            //if (model == "gpt-4-0314") { totalTokens = 8192; }
            //if (model == "gpt-4-1106-preview") { totalTokens = 128000; }
            //if (model == "gpt-4-0125-preview") { totalTokens = 128000; }
            //if (model == "gpt-4-turbo-preview") { totalTokens = 128000; }
            //if (model.StartsWith("ft:gpt-3.5-turbo-1106:jim-rutt-show")) { totalTokens = 16385; }
            //if (model == "gpt-3.5-turbo-16k-0613") { totalTokens = 16385; }
            //if (model == "gpt-3.5-turbo-1106") { totalTokens = 16385; }
            //if (model == "claude-2.1") { totalTokens = 200000; }

            if (model.StartsWith("or/"))
            {
                totalTokens = int.Parse(FormApp1._openRouterModels.Where(x => x.id == model.Replace("or/","")).FirstOrDefault().per_request_limits.prompt_tokens);
            }

            int promptTokens = tokenCount(prompts);

            int availTokens = totalTokens - promptTokens;
            if (availTokens < 500) { availTokens = -1; }

            return availTokens;
        }

        public static string makeDoGPTErrror(string input)
        {
            string reply = "";

            reply = $"DoGPTErrror: {input}";
            return reply;

        }

        public static Boolean checkForGPTErrors(string input, FormApp1 myForm)

        {
            Boolean isError = false;

            if (input.Contains("#ErrorCountExceeded"))
            {
                myForm.updateGPTStatus("GPT Status: Too many GPT errors, aborting this request", Color.LightGreen);
                isError = true;
            }
            else if (input.Contains("#ErrorPromptSize"))
            {
                myForm.updateGPTStatus("GPT Status: Prompts too large, aborting this request", Color.LightGreen);
                isError = true;
            }

            if (isError)
            {
                Utils.nop();
            }
            return isError;
        }

        public static List<StyleElements> getAuthorStyles()
        {
            string author = "author";
            string prose = "prose";
            string example = "example";

            List<StyleElements> styles = new List<StyleElements>();


            styles.Add(new StyleElements("-none-", author, "-none-"));
            styles.Add(new StyleElements("JJ Abrams", author, "JJ Abrams"));
            styles.Add(new StyleElements("Judd Apatow", author, "Judd Apatow"));
            styles.Add(new StyleElements("James Cameron", author, "James Cameron"));
            styles.Add(new StyleElements("Coen Brothers", author, "Coen Brothers"));
            styles.Add(new StyleElements("Francis Ford Coppola", author, "Francis Ford Coppola"));
            styles.Add(new StyleElements("Nora Ephron", author, "Nora Ephron"));
            styles.Add(new StyleElements("Greta Gerwig", author, "Greta Gerwig"));
            styles.Add(new StyleElements("Stanley Kubrick", author, "Stanley Kubrick"));
            styles.Add(new StyleElements("Spike Lee", author, "Spike Lee"));
            styles.Add(new StyleElements("Nancy Meyers", author, "Nancy Meyers"));
            styles.Add(new StyleElements("Christopher Nolan", author, "Christopher Nolan"));
            styles.Add(new StyleElements("Jordan Peele", author, "Jordan Peele"));
            styles.Add(new StyleElements("Kevin Smith", author, "Kevin Smith"));
            styles.Add(new StyleElements("Quentin Tarantino", author, "Quentin Tarantino"));
            styles.Add(new StyleElements("Robert Towne", author, "Robert Towne"));
            styles.Add(new StyleElements("Lina Wertmüller", author, "Lina Wertmüller"));
            styles.Add(new StyleElements("Billy Wilder", author, "Billy Wilder"));

            return styles;
        }

        public static List<StyleElements> getScriptStyleGuides()
        {
            List<StyleElements> styleGuides = new List<StyleElements>();
            styleGuides.Add(new StyleElements("-none-", "guide", "-none-"));
            styleGuides.Add(new StyleElements("McKee Story Style", "guide", Utils.getMcKeeScriptStyleGuide()));
            styleGuides.Add(new StyleElements("Ironic Comedy", "guide", Utils.getIronicComedyScriptStyleGuide()));
            styleGuides.Add(new StyleElements("19th Century Theatrical", "guide", Utils.get19thCenturyTheatricalScriptStyleGuide()));
            return styleGuides;
        }

        public static List<StyleElements> getMovieTextStyleGuides()
        {
            List<StyleElements> styleGuides = new List<StyleElements>();
            styleGuides.Add(new StyleElements("-none-", "guide", "-none-"));
            styleGuides.Add(new StyleElements("McKee Style", "guide", Utils.getMcKeeMovieTextStyleGuide()));
            styleGuides.Add(new StyleElements("Depurple", "guide", Utils.getDepurpleMovieTextStyleGuide()));
            return styleGuides;
        }

        public static string stripNonASCII(string input)
        {
            return Regex.Replace(input, @"[^\u0000-\u007F]+", "");
        }

        public static string rightOfArrow(string input)
        {
            int verIndex = -1;
            int startIndex = input.IndexOf('>');
            string working = "";

            if (input.Length > 3)
            {
                if (input.Substring(0, 4) == "Base")
                {
                    return "Base";
                }
            }

            if (startIndex != -1)
            {
                working = input.Substring(startIndex + 1).Trim();
                working = working + "   ";
                verIndex = working.IndexOf("Ver");
                input = working.Substring(verIndex, 7).Trim();
            }




            return input;
        }
        public static string makeFilename(string input)
        {
            input = input.Trim();
            string pattern = "[^a-zA-Z0-9\\s-_]";
            string replacement = "";
            string result = Regex.Replace(input, pattern, replacement);
            result = result.Replace(' ', '-');
            result += ".shf";
            return result;

        }
        public static string JSONFixer(string incorrectJson)
        {
            // remove trailing and leading space
            string outLine = "";
            string workLine = "";
            string outString = "";
            char lastChar;
            incorrectJson = incorrectJson.Trim();

            incorrectJson = Utils.trimToSquare(incorrectJson);




            // Remove whitespace between double [[ or ]]
            incorrectJson = Regex.Replace(incorrectJson, @"\[\s+\[", "[[");
            incorrectJson = Regex.Replace(incorrectJson, @"\]\s+\]", "]]");

            // Remove white space ", " to ","


            incorrectJson = Regex.Replace(incorrectJson, "\",\\s+", "\",");

            //inserts missing " at beginning and end of string


            incorrectJson = InsertQuote(incorrectJson);

            // take the substring not including first and last character 

            incorrectJson = incorrectJson.Substring(1, incorrectJson.Length - 2);



            incorrectJson = incorrectJson.Replace("[[", "[").Replace("]]", "]");

            string[] lines = incorrectJson.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines.Length < 5)
            {
                incorrectJson = incorrectJson.Replace("\"],", "\"],\r\n");
                lines = incorrectJson.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            }
            foreach (string line in lines)
            {
                workLine = line.Trim();

                if (line.Contains("[") && line.Contains("]"))
                {
                    workLine = Regex.Replace(workLine, "(?<=[^\"\\]])]", "\"]");

                    lastChar = workLine[workLine.Length - 1];

                    if (lastChar == ',')

                    {
                        outLine = workLine.Substring(0, workLine.Length - 2) + "],";
                    }

                    if (lastChar == '"')

                    {
                        outLine = workLine + "],";
                    }
                }
                else
                {

                }
                outString += "\r\n" + outLine;
            }
            incorrectJson = outString;
            incorrectJson = incorrectJson.Replace("\r\n", "").Replace("\n", "");
            incorrectJson = "[" + incorrectJson + "]";

            string lastTwoChars = "";
            string lastThreeChars = "";


            lastThreeChars = incorrectJson.Substring(incorrectJson.Length - 3, 3);

            if (lastThreeChars == "],]")
            {
                incorrectJson = incorrectJson.Substring(0, incorrectJson.Length - 3) + "]]";
            }

            lastTwoChars = incorrectJson.Substring(incorrectJson.Length - 2, 2);
            if (lastTwoChars != "]]")
            {
                incorrectJson = incorrectJson.Substring(0, incorrectJson.Length - 2) + "]]";
            }

            if (incorrectJson.Substring(0, 2) == "[\"")
            {
                incorrectJson = "[" + incorrectJson.Substring(2, incorrectJson.Length - 2);
            }
            lines = incorrectJson.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines.Length < 3)
            {
                incorrectJson = incorrectJson.Replace("\"],", "\"],\r\n");

            }

            // add new lines to out brackets

            incorrectJson = incorrectJson.Replace("[[", "[\r\n[");
            incorrectJson = incorrectJson.Replace("]]", "]\r\n]");
            return incorrectJson;
        }

        public static string trimToSquare(string input)
        {
            int position = 0;
            string output = input;
            string output2 = "";

            position = output.IndexOf('[');

            if (position != -1) // -1 means '[' was not found in the string
            {
                output = input.Substring(position);

            }
            position = output.LastIndexOf(']');

            if (position != -1) // -1 means ']' was not found in the string
            {
                output2 = output.Substring(0, position + 1);

            }


            return output2;
        }

        public static string InsertQuote(string inputString)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < inputString.Length; i++)
            {
                sb.Append(inputString[i]);

                // Check for the pattern '",x' where x is any character not '"'
                if (i > 1 && inputString[i - 2] == '"' && inputString[i - 1] == ',' && inputString[i] != '"')
                {
                    // Insert '"' before x
                    sb.Insert(sb.Length - 1, '"');
                }
            }

            return sb.ToString();
        }

        public static string rootDir()
        {
            return "c:\\scripthelper-001\\";
        }

        public static string moviesDir()
        {
            return rootDir() + "Movies";
        }
        public static void nop()
        {
            return;
        }

        public static StyleElements noneStyle()
        {
            StyleElements none = new StyleElements("-none-", "author", "-none-");
            return none;

        }

        public static string escapeQuotes(string input)
        {

            string output = input.Replace("\"", "'");
            return output;
        }

        public static string getGoneWithTheWind()
        {

            string text = @"

dialogue should be a mix of both formal and colloquial.  The language should feel grand and often dramatic.

Character Descriptions should be detailed and complex. Provide rich details to make characters come alive - their physical appearance, clothing, demeanor, and their actions. However, remember to maintain the balance between description and action.

Setting Descriptions should create a vivid image in the reader's mind. Descriptions should be filled with sensory details.

The scene script should have strong emotional stakes. Conflicts, both internal and external, should be deeply emotional, often hinging on themes of love, honor, and survival.

Use descriptive language to show rather than tell emotions. Characters should often be at odds with their own desires and the demands or expectations of society, leading to intense inner conflicts.

The pacing should be steady and the drama high. Even in quieter, more introspective moments, the tension should be palpable. The story should flow from one dramatic moment to the next, building in intensity.

Occasionally use voice-over narration for key moments or turning points in the story. The narration should reflect the character's internal thoughts and feelings, often revealing a contradiction between their inner world and their actions.";

            return text;

        }
        public static string getMcKeeScriptStyleGuide()
        {
            string text = "";

            text = @"
Movie Scripts consist mostly of dialogue and decriptions.  In writing this script, please follow these rules:

dialogue:

Movie dialogue must say the maximum in the fewest possible words. Second, it must have direction. Each exchange of dialogue must turn the beats of the scene in one direction or another across the changing behaviors, without repetition.

Movie dialogue must sound like human talk, using an informal and natural vocabulary, complete with contractions, slang, and even, if necessary, profanity.

Movie dialogue should consist of short, simply constructed sentences; generally, a movement from noun to verb to object or from noun to verb to complement in that order.

Movie dialogue doesn't require complete or grammatical sentences.  We often drop the opening article or pronoun. We often speak in phrases.

Movie dialogue is generally constructed from rapid exchanges of short speeches.  

In Movie dialogue rarely use long speeches.  When you do use a long speech break the dialogue with listeners' action or reaction descriptions.


Descriptions:

All description should be in the present tense.  Even in flash backs or flash forwards we are jumping to a new now.

In descriptions avoid generic nouns and verbs with adjectives and adverbs attached and seek the name of the thing: For example instead of  “The carpenter uses a big nail,” use “The carpenter hammers a spike.”

In descriptions minimize the use of inflections of the verb ""to be"" such as ""is"", ""was"", ""are"", ""were"", etc.

In descriptions avoid the use of similes and metaphors.

In descriptions do not use “we see” and “we hear.”

Do not provide camera or editting direction such as ""CUT"", ""FADE IN"", ""FADE OUT"", ""TO BLACK"" .
                ";
            text = text.Trim();

            return text;
        }

        public static string getIronicComedyScriptStyleGuide()
        {
            string text = @"Language and dialogue: Keep the dialogue sharp, funny, and filled with irony. The characters should engage in rapid, witty exchanges. Employ elements of surprise and unexpected humor in the dialogue to enhance the comedic effect.

    Character Interaction: Each character should distinctly contribute to the comedic tone of the scene. Showcase their unique quirks or personality traits in their reactions and interactions with each other.

    Setting Details: Describe the setting in a way that contributes to the comedy. Small, unique details about the environment can add an extra layer of humor to the scene.

    Pacing and Timing: The scene should move quickly, with comedy arising from the pace of the dialogue and actions. Pay attention to comedic timing when placing punchlines or humorous reactions.

    Stage Directions: Stage directions should not only dictate the actions but also suggest the timing and delivery of the comedic elements. They can also highlight physical comedy or visual humor.

    Be Ironic: This can come from situational irony (where actions have an effect that is opposite from what was intended), dramatic irony (where the audience knows something that the characters do not), or verbal irony (where what is said is the opposite of what is meant).

    Conflict and Humor: Use conflict to drive humor in the scene. This could be a misunderstanding, a clash of personalities, or an awkward situation that the characters need to navigate.

    Subversion of Expectations: Incorporate elements of surprise to create humor by subverting audience expectations. This could be a character reacting unusually to a common situation or the situation unfolding contrary to what the characters anticipate.

    Character Moment: Ensure that the scene contributes to the characters' arcs in the larger story. Even comedic scenes should reveal something about the characters or move their story forward.";

            return text;
        }

        public static string get19thCenturyTheatricalScriptStyleGuide()
        {
            string text = @"Language and Dialogue: The dialogue should be written in a formal, elegant, and sometimes grandiloquent manner. Characters should express themselves in complex sentences, filled with poetic metaphors, rich vocabulary, and refined witticisms. 

Character Descriptions: Characters should be vividly detailed and larger-than-life with extra attention given to how chracters are dressed.

Setting Descriptions: The setting descriptions should be lush and highly detailed. 

Dramatic Tension and Pacing: The narrative should be punctuated with melodramatic conflicts and revelations, typical of 19th-century theater. Pacing should vary, with moments of heightened drama and tension contrasted with quieter, more introspective scenes.

Stage Directions: Stage directions should be elaborate, indicating not only the characters' actions but also their emotional state, tone of voice, and implied motivations.";
            return text;
        }

        public static string getDepurpleMovieTextStyleGuide()
        {
            string text = @"Please simplify.  Divide long and complex sentences into multiple shorter and simpler sentences.    Reduce the number of adjectives and adverbs.   Use forms of the verb ""to be"" sparingly.  In general, apply the principles of Strunk and White.";
            text = text.Trim();
            return text;
        }

        public static string getMcKeeMovieTextStyleGuide()
        {
            string text = @"

# Movie and Scene Texts predominantly consist of sequential actions, each narratively articulated. While composing these texts, adhere to the following principles:
## Writing Concise, Compelling Descriptions
  1. Prioritize Specificity: Instead of vague descriptions like ""The carpenter uses a big nail,"" aim for specificity with ""The carpenter secures the beam with a thick, steel spike.""
  2. Eliminate Passive Constructions: Replace forms of ""to be"" with more active, vivid verbs. For instance, rather than ""The room was quiet,"" try ""Silence enveloped the room.""
  3. Directness over Figurative Language: Avoid similes and metaphors to maintain narrative immediacy and realism.
  4. Remove Observer Language: Exclude phrases like “we see” or “we hear,” to immerse the reader directly in the action.
## You're Mostly Narrating Action
  1. Essential Actions Only: Ensure every action described is crucial to advancing the narrative. Unnecessary details can dilute the story's impact.
  2. Emphasize Active Voice: Shift from passive to active voice for a more engaging narrative. Instead of ""The door was opened by John,"" use ""John thrusts open the door.""
  3. Detail in Movement: Describe actions vividly. Change “John walks” to “John strides with purpose, his heels clicking against the floor.”
  4. Logical Sequence of Events: Arrange actions in a coherent, chronological order to maintain narrative flow and logic.
## You're Framing Dialogue Which Will Be Written Later
  1. Focus on Purpose: Rather than writing lines of dialogue, instead summarize with a description of its purpose. So instead of writing ' ""Jonathan? Where are you?"" yelled Greta', it would be 'Greta yells for Jonathan, her voice uncertain'.
  2. Establish Dialogue Dynamics: Outline the power dynamics and emotional undercurrents between characters. Indicate if the dialogue is confrontational, cooperative, or something else, setting the tone for how characters interact verbally.
  3. Highlight Implicit Conflict: Point out any underlying tensions or conflicts in the conversation, even if subtle, adding depth to the dialogue.
  4. Outline Subtext and Intentions: Note the underlying motivations and unspoken thoughts driving the characters' conversation. Explicitly state what each character is trying to achieve through this interaction.
  5. Plan Dialogue Rhythm and Pace: Provide a sense of the dialogue’s tempo. Is it a rapid back-and-forth or a slow, thoughtful exchange? This helps set the pacing for when actual lines are written.
  6. Environment Affects Dialogue: Clearly depict the setting of the dialogue, as it influences the scene's tone. A conversation in a bustling market differs vastly from one in a secluded library.
  7. Pre-dialogue Character States: Detail characters' states just before they speak to add depth. If a character is anxious, describe their jittery hands or shallow breathing.
  8.Show Actions and Reactions: Enhance dialogue with descriptions of physical actions or facial expressions, like a furrowed brow or a tentative smile.
## Avoiding Ornate or ""Purple"" Prose
  1. Clarity Over Complexity: Choose simple, clear language. Replace ""He surveyed his surroundings with apprehension"" with ""He looked around nervously.""
  2. Economical Use of Descriptors: Only use adjectives and adverbs that add essential meaning.
  3. Substance Drives Style: Focus on the story’s essence. Every element should serve the narrative.
  4. Originality in Descriptions: Avoid clichés and seek fresh, unique ways to describe scenes and actions.
  5. Purposeful Figurative Language: Use metaphors and similes only when they add significant value and clarity.";
            text = text.Trim();
            return text;
        }


        public static void EnableControlRightClickForCutPaste(Control control)
        {
            if (control is RichTextBox richTextBox)
            {
                richTextBox.ContextMenu = new ContextMenu();
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Undo", (sender, e) => richTextBox.Undo()));
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Redo", (sender, e) => richTextBox.Redo()));
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Cut", (sender, e) => richTextBox.Cut()));
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Copy", (sender, e) => richTextBox.Copy()));
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Paste", (sender, e) => richTextBox.Paste()));
                richTextBox.ContextMenu.MenuItems.Add(new MenuItem("Delete", (sender, e) => richTextBox.SelectedText = ""));
            }

            foreach (Control childControl in control.Controls)
            {
                EnableControlRightClickForCutPaste(childControl); // Recursive call for child controls
            }
        }


        public static void EnableFormRightClickForCutPaste(Form form)
        {
            foreach (Control control in form.Controls)
            {
                EnableControlRightClickForCutPaste(control);
            }
        }

        public static int getIntFromDataDict(string myKey, int defaultValue)
        {
            object retrievedObject;
            int returnValue;
            if (dataDictionary.TryGetValue(myKey, out retrievedObject))
            {
                try
                {
                    // returnValue = (int)retrievedObject;
                    returnValue = Convert.ToInt32(retrievedObject);
                }
                catch
                {
                    returnValue = defaultValue;
                }

            }
            else
            {
                returnValue = defaultValue;
            }

            return returnValue;






        }

        public static Boolean checkDumpPWD(string input)
        {
            Boolean pwdFlag = false;

            if (input == Utils.dumpJSONpwd) { pwdFlag = true; } else { pwdFlag = false; }

            return pwdFlag;
        }
        public static string spliceWithLocs(string originalScript, string selectedText, int selectedStart, int selectedLength, string spliceInText)
        {


            string output = "";
            string leftSide = originalScript.Substring(0, selectedStart);
            string rightSide = originalScript.Substring(selectedStart + selectedLength);

            if (leftSide.Length > 0)
            {
                if (leftSide[leftSide.Length - 1] == ' ')
                {
                    output = leftSide + spliceInText.Trim();
                }
                else
                {
                    output += leftSide + " " + spliceInText.Trim();
                }
            }
            else
            {
                output = spliceInText.Trim();
            }

            if (rightSide.Length > 0)
            {
                if (rightSide[0] == ' ')
                {
                    output += rightSide;
                }
                else
                {
                    output += " " + rightSide;
                }
            }
            else
            {
                Utils.nop();
            }

            return output;


        }

        public static string addSceneBreak(string originalScript,  int selectedStart, string spliceInText)
        {

            
            int selectedLength = 0;
            string output = "";
            string leftSide = originalScript.Substring(0, selectedStart);
            string rightSide = originalScript.Substring(selectedStart + selectedLength);

            if (leftSide.Length > 0)
            {
                if (leftSide[leftSide.Length - 1] == ' ')
                {
                    output = leftSide + spliceInText;
                }
                else
                {
                    output += leftSide + " " + spliceInText;
                }
            }
            else
            {
                output = spliceInText;
            }

            if (rightSide.Length > 0)
            {
                
                
                output += rightSide;
                
            }
            else
            {
                Utils.nop();
            }

            return output;


        }
        public static string spliceIn(string originalScript, string originalSelectedText, string splice)
        {



            int originalSelectionLength = originalSelectedText.Length;

            int startPosition = originalScript.IndexOf(originalSelectedText);


            string output = "";
            string leftSide = originalScript.Substring(0, startPosition);
            string rightSide = originalScript.Substring(startPosition + originalSelectionLength);

            // output = leftSide + " " + splice.Trim() + " " + rightSide;

            if (leftSide.Length > 0)
            {
                if (leftSide[leftSide.Length - 1] == ' ' || IsEnglishPunctuation(leftSide[leftSide.Length - 1]))
                {
                    output = leftSide + splice;
                }
                else
                {
                    output += leftSide + " " + splice;
                }
            }
            else
            {
                output = splice;
            }


            if (rightSide[0] == ' ' || IsEnglishPunctuation(rightSide[0]))
            {
                output += rightSide;
            }
            else
            {
                output += " " + rightSide;
            }
            return output;

        }

        public static string SpliceInAtCursor(string originalScript, string splice, int cursorPosition)
        {
            // Ensure cursor position is within the bounds of the original script
            if (cursorPosition < 0 || cursorPosition > originalScript.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(cursorPosition));
            }

            // Split the original script into two parts at the cursor position
            string leftSide = originalScript.Substring(0, cursorPosition);
            string rightSide = originalScript.Substring(cursorPosition);

            // Insert 'splice' between leftSide and rightSide
            string output = leftSide + splice + rightSide;

            return output;

        }

            static bool IsEnglishPunctuation(char c)
        {
            return ".!?,;:'\"-()[]{}".Contains(c.ToString());
        }
        public static List<string> cleanupScriptArray(string script)
        {
            script = script.Replace("*", "");

            List<string> firstLines = script.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            int firstKount = firstLines.Count;

            firstLines = firstLines.Select(s => s.Trim()).ToList();

            List<string> secondLines = new List<string>();


            Boolean copyFlag = false;

            foreach (string line in firstLines)
            {
                if (line.Length >= 4)
                {
                    if (line.ToUpper().Substring(0, 4) == "EXT." || line.ToUpper().Substring(0, 4) == "INT.")
                    { copyFlag = true; }

                }
                if (copyFlag)
                {
                    if (!line.Contains("FADE OUT") && !line.Contains("FADE TO BLACK") && !line.Contains("CUT TO BLACK"))


                    { secondLines.Add(line); }
                }


            }

            int secondKount = secondLines.Count;

            double shrink = 0;

            if (firstKount > 0)

            {
                shrink = (double)secondKount / (double)firstKount;
            }
            else
            {
                shrink = .25;
            }

            if (shrink > .75)
            {
                return secondLines;
            }
            else
            {
                return firstLines;
            }


        }

        public static string assembleUnformattedScriptFromArrayRTF(List<string> scriptArray)
        {
            string script = "";
            string scriptRTF;
            foreach (string line in scriptArray)
            {
                script = script + paritLEFT(line);

            }

            scriptRTF = @"{\rtf1\ansi " + script + "}";

            return scriptRTF;
        }

        public static Boolean lowerAlpha(string input)
        {
            foreach (char c in input)
            {
                if (char.IsLower(c))
                {
                    return true; // Return true if a lowercase character is found
                }
            }
            return false;  // else return false
        }

        public static Boolean upperAlpha(string input)

        {
            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    return true; // Return true if an uppercase character is found
                }
            }
            return false;  // else return false
        }

        public static Boolean isBlank(string input)
        {
            if (input.Trim().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string cleanSpeaker(string input)
        {
            input = input.Trim();
            input = input.Replace(":", "");
            input = input.Replace("\"", "");
            input = input.Replace("'", "");
            input = input.Replace("*", "");

            input = input.ToUpper();

            return input;
        }

        public static string paritLEFT(string input)
        {
            input = "\r\n\r\n" + "\\pard\\f0\\fs20\\ql " + input + "\\par" + "\r\n\r\n";


            return input;
        }

        public static string paritCENTER(string input)
        {
            input = "\r\n\r\n" + "\\pard\\f0\\fs20\\qc\\li770\\ri770 " + input + "\\par" + "\r\n\r\n";

            return input;
        }

        public static string paritINDENTED(string input)
        {
            input = "\r\n\r\n" + "\\pard\\f0\\fs20\\ql\\li770\\ri770 " + input + "\\par" + "\r\n\r\n";
            return input;
        }

        public static string makeRTFBlank()
        {
            string reply = @"{\rtf1\ansi\deff0{\fonttbl{\f0 Courier New;}}" + "   }";
            return reply;
        }

        public static Boolean isParens(string input)
        {

            input = input.Trim();
            if (input[0] == '(' && input[input.Length - 1] == ')')


            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean isSpeakerCandidate(string input)
        {

            input = input.Trim();
            int parensLocation = input.IndexOf('(');



            if (parensLocation > -1)
            {
                if (parensLocation > -1)
                {
                    input = input.Substring(0, parensLocation);
                }


            }

            if (input.Length > 0)
            {
                if (!lowerAlpha(input))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public static string assembleFORMATTEDScriptFromArrayRTF(List<string> scriptLines)
        {


            string script = "";
            string scriptRTF = "";
            int lineKount = scriptLines.Count;
            string currentLine = "";
            string nextLine = "";


            Boolean inDialog = false;
            for (int j = 0; j < lineKount; j++)
            {
                currentLine = scriptLines[j];
                if (j + 1 < lineKount)
                {
                    nextLine = scriptLines[j + 1];
                }
                else
                {
                    nextLine = "***THEEND***";
                }

                if (lowerAlpha(currentLine) && !isSpeakerCandidate(currentLine) && !inDialog)
                {
                    script = script + paritLEFT(currentLine);

                }
                else if (isBlank(currentLine) && !inDialog)
                {
                    script = script + paritLEFT("      ");
                }
                else if (isSpeakerCandidate(currentLine) && !inDialog)
                {
                    if (isBlank(nextLine) && !inDialog)
                    {
                        script = script + paritLEFT(currentLine);
                    }
                    else
                    {
                        inDialog = true;
                        currentLine = cleanSpeaker(currentLine);
                        script = script + paritCENTER(currentLine);


                    }
                }
                else if (!isBlank(currentLine) && inDialog)
                {
                    if (isParens(currentLine))
                    {

                        script = script + paritCENTER(currentLine);

                    }
                    else { script = script + paritINDENTED(currentLine); }


                }
                else if (isBlank(currentLine) && inDialog)
                {
                    script = script + paritINDENTED("    ");
                    inDialog = false;
                }
                else if (inDialog && nextLine == "***THEEND***")
                {
                    script = script + paritINDENTED(currentLine);
                }
                else if (!inDialog && nextLine == "***THEEND***")
                {
                    script = script + paritLEFT(currentLine);
                }
                else
                {
                    script = script + paritLEFT(currentLine);
                }

            }

            scriptRTF = @"{\rtf1\ansi\deff0{\fonttbl{\f0 Courier New;}}" + "\r\n" + script + "}";


            return scriptRTF;

        }
        public static string formatScript(string script, Boolean formatFlag)
        {

            List<string> lines = cleanupScriptArray(script);



            if (!formatFlag) { return assembleUnformattedScriptFromArrayRTF(lines); }




            return assembleFORMATTEDScriptFromArrayRTF(lines);




        }

        public static List<string> ExtractTextBetweenAngleBrackets(string myString)
        {
            List<string> results = new List<string>();
            Regex regex = new Regex("<(.*?)>", RegexOptions.Singleline);

            foreach (Match match in regex.Matches(myString))
            {
                results.Add(match.Groups[1].Value);
            }

            // Remove duplicates and sort in alphabetical order
            return results.Distinct().OrderBy(s => s).ToList();
        }

        public static string TrimOutsideBrackets(string input)
        {
            int startIndex = input.IndexOf('[');
            int endIndex = input.LastIndexOf(']');

            if (startIndex == -1 || endIndex == -1 || startIndex > endIndex)
            {
                // Return empty string in brackets incorrect.
                return "";
            }

            return input.Substring(startIndex, endIndex - startIndex + 1);
        }


        public static string TrimOutsideCurlyBrackets(string input)
        {
            int startIndex = input.IndexOf('{');
            int endIndex = input.LastIndexOf('}');

            if (startIndex == -1 || endIndex == -1 || startIndex > endIndex)
            {
                // Return empty string in brackets incorrect.
                return "";
            }

            return input.Substring(startIndex, endIndex - startIndex + 1);
        }

        public static string[] SplitStringAtNewLines(string input)
        {
            // Split the string on both Unix and Windows-style newlines
            return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
        public static string ExtractFirstNumericString(string input)
        {
            Match match = Regex.Match(input, @"\d+");  // \d represents numeric characters and + ensures continuity

            return match.Success ? match.Value : null;
        }
        public static Boolean CanConvertToInt(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        public static string getAngleBracketPrompt()
        {
            string prompt = @"Instructions for Formatting Character Names:
- In your output, use first names only for the characters. 
- For every occurrence of a character name in the output, enclose the character name within angle brackets <>. 
- Nonhuman beings are all considered characters if they have a name, and therefore their names should be enclosed in angle brackets <>.
- When using the possessive form of a character name, place the apostrophe outside of the angle brackets.";
            prompt = "\r\n\r\n" + prompt + "\r\n\r\n";
            return prompt;
        }

        public static string lookupCharacterRadioButton(string rbName, CharacterObj character)
        {
            string attributePrompt = "";
            switch (rbName)
            {
                case "radioBackStory":
                    attributePrompt = $"the back story for {character.tagName}, age {character.age}. ";
                    attributePrompt += "The back story should be a two-paragraph biography, starting with the character's age and then describing their life prior to the story, including education, socioeconomic background, and important life events.";
                    break;
                case "radioPhysical":
                    attributePrompt = $"a physical description for {character.tagName}, age {character.age}. ";
                    attributePrompt += "The physical description should be a paragraph providing a physical description of the character at the time of the story in the Story Text. Include descriptions of how the character dresses and how they carry themselves in the world. ";

                    break;
                case "radioPersonality":
                    attributePrompt = $"a description of the personality for {character.tagName}, age {character.age}. ";
                    attributePrompt += "The personality description should be a paragraph describing the character's personality, including traits, core beliefs, temperament, and other relevant characteristics. ";
                    break;

                case "radioSpeakingStyle":
                    attributePrompt = $"a description of the speaking style for {character.tagName}, age {character.age}. ";
                    attributePrompt += "The speaking style description should be a paragraph describing the style of speech of the character including their typical register of speech, formality or informality, adherence to standard grammar, degree of verbosity, vocabulary, voice tone, gestures, dialect, and other relevant characteristics of their communication style. ";
                    break;
                default:
                    attributePrompt = null;
                    return null;

            }

            return attributePrompt;
        }

        public static string workingTag()
        {
            return "::: Working :::";


        }

        public static string singleAngleBrackets(string input)
        {
            input = input.Replace(">>", ">");
            input = input.Replace("<<", "<");

            
            return input;
        }

        public static string replaceQuoteWithSingleQuotes(string input)
        {
            input.Replace('"','\'');
            return input;
        }

        public static Boolean checkPropertyExistsInObj(object myObj, string myProperty)
        {
            Type type = myObj.GetType();

            PropertyInfo propInfo = type.GetProperty(myProperty, BindingFlags.Public | BindingFlags.Instance);
            if (propInfo != null)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }

        public static int getOriginDays()
        {
            // Define the origin date
            DateTime origin = new DateTime(1970, 1, 1);

            // Get the current date and time
            DateTime now = DateTime.UtcNow;

            // Calculate the difference in days
            TimeSpan diff = now - origin;

            // Get the number of days (whole days)
            int days = diff.Days;

            return days;
        }

        public static int checkGPT4TurboLimit()
        {
            Boolean flag = true;
            int day = Utils.getOriginDays();
            int count = 0;
            string oFile = Utils.rootDir() + "GPT4TurboLimit.Txt";
            string data;
            string oText = "";

            if (File.Exists(oFile))
            {
                data = File.ReadAllText(oFile);
                string[] tokens = data.Split(new char[] { ':' }, 2);
                if (int.Parse(tokens[0]) == 666) { return 1; }
                if (int.Parse(tokens[0]) == day)
                {
                    count = int.Parse(tokens[1]);
                    count++;
                }
                else
                {
                    count = 1;
                }
                oText = $"{day} : {count}";
                File.WriteAllText(oFile, oText);

            }
            else
            {
                count = 1;
                oText = $"{day} : {count}";
                File.WriteAllText(oFile, oText);

            }

            return count;


        }

        public static List<string> getUSARatings()
        {
            List<string> ratings = new List<string>();

            ratings.Add("-none-");
            ratings.Add("G");

            ratings.Add("PG");
            ratings.Add("PG-13");
            ratings.Add("R");
            ratings.Add("NC-17");

            return ratings;
        }

        public static string getProfilePrompt(FormApp1 myForm,string myType)
        {
            MovieObj movieObj = new MovieObj();
            movieObj = myForm.getMyMovie();
            string promptString = "";
            Boolean makePrompt = false;

            if (myType == "MovieText" || myType == "SceneText" || myType == "SceneScript" || myType == "MakeScenes" || myType == "Character")
            {
                makePrompt = true;
            }

            if (!makePrompt) { return ""; }

            if (movieObj.genre.Length > 0)
            {
                promptString += $"The genre of the movie is {movieObj.genre}. ";
            }
            if (movieObj.audience.Length > 0) 
            { 
                promptString += $"The audience for the movie is {movieObj.audience}. ";
            }
            if (movieObj.ratingUSA != "-none-")
            {
                promptString += $"The Motion Picture Association rating for the movie is {movieObj.ratingUSA}. ";
            }
            if (movieObj.guidance.Length > 0) 
            { 
                promptString += $"Guidance about the movie: {movieObj.guidance}. ";
            
            }

            if (promptString.Length > 0) { promptString = $"\r\n\r\n{promptString}\r\n\r\n"; }

            if (myType == "MovieText" && !myForm.checkedUseProfileMovieText())
            {
                return "";
            }

            if (myType == "SceneText" && !myForm.checkedUseProfileSceneText())
            {
                return "";
            }
            if (myType == "SceneScript" && !myForm.checkedUseProfileSceneScript())
            {
                return "";
            }
            if (myType == "MakeScenes" && !myForm.checkedUseProfileMakeScenes())
            {
                return "";
            }
            return promptString;
        }

        public static List<string> noWhiteSpaceList(List<string> input)
        {


            return input.Where(item => !string.IsNullOrWhiteSpace(item.Trim())).ToList();

        }

        public static List<SceneObj> splitMakeSceneObjListCompress(string input, string inputCompressed)
        {

            input = $"\r\n{input}\r\n";
            string workPara = "";
            string sceneTitle = "";
            string sceneHint = "";
            string compressedSceneHint = "";

            int index = -1;
            List<SceneObj> sceneObjList = new List<SceneObj>();

            List<TitleText> titleTexts = new List<TitleText>();
            List<TitleText> compressedTitleTexts = new List<TitleText>();

            SceneObj workScene = new SceneObj();
            Boolean firstTitleFlag = false;

            // string[] inputLines = input.Split(new[] { "===" }, StringSplitOptions.None);

            titleTexts = JsonConvert.DeserializeObject<List<TitleText>>(input);
            compressedTitleTexts = JsonConvert.DeserializeObject<List<TitleText>>(inputCompressed);

            int loopKount = 0;

            foreach (TitleText titleText in titleTexts)
            {
                sceneTitle = titleText.title.Trim(); ;
                sceneHint = titleText.text.Trim();

                if (loopKount <= compressedTitleTexts.Count)
                {
                    index = compressedTitleTexts.FindIndex(a => a.title == sceneTitle);

                    
                    if (index > -1)
                    {
                        compressedSceneHint = compressedTitleTexts[index].text.Trim();
                    }
                    else
                    {
                        compressedSceneHint = sceneHint;
                    }


                }
                if (sceneTitle.Length == 0 || sceneHint.Length == 0)
                {
                }
                else
                {
                    workScene = new SceneObj();
                    workScene.Title = sceneTitle;
                    workScene.Hint = sceneHint;
                    workScene.compressedHint = compressedSceneHint;
                    workScene.splitSceneMakeFlag = true;
                    sceneObjList.Add(workScene);
                }
                loopKount++;
            }

            /* foreach (string para in inputLines) 
            {
                
                sceneTitle = splitGetTitle(para);
                sceneHint = splitMakeSceneHint(para);

                if (sceneTitle.Length == 0 || sceneHint.Length == 0)
                {
                    
                }
                else
                {
                    workScene = new SceneObj();
                    workScene.Title = sceneTitle;
                    workScene.Hint = sceneHint;
                    workScene.splitSceneMakeFlag = true;
                    sceneObjList.Add(workScene);
                }

            } */

            return sceneObjList;
        }
        public static List<SceneObj> splitMakeSceneObjList(string input)
        {

            input = $"\r\n{input}\r\n";
            
            string sceneTitle = "";
            string sceneHint = "";
            

           
            List<SceneObj> sceneObjList = new List<SceneObj>();

            List<TitleText> titleTexts = new List<TitleText>();
            

            SceneObj workScene = new SceneObj();
            
            titleTexts = JsonConvert.DeserializeObject<List<TitleText>>(input);
            
            foreach (TitleText titleText in titleTexts)
            {
                sceneTitle = titleText.title.Trim(); ;
                sceneHint = titleText.text.Trim();

                if (sceneTitle.Length == 0 || sceneHint.Length == 0)
                {
                }
                else
                {
                    workScene = new SceneObj();
                    workScene.Title = sceneTitle;
                    workScene.Hint = sceneHint;
                    workScene.compressedHint = "";
                    workScene.splitSceneMakeFlag = true;
                    sceneObjList.Add(workScene);
                }
                
            }

            

            return sceneObjList;
        }
        public static string splitGetTitle(string input)
        {

            input = input.Trim();

            List<string> lines = new List<string>();
            lines = splitLines(input);

            foreach (string line in lines)
            {
                if (line.Length > 7)
                {
                    if (line.Substring(0, 6) == "Title:")
                    {
                        return line.Substring(6).Trim();
                    }
                }
                
            }
            return "";
        }

        public static List<string> splitLines(string input)
        {
            input = input.Trim();
            List<string> output = new List<string>();
            string[] inputLines = input.Split(new[] { "\r\n","\n" }, StringSplitOptions.None);
            foreach (string line in inputLines)
            {
                output.Add(line.Trim());
            }
            return output;
        }
        
        public static string splitMakeSceneHint(string input)
        {
            input = input.Trim();
            string outputString = "";
            string workLine = "";
            List <string> output = new List<string>();
            output = splitLines(input);
            foreach (string line in output)
            {
                
                workLine = line.Trim();

                if (!workLine.Contains("==="))
                {
                    if (workLine.Length > 7)
                    {
                        if (workLine.Substring(0, 6) == "Title:")
                        {

                        }
                        else
                        {
                            outputString += workLine + "\r\n";
                        }
                    }
                    else
                    {
                        outputString += workLine + "\r\n";
                    }
                }
                

            }

            return outputString;
        }

        public static string truncStringAtWord(string input, int truncLength)
        {
            // If the string length is truncLength characters or less, return it as is
            if (input.Length <= truncLength)
            {
                return input;
            }

            // Find the first space after the 120th character
            int truncateIndex = input.IndexOf(' ', truncLength -1);

            
            if (truncateIndex == -1)
            {
                return input;
            }

            // Otherwise, truncate the string at the first space after the 120th character
            return input.Substring(0, truncateIndex).Trim();
        }

        public static int CountSubstringOccurrences(string input, string substring)
        {
            int count = 0;
            int index = 0;

            while ((index = input.IndexOf(substring, index)) != -1)
            {
                count++;
                index += substring.Length;
            }

            return count;
        }
        public static string RemoveSubstring(string input, int startIndex, int length)
        {
            // Check if the startIndex and length are valid
            if (startIndex < 0 || startIndex >= input.Length || startIndex + length > input.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex or length is out of range.");
            }

            // Split the string and concatenate
            string firstPart = input.Substring(0, startIndex);
            string secondPart = input.Substring(startIndex + length);

            return firstPart + secondPart;
        }

        public static string PadEndIfNeeded(string input, string pad)
        {
            int padLength = pad.Length;

            input = input.Trim();
            if (input.Length < pad.Length) { return input + "\r\n" + pad; }

            if (input.Substring(input.Length - padLength) == pad)
            {
                return input;
            }
            else
            {
                return input + "\r\n" + pad;
            }

        }
        public static string[] splitMovieTextAtEquals(string movieText)
        {
            string[] response;
            string workMovieText = movieText.Trim();
            
            workMovieText = PadEndIfNeeded(workMovieText,"===");
            response = workMovieText.Split(new string[] { "===" }, StringSplitOptions.RemoveEmptyEntries);

            return response;

        }

        public static (string,string[]) spltMovieTextAtEqualsJSON(string movieText)
        {
            string[] sceneArray = splitMovieTextAtEquals(movieText);
            string returnJSON = "";
            int sceneKount = sceneArray.Length;
            int loopKount = 0;
            foreach(string scene in sceneArray) 
            
            { 
                returnJSON += $"{{\"scene_number\":\"{loopKount + 1}\",\"text\":\"{scene}\"}}";

                if (loopKount < sceneKount - 1)
                {
                    returnJSON += ",";
                }
            
            }


            returnJSON = $"[\r\n{returnJSON}\r\n]";

            return (returnJSON,sceneArray) ;

        
        }

        public static int countSubStringOccurances(string mainString, string subString)
        {
            
                int count = 0;
                int index = 0;

                while ((index = mainString.IndexOf(subString, index, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    count++;
                    index += subString.Length;
                }

                return count;
            
        }

        internal static string ConvertJsonrrayToCsv(string jsonArray)
        {
            var array = JArray.Parse(jsonArray);
            if (array.Count == 0)
            {
                return string.Empty;
            }

            var csvBuilder = new StringBuilder();
            var headers = new List<string>();

            // Using the first object to get the field names and types
            foreach (var token in array.First.Children<JProperty>())
            {
                headers.Add(token.Name);
            }

            // Writing the header to CSV
            csvBuilder.AppendLine(string.Join(",", headers));

            // Writing the rows
            foreach (var obj in array.Children<JObject>())
            {
                var values = new List<string>();
                foreach (var header in headers)
                {
                    var value = obj[header]?.ToString() ?? "";
                    value = value.Replace("\"", "\"\""); // Escape double quotes
                    values.Add($"\"{value}\""); // Quote the field
                }
                csvBuilder.AppendLine(string.Join(",", values));
            }

            return csvBuilder.ToString();
        }
    }
}
