using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScriptHelper
{
    public class MyGPT
    {
        public static async Task<string> JimRutt(string input, string model)
        {
            return "nothing here";
        }

        public static async Task<string> makeMovieText(string input, string model, int lengthMinutes, StyleElements movieAndSceneTextStyle, FormApp1 myForm, Boolean characterFlag, List<CharacterObj> myCharacters)
        {
            string systemPrompt = "";
            string userPrompt = "";
            string errorMsg = "";

            string MovieLengthHHMM = "";

            int lengthHours = lengthMinutes / 60;
            int lengthRumpMinutes = lengthMinutes - (lengthHours * 60);

            if (lengthHours == 0)
            {
                MovieLengthHHMM = $"{lengthMinutes} minutes";
            }

            else
            {
                MovieLengthHHMM = $"{lengthHours} hours and {lengthRumpMinutes} minutes";
            }

            if (myCharacters.Count == 0) { characterFlag = false; }

            systemPrompt = $"You are a talented screenwriter working on a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}";
            systemPrompt += "Today's task will be to write a movie narrative, a detailed synopsis of the movie that will be used later as a basis for generating the screenplay. ";

            systemPrompt += "\r\n\r\nYou will be provided with a \"Movie Hint,\" a short text expressing the movie's premise or main idea. You will write a narrative text based on the Movie Hint, but much expanded in length and detail. ";
            
            if (characterFlag) 
            {
                systemPrompt += "\r\n\r\nYou will be provided with a list of characters and their attributes that you will use in writing the narrative. ";
                systemPrompt += "You will likely need to invent several additional characters to fill out the story. Use your creative powers to invent compelling new characters who will enrich the narrative. ";
            }

            systemPrompt += $"\r\n\r\nPlease use your powerful creative skills to interpolate additional actions, events, descriptions, and details into the narrative description so that it is long enough to create a movie script that is about {lengthMinutes} minutes long.";
            systemPrompt += Utils.getAngleBracketPrompt();

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            userPrompt = "\r\nHere is the Movie Hint to use for writing the detailed synopsis:\r\n\r\n";
            userPrompt += input;

            if (characterFlag)
            {
                userPrompt += "\r\n\r\nHere are the characters and their attributes that you should use in writing the narrative. You must include all of these characters in the movie narrative:\r\n\r\n";
                foreach (CharacterObj character in myCharacters)
                {
                    userPrompt += $"Character Name: <{character.tagName}>\r\n\r\n";

                    if (character.age.Trim() == "10000")
                    {
                        userPrompt += "Age: Immortal\r\n\r\n";
                    }
                    else
                    {
                        userPrompt += $"Age: {character.age}\r\n\r\n";
                    }

                    userPrompt += $"Back Story: {character.backStory}\r\n\r\n";
                    userPrompt += $"Physical Description: {character.physicalDescription}\r\n\r\n";
                    userPrompt += $"Personality: {character.personality}\r\n\r\n";
                    // userPrompt += $"Style of Speaking: {character.speechStyle}\r\n\r\n";

                    userPrompt += $"END OF CHARACTER ATTRIBUTES FOR <{character.tagName}>\r\n\r\n";
                }
                userPrompt += "END OF CHARACTER LIST\r\n\r\n";
            }
            
            //userPrompt += "Stylistically, the writing of this description should be simple, direct, and functional. Avoid florid and purple prose, and include only details that are relevant to the plot. ";

            userPrompt += "\r\n\r\nPlease DO NOT include any disclaimers, e.g. \"Here is the rewritten narrative:\". Begin now with the first sentence of the movie narrative. ";


            string messageString = "making Movie Text";

            if (characterFlag) { messageString += " with Character Attributes"; }

            string response = await UtilsGPT.doGPT("MakeMovieText", model, 3500, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Movie Text");

            return response;
        }

        public static async Task<string> makeScenesFromMovieText(string movieText, string model, int sceneKount, string errorOut, FormApp1 myForm)
        {

            string systemPrompt, userPrompt;
            string input = "";

            input = Utils.escapeQuotes(movieText);
            systemPrompt = "";
            userPrompt = "";



            systemPrompt = $"You are a visionary screenwriter developing a movie script. {Utils.getProfilePrompt(myForm, "MakeScenes")}";
            systemPrompt += "You will be presented with a synopsis, the Movie Narrative. You will read the Movie Narrative carefully and translate it into a Scene List. The scenes should be described in full detail, such that if you had to reconstruct the full Movie Narrative from the Scene List, you could.";
            systemPrompt += $"\r\n\r\nFor this movie, you will be making exactly {sceneKount.ToString()} scenes. For each of the {sceneKount.ToString()} scenes, please include a Scene Title and a Scene Description. Do not include scene numbers.";
            systemPrompt += "\r\n\r\nEach scene should include a title and a description of the action in that scene. Each scene description must be a full paragraph. Describe sequences of action in as much detail as space permits, adding detail as necessary. Do not include scene numbers. ";
            systemPrompt += "\r\n\r\nYour output will consist entirely of a JSON list of lists. In this form: ";
            systemPrompt += "\r\n\r\n[[\"scene #1 title\",\"scene #1 description\"],[\"scene #2 title\",\"scene #2 description\"],[\"scene #3 title\",\"scene #3 description\"]], etc";
            systemPrompt += $"\r\n\r\nEvery event described in the Movie Narrative must be included in the Scene List. Preserve details as much as possible. It's crucial that NO information gets lost between the Movie Narrative and the scenes list. Detail MAY be added. Leave no ambiguities about the setting or the way action unfolds. After writing a Scene, double-check to make sure no action is missing that was in the original Movie Narrative. ";
            systemPrompt += $"\r\n\r\nRemember, you must output exactly {sceneKount} scenes. In the event that the provided Movie Narrative doesn't supply enough material for {sceneKount} distinct scenes, you should add events between the beginning and end to meet the quota. Do not add scenes after where the Movie Narrative ends. Do not duplicate scenes. Each title and description should be unique.";
            systemPrompt += "\r\n\r\nPlease follow these style rules: ";
            systemPrompt += "\r\n\r\n- Writing style should be straightfoward and functional. Avoid decorative language. Each sentence should be short, direct, and in the active voice. ";
            systemPrompt += "\r\n\r\n- Always mention every character that will appear in the scene. ";
            systemPrompt += "\r\n\r\n- Every scene should have a clearly named setting that makes sense and is cinematically engaging. ";
            systemPrompt += "\r\n\r\n- Scenes should never blend together, but should be carefully delineated. ";
            systemPrompt += "\r\n\r\n- Every scene should advance the plot. That means, by the end of the scene, something should have changed for the characters and/or their relationships. ";
            systemPrompt += "\r\n\r\n- Scenes must be created in the order in which they appear in the Movie Narrative. ";
            systemPrompt += "\r\n\r\n- Do not create any scenes for events that occur AFTER the final action in the Movie Narrative. ";
            systemPrompt += "\r\n\r\n- When choosing between a less precise word and a more precise one, choose the more precise.";
            systemPrompt += "\r\n\r\n- Enclose every instance of a character's name in angle brackets <>. For example, <Robert>. ";

            userPrompt += "\r\n\r\nHere is the Movie Narrative that you'll use as the basis for creating the scenes:";
            userPrompt += "\r\n\r\n" + input + "\r\nEND MOVIE NARRATIVE";
            userPrompt += "\r\n\r\nClose your eyes, take a deep breath, and imagine the full movie in vivid detail before you begin. Then, carefully consider how each set of actions in the movie could best be divided cleanly into a series of clear, detailed, and dramatic scenes. ";
            string response = await UtilsGPT.doGPT("MakeSceneFromMovieText", model, 4000, .7, userPrompt, systemPrompt, errorOut, myForm, "making Scenes from Movie text");




            return response;
        }
        /*public static async Task<string> makeSceneText(string model, MovieObj myMovie, List<SceneObj> sceneList, int sceneNum, FormApp1 myForm)
        {
            string userPrompt = "";
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneText")}";
            

            systemPrompt += "Your task will be to take a Scene Hint that is provided in the user prompt and to write a detailed narrative description of the movie scene (aka \"Scene Text\"). Please ensure that your Scene Text does not re-describe characters or events that have already been introduced or have occurred in the preceding scenes. Your scene should also fit logically into the timeline of the story and maintain chronological consistency with the events described in previous scenes. ";
            systemPrompt += "\r\n\r\nBelow is the Movie Text, a synopsis of the entire movie:\r\n\r\n";
            systemPrompt += myMovie.movieText + "</movietext>";

            if (sceneNum > 1)
            {
                systemPrompt += "\r\n\r\nHere are the scenes that come prior to the scene you will be writing:";

                int sceneKount = 0;
                string sceneKountStr;
                foreach (SceneObj myScene in sceneList)
                {
                    sceneKount += 1;
                    if (sceneKount < (sceneNum - 1))
                    {
                        sceneKountStr = sceneKount.ToString();
                        systemPrompt += $"\r\n\r\nScene {sceneKountStr} Title: " + myScene.Title + $"\r\nScene {sceneKountStr} Hint: " + myScene.Hint;
                    }
                    else
                    {
                        break;
                    }


                }

                if (sceneList[sceneNum - 2].BeatSheetText != null && sceneList[sceneNum - 2].BeatSheetText.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Beat Sheet (a list of the narrative beats) for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + "</scenehint>";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Beat Sheet:\r\n" + sceneList[sceneNum - 1].BeatSheetText + "</beatsheet>";
                }

                else if (sceneList[sceneNum - 2].NarrativeText != null && sceneList[sceneNum - 2].NarrativeText.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Scene Text for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Text: " + sceneList[sceneNum - 2].NarrativeText + $"</scenetext>";
                }

                else if (sceneList[sceneNum - 2].Hint != null && sceneList[sceneNum - 2].Hint.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
                }

                systemPrompt += "\r\n\r\nYou will refer to the above prior scenes to ensure chronological consistency and to avoid redundancy in character descriptions or events. ";

            }


            userPrompt = Utils.getAngleBracketPrompt();
            userPrompt += "\r\n\r\nDo not provide a title for the scene narrative. ";

            if (sceneNum > 1)
            {
                userPrompt += "\r\n\r\nStart your scene with a brief introductory phrase that situates it with respect to the preceding scene. ";
            }
            userPrompt += "\r\n\r\nPlease write a detailed narrative scene description from this scene hint:\r\n\r\n" + sceneList[sceneNum - 1].Hint + "</scenehint>";
            userPrompt += "\r\n\r\nStylistically, the writing of the scene description should be simple and direct. Avoid florid and purple prose, and include only details that are relevant to the plot. Write in the present tense and remember that this is a description of a film scene. ";
            userPrompt += "\r\n\r\n- Do not enclose your response in any metadata tags.";
            userPrompt += "\r\n- Do not finish with \"END SCENE\" or similar. Simply finish with the final action of the scene.";
            userPrompt += "\r\n- Do not describe the scene fading out or its effect on the audience.";
            userPrompt += "\r\n\r\nPlease write the scene description immediately, with no preamble or introductory sentence.";


            string response = await UtilsGPT.doGPT(model, 1000, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Scene Text");

            return response;
        } */


        public static async Task<string> makeSceneText(string model, MovieObj myMovie, List<SceneObj> sceneList, int sceneNum, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            string userPrompt = "";
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneText")}";
            systemPrompt += "Your task will be to take a Scene Hint that is provided in the user prompt below and to write a detailed narrative description of the movie scene (aka \"Scene Text\").";

            if (sceneNum == 1)
            {
                systemPrompt += "\r\n\r\nToday you will be writing the Scene Text for the first scene in the movie. Any characters should be introduced with their age, physical characteristics, and attitude.";
            }
            else if ((sceneNum > 1) && (sceneNum != sceneList.Count))
            {
                systemPrompt += "\r\n\r\nPlease ensure that your Scene Text does not re-describe characters or events that have already been introduced or have occurred in the preceding scenes. If a character IS being introduced for the first time, describe their age, appearance, and attitude. ";
                systemPrompt += "\r\n\r\nYour scene should also fit logically into the timeline of the story and maintain chronological consistency with the events described in previous scenes. ";
            }
            else if ((sceneNum == sceneList.Count))
            {
                systemPrompt += "\r\n\r\nToday you will be writing the Scene Text for the final scene in the movie. At this point, no characters should be re-described or re-introduced.";
                systemPrompt += "\r\n\r\nYour scene should also fit logically into the timeline of the story and maintain chronological consistency with the events described in previous scenes. As it is the last scene, it should leave the audience with a sense of completion, even if elements of the ending happen to be ambiguous. ";
            }

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            systemPrompt += "\r\n\r\nBelow is the Movie Text, a synopsis of the entire movie:\r\n\r\n";
            systemPrompt += myMovie.movieText + "</movietext>";

            if (sceneNum > 1)
            {
                systemPrompt += "\r\n\r\nHere are the scenes that come prior to the scene you will be writing:";

                int sceneKount = 0;
                string sceneKountStr;
                foreach (SceneObj myScene in sceneList)
                {
                    sceneKount += 1;
                    if (sceneKount < (sceneNum - 1))
                    {
                        sceneKountStr = sceneKount.ToString();
                        systemPrompt += $"\r\n\r\nScene {sceneKountStr} Title: " + myScene.Title + $"\r\nScene {sceneKountStr} Hint: " + myScene.Hint;
                    }
                    else
                    {
                        break;
                    }


                }

                if (sceneList[sceneNum - 2].BeatSheetText != null && sceneList[sceneNum - 2].BeatSheetText.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Beat Sheet (a list of the narrative beats) for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + "</scenehint>";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Beat Sheet:\r\n" + sceneList[sceneNum - 1].BeatSheetText + "</beatsheet>";
                }

                else if (sceneList[sceneNum - 2].NarrativeText != null && sceneList[sceneNum - 2].NarrativeText.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Scene Text for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Text: " + sceneList[sceneNum - 2].NarrativeText + $"</scenetext>";
                }

                else if (sceneList[sceneNum - 2].Hint != null && sceneList[sceneNum - 2].Hint.Trim().Length > 0)
                {
                    systemPrompt += "\r\n\r\nNow I will give you the Scene Hint for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                    systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
                }

                systemPrompt += "\r\n\r\nYou will refer to the above prior scenes to ensure chronological consistency and to avoid redundancy in character descriptions or events. ";

            }


            userPrompt = Utils.getAngleBracketPrompt();
            userPrompt += "Do not provide a title for the scene narrative. ";

            if (sceneNum > 1)
            {
                userPrompt += "\r\n\r\nStart your scene with a brief introductory phrase that situates it with respect to the preceding scene. ";
            }
            userPrompt += "\r\n\r\nPlease write a detailed narrative scene description from this scene hint:\r\n\r\n" + sceneList[sceneNum - 1].Hint + "</scenehint>";
            //userPrompt += "\r\n\r\nStylistically, the writing of the scene description should be simple and direct. Avoid florid and purple prose, and include only details that are relevant to the plot. Write in the present tense and remember that this is a description of a film scene. ";
            userPrompt += "\r\n\r\nWrite in the present tense and remember that this is a description of a film scene. ";
            userPrompt += "\r\n\r\n- Do not enclose your response in any metadata tags.";
            userPrompt += "\r\n- Do not finish with \"END SCENE\" or similar. Simply finish with the final action of the scene.";
            userPrompt += "\r\n- Do not describe the scene fading out or its effect on the audience.";
            userPrompt += "\r\n\r\nPlease write the scene description immediately, with no preamble or introductory sentence.";


            string response = await UtilsGPT.doGPT("MakeSceneText", model, 1000, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Scene Text");

            return response;
        }

        public static async Task<string> makeSplitSceneText(string model, MovieObj myMovie, List<SceneObj> sceneList, int sceneNum, FormApp1 myForm)
        {
            string userPrompt = "";
            string errorMsg = "";
            string systemPrompt = $"You are a talented screenwriter writing a movie script. {Utils.getProfilePrompt(myForm, "SceneText")}";
            
            
            systemPrompt += "Your task will be to take a Scene Hint that is provided in the user prompt below and to write a detailed narrative description of the movie scene (aka \"Scene Text\").";

            if (sceneNum == 1)
            {
                systemPrompt += "\r\n\r\nToday you will be writing the Scene Text for the first scene in the movie. Any characters should be introduced with their age, physical characteristics, and attitude.";
            }

            else if ((sceneNum > 1) && (sceneNum != sceneList.Count))
            {
                systemPrompt += "\r\n\r\nPlease ensure that your Scene Text does not re-describe characters or events that have already been introduced or have occurred in the preceding scenes. If a character IS being introduced for the first time, describe their age, appearance, and attitude. ";
                systemPrompt += "\r\n\r\nYour scene should also fit logically into the timeline of the story and maintain chronological consistency with the events described in previous scenes. ";
            }

            else if ((sceneNum == sceneList.Count))
            {
                systemPrompt += "\r\n\r\nToday you will be writing the Scene Text for the final scene in the movie. At this point, no characters should be re-described or re-introduced.";
                systemPrompt += "\r\n\r\nYour scene should also fit logically into the timeline of the story and maintain chronological consistency with the events described in previous scenes. As it is the last scene, it should leave the audience with a sense of completion, even if elements of the ending happen to be ambiguous. ";
            }

            // systemPrompt += "\r\n\r\nBelow is the Movie Text, a synopsis of the entire movie:\r\n\r\n";
            // systemPrompt += myMovie.movieText + "</movietext>";

            
            
            systemPrompt += "\r\n\r\nHere is an ordered list of summary versions of all the Scenes in the movie. ";

            int sceneKount = 0;
            string sceneKountStr;
            foreach (SceneObj myScene in sceneList)
            {
                sceneKount += 1;
                    
                    
                sceneKountStr = sceneKount.ToString();
                systemPrompt += $"\r\n\r\nScene {sceneKountStr} Title: " + myScene.Title + $"\r\nScene {sceneKountStr} Hint: " + myScene.Hint;
                    
                    
            }

            systemPrompt += "\r\n\r\nEnd of Scene Summaries.\r\n";

            /*
            if (sceneList[sceneNum - 2].BeatSheetText != null && sceneList[sceneNum - 2].BeatSheetText.Trim().Length > 0)
            {
                systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Beat Sheet (a list of the narrative beats) for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + "</scenehint>";
                systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Beat Sheet:\r\n" + sceneList[sceneNum - 1].BeatSheetText + "</beatsheet>";
            }

            else if (sceneList[sceneNum - 2].NarrativeText != null && sceneList[sceneNum - 2].NarrativeText.Trim().Length > 0)
            {
                systemPrompt += "\r\n\r\nNow I will give you the Scene Hint and the Scene Text for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
                systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Text: " + sceneList[sceneNum - 2].NarrativeText + $"</scenetext>";
            }

            else if (sceneList[sceneNum - 2].Hint != null && sceneList[sceneNum - 2].Hint.Trim().Length > 0)
            {
                systemPrompt += "\r\n\r\nNow I will give you the Scene Hint for the scene immediately preceding the one for which you're about to write the Scene Text: ";
                systemPrompt += $"\r\n\r\nScene {sceneNum - 1} Title: " + sceneList[sceneNum - 2].Title + $"\r\nScene {sceneNum - 1} Hint: " + sceneList[sceneNum - 2].Hint + $"</scenehint>";
            }
            */

            systemPrompt += $"\r\n\r\nYou will be writing the Scene Text for Scene #{sceneNum} Title: {sceneList[sceneNum - 1].Title}\r\nThe Scene Hint will will be provided in the user prompt. ";

            if (sceneNum > 1 && sceneList[sceneNum -2].NarrativeText.Trim().Length > 50)
            {
                systemPrompt += "\r\n\r\nHere is the Scene Text for the scene immediately preceding the one for which you're about to write.  Be sure not repeat any events or actions from the previous scene. The scenes must be clearly seperate. ";
                systemPrompt += $"\r\nScene {sceneNum - 1} Text: " + sceneList[sceneNum - 2].NarrativeText;
                systemPrompt += "\r\n\r\nEnd of Scene Text for the previous scene. ";
            }
            else
            {
                if (sceneNum > 1) 
                {
                    systemPrompt += "\r\n\r\nHere is the Scene Hint for the scene immediately preceding the one for which you're about to write.  Be sure not repeat any events or actions from the previous scene. The scenes must be clearly seperate. ";
                    systemPrompt += $"\r\nScene {sceneNum - 1} Text: " + sceneList[sceneNum - 2].Hint;
                    systemPrompt += "\r\n\r\nEnd of Scene Hint for the previous scene. ";
                }
                
                
            }
            
            if (sceneNum < sceneList.Count)
            {
                systemPrompt += "\r\n\r\nHere is the Scene Hint for the scene immediately following the one for which you're about to write.  Be sure not repeat any events or actions from the following scene. The scenes must be clearly seperate. ";
                systemPrompt += $"\r\nScene {sceneNum + 1} Text: " + sceneList[sceneNum].Hint;
                systemPrompt += "\r\n\r\nEnd of Scene Hint for the following scene. ";
            }
           
            
            systemPrompt += "\r\n\r\nYou will refer to the above Scene summaries to ensure chronological consistency and to avoid redundancy in character descriptions or events. ";

            

            userPrompt = Utils.getAngleBracketPrompt();
            userPrompt += "Do not provide a title for the scene narrative. ";

            if (sceneNum > 1)
            {
                userPrompt += "\r\n\r\nStart your Scene Text with a brief introductory phrase that situates it with respect to the preceding scene. ";
            }
            
            userPrompt += "\r\n\r\nStylistically, the writing of the scene description should be simple and direct. Avoid florid and purple prose, and include only details that are relevant to the plot. Write in the present tense and remember that this is a description of a film scene. ";
            userPrompt += "\r\n\r\n- Do not enclose your response in any metadata tags.";
            userPrompt += "\r\n- Do not finish with \"END SCENE\" or similar. Simply finish with the final action of the scene.";
            userPrompt += "\r\n- Do not describe the scene fading out or its effect on the audience.";
            
            userPrompt += "\r\n\r\nPlease write the Scene Text immediately, with no preamble or introductory sentence.";
            userPrompt += "\r\n\r\nPlease write a detailed narrative Scene Text from this scene hint:\r\n\r\n" + sceneList[sceneNum - 1].Hint + "</scenehint>";

            string response = await UtilsGPT.doGPT("MakeSplitSceneText", model, 1000, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Scene Text");

            return response;
        }
        public static async Task<string> gptCompress(string input, string model, int maxTokens, FormApp1 myForm)
        {

            string errorMsg = "";
            string systemPrompt = @"Compress the user prompt text such that you (GPT) can reconstruct the intention of the human who wrote the text with the full original intention. This is for yourself. It does not need to be human-readable or human-understandable. Abuse of language mixing, abbreviations, symbols (unicode and emoji), or any other encodings or internal representations are all permissible, so long as, used in a future prompt, the string will yield results near-identical to the original text: ";

            string userPrompt = input;

            string response = await UtilsGPT.doGPT("CompressPrompt", model, maxTokens, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Scene Text");


            return response;

        }

        public static async Task<string> getTitle(string input, string model, FormApp1 myForm)

        {
            string errorMsg = "";
            string systemPrompt = @"We are working on a movie script. The user prompt will be a description of the movie. Please return a title. ";
            string userPrompt = input;
            string response = await UtilsGPT.doGPT("MakeTitle", model, 50, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Title");
            response = response.Replace("\"", "");
            return response;
        }

        public static async Task<String> fixJSON(string input, string model, string errorMsg, FormApp1 myForm)
        {


            string systemPrompt = "You will repair the errors in the JSON List of Lists provided in the user prompt. ";
            systemPrompt += "Your reponse will be in the form of a JSON List of Lists, each inner list consisting of two strings. ";

            systemPrompt += "\r\n\r\nAn example of correct JSON: [[\"The first title\",\"This is the first description of the action in the scene\"]";
            systemPrompt += ",[\"The second title\",\"This is the description of the action in the second scene\"]]\r\n\r\n";
            systemPrompt += "If there are no errors, return the input string.";


            string userPrompt = input;
            string response = await UtilsGPT.doGPT("FixJSON", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, errorMsg);

            return response;
        }

        public static async Task<string> makeBeatSheet(MovieObj myMovie, string sceneText, string model, StyleElements style, FormApp1 myForm)
        {

            Boolean noBeatSheetFlag = myForm.getNoBeatSheetFlag();

            if (noBeatSheetFlag)
            {
                return "";
            }

            string userPrompt = "";
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneScript")}";
            

            systemPrompt += "Your task will be to create a scene beat sheet from the scene description in the user prompt. ";
            systemPrompt += "\r\n\r\nA beat sheet breaks a scene down into a series of beats or moments that are essential for moving the plot forward. These beats can include character introductions, important actions or decisions, turning points, conflicts, and resolutions. The beat sheet is organized in chronological order. Do not include any Acts or Act numbers in the beat sheet.";
            systemPrompt += "\r\n\r\nFor context, here is a synopsis that describes the movie as a whole:\r\n\r\n";
            systemPrompt += myMovie.movieText + "\r\n\r\nEND SYNOPSIS\r\n\r\n";

            systemPrompt += "Your task will be to create a scene beat sheet from the scene description in the user prompt. Do not include a title for the beat sheet or any introductory preambles.";


            userPrompt = "Please write a beat sheet for the following scene description:\r\n\r\n";

            userPrompt += sceneText + "\r\n\r\nEND SCENE TEXT";

            userPrompt += "\r\n\r\nWrite the beat sheet without any introductory preambles.";

            if (style.style != "-none-" && style.type == "author")
            {
                userPrompt += $"\r\n\r\nPlease write the beat sheet in the style of {style}.";
            }



            string response = await UtilsGPT.doGPT("MakeBeatSheet", model, 1000, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Beat Sheet");
            // string response = await chat.GetResponseFromChatbotAsync();

            return response;
        }

        public static async Task<string> makeSceneScript(MovieObj myMovie, List<CharacterObj> myCharacters , string beatSheet, string sceneText, string model, Boolean ScreenplayFormatFlag, StyleElements styleGuide, StyleElements style, string dialogFlavor, string customFlavor, FormApp1 myForm)
        {
            string errorMsg = "SceneScript";
            string userPrompt = "";
            string systemPrompt = $"You are a talented screenwriter writing a movie script. In this case we are writing the script for a single scene in a larger movie. ";
            systemPrompt += Utils.getProfilePrompt(myForm, "SceneScript");
            int index;
            Boolean noBeatSheetFlag = myForm.getNoBeatSheetFlag();

            List<string> sceneChars = Utils.ExtractTextBetweenAngleBrackets(sceneText);


            if (styleGuide.label != "-none-")
            {
                systemPrompt += "Here is the style guide we are using for writing scripts:\r\n\r\n" + styleGuide.style + "\r\n\r\nEND STYLE GUIDE";

            }

            systemPrompt += "\r\n\r\nPlease use Fountain markup to format your output.\r\n\r\n";
            systemPrompt += @"- For all dialogue the name of the speaker must be preceded by a blank line and be on a line by itself with the whole line being in all capital letters.

- An optional description of blocking or speech tone (mode of speaking) can be inserted on a line, after the name of the speaker and before the dialogue.

- No blank line should be inserted between the speaker's name and the optional decription. Do not include any description of the speaker themselves on this line.

- This optional description line should be in lowercase letters and in parentheses. The dialogue text must be on a line by itself. No blank line just prior to it.

- There must be a blank line after the line of dialogue. There must be no blank lines between the name of the speaker, the optional description, and the dialogue text.";



            /* if (ScreenplayFormatFlag)
            {
                systemPrompt += "Dialogue should be centered on the page.  The name of the speaker of each piece of dialogue should be on a line by itself, centered on the page, and in all upper case. ";
                systemPrompt += "An optional description of blocking or speech tone (mode of speaking) can be inserted on a line, centered on the page, after the name of the speaker and before the dialogue. Do not include any description of the speaker themselves on this line. This line should be in lowercase letters and in parentheses. ";
                systemPrompt += "Here is an example of how to format dialogue:\r\n\r\n";

                systemPrompt += @"
                           ALEX
                   (muttering to himself)
              it's just not the same without her.

                           JOHN
                     (speaking softly)
                We'll manage, Alex. We have to. 

                            MARY
                    She'll come back, I hope." + "\r\n\r\n";

                systemPrompt += "For purposes of centering each of the dialogue, the name of the speaker, and the optional line for mode of speaking, assume a fixed-width font like New Courier";
                systemPrompt += " with a line width of 70 characters. The center of all of the centered lines should be at position 35 in the line. The centered lines include: the speaker of the dialogue in upper case, ";
                systemPrompt += "a lowercase optional line in parenthesis indicating the mode of speaking, and the dialogue itself. ";
                systemPrompt += "The speaker's name in upper case should be centered at the 35th position. ";
                systemPrompt += "The optional mode of speaking line in parentheses after the speaker's name must also be centered at position 35. ";
                systemPrompt += "The dialogue itself should be divided with new lines so that no line on the page is longer than 40 characters. Each of the dialogue lines separated by new lines should be centered at character position 35. ";
                
                systemPrompt += "Use spaces, not tabs, to center the text. The dialogue should not be have quotation marks around it. ";
                systemPrompt += "All other lines of text must be left justified.\r\n\r\n";
            }*/

            systemPrompt += "\r\n\r\nFor necessary context, here is a narrative synopsis of the movie as a whole:\r\n\r\n";
            systemPrompt += myMovie.movieText + "\r\n\r\nEND SYNOPSIS\r\n\r\n";

            if (noBeatSheetFlag)
            {
                systemPrompt += "Your task will be to write the script for this one scene using the scene narrative description in the user prompt.";
                systemPrompt += "\r\n\r\nIt is crucial that every detail of the scene narrative description be included in the script.";
            }
            else
            {
                systemPrompt += "Your task will be to write the script for this one scene using the scene beat sheet and the scene narrative description in the user prompt. ";
                systemPrompt += "\r\n\r\nIt is crucial that every detail of the scene narrative description be included in the script. ";
                systemPrompt += "\r\n\r\nIt is also crucial that every beat in the beat sheet be included in the script. ";
            }

            systemPrompt += "\r\n\r\nFURTHER RULES:";
            systemPrompt += "\r\n\r\n- Note where this scene appears in the chronological whole. Unless this is the first scene in which a character is introduced, do not include introductory details about the character (e.g. age or appearance). ";
            systemPrompt += "\r\n\r\n- Always begin with a slug line and a description of the scene setting.";
            systemPrompt += "\r\n\r\n- Be sure to include plentiful dialogue, when appropriate. Dialogue, setting, and action descriptions are all important elements in a movie script. ";
            systemPrompt += "\r\n\r\n- Dialogue should never be summarized. It should always be written out fully in dialogue format. ";
            systemPrompt += "\r\n\r\n- Do not number any of the parts of the script. ";
            systemPrompt += "\r\n\r\n- Do not include any camera or editing instructions such as \"CLOSE-UP\", \"FADE IN\", \"FADE OUT\",\"FADE TO BLACK\",\"CUT TO\", \"SHIFT TO\", or any other camera or editing instructions.";
            systemPrompt += "\r\n\r\n- Do not include \"to be continued\" or anything similar at the end of the script.";


            if (noBeatSheetFlag)
            {
                userPrompt = "Please write a scene in movie industry standard screenplay format using the scene narrative description below:";
                userPrompt += "\r\n\r\nScene Narrative Description:\r\n\r\n" + sceneText + "\r\n\r\nEND SCENE NARRATIVE DESCRIPTION\r\n\r\n";

                userPrompt += "Be absolutely sure to use each and every detail from the scene narrative description in the creation of the Scene Script.";
            }
            else
            {
                userPrompt = "Please write a scene in movie industry standard screenplay format using the scene narrative description and the beat sheet below:";
                userPrompt += "\r\n\r\nScene Narrative Description:\r\n\r\n" + sceneText + "\r\n\r\nEND SCENE NARRATIVE DESCRIPTION\r\n\r\n";

                userPrompt += "Beat Sheet:\r\n\r\n" + beatSheet + "\r\n\r\nEND BEAT SHEET\r\n\r\n";
                userPrompt += "- Be absolutely sure to use each and every detail from both the scene narrative description and the beat sheet in the creation of the Scene Script. Double-check this if necessary.";
                userPrompt += "\r\n\r\nDo not include any beat labels in the script for example do not include: \"Beat One\" \"BEAT 1\",  Include no beat labels in any form. ";
            }

            userPrompt += "\r\n\r\n- Include plenty of realistic dialogue, whenever appropriate to the scene. Include information about the scene location. Fully describe the scene action.";
            userPrompt += "\r\n\r\n- The scene script should have a rich balance of dialogue, location description, and action descriptions.";

            if (style.label != "-none-" && style.type == "author")
            {
                userPrompt += $"\r\n\r\nPlease write the Scene Script in the style of {style.style}.";

            }

            

            if (dialogFlavor != "FlavorNone" && dialogFlavor != "FlavorCustom")
            {
                userPrompt += "\r\n\r\nPlease write all of the dialogue in the Scene Script in ";
                if (dialogFlavor == "FlavorCoolLaconic")
                {
                    userPrompt += @"a cool and laconic style which is characterized by its brevity, detachment, and understated wit. The dialogue should be stripped down to essentials, with characters expressing themselves in short, direct sentences, avoiding verbosity. There's an air of effortless confidence, with words chosen for maximum impact in minimal syllables.";

                }
                else if (dialogFlavor == "FlavorRomantic")
                {
                    userPrompt += @"a style suitable for scenes where characters are engaged in or considering a romantic relationship. That style is characterized by its tenderness, nuance, and subtext. The dialogue often includes subtle hints of attraction, longing, or uncertainty. There's an emphasis on what's left unsaid, with pauses, lingering looks, or loaded phrases suggesting deeper feelings beneath the surface.";
                }
                else if (dialogFlavor == "FlavorEmotional")
                {
                    userPrompt += @"a style where characters are highly emotional, which is characterized by raw intensity, vulnerability, and often, unpredictability. The dialogue delves deep into the characters' emotional states, whether it be overwhelming grief, fiery anger, intense joy, profound love, or other strong emotional states. Exchanges are marked by an outpouring of feelings, often with fragmented sentences, exclamations, and pauses that reflect the tumult of emotions.";


                }
                else if (dialogFlavor == "FlavorComedic")
                {

                    userPrompt += @"a comedic style that is characterized by its wit, playful exaggeration, and often, unexpected twists. Dialogue in the scene often uses humor devices like puns, wordplay, comedic timing, and situational irony. Characters may misunderstand one another, use literal interpretations in absurd ways, or deliver lines with a deadpan demeanor, all in the service of eliciting laughter.";

                }

                else if (dialogFlavor == "FlavorSeriousDiscussion")
                {
                    userPrompt += @"a style suitable for a serious discussion which is characterized by its directness and depth. The dialogue treats its subject as consequential, and characters express their viewpoints with a sense of gravity. Interruptions are minimal, and there's an emphasis on listening, ensuring each participant's perspective is understood.";

                }

                else if (dialogFlavor == "FlavorBantering")
                {
                    userPrompt += @"a bantering style which is characterized by its playful, teasing, and often light-hearted exchanges between characters. It's filled with quick retorts, clever comebacks, and humorous observations. The dialogue often flows rapidly, with characters playfully one-upping each other or engaging in friendly verbal sparring.";
                }

                else if (dialogFlavor == "FlavorActionOriented")
                {
                    userPrompt += @"a style suitable for action-oriented scenes which is characterized by its brevity, urgency, and immediacy. Dialogue in such scenes often gets straight to the point, with short and sharp exchanges that maintain the scene's pacing and intensity. Extraneous details are trimmed away, ensuring the dialogue drives the action forward or accentuates the stakes involved.";
                }

                else if (dialogFlavor == "FlavorConfrontational")
                {
                    userPrompt += @"a style suitable for confrontational scenes, characterized by tension, directness, and emotional weight. Dialogue in such scenes often reveals underlying conflicts, grudges, or differences in opinion. Exchanges are charged with emotion, may include accusations, defensiveness, aggression, or expressions of hurt, and there's a clear back-and-forth dynamic between the characters.";
                }
                else
                {
                    userPrompt += "a style suitable for the contents of the scene.";
                }
            }

            if (dialogFlavor == "FlavorCustom")
            {
                userPrompt += "\r\n\r\nPlease write all of the dialogue in the Scene Script in this style: ";
                userPrompt += customFlavor;

            }

            if (dialogFlavor != "FlavorNone")
            {
                userPrompt += " Only use this style for the dialogue. The rest of the Scene Script should be written in a neutral style.";
            }


            if (sceneChars.Count > 0 && (myForm.boolSpeakScript() || myForm.boolpersonalityScript() ))
            {
                Boolean introFlag = true;
                foreach (string myChar in sceneChars)
                {
                   CharacterObj workCharacter =  myCharacters.Find(p => p.tagName == myChar);
                    if (workCharacter != null)
                    {
                        if (workCharacter.speechStyle.Length > 0)
                        {
                            if (introFlag)
                            {
                                if (myForm.boolpersonalityScript() && myForm.boolSpeakScript())
                                {
                                    userPrompt += "\r\n\r\nApply the following character speaker styles and personality attributes when writing dialogue:\r\n\r\n";
                                }
                                if (myForm.boolpersonalityScript() && !myForm.boolSpeakScript())
                                {
                                    userPrompt += "\r\n\r\nApply the following personality attributes when writing dialogue:\r\n\r\n";
                                }
                                if (!myForm.boolpersonalityScript() && myForm.boolSpeakScript())
                                {
                                    userPrompt += "\r\n\r\nApply the following character speaker styles when writing dialogue:\r\n\r\n";
                                }

                                introFlag = false;
                            }

                            if (myForm.boolSpeakScript()) userPrompt += $"Speaking style for {workCharacter.tagName}: {workCharacter.speechStyle}\r\n\r\n";
                            if (myForm.boolpersonalityScript()) userPrompt += $"Personality attributes for {workCharacter.tagName}: {workCharacter.personality}\r\n\r\n";

                        }
                        
                        
                    }
                }

            }

            string response = await UtilsGPT.doGPT("MakeSceneScript", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "making Scene Script");

            response = response.Replace("```", "");

            return response;
        }


        public static async Task<string> NotesForMovieText(string model, string movieText, string movieHint, string textNote, FormApp1 myForm)
        {
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}";
            
            systemPrompt += "In the user prompt you will be provided with a detailed narrative description of the movie, henceforth called the original \"Movie Narrative\". ";

            systemPrompt += "You will also be provided in the user prompt with \"Notes\", which are instructions for rewriting the Movie Narrative. ";
            systemPrompt += "You will rewrite the Movie Narrative, taking into consideration the Notes. Unless instructed otherwise by the \"Notes\", ";
            systemPrompt += "you will retain all of the details from the original Movie Narrative. It is common for the rewritten version of the Movie Narrative to be longer than the original version unless specified otherwise in the Notes. ";

            systemPrompt += "The rewritten Movie Narrative will be used later as a prompt for writing the movie screenplay for a feature-length movie. ";

            systemPrompt += Utils.getAngleBracketPrompt();
            systemPrompt += "Unless directed otherwise by the Notes, you will also include all details from the Movie Seed. The Movie Seed:\r\n\r\n";
            systemPrompt += movieHint;

            string userPrompt = "You will rewrite the Movie Narrative, taking into consideration the Notes. To the degree possible, and unless instructed otherwise by the \"Notes\", ";
            userPrompt += " you will retain all the details from the original Movie Narrative. Do not try and restrict the length of your reponse. It is common for the new version of the Movie Narrative to be longer than the original version of the Movie Narrative. ";
            userPrompt += "Please DO NOT include any prefacing sentence, e.g. \"Here is the rewritten narrative:\". Begin with the first sentence of the updated description. ";
            userPrompt += "\r\n\r\nHere is the original Movie Narrative:\r\n\r\n";
            userPrompt += movieText;
            userPrompt += "\r\n\r\nHere are the Notes to use for rewriting the Movie Narrative:\r\n\r\n";
            userPrompt += textNote;
            userPrompt += " Please rewrite the Movie Narrative, considering the Notes.";
            userPrompt += Utils.getAngleBracketPrompt();
            string response = await UtilsGPT.doGPT("ApplyNotesOld", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Movie Text");

            return response;
        }

        public static async Task<string> AddCharactersMovieText(string model, string movieText, string movieHint, string textNote, List<CharacterProfiles> myCharacters, FormApp1 myForm)
        {
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}";
            
            systemPrompt += "You will be rewriting a detailed narrative description of the movie, henceforth called the \"Movie Narrative\". ";
            systemPrompt += "This is the Movie Narrative: \r\n\r\n";
            systemPrompt += movieText;

            systemPrompt += "You will be provided in the user prompt with descriptions of attributes of some or all of the characters in the Movie Narrative. ";
            systemPrompt += "You will rewrite the Movie Narrative, taking into consideration the attributes of the characters. ";
            systemPrompt += "you will retain all of the details from the original Movie Narrative. It is common for the rewritten version of the Movie Narrative to be longer than the original version. ";

            systemPrompt += "The rewritten Movie Narrative will be used later as a prompt for writing the movie screenplay for a feature-length movie. ";

            systemPrompt += "In your response, be sure to enclose all occurrences of character names in angle brackets <>. Example: <Mary>. ";

            string userPrompt = "You will rewrite the Movie Narrative, taking into consideration the following list of character attributes: \r\n\r\n";

            foreach (CharacterProfiles myProfile in myCharacters)
            {
                userPrompt += $"{myProfile.Name} back story: {myProfile.BackStory}.\r\n\r\n";
                userPrompt += $"{myProfile.Name} physical description: {myProfile.Physical}.\r\n\r\n";
                userPrompt += $"{myProfile.Name} speaking style: {myProfile.Speech}.\r\n\r\n";
            }

            userPrompt += " you will retain all the details from the original Movie Narrative. Do not try and restrict the length of your reponse. It is common for the new version of the Movie Narrative to be longer than the original version of the Movie Narrative. ";
            userPrompt += "Please DO NOT include any prefacing sentence, e.g. \"Here is the rewritten narrative:\". Begin with the first sentence of the updated Movie Narrative. ";
            userPrompt += "\r\n\r\nHere is the original Movie Narrative:\r\n\r\n";
            userPrompt += movieText;
            userPrompt += "\r\n\r\nHere are the Notes to use for rewriting the Movie Narrative:\r\n\r\n";
            userPrompt += textNote;
            userPrompt += " Please rewrite the Movie Narrative, considering the Notes.";
            string response = await UtilsGPT.doGPT("AddCharactersMovieText", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Movie Text");

            return response;
        }

        public static async Task<string> NotesForSceneText(string model, string sceneText, string sceneNote, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneText")}";
            

            systemPrompt += "In the user prompt you will be provided with a narrative description of a scene from a movie. ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the narrative description of the scene. ";
            systemPrompt += "You will rewrite the detailed narrative description of the scene, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the \'Note\', ";
            systemPrompt += "you will retain all the details from the original version. ";
            systemPrompt += Utils.getAngleBracketPrompt();
            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            string userPrompt = "You will rewrite the detailed narrative description of the scene, taking into consideration the notes. To the degree possible, unless instructed otherwise by the \'Note\', ";
            userPrompt += "you will retain all the details from the original version. ";
            userPrompt += "\r\n\r\nHere is the detailed narrative description of the scene:\r\n\r\n";
            userPrompt += sceneText;
            userPrompt += "\r\n\r\nHere are the notes to use for rewriting the detailed narrative description of the scene:\r\n\r\n";
            userPrompt += sceneNote;
            userPrompt += "Please rewrite the detailed narrative description of the scene, considering the Notes";
            string response = await UtilsGPT.doGPT("ApplyNotesSceneText", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Scene Text");

            return response;
        }
        public static async Task<string> NotesForSceneScript(string model, string scriptText, string sceneText, string scriptNote, Boolean ScreenplayFormatFlag, StyleElements styleGuide, StyleElements style, FormApp1 myForm)
        {
            string errorMsg = "";
            string systemPrompt = $"You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneScript")}";
            

            if (styleGuide.label != "-none-")
            {
                systemPrompt += "This is the style guide we are using for writing scripts:\r\n\r\n" + styleGuide.style + "\r\n\r\n";

            }
            if (ScreenplayFormatFlag)
            {
                systemPrompt += "Dialogue should be centered on the page. The name of the speaker of each piece of dialogue should be on a line by iteself, centered on the page, and in all upper case. ";
                systemPrompt += "An optional description of blocking or speech tone (mode of speaking) can be inserted on a line, centered on the page, after the name of the speaker and before the dialogue. Do not include any description of the speaker on this line. This line should be in lowercase letters and in parentheses. ";
                systemPrompt += "Here is an example of how to format dialogue:\r\n\r\n";

                systemPrompt += @"
                           ALEX
                   (muttering to himself)
              it's just not the same without her.

                           JOHN
                We'll manage, Alex. We have to. " + "\r\n\r\n";

                systemPrompt += "For purposes of centering each of the dialogue, the name of the speaker, and the optional line for mode of speaking, assume a fixed-width font like New Courier ";
                systemPrompt += "with a line width of 60 characters. The center of all of the centered lines should be at position 30 in the line. The centered lines include: the speaker of the dialogue in upper case, ";
                systemPrompt += "the lowercase optional line in parentheses for mode of speaking, and the dialogue itself. ";
                systemPrompt += "The dialogue itself should be divided with new lines so that no line on the page is longer than 40 characters. Each of the dialogue lines seperated by new lines should have its center at position 30. The optional mode of speaking line after the speaker's name should also have its center at position 30. ";
                systemPrompt += "The speaker's name in upper case should also be centered at the 30th position. ";
                systemPrompt += "Use spaces, not tabs, to center the text. The dialogue should not have quotation marks around it.\r\n\r\n";
            }
            systemPrompt += "In the user prompt you will be provided with a script for one scene from the movie, which we will call the \"Scene Script\". ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the Scene Script. ";
            systemPrompt += "You will rewrite the Scene Script, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the \'Note\', ";
            systemPrompt += "you will retain all the details from the original version. ";
            systemPrompt += "\r\n\r\nThe scene script was created from a prose scene narrative. The rewritten version of the scene script must include each and every detail from both the scene narrative and the original scene script. ";
            systemPrompt += "This is the scene narrative on which the scene script was based:\r\n\r\n";
            systemPrompt += sceneText;


            string userPrompt = "You will rewrite the scene script, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the \'Note\', ";
            userPrompt += "you will retain all the details from the original version of the scene script and from the scene narrative. ";
            userPrompt += "Here is the scene script:\r\n\r\n";
            userPrompt += scriptText;
            userPrompt += "\r\nHere are the notes to use for rewriting the scene script:\r\n\r\n";
            userPrompt += scriptNote;
            userPrompt += "\r\n\r\nPlease rewrite the scene script, considering the Notes.";
            if (style.label != "-none-" && style.type == "author")
            {
                userPrompt += $"\r\n\r\nPlease rewrite the scene script in the style of {style.style}.";

            }

            string response = await UtilsGPT.doGPT("ApplyNotesSceneScript", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Scene Script");

            return response;
        }

        public static async Task<string> notesForSELECTEDSceneText(string model, string sceneText, string selectedText, int selectedStart, string sceneNote, StyleElements movieAndSceneTextStyle, FormApp1 myForm)

        {
            string errorMsg = "";
            string systemPrompt = $" You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneText")}";
            

            systemPrompt += "We are working on the Scene Text, which is ";
            systemPrompt += "a narrative text describing one scene from the movie.\r\n\r\n";


            systemPrompt += "Your task is to rewrite a selected part of the Scene Text. ";

            systemPrompt += "You will be provided in the user prompt with a selected part of the Scene Text, which we will call the \"Selected Text\". ";
            systemPrompt += "This is the Scene Text which includes the Selected Text:\r\n\r\n";
            systemPrompt += sceneText;

            systemPrompt += "In the user prompt you will be provided with the Selected Text from the Scene Text. ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the Selected Text. ";
            systemPrompt += "You will rewrite the Selected Text, taking into consideration the Notes.  To the degree possible, unless instructed otherwise by the \'Note\', ";
            systemPrompt += "you will retain all the details from the original version. You will use a writting style similar to that of the Scene Textt";

            systemPrompt += Utils.getAngleBracketPrompt();

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            string userPrompt = "You will rewrite the Selected Text, taking into consideration the Notes. ";
            userPrompt += "You will retain all the details from the original version of the Selected Text. ";
            userPrompt += "Here is the Selected Text:\r\n\r\n";
            userPrompt += selectedText;
            userPrompt += "\r\n\r\nHere are the notes to use for rewriting the Selected Text:\r\n\r\n";
            userPrompt += sceneNote;
            userPrompt += "\r\n\r\nPlease rewrite only the Selected Text, considering the Notes.";


            string response = await UtilsGPT.doGPT("ApplyNotesSelectedSceneText", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Selected Text in the Scene Text");


            return response;

        }

        public static async Task<string> notesForSELECTEDMovieText(string model, string movieText, string selectedText, int selectedStart, string movieNote, FormApp1 myForm)

        {
            string errorMsg = "";
            string systemPrompt = $" You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm,"MovieText")}We are working on the Movie Text, which is ";
            systemPrompt += "a detailed narrative synopsis of the movie.\r\n\r\n";


            systemPrompt += "Your task is to rewrite a selected part of the Movie Text. ";

            systemPrompt += "You will be provided in the user prompt with a selected part of the Movie Text, which we will call the \"Selected Text\". ";
            systemPrompt += "\r\n\r\nHere is the Movie Text which includes the Selected Text:\r\n\r\n";
            systemPrompt += movieText + "\r\nEND MOVIE TEXT\r\n";

            systemPrompt += "In the user prompt you will be provided with the Selected Text from the Movie Text. ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the Selected Text. ";
            systemPrompt += "You will rewrite the Selected Text, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the Notes, ";
            systemPrompt += "you will retain all the details from the original version. You will use a writting style similar to that of the Movie Text.";
            systemPrompt += Utils.getAngleBracketPrompt();



            string userPrompt = "You will rewrite the Selected Text, taking into consideration the Notes. ";
            userPrompt += "You will retain all the details from the original version of the Selected Text. ";
            userPrompt += "Here is the Selected Text:\r\n\r\n";
            userPrompt += selectedText + "\r\nEND SELECTED TEXT";
            userPrompt += "\r\n\r\nHere are the notes to use for rewriting the Selected Text:\r\n\r\n";
            userPrompt += movieNote + "\r\nEND NOTES";
            userPrompt += "\r\n\r\nPlease rewrite only the Selected Text, considering the Notes.";


            string response = await UtilsGPT.doGPT("ApplyNotesSelectedMovieText", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Selected Text in Movie Text");


            return response;

        }
        public static async Task<string> NotesForSELECTEDSceneScript(string model, string scriptText, string selectedText, int selectedStart, string sceneText, string scriptNote, Boolean ScreenplayFormatFlag, StyleElements styleGuide, StyleElements style, FormApp1 myForm)
        {

            string errorMsg = "";
            string systemPrompt = $" You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "SceneScript")}We are working on the Scene Script, which is ";
            systemPrompt += "a script for one scene from the movie.\r\n\r\n";
            if (styleGuide.label != "-none-")
            {
                systemPrompt += "This is the style guide we are using for writing scripts:\r\n\r\n" + styleGuide.style + "\r\n\r\n";

            }
            if (ScreenplayFormatFlag)
            {
                systemPrompt += "Dialogue should be centered on the page. The name of the speaker of each piece of dialogue should be on a line by iteself, centered on the page, and in all upper case. ";
                systemPrompt += "An optional description of blocking or speech tone (mode of speaking) can be inserted on a line, centered on the page, after the name of the speaker and before the dialogue. Do not include any description of the speaker on this line. This line should be in lowercase letters and in parentheses. ";
                systemPrompt += "Here is an example of how to format dialogue:\r\n\r\n";

                systemPrompt += @"
                           ALEX
                   (muttering to himself)
              it's just not the same without her.

                           JOHN
                We'll manage, Alex. We have to. " + "\r\n\r\n";

                systemPrompt += "For purposes of centering each of the dialogue, the name of the speaker, and the optional line for mode of speaking, assume a fixed-width font like New Courier ";
                systemPrompt += "with a line width of 60 characters. The center of all of the centered lines should be at position 30 in the line. The centered lines include: the speaker of the dialogue in upper case, ";
                systemPrompt += "the lowercase optional line in parentheses for mode of speaking, and the dialogue itself. ";
                systemPrompt += "The dialogue itself should be divided with new lines so that no line on the page is longer than 40 characters. Each of the dialogue lines seperated by new lines should have its center at position 30. The optional mode of speaking line after the speaker's name should also have its center at position 30. ";
                systemPrompt += "The speaker's name in upper case should also be centered at the 30th position. ";
                systemPrompt += "Use spaces, not tabs, to center the text. The dialogue should not have quotation marks around it.\r\n\r\n";
            }

            systemPrompt += "Your task is to rewrite a selected part of the Scene Script. ";

            systemPrompt += "You will be provided in the user prompt with a selected part of the Scene Script, which we will call the \"Selected Text\". ";
            systemPrompt += "This is the Scene Script which includes the Selected Text:\r\n\r\n";
            systemPrompt += scriptText;

            systemPrompt += "In the user prompt you will be provided with the Selected Text from the Scene Script. ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the Selected Text. ";
            systemPrompt += "You will rewrite the Selected Text, taking into consideration the Notes.  To the degree possible, unless instructed otherwise by the \'Note\', ";
            systemPrompt += "you will retain all the details from the original version. You will use a writting style similar to that of the Scene Script";



            string userPrompt = "You will rewrite the Selected Text, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the \'Note\', ";
            userPrompt += "you will retain all the details from the original version of the Selected Text. You will use a writting style similar to that of the Scene Script.";
            userPrompt += "Here is the Selected Text:\r\n\r\n";
            userPrompt += selectedText;
            userPrompt += "\r\n\r\nHere are the notes to use for rewriting the Selected Text:\r\n\r\n";
            userPrompt += scriptNote;
            userPrompt += "\r\n\r\nPlease rewrite the Selected Text, considering the Notes.";
            if (style.label != "-none-" && style.type == "author")
            {
                userPrompt += $"\r\n\r\nPlease rewrite the Selected Text in the style of {style.style}.";

            }

            string response = await UtilsGPT.doGPT("ApplyNotesSelectedSceneScript", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "applying Notes to Selected Text in Scene Script");

            // string returnText = Utils.spliceIn(scriptText, selectedText, response);




            return response;

        }


        public static async Task<string> refactorSceneAfter(string model, List<SceneObj> sceneList, int mySceneNumber, int totalScenes, string sceneText, string myMovieText, FormApp1 myForm)
        {

            string tempTitle, tempDescription, outLine;

            int scenesToCreate = totalScenes - mySceneNumber;

            string systemPrompt = $"You are a talented assistant working with a screenwriter to develop a movie script. {Utils.getProfilePrompt(myForm, "MakeScenes")}";
                
            systemPrompt += "You will be adding scenes to an existing list of scenes to complete a set of scenes for later writing of a movie script.";
            systemPrompt += "\r\n\r\nEach scene should include a title and a description of the action in that scene. Each scene description must be a full paragraph. Do not include scene numbers. ";
            if (Utils.refactorMovieText)
            {
                systemPrompt += $"\r\n\r\nHere is the narrative description of the movie that we are working on:\r\n\r\n<movie_narrative>{myMovieText}</movie_narrative>\r\n\r\n";
            }

            systemPrompt += "\r\n\r\nIn the user prompt, you will be provided with a list of the scenes created so far. ";
            systemPrompt += "\r\n\r\nThe scenes will be provided to you in the form: [[scene title][scene description][scene title][scene description]]. ";
            systemPrompt += $"\r\n\r\nHere is a detailed description of the last scene created, to give you a sense of how the story is unfolding:\r\n\r\n<last_scene>{sceneText}</last_scene>\r\n\r\n";
            systemPrompt += $"\r\n\r\nYou will be instructed to create {scenesToCreate} additional scenes and to return them in JSON format as a JSON list of lists. In this form:\r\n\r\n";
            systemPrompt += "[[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"]]\r\n\r\n";

            string userPrompt = $"\r\n\r\nHere are the titles and descriptions of the first {mySceneNumber} scenes: [";
            for (int j = 0; j <= mySceneNumber - 1; j++)
            {
                tempTitle = sceneList[j].Title;
                tempDescription = sceneList[j].Hint;

                outLine = $"[{tempTitle}][{tempDescription}]\r\n\r\n";
                userPrompt += outLine;

            }

            userPrompt += "]";

            userPrompt += $"\r\n\r\nPlease create {scenesToCreate} additional scene titles and descriptions to complete the movie. ";
            userPrompt += "\r\n\r\nDo not include scene numbers in the title or description. ";
            userPrompt += "\r\n\r\nStyle Rules:";
            userPrompt += "\r\n\r\n- Keep the writing of the scene descriptions straightfoward and functional. Avoid decorative language. Each sentence should be short, direct, and in the active voice. ";
            userPrompt += "\r\n\r\n- Every scene should have a clearly named setting that makes sense and is cinematically engaging. ";
            userPrompt += "\r\n\r\n- Scenes should never blend together, but should be carefully delineated. ";
            userPrompt += "\r\n\r\n- Every scene should advance the plot. That means, by the end of the scene, something should have changed in the characters or their relationships. ";
            userPrompt += "\r\n\r\n- Every scene should have a beginning and an end.";
            userPrompt += "\r\n\r\nPlease return the additional scenes as a JSON list of lists in this form:\r\n\r\n";
            userPrompt += "[[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"]]";

            string response = await UtilsGPT.doGPT("RefactorSceneAfter", model, 2000, .7, userPrompt, systemPrompt, "", myForm, $"refactoring Scenes after Scene # {mySceneNumber}");

            return response;
        }

        public static async Task<string> refactorSceneAfterHeavy(string model, List<SceneObj> sceneList, int mySceneNumber, int totalScenes, string sceneText, string myMovieText, FormApp1 myForm)
        {

            string tempTitle, tempDescription, outLine;

            int scenesToCreate = totalScenes - mySceneNumber;

            string systemPrompt = $"You are a talented assistant working with a screenwriter to develop a movie script. {Utils.getProfilePrompt(myForm, "MakeScenes}")}";
            systemPrompt += "You will be adding scenes to an existing list of scenes to complete a set of scenes for later writing of a movie script.";
            systemPrompt += "\r\n\r\nEach scene should include a title and a description of the action in that scene. Each scene description (aka Scene Seed) must be a paragraph of no more than 3 sentences. Do not include scene numbers. ";

            if (Utils.refactorMovieText)
            {
                systemPrompt += $"\r\n\r\nHere is the narrative description of the movie that we are working on:\r\n\r\n<movie_narrative>{myMovieText}</movie_narrative>";
            }

            systemPrompt += "\r\n\r\nIn the user prompt, you will be provided with a list of the scenes created so far. ";
            systemPrompt += "\r\n\r\nAll scenes provided will include their scene title. Some of the scenes created so far will include a full Scene Text (full synopsis of the scene). Other scenes will only be provided with Scene Seeds (briefer scene descriptions). ";
            // systemPrompt += "The scenes will be provided to you in the form: [[scene title][scene description][scene title][scene description]]";
            // systemPrompt += $"Here is a detailed description of the last scene created to give you a sense of how the story is unfolding: {sceneText}. ";
            systemPrompt += $"\r\n\r\nYou will be instructed to create {scenesToCreate} additional scenes and to return them in JSON format as a JSON list of lists. ";
            systemPrompt += "\r\n\r\nYour output will be in JSON as a list of lists containing all of the additional scenes. In this form:\r\n\r\n";
            systemPrompt += "[[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"]]";

            string userPrompt = $"Here are the scene titles and Scene Seeds or Scene Texts (whichever available) of the first {mySceneNumber} scenes:\r\n\r\n";
            for (int j = 0; j <= mySceneNumber - 1; j++)
            {
                tempTitle = sceneList[j].Title;
                if (!string.IsNullOrEmpty(sceneList[j].NarrativeText))
                {
                    tempDescription = sceneList[j].NarrativeText;
                    outLine = $"Scene {j + 1} Title: {tempTitle}\r\nScene {j + 1} Text:\r\n{tempDescription}\r\n";
                }
                else
                {
                    tempDescription = sceneList[j].Hint;
                    outLine = $"Scene {j + 1} Title: {tempTitle}\r\nScene {j + 1} Seed:\r\n{tempDescription}\r\n";
                }
                userPrompt += outLine;
            }

            userPrompt += $"\r\n\r\nPlease create {scenesToCreate} additional scene titles and descriptions to complete the movie. ";
            userPrompt += "\r\n\r\nStyle Rules: ";
            userPrompt += "\r\n\r\n- Keep the writing of the Scene Seeds straightfoward and functional. Avoid decorative language. Each sentence should be short, direct, and in the active voice. ";
            userPrompt += "\r\n\r\n- Every scene should have a clearly named setting that makes sense and is cinematically engaging. ";
            userPrompt += "\r\n\r\n- Scenes should never blend together, but should be carefully delineated. ";
            userPrompt += "\r\n\r\n- Every scene should advance the plot. That means, by the end of the scene, something should have changed in the characters or their relationships. ";
            userPrompt += "\r\n\r\n- Every scene should have a beginning and an end.";
            userPrompt += "\r\n\r\n- Scene Seeds should be no longer than 3 sentences.";
            userPrompt += "\r\n\r\nFormatting:";
            userPrompt += "\r\n\r\nDo not include scene numbers in the title or description. ";
            userPrompt += "\r\n\r\nPlease return the additional scenes as a JSON list of lists in this form:";
            userPrompt += "\r\n\r\n[[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"],[\"scene title\",\"scene description\"]]";

            string response = await UtilsGPT.doGPT("RefactorSceneAfterHeavy", model, 2000, .7, userPrompt, systemPrompt, "", myForm, $"refactoring Scenes after Scene # {mySceneNumber}");

            return response;
        }

        public static async Task<string> makeMagicSceneTextNote(string model, string sceneSeed, string sceneText, MovieObj myMovie, List<SceneObj> myScenes, List<string> listDirections, FormApp1 myForm)
        {
            string response = "";
            string systemPrompt = "";
            string userPrompt = "";

            if (listDirections.Count == 0) // not from form - most likely from Jump Start 
            {
                systemPrompt = $"You are a professional screenwriting consultant tasked with evaluating a Scene Narrative for a movie. {Utils.getProfilePrompt(myForm, "SceneText")}";
                
                systemPrompt += "A Scene Narrative is a detailed prose description of a Scene that will be used as the basis for writing a scene script. Your expertise lies in providing insightful, specific, and actionable feedback on the scene's coherence, dramatic impact, character development, and any other elements you consider relevant. Focus on points of improvement, not what's already working. Do not simply evaluate but explain HOW to make the improvements, with specific textual examples. ";
                
                systemPrompt += "\r\nBelow is the Scene Narrative you are to evaluate. After reading it, please provide your suggestions on how it can be improved. Your feedback will then be used to guide a revision of the narrative. Present your suggestions as a numbered list. ";
                userPrompt = "Scene Narrative for evaluation:\r\n\r\n";
                userPrompt += sceneText + "\r\nEND SCENE SEED ";
                userPrompt += "\r\n\r\nPlease provide an overall maximum of five focused points of suggestion. Provide your response as a numbered list. ";
            }
            else   // from custom form
            {

                systemPrompt = $"You are a professional screenwriting consultant tasked with evaluating a Scene Narrative for a movie. {Utils.getProfilePrompt(myForm, "SceneText")}";
                systemPrompt += "A Scene Narrative is a detailed prose description of a Scene, serving as the basis for writing a scene script. Your expertise lies in providing insightful, specific, and actionable feedback. Focus on points of improvement, not what's already working. Do not simply evaluate but explain HOW to make the improvements, with specific textual examples. ";
                systemPrompt += "\r\nWhat follows is the Scene Seed. The Scene Seed is a shorter document that was used as the basis for writing for the Scene Narrative. The Scene Seed was the foundation for the Scene Narrative and should be wholly encompassed within it. However, the Scene Narrative can expand on this foundation considerably, incorporating new details or introducing new elements. ";
                systemPrompt += "\r\n\r\nScene Seed:\r\n\r\n";
                systemPrompt += sceneSeed + "\r\nEND SCENE SEED\r\n\r\n";

                systemPrompt += "\r\nIn the user prompt below, you will receive the current Scene Narrative. The Scene Narrative is the document that will later be used as the source material for writing the Scene Script. ";
                systemPrompt += "Your feedback will later be used as a prompt back to you to instruct a revision of the Scene Narrative, so make your response suitable for that purpose.";
                systemPrompt += "\r\n\r\nPlease provide feedback, in this order, on the following key areas (and ONLY these areas):\r\n\r\n";


                foreach (string direction in listDirections)
                {



                    if (direction == "SceneSeed")
                    {
                        systemPrompt += "Scene Seed Coverage: The Scene Narrative should encapsulate every event and character from the Scene Seed. If any elements from the Scene Seed are absent in the Scene Narrative, detail them in a numbered list. If the Scene Narrative fully captures the Scene Seed, just say: 'Scene Seed coverage is OK.' Double-check before listing any missing elements.\r\n\r\n";
                    }


                    if (direction == "Intensity")
                    {
                        systemPrompt += "Intensity: The Scene, once filmed, should evoke strong emotions in its audience. Examine the action for places where the audience's emotional response can be heightened. Offer suggestions, with specific examples, for amplifying the intensity.\r\n\r\n";

                    }

                    if (direction == "CharacterInteractions")
                    {
                        systemPrompt += "Character Interactions: The interactions between characters in the Scene Narrative should be dynamic, revealing, and surprising. Offer suggestions, with specific examples, for enriching the quantity and depth of interactions.\r\n\r\n";
                    }
                    if (direction == "MorePlotDetails")

                    {
                        systemPrompt += "Plot Elaboration: The Scene Narrative should contain sufficient plot elements to allow for meaningful change, and should lead seamlessly to a natural beat or setting change. Estimate the duration the scene would take to play out on-screen. If the scene seems too brief or lacking in pivotal events, suggest additional plot elements, actions, or reversals to enrich its content.\r\n\r\n";
                    }
                    if (direction == "MoreAction")

                    {
                        systemPrompt += "More Action: The Scene Narrative should be driven by a sequence of meaningful actions. Propose specific examples of ways to inject more character actions or dynamic movements, ensuring they feel organic and do not disrupt the established plot.\r\n\r\n";
                    }

                    if (direction == "CharacterDevelopment")
                    {
                        systemPrompt += "Character Development: The story in the Movie Text should force its characters to make decisions that reveal their true nature, and/or that lead to lasting change. Provide suggestions, with specific examples, for how to improve character development.\r\n\r\n";
                    }

                    if (direction == "CharacterDescriptions")
                    {
                        systemPrompt += "Character Descriptions: Characters come alive through detailed descriptions. Highlight areas in the Scene Narrative where characters can be portrayed more vividly, whether through physical attributes, mannerisms, or attire. Offer suggestions, with specific examples, for making characters more memorable.\r\n\r\n";
                    }
                    if (direction == "Location")

                    {
                        systemPrompt += "Location: The setting of the Scene Narrative should convey a sense of atmosphere and independent reality. Evaluate the current choice and depiction of setting. Offer suggestions, with specific examples, for improving the setting.\r\n\r\n";
                    }

                    if (direction == "DramaticTension")
                    {
                        systemPrompt += "Dramatic Tension: Dramatic tension keeps an audience engaged. The Scene Narrative should effectively build anticipation, suspense, and uncertainty in pivotal moments. Identify moments where the stakes can be raised or where outcomes are uncertain. Offer suggestions, with specific examples, on how to introduce conflicts, obstacles, or dilemmas that amplify suspense.\r\n\r\n";
                    }
                    if (direction == "InterpersonalTension")
                    {
                        systemPrompt += "The Scene Narrative should showcase the friction between characters resulting from differing desires, values, or histories. Pinpoint interpersonal dynamics that feel flat or harmonious and suggest ways to introduce or escalate conflicts. (These conflicts can be antagonistic, but don't need to be; they can also be subtle disagreements, unmet expectations, or miscommunications.) Detail how characters might challenge or push against each other in ways that feel organic to their personalities and context.\r\n\r\n";

                    }
                    if (direction == "Beginning")
                    {
                        systemPrompt += "A scene's start sets its tone. Reflect on the Scene Narrative's opening: does it effectively pull the viewer in? Could it start later to avoid filler? Is it visually arresting? Offer suggestions, with specific examples, for refining the scene's introduction, ensuring it effectively establishes context and intrigue.\r\n\r\n";
                    }
                    if (direction == "Ending")
                    {
                        systemPrompt += "Ending: The Scene Text's ending should complete the dramatic objective of this scene while generating curiosity about the next (unless of course this is the final scene). Offer suggestions, with specific examples, for enhancing the scene's conclusion.\r\n\r\n";
                    }
                }

                systemPrompt += "Do not provide any feedback about the use of angle brackets <> to enclose character names: example <Mary>. ";
                systemPrompt += "\r\n\r\nPlease provide an overall maximum of five focused points of suggestion.";

                userPrompt = "\r\n\r\nHere is the Scene Narrative for you to evaluate:\r\n\r\n";
                userPrompt += sceneText + "\r\nEND SCENE NARRATIVE";
                userPrompt += "\r\n\r\nPlease provide your response as a numbered list.";

            }


            response = await UtilsGPT.doGPT("MakeMagicSceneTextNote", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making a Magic Scene Text Note");
            return response;
        }
        public static async Task<string> makeMagicSceneScriptNote(string model, string scriptText, string sceneText, List<string> listDirections, FormApp1 myForm)

        {
            string response = "";
            string systemPrompt = "";
            string userPrompt = "";
            if (listDirections.Count == 0)  // not from form
            {
                systemPrompt = $"You are a professional screenwriting consultant tasked with evaluating a Scene Script for a movie. {Utils.getProfilePrompt(myForm, "SceneScript")}";
                systemPrompt += "Your expertise lies in providing insightful, specific, and actionable feedback on coherence, dramatic impact, character development, and any other elements you consider relevant. Your feedback will later be used as part of a prompt back to you to provide guidance in rewriting the Scene Script, so make it suitable for that purpose. Focus on points of improvement, not what's already working. Do not simply evaluate but explain HOW to make the improvements, with specific textual examples. ";
                systemPrompt += "\r\n\r\nThe Scene Script is to closely follow the Scene Narrative. This is the Scene Narrative for the scene:\r\n\r\n";
                systemPrompt += sceneText;

                userPrompt = "This is the Scene Script that you are to evaluate and provide feedback to improve:\r\n\r\n";
                userPrompt += scriptText;
                userPrompt += "\r\n\r\nPlease provide your response as a numbered list.";
            }
            else // from form

            {
                systemPrompt = $"You are a professional screenwriting consultant tasked with evaluating a Scene Script for a movie. {Utils.getProfilePrompt(myForm, "SceneScript")}";
                systemPrompt += "Your expertise lies in providing insightful, specific, and actionable feedback on coherence, dramatic impact, character development, and any other elements you consider relevant. Your feedback will later be used as part of a prompt back to you to provide guidance in rewriting the Scene Script, so make it suitable for that purpose. Focus on points of improvement, not what's already working. Do not simply evaluate but explain HOW to make the improvements, with specific textual examples. ";
                systemPrompt += "You will be provided in the user prompt with a list of topics to provide feedback on. Restrict your responses to those topics. ";
                systemPrompt += "The Scene Script is to closely follow the Scene Narrative. This is the Scene Narrative for the Scene:\r\n\r\n";
                systemPrompt += sceneText;

                userPrompt = "This is the Scene Script that you are to evaluate and provide feedback to improve:\r\n\r\n";
                userPrompt += scriptText + "\r\n\r\n";

                userPrompt += "These are the topics for you to provide feedback on:\r\n\r\n";
                foreach (string direction in listDirections)
                {
                    if (direction == "SettingAtmosphere")
                    {
                        userPrompt += "Setting and Atmosphere: The Scene Script should provide rich descriptions of settings and atmospheres. Each setting description should paint a clear visual picture while conveying a sense of atmosphere and independent reality. Assess if there are any locations in the Scene Script that could benefit from more detailed descriptions and provide suggestions, with specific examples, for improving them.\r\n\r\n";
                    }
                    if (direction == "CharacterDevelopment")
                    {
                        userPrompt += "Character Development: The story in the Scene Script should force its characters to make decisions, however small, that reveal their true nature and/or lead to change. Provide suggestions, with specific examples, for how to improve character development.\r\n\r\n";
                    }
                    if (direction == "DialogueRefine")
                    {
                        userPrompt += "Dialogue: Refine: Assess the Scene Script for the overall quality of dialogue. Every line should feel authentic and plausible, and contribute to character development or move the plot forward. Provide suggestions, with specific examples, for refining the dialogue.\r\n\r\n";
                    }
                    if (direction == "DialogueIncrease")
                    {
                        userPrompt += "Dialogue: Increase Amount: This Scene Script does not contain a sufficient amount of dialogue, and/or dialogue is summarized instead of written out in full. Provide suggestions, with specific examples (including actual lines where appropriate), for more or expanded dialogue.\r\n\r\n";
                    }
                    if (direction == "IncreaseConflict")
                    {
                        userPrompt += "Increase Conflict: The Scene Script should effectively build anticipation, suspense, and uncertainty in pivotal scenes. Identify moments where the stakes can be raised or where outcomes are uncertain. Offer suggestions, with specific examples, on how to introduce conflicts, obstacles, or dilemmas that amplify suspense.\r\n\r\n";
                    }
                    if (direction == "IncreaseVividness")
                    {
                        userPrompt += "Increase Vividness: The Scene Script should be vivid in all its aspects. Provide detailed feedback, with examples, on ways to make descriptions, actions, and dialogue more vivid and engaging.\r\n\r\n";
                    }
                    if (direction == "IncreaseAction")
                    {
                        userPrompt += "Increase Action: Assess the Scene Script for its action descriptions. These should be clear and engaging and should contribute to the pacing and excitement of the scene. Provide suggestions, with examples, to increase the amount or improve the quality of actions.\r\n\r\n";
                    }
                    if (direction == "PacingRhythm")
                    {
                        userPrompt += "Pacing and Rhythm: Each event in the Scene Script should be a cause or effect, leading to the next, building a chain of interconnected events. Offer suggestions, with specific examples, for restructuring or refining the plot to improve its pacing and rhythm.\r\n\r\n";
                    }
                    if (direction == "EmotionalResonance")
                    {
                        userPrompt += "Emotional Resonance: The Scene Script should resonate emotionally with the audience. It should force its characters to make decisions that reveal their true emotional states or lead to emotional changes. Provide suggestions, with specific examples, on how to improve emotional resonance in the script.\r\n\r\n";
                    }
                    if (direction == "DePurpleProse")
                    {
                        userPrompt += "De-Purple Prose: Examine the Scene Script for any overuse of descriptive language that doesn't serve the story, commonly referred to as 'purple prose'. Provide specific rewrites and suggested cuts to reduce purpleness while maintaining the strengths of the narrative.\r\n\r\n";
                    }
                    if (direction == "CoverageSceneText")
                    {
                        userPrompt += "Coverage of Scene Text: The Scene Script needs to include every event and character from the Scene Narrative. IF there are elements in the Scene Narrative that are missing from the Scene Script, provide a numbered list of them. If NO elements in the Scene Narrative are missing from the Scene Script, simply say 'Scene Narrative coverage is OK.'\r\n\r\n";
                    }
                    if (direction == "IncreaseSceneLength")
                    {
                        userPrompt += "Increase Scene Length: This Scene Script is not sufficiently elaborated to serve its purpose in the screenplay. Suggest additional events, interludes, or reversals in the plot. Be imaginative and specific while maintaining the dramatic arc of the original.\r\n\r\n";
                    }
                }
                userPrompt += "\r\n\r\nPlease provide your response as a numbered list.";
            }

            response = await UtilsGPT.doGPT("MakeMagicScriptNote", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making a Magic Script Note");
            return response;
        }

        public static async Task<string> makeMagicMovieTextNote(string model, string movieText, string movieHint, List<string> listDirections, FormApp1 myForm)
        {
            string response = "";
            string systemPrompt = "";
            string userPrompt = "";

            if (listDirections.Count == 0) // not from custom form  - possibly from Jump Start
            {
                systemPrompt = $"You are a professional consulting critic helping a screenplay writer develop a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}";

                systemPrompt += "\r\n\r\nIn the user prompt below you will receive a narrative text, henceforth called the \"Movie Text\". The Movie Text is the narrative document that will later be the source material for writing the movie screenplay. ";
                systemPrompt += "\r\n\r\nYou will provide helpful, incisive, and detailed feedback to improve the Movie Text with respect to coherence, plot and plot details, dramatic impact and arc, character and character development, and anything else you care to advise on to improve the Movie Text. ";
                systemPrompt += "\r\n\r\nThe Movie Text should provide a detailed description of the plot, characters, and settings of the movie, in sufficient detail to serve as the basis for a feature-length film. ";
                systemPrompt += "\r\n\r\nYour feedback will be used later to prompt you for rewriting the Movie Text, so provide your feedback in a form suitable for that purpose. Make your response specific, detailed, and actionable. Focus on what can be improved, not on what is currently good. Do not simply evaluate the narrative but explain HOW to make the improvements, providing concrete textual examples whenever possible. ";

                systemPrompt += "\r\n\r\nWhat follows is the \"Movie Seed\". The Movie Seed is a shorter document that was used as the basis for writing the Movie Text. The Movie Seed provides in whole or part a summary of the movie plot and characters. (Everything in the Movie Seed should be represented in the Movie Text, though the Movie Text may have much more detail and may include events and characters not in the Movie Seed.) ";
                systemPrompt += "\r\n\r\nMovie Seed:\r\n\r\n";
                systemPrompt += movieHint + "\r\nEND MOVIE SEED\r\n\r\n";

                systemPrompt += "\r\n\r\nYour feedback will later be used as part of a prompt to provide you with guidance in rewriting the Movie Text, so make your response ";
                systemPrompt += "suitable for that purpose. Make your response specific, detailed, and actionable. Focus on what can be improved, not on what is currently good. ";

                systemPrompt += "\r\nPlease provide your response as a numbered list. Provide a maximum of four focused points of suggestion.";

                userPrompt = "Here is the Movie Text that you are to evaluate and provide feedback to improve:\r\n\r\n";
                userPrompt += movieText + "\r\nEND MOVIE TEXT";
                userPrompt += "\r\n\r\nPlease provide your response as a numbered list. Provide a maximum of four focused points of suggestion.";
            }
            else // from Form 
            {
                systemPrompt = $"You are a professional consulting critic helping a screenplay writer develop a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}";
                systemPrompt += "In the user prompt below you will receive a narrative text, henceforth called the \"Movie Text\". The Movie Text is the narrative document that will later be the source material for writing the movie screenplay. ";

                systemPrompt += "\r\n\r\nWhat follows is the \"Movie Seed\". The Movie Seed is a shorter document that was used as the basis for writing the Movie Text. The Movie Seed provides in whole or part a summary of the movie plot and characters. (Everything in the Movie Seed should be represented in the Movie Text. The Movie Text may have much more detail and may include events and characters not in the Movie Seed.) ";
                systemPrompt += "\r\n\r\nMovie Seed:\r\n\r\n";
                systemPrompt += movieHint + "\r\nEND MOVIE SEED\r\n\r\n";

                systemPrompt += " You will provide helpful, incisive, detailed, and specific feedback to improve the Movie Text. In your feedback, minimize what you find good and emphasize improvements that could be made. Be precise, declarative, and detailed in your recommendations. ";
                systemPrompt += "Your feedback will be used later to prompt you for rewriting the Movie Text later, so provide your feedback in a form suitable for that purpose. Please provide feedback, in order, on the following list of topics. Please DO NOT provide feedback on ANY topics not listed below.\r\n\r\n";

                foreach (string direction in listDirections)
                {
                    if (direction == "MovieSeed")
                    {
                        systemPrompt += "Movie Seed Coverage: The Movie Text needs to include every event and character from the Movie Seed. IF there are elements in the Movie Seed that are missing from the Movie Text, provide a numbered list of them. If NO elements in the Movie Seed are missing from the Movie Text, simply say \"Movie Seed coverage is OK.\" and do not provide any further notes on this topic. Please double-check that elements are actually missing before including them in the list. ";
                    }


                    if (direction == "Coherence")
                    {
                        systemPrompt += "Coherence: When reading the Movie Text, one should never be jolted out of immersion by plot points or character behaviors that don't align with the narrative. Assess the Movie Text for any sequences that seem out of place or actions that feel inauthentic for a character. Provide suggestions, with specific examples, to smooth out these inconsistencies, ensuring the story flows seamlessly. Every event or choice should have a logical cause or motivation, even if it's revealed later.\r\n\r\n";

                    }

                    if (direction == "DramaticArc")
                    {
                        systemPrompt += "Dramatic Arc: The Movie Text should have a compelling dramatic arc that guides the audience through an engaging journey. Assess whether and how well the story follows the vital phases of storytelling: exposition, rising action, climax, and resolution. Offer suggestions, with specific examples, for enhancing the dramatic tension. Large-scale revisions are as welcome as minor ones.\r\n\r\n";
                    }
                    if (direction == "PlotStructure")
                    {
                        systemPrompt += "Plot Structure: : Each event in the Movie Text should be a cause or effect, leading to the next, building a chain of interconnected events. Significant events should proceed logically, forming a clear causal chain that's both interesting and, at times, surprising. Offer suggestions, with specific examples, for restructuring or refining the plot.\r\n\r\n";
                    }
                    if (direction == "PlotElaboration")
                    {
                        systemPrompt += "Plot Elaboration: The Movie Text should have a plot that is sufficiently elaborated to serve as the basis for a feature-length film. Estimate how many minutes the current plot would take to unfold and whether this is within the usual range for a feature film. If you believe more plot is needed, suggest some additional events, interludes, or reversals in the plot. Be imaginative while maintaining the overall dramatic arc of the original.\r\n\r\n";
                    }
                    if (direction == "CharacterDescription")
                    {
                        systemPrompt += "Character Description: The characters in the Movie Text should be richly realized and described (appearance, motivations, backstory when relevant, etc). Provide suggestions, with specific examples, for improving character descriptions. Be imaginative while maintaining plot coherence and the dramatic arc.\r\n\r\n";
                    }
                    if (direction == "CharacterDevelopment")
                    {
                        systemPrompt += "Character Development: The story in the Movie Text should force its characters to make decisions that reveal their true nature, and/or that lead to lasting change. Provide suggestions, with specific examples, for how to improve character development.\r\n\r\n";
                    }
                    if (direction == "LocationDescription")
                    {
                        systemPrompt += "Location Description: The Movie Text should provide rich descriptions of locations. Each setting description should paint a clear visual picture while conveying a sense of atmosphere and independent reality. Assess if there are any locations in the Movie Text that could benefit from more detailed descriptions and provide suggestions, with specific examples, for improving them.\r\n\r\n";
                    }
                    if (direction == "DramaticTension")
                    {
                        systemPrompt += "Dramatic Tension: Dramatic tension keeps an audience engaged. The Movie Text should effectively build anticipation, suspense, and uncertainty in pivotal scenes. Identify moments where the stakes can be raised or where outcomes are uncertain. Offer suggestions, with specific examples, on how to introduce conflicts, obstacles, or dilemmas that amplify suspense.\r\n\r\n";
                    }
                    if (direction == "Interpersonal")
                    {
                        systemPrompt += "Interpersonal Tension: The Movie Text should showcase the friction between characters resulting from differing desires, values, or histories. Pinpoint interpersonal dynamics that feel flat or harmonious and suggest ways to introduce or escalate conflicts. (These conflicts can be antagonistic, but don't need to be; they can also be subtle disagreements, unmet expectations, or miscommunications.) Detail how characters might challenge or push against each other in ways that feel organic to their personalities and context.\r\n\r\n";

                    }
                    if (direction == "Beginning")
                    {
                        systemPrompt += "Beginning: The beginning of the Movie Text should set the tone, introduce the world and characters, and hook the audience. Reflect on whether the current beginning effectively grabs attention and sets the stage. Are the main characters introduced compellingly? Is the setting clear and evocative? Can the audience quickly understand the stakes of the story and its core concerns? Offer suggestions on how to improve the beginning. (Sometimes reshuffling events, starting in medias res, or introducing a mystery can make an impact.)\r\n\r\n";
                    }
                    if (direction == "Ending")
                    {
                        systemPrompt += "Ending: The ending of the Movie Text should provide resolution, leaving the audience satisfied, moved, or contemplative. It's the culmination of all preceding events and should resonate emotionally while tying up narrative threads. Consider if the current conclusion feels earned, if it’s consistent with the story's trajectory, and if it leaves a lasting impression. Provide suggestions on how to improve the ending.\r\n\r\n";
                    }
                }
                systemPrompt += "Do not provide any feedback about the use of angle brackets <> to enclose character names: example <Mary>.";
                systemPrompt += "Please provide an overall maximum of five focused points of suggestion. ";

                userPrompt = "Here is the Movie Text that you are to evaluate and provide feedback to improve:\r\n\r\n";
                userPrompt += movieText + "\r\nEND MOVIE TEXT";
                userPrompt += "\r\n\r\nPlease provide an overall maximum of five focused points of suggestion. Provide your response as a numbered list. ";
            }

            response = await UtilsGPT.doGPT("MakeMagicMovieTextNote", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making a Magic Movie Text Note");
            return response;
        }


        public static async Task<string> makeMovieTextLengthNote(string model, string movieText, int TimeLength, FormApp1 myForm)
        {
            double percentDbl = 0.0;
            int percent = 0;

            int lengthMovieText = movieText.Length;
            int targetMovieTextLength = TimeLength * 100;

            percentDbl = (double)targetMovieTextLength / (double)lengthMovieText;

            percent = (int)(percentDbl * 100);
            percent = percent - 100;

            string response = "";

            string systemPrompt = $"You are a professional analyst and writer for a movie studio working on a movie. {Utils.getProfilePrompt(myForm, "MovieText")}"; 
            systemPrompt += "Your task is to make detailed suggestions of material to add to a given prose narrative, referred to as the \"Movie Text\", to make it a suitable length for a feature-length film script. ";
            systemPrompt += "\r\n\r\nYour Role:\r\n\r\n- You'll receive the Movie Text as part of the user prompt. This text outlines all existing elements that will be in the movie script. ";
            systemPrompt += "\r\n\r\n- You will also be given a percentage indicating how much longer the narrative needs to be. For example, \"50%\" means that we're looking to make the Movie Text 50% longer. ";

            systemPrompt += "\r\n\r\nYour Output:\r\n\r\n- Reply with no more than five (5) detailed suggestions for meeting the specified lengthening target. ";
            systemPrompt += "\r\n\r\n- Your suggestions can optionally include new scenes, characters, plot twists, locations, elements of dramatic or interpersonal tension, further character development, or new beginnings and endings. Use your extensive expertise in film to ensure that your suggestions enhance the movie quality while maintaining narrative coherence. ";
            systemPrompt += "\r\n\r\n- State your suggestions as directions, not hypotheticals. Instead of leaving specifics to be filled in at the next stage, provide them now. Avoid words like \"could\" and \"perhaps.\" Whenever possible, make your suggestions such that they could be interpolated in the movie text without extensive adjustment. ";
            systemPrompt += "\r\n\r\n- Pursue depth over breadth. ";
            systemPrompt += "\r\n\r\n- All character names must be enclosed in angle brackets, like so: <Mary>. ";


            string userPrompt = "Below is the Movie Text for your evaluation:\r\n\r\n";
            userPrompt += movieText;
            userPrompt += $"\r\n\r\nPlease provide a numbered list of detailed suggestions for making the Movie Text {percent}% longer. ";

            string msg = $"Creating Expansive Notes to expand the Movie Text";
            response = await UtilsGPT.doGPT("MakeExpansiveNotesMovieText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, msg);

            return response;
        }

        public static async Task<string> makeSceneTextLengthNote(string model, string sceneText, FormApp1 myForm)
        {

            string response = "";

            string systemPrompt = $"You are a professional analyst and writer for a movie studio working on a movie. {Utils.getProfilePrompt(myForm, "SceneText")}";
            systemPrompt += "Your task is to make detailed suggestions of material to add to a given prose narrative, referred to as the \"Scene Text,\" which you have deemed to be too short in duration. ";
            systemPrompt += "Your suggestions for additional material are intended to make the Scene Text longer in run time when filmed. ";
            systemPrompt += "\r\n\r\nYour Role:\r\n\r\n- You'll receive the Scene Text as part of the user prompt. This text outlines all existing elements that will be in the Scene. ";


            systemPrompt += "\r\n\r\nYour Output:\r\n\r\n- Reply with no more than five (5) detailed suggestions for meeting the specified lengthening target. ";
            systemPrompt += "\r\n\r\n- Your suggestions can optionally include new characters, plot twists, elements of dramatic or interpersonal tension, further character development, or new beginnings and endings. Use your extensive expertise in film to ensure that your suggestions enhance the movie quality while maintaining narrative coherence. ";
            systemPrompt += "\r\n\r\n- State your suggestions as directions, not hypotheticals. Instead of leaving specifics to be filled in at the next stage, provide them now. Avoid words like \"could\" and \"perhaps.\" Whenever possible, write suggestions in such a way that they could be interpolated in the Scene Text without extensive adjustment. ";
            systemPrompt += "\r\n\r\n- Pursue depth over breadth. ";
            systemPrompt += "\r\n\r\n- All character names must be enclosed in angle brackets, like so: <Mary>. ";


            string userPrompt = "Below is the Scene Text for your evaluation:\r\n\r\n";
            userPrompt += sceneText + "\r\nEND SCENE TEXT";
            userPrompt += $"\r\n\r\nPlease provide an overall maximum of five points of suggestion. Provide your response as a numbered list. ";

            string msg = $"Creating Expansive Notes to expand the Scene Text";
            response = await UtilsGPT.doGPT("MakeExpansiveNotesSceneText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, msg);

            return response;
        }

        public static async Task<List<CharacterNameAge>> getAgesOfCharacters(string model, string MovieText, List<CharacterObj> myCharacters, FormApp1 myForm)

        {
            List<CharacterNameAge> characterNameAges = new List<CharacterNameAge>();

            string systemPrompt = @"You are movie screen writing consultant. Your job today is to read a synopsis of a movie, called the Movie Narrative, and from hints in the Movie Narrative and from your general knowledge, estimate the age of every character in a provided list of characters (all of whom appear in the Movie Narrative). If the age of a character is unclear from the information in the movie narrative, or unmentioned,  make your best estimate of the character's age. You must always provide a numerical value of age, in years, for each name on the list. If you have no good evidence, you must guess an age. Only if you are very, very confident that the character is an immortal being (such as a god), use 10000 as the age. Note that in the Movie Narrative, character names are enclosed in angle brackets <>. Examples <Mary> <Bob>.";

            systemPrompt += "\r\n\r\nHere are the character names:\r\n\r\n";

            foreach (CharacterObj character in myCharacters)
            {
                systemPrompt += character.tagName + "\r\n";
            }

            string userPrompt = "This is the movie narrative:\r\n\r\n";
            userPrompt += MovieText + "\r\n\r\nEND MOVIE NARRATIVE";

            userPrompt += "\r\n\r\nIf the age of a character is unclear from the information in the movie narrative or unmentioned, make your best estimate of the character's age. You must always provide a numerical value of age, in years, for each name on the list. If you have no good evidence, you must guess an age. Only if you are very, very confident that the character is an immortal being such as a god, use 10000 as the age.\r\n\r\n";

            userPrompt += $"Please output the Name and Age of all {myCharacters.Count} characters listed using the specified format. Output the Name as only the name and the Age as a numeric value with no qualifications, no other text.\r\n\r\n";
            userPrompt += @"Name : Age

Example:

Susan  :  21
Bob  :  35
Linda  :   67";

            string msg = "Estimating Ages for Characters From Movie Text";
            string response = await UtilsGPT.doGPT("EstimateCharacterAges", model, 2000, .7, userPrompt, systemPrompt, "", myForm, msg);
            string rightHalf = "";
            string leftHalf = "";
            string Name = "";
            string Age = "";
            string[] lines = Utils.SplitStringAtNewLines(response);

            foreach (string line in lines)
            {
                if (!line.Contains(":"))
                { continue; }

                leftHalf = line.Substring(0, line.IndexOf(":"));
                rightHalf = line.Substring(line.IndexOf(":") + 1);
                Name = leftHalf.Trim();
                Age = Utils.ExtractFirstNumericString(rightHalf);

                CharacterNameAge newItem = new CharacterNameAge();
                newItem.Name = Name;
                newItem.Age = Age;
                characterNameAges.Add(newItem);
            }
            return characterNameAges;

        }
        public static async Task<string> makeCharacterProfiles(string model, string storyText, List<CharacterObj> myCharacters, FormApp1 myForm, Boolean skipAgeFind, string year)
        {
            // get ages of characters
            int goodCharacters = 0;
            year = year.Trim();

            //if (myCharacters.Count <= 10)
            //{
            //    model = "gpt-4-turbo-preview";
            //}
            //else
            //{
            //    model = "gpt-3.5-turbo-16k-0613";
            //}

            if (!skipAgeFind)
            {

                

                List<CharacterNameAge> nameAgeCharacters = await getAgesOfCharacters(model, storyText, myCharacters, myForm);
                
                foreach (CharacterNameAge ageNameChar in nameAgeCharacters)
                {

                    CharacterObj fullChar = myCharacters.Find(p => p.tagName == ageNameChar.Name);

                    if (fullChar != null && fullChar.age.Length == 0)
                    {

                        
                        fullChar.age = ageNameChar.Age;


                    }
                    goodCharacters++;
                }

            }

            else
            {
                goodCharacters = myCharacters.Count;
            }

            if (myCharacters.Count == 0) { MessageBox.Show("No characters in character list."); return "<NA>"; }

            string systemPrompt = "You are a world-famous creative consultant for a movie studio. Your task is to take a synopsis of a movie, which we will call the \"Story Text\", and a list of characters, and to output detailed information about the characters in JSON format. The Story Text may provide some hints to use when creating the detailed character information, but in other cases you should use your creative powers to invent details that feel like they are congruent with the narrative in the Story Text.";
            systemPrompt += "\r\n\r\nFor each character, you will provide a series of attribute descriptions. For each description you will provide a tag, and a block of text one to two paragraphs in length. The information you will produce for each character is as follows: \r\n\r\n";

            systemPrompt += "Tag: Name \r\nText: the name of the character provided in the user prompt. \r\n\r\n";
            systemPrompt += "Tag: Age \r\nText: age of the character in years as a numeric value.\r\n\r\n";
            systemPrompt += "Tag: BackStory \r\nText: a two-paragraph biography, starting with the character's age and then describing their life prior to the story, including education, socioeconomic background, and important life events. \r\n\r\n";
            systemPrompt += "Tag: Physical \r\nText: a paragraph providing a physical description of the character at the time of the story in the Story Text. Include descriptions of how the character dresses and how they carry themselves in the world. \r\n\r\n";
            systemPrompt += "Tag: Personality \r\nText: a paragraph describing the character's personality, including traits, core beliefs, temperament, and other relevant characteristics. \r\n\r\n";
            systemPrompt += "Tag: Speech \r\nText: a paragraph describing the style of speech of the character including their typical register of speech, formality or informality, adherence to standard grammar, degree of verbosity, vocabulary, voice tone, gestures, dialect, and other relevant characteristics of their communication style. \r\n\r\n";


            systemPrompt += $"You will create the full set of attributes for all {goodCharacters} characters. You will not truncate the list. \r\n\r\n";
            systemPrompt += $"You will return a JSON list of the attributes for each of the {goodCharacters} characters, in the following format:\r\n\r\n";

            systemPrompt += "[\r\n\r\n";
            systemPrompt += "{\r\n\r\n";

            systemPrompt += "\"Name\": \"the character's name\",";
            systemPrompt += "\"Age\": \"age of the character in years as a numeric value\",";
            systemPrompt += "\"BackStory\": \"a two-paragraph biography, starting with the character's age and then describing their life prior to the story, including education, socioeconomic background, and important life events\",";
            systemPrompt += "\"Physical\": \"a paragraph providing a physical description of the character at the time of the story in the Story Text. Include descriptions of how the character dresses and how they carry themselves in the world.\",";
            systemPrompt += "\"Personality\": \"a paragraph describing the character's personality, including traits, core beliefs, temperament, and other relevant characteristics.\",";
            systemPrompt += "\"Speech\": \"a paragraph describing the style of speech of the character including their typical register of speech, formality or informality, adherence to standard grammar, degree of verbosity, vocabulary, voice tone, gestures, dialect, and other relevant characteristics of their communication style. \r\n\r\n";
            systemPrompt += "}\r\n\r\n";
            systemPrompt += "]\r\n\r\n";

            systemPrompt += $"Your JSON string will include: Name, Age, BackStory, Physical, Personality, and Speech for every one of the {goodCharacters} characters listed in the user prompt. ";

            systemPrompt += "\r\n\r\nHere is the Story Text that you will be using as a source for information about the characters: \r\n\r\n";
            systemPrompt += storyText;
            systemPrompt += "\r\n\r\nEND STORY TEXT\r\n\r\n";

            string userPrompt = $"Please create details as specified above for all {goodCharacters} character names below:\r\n\r\n";
            userPrompt += $"Do not truncate the list of character details. Return the entire list of {goodCharacters} character details.\r\n\r\n";
            string workDescription;

            foreach (CharacterObj character in myCharacters)
            {
                workDescription = "";
                if (character.briefDescription.Trim().Length > 0)
                {
                    workDescription = "Description: " + character.briefDescription;
                }

                if (character.age.Trim().Length > 0)
                {
                    if (character.age == "10000" || character.age == "10,000")
                    {
                        userPrompt += $"Name: {character.tagName} Age: immortal {workDescription}\r\n";
                    }
                    else
                    {
                        userPrompt += $"Name: {character.tagName} Age: {character.age} {workDescription}\r\n";
                    }

                }
                

            }

            userPrompt += "\r\n\r\nEND LIST OF CHARACTERS\r\n\r\n";
            if (year.Length > 0)
            {
                userPrompt += $"The story takes place in the year {year}. Be sure to create character attributes appropriate for that time.\r\n\r\n";
            }
            userPrompt += "Return only the JSON string.\r\n\r\n";

            string msg = "Creating Character Details From Story Text";
            string response = await UtilsGPT.doGPT("CreateCharacterDetailsFromMovieText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, msg);


            return response;

        }

        public static async Task<string> rewriteMovieTextWithCharacterDetails(string model, string MovieText, List<CharacterObj> myCharacters, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {

            string errorMsg = "";
            string systemPrompt = $"You are a talented screenwriter working on a movie. {Utils.getProfilePrompt(myForm,"MovieText")}";
            systemPrompt += "You are writing a new version of a \"Movie Narrative,\" which is a detailed prose synopsis of a movie. The Movie Narrative will later be used as the basis for writing a movie script. ";
            
            systemPrompt += "In the user prompt below, you will be provided with the original Movie Narrative. ";
            systemPrompt += "You will also be provided with a list of characters with their attributes: Name, Age, Back Story, Physical Description, Personality, and Speech Style. ";

            systemPrompt += "\r\n\r\nYou will rewrite the Movie Narrative, drawing on the original Movie Narrative as a source for plot elements and taking into consideration the attributes of the characters provided in the user prompt. ";
            systemPrompt += "\r\n\r\nThe new Movie Narrative that you are writing should draw on plot elements and themes from the original Movie Narrative as inspiration, but should also reenvision the story considering how the characters might behave in the story consistent with their character attributes. If you believe plot elements aren't consistent with how a character would behave, change the plot elements to be consistent with how the character would behave. Characters should behave in a manner consistent with their attributes, unless/until they are changed or transformed by the events of the story. ";
            systemPrompt += "\r\n\r\nDo not restrict the length of your reponse. It is common for the rewritten version of the Movie Narrative to be quite different and longer than the original Movie Narrative. \r\n\r\n";
            //systemPrompt += "\r\n\r\nStylistically, the writing of the Movie Text should be simple, direct, and functional. Avoid florid and purple prose, and include only details that are relevant to the plot. ";

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            // systemPrompt += Utils.getAngleBracketPrompt();
            /* systemPrompt += "\r\n\r\nHere are the characters and their attributes:\r\n\r\n";

            foreach (CharacterObj character in characters)
            {

                if (Utils.CanConvertToInt(character.age.Trim()))
                {
                    systemPrompt += $"Character Name: {character.tagName}\r\n";

                    if (character.age.Trim() == "10000")
                    {
                        systemPrompt += "Age: Immortal\r\n";
                    }
                    else
                    {
                        systemPrompt += $"Age: {character.age}\r\n";
                    }

                    systemPrompt += $"Back Story: {character.backStory}\r\n\r\n";
                    systemPrompt += $"Physical Description: {character.physicalDescription}\r\n\r\n";
                    systemPrompt += $"Personality: {character.personality}\r\n\r\n";
                    systemPrompt += $"Style of Speaking: {character.speechStyle}\r\n\r\n";
                }

            }  */


            string userPrompt = "";

            userPrompt += "Here is the original Movie Narrative that you will be rewriting:\r\n\r\n";
            userPrompt += MovieText + "\r\n\r\n";
            userPrompt += "END OF MOVIE NARRATIVE\r\n\r\n";
            userPrompt += "You will now rewrite the Movie Narrative, taking into consideration the following characters and their attributes: ";

            foreach (CharacterObj character in myCharacters)
            {
                    userPrompt += $"\r\n\r\nCharacter Name: <{character.tagName}>\r\n\r\n";

                    if (character.age.Trim() == "10000")
                    {
                        userPrompt += "Age: Immortal\r\n\r\n";
                    }
                    else
                    {
                        userPrompt += $"Age: {character.age}\r\n\r\n";
                    }

                    userPrompt += $"Back Story: {character.backStory}\r\n\r\n";
                    userPrompt += $"Physical Description: {character.physicalDescription}\r\n\r\n";
                    userPrompt += $"Personality: {character.personality}\r\n\r\n";
                // userPrompt += $"Style of Speaking: {character.speechStyle}\r\n\r\n";

                userPrompt += $"END OF CHARACTER ATTRIBUTES FOR <{character.tagName}>";


            }
            userPrompt += "\r\n\r\nEND OF CHARACTER LIST";

            userPrompt += Utils.getAngleBracketPrompt();

            userPrompt += "\r\n\r\nYou will now rewrite the Movie Narrative, following the instructions in the system prompt above. ";
            
            userPrompt += "\r\n\r\nPlease DO NOT include any preamble sentence such as \"Here is the rewritten narrative:\". Begin now with the first sentence of the rewritten Movie Narrative. ";

            string response = await UtilsGPT.doGPT("RewriteMovieTextWithCharacters", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "Rewriting Movie Text using Character Details");

            return response;
        }

        public static async Task<string> heavyRewriteTextWithCharacterDetails(string model, string MovieText, List<CharacterObj> myCharacters, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            string systemPrompt = "";
            string userPrompt = "";
            string response = "";
            string MovieOutline = "";

            systemPrompt = $"You are a talented screenwriting consultant working on a movie. {Utils.getProfilePrompt(myForm,"MovieText")}";
            systemPrompt += "You have been hired to read a Movie Text, which is a detailed prose synopsis of a movie, and then convert it into a detailed list of plot points. ";
            
            systemPrompt += "You will output an unnumbered list of all of the plot points in the Movie Text. Each plot point will include the names of the characters taking part in that plot point. ";

            userPrompt += "This is the Movie Text that you will read and convert into a detailed list of plot points.\r\n\r\n";
            userPrompt += MovieText + "\r\n\r\n";
            userPrompt += "END OF MOVIE TEXT\r\n\r\n";
            userPrompt += "You will output an unnumbered list of all of the plot points in the Movie Text. Each plot point will include the names of the characters taking part in that plot point. ";
            userPrompt += Utils.getAngleBracketPrompt();

            response = await UtilsGPT.doGPT("MakeOutlineOfMovieText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making outline of Movie Text");
            MovieOutline = response;

            string errorMsg = "";

            systemPrompt = "You are a talented screenwriter who specializes in writing a \"Movie Text,\" a detailed prose synopsis of a movie, from a provided \"Movie Outline\" and Character List (a list of the leading characters and their attributes), provided in the user prompt below. The Movie Text you write should be based on the plot points presented in the Movie Outline, but much expanded in length and detail, and incorporating the characters from the Character List. You will later use this Movie Text as the basis for creating the screenplay.";
            
            systemPrompt += $"\r\n\r\nPlease use your powerful creative skills to imagine and interpolate additional actions, events, descriptions, and details into the narrative description so that it is long enough to be the basis for a screenplay for a feature-length film. ";

            systemPrompt += "\r\n\r\nDo not provide a title for the movie.";

            systemPrompt += "\r\n\r\nBe creative! As needed, create additional characters. Do add details and plot events as warranted. Do not provide a summary or moral at the end of the narrative, or any closing markers. When the narrative action finishes, that is the end of the narrative.";

            systemPrompt += Utils.getAngleBracketPrompt();

            systemPrompt += "The characters in the Movie Text should act in alignment with their character attributes. In other words, you will rewrite the Movie Text so that it is driven by the characters described in the Character List. ";

            //systemPrompt += "\r\n\r\nStylistically, the writing of the Movie Text should be simple, direct, and functional. Avoid florid and purple prose, and include only details that are relevant to the plot. Write the narrative entirely in the present tense.";
            systemPrompt += "\r\n\r\nWrite the narrative entirely in the present tense.";

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            userPrompt = "\r\n\r\nHere is the Movie Outline:\r\n\r\n";
            userPrompt += MovieOutline + "\r\nEND OF MOVIE OUTLINE";

            userPrompt += "\r\n\r\nHere are the Characters and their Attributes:\r\n\r\n";

            foreach (CharacterObj character in myCharacters)
            {
                userPrompt += $"Character Name: <{character.tagName}>\r\n";

                if (character.age.Trim() == "10000")
                {
                    userPrompt += "Age: Immortal\r\n\r\n";
                }
                else
                {
                    userPrompt += $"Age: {character.age}\r\n\r\n";
                }

                userPrompt += $"Back Story: {character.backStory}\r\n\r\n";
                userPrompt += $"Physical Description: {character.physicalDescription}\r\n\r\n";
                userPrompt += $"Personality: {character.personality}\r\n\r\n";
                // userPrompt += $"Style of Speaking: {character.speechStyle}\r\n\r\n";

                userPrompt += $"END OF CHARACTER ATTRIBUTES FOR <{character.tagName}>\r\n\r\n";

            }
            userPrompt += "END OF CHARACTER LIST\r\n\r\n";

            userPrompt += "Please write a Movie Text using the Movie Outline and Character Attributes. You must include every plot element from the Movie Outline. ";
            
            userPrompt += "If you believe some plot elements aren't consistent with how a character would behave, change those plot elements to be consistent with how the character would behave. ";
            userPrompt += "\r\n\r\nCharacters should behave in a manner consistent with their attributes, unless/until they are changed or transformed by the events of the story.";
            userPrompt += "\r\n\r\nDo not try and restrict the length of your reponse. It is common for the rewritten version of the Movie Narrative to be longer than the original version of the Movie Narrative. ";
            userPrompt += "Be sure the Movie Text is a coherent narrative that makes sense as a story around which to create a Movie. ";
            userPrompt += Utils.getAngleBracketPrompt();
            userPrompt += "Please DO NOT include any preamble sentence, such as \"Here is the rewritten narrative:\". Begin now with the first sentence of the rewritten Movie Narrative. ";
            response = await UtilsGPT.doGPT("RewriteMovieTextWithCharactersHeavy", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, "Rewriting Movie Text using Character Details");

            return response;
        }

        public static async Task<string> rewriteCharacterAttribute(string model, string attributeText, string note, string rbName, CharacterObj character, FormApp1 myForm, string year)
        {
            string attributePrompt = "";
            
            string messageName = rbName.Replace("radio", "");
            
            year = year.Trim();

            attributePrompt = Utils.lookupCharacterRadioButton(rbName, character);

            string systemPrompt = $"You are a talented screenwriter who specializes in rewriting character descriptions according to provided instructions. Your instructions will take the form of a Note provided in the user prompt. ";
            systemPrompt += $"\r\n\r\nYou are rewriting {attributePrompt} ";
            systemPrompt += "\r\n\r\nHere is the original description of the character attribute:\r\n\r\n";
            systemPrompt += $"{attributeText}\r\n\r\n";


            string userPrompt = "Please rewrite the character attribute as specified in the following Note:\r\n\r\n";
            userPrompt += $"Note:\r\n{note}. " ;
            if (year.Length > 0)
            {
                userPrompt += $"\r\n\r\nThe story is set in the year {year}. ";
            }

            userPrompt += "\r\n\r\n";
            userPrompt += "END OF NOTE";
            
            userPrompt += Utils.getAngleBracketPrompt();
            userPrompt += "Please DO NOT include any preamble sentence, such as \"Here is the rewritten attribute:\". Provide only the rewritten attribute. ";

            string response = await UtilsGPT.doGPT("RewriteCharacterAttribute", model, 2000, .7, userPrompt, systemPrompt, "", myForm, $"Rewriting Character {character.tagName} Attribute: {messageName}");
            return response;
        }

        public async static Task<string> rewriteSELECTEDCharacterAttribute(string model, string originalText, string selectedText, string notes, string rbName, CharacterObj character, FormApp1 myForm)
        {
            string response = "";
            

            string attributePrompt = "";
            attributePrompt = Utils.lookupCharacterRadioButton(rbName, character);

            string systemPrompt = $"You are a talented screenwriter who specializes in character development. Today we are working on the character attributes for a movie script. ";
            systemPrompt += Utils.getProfilePrompt(myForm, "Character");
            systemPrompt += $"Your task is to rewrite a specified segment of {attributePrompt}\r\n\r\n";

            systemPrompt += $"Here is the original character attribute:\r\n\r\n{originalText}.\r\n\r\n";
            systemPrompt += "You will be provided in the user prompt below with the selected part of the original character attribute to rewrite.\r\n\r\n";

            

            systemPrompt += "You will also be provided with \"Notes,\" which are a set of instructions for rewriting the selected part. ";
            systemPrompt += "You will rewrite the selected part, taking into consideration the Notes. To the degree possible, unless instructed otherwise by the Notes, you will retain all other details from the original version.";


            string userPrompt = "Here is the selected part:\r\n\r\n";
            userPrompt += selectedText;
            userPrompt += "\r\n\r\nHere are the Notes to use for rewriting the selected part:\r\n\r\n";
            userPrompt += notes;
            userPrompt += "\r\n\r\nPlease rewrite only the Selected Text, taking into consideration the Notes.";

            response = await UtilsGPT.doGPT("RewriteCharacterAttributeSelectedText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, $"Rewriting Selected Region of Character Attribute: {rbName}");

            
            return response;
        }

        public static async Task<string> askName(string model, string question, CharacterObj myCharacter, FormApp1 myForm,string year)
        {

            string systemPrompt = "You are going to pretend to be a real person. You will be provided with some attributes of a real person. ";
            systemPrompt += "You will then be asked a question in the user prompt below. ";
            systemPrompt += "Please answer the question acting as if you are the person whose attributes were provided. In all cases, provide an answer even if you have to creatively invent something. Always answer the question.  Only provide the answer to the question.\r\n\r\n";
            systemPrompt += $"Your name is {myCharacter.tagName}.\r\n\r\n"; 
            
            if (myCharacter.age.Trim() == "10000")
            {
                systemPrompt += "Your age is \"Immortal\".";
            }
            else
            {
                systemPrompt += $"You are {myCharacter.age} years old.\r\n\r\n";
            }

            systemPrompt += $"Here is a physical description of you: \r\n{myCharacter.physicalDescription}.\r\n\r\n";
            systemPrompt += $"Here is a description of your personality: \r\n{myCharacter.personality}.\r\n\r\n";
            systemPrompt += $"Here is a description of your speaking style: \r\n{myCharacter.speechStyle}.\r\n\r\n";

            string userPrompt = "";
            if (year.Length > 0)
            {
                userPrompt += $"The year is {year}. Answer the question in a way that is consistent with that period in time.\r\n\r\n";
            }
            userPrompt += "Please answer the following question, acting as if you are the person whose attributes were provided. In all cases, provide an answer even if you have to creatively invent something. Always answer the question. Only provide the answer to the question:\r\n\r\n";
            userPrompt += question;

            string response = await UtilsGPT.doGPT("AnswerQuestionAsCharacter", model, 2000, .7, userPrompt, systemPrompt, "", myForm, $"Answering a question as if <{myCharacter.tagName}>...");
            return response;
        }

        public static async Task<string> makeCharacterProfileFromBriefDescription(string model,List<CharacterObj> myCharacters, FormApp1 myForm, string year)
        {

            //if (myCharacters.Count <= 10)
            //{
            //    model = "gpt-4-turbo-preview";
            //}
            //else
            //{
            //    model = "gpt-3.5-turbo-16k-0613";
            //}

            int goodCharacters = myCharacters.Count;
            if (myCharacters.Count == 0) { MessageBox.Show("No characters in character list."); return "(NA)"; }

            string systemPrompt = $"You are a world-famous creative consultant for a movie studio working on a movie. {Utils.getProfilePrompt(myForm, "Character")}Your task is to take a list of characters and to output detailed information about the characters in JSON format. ";
            
            systemPrompt += "You will use your creative powers to invent details that would make for a compelling character and are congruent with the character's description. ";


            systemPrompt += "\r\n\r\nFor each character, you will provide a series of attribute descriptions. For each description you will provide a tag, and a block of text one to two paragraphs in length. The information you will produce for each character is as follows: \r\n\r\n";

            systemPrompt += "Tag: Name \r\nText: the name of the character provided in the user prompt. \r\n\r\n";
            systemPrompt += "Tag: Age \r\nText: age of the character in years as a numeric value.\r\n\r\n";
            systemPrompt += "Tag: BackStory \r\nText: a two-paragraph biography, starting with the character's age and then describing their life prior to the story, including education, socioeconomic background, and important life events. \r\n\r\n";
            systemPrompt += "Tag: Physical \r\nText: a paragraph providing a physical description of the character at the time of the story in the Story Text. Include descriptions of how the character dresses and how they carry themselves in the world. \r\n\r\n";
            systemPrompt += "Tag: Personality \r\nText: a paragraph describing the character's personality, including traits, core beliefs, temperament, and other relevant characteristics. \r\n\r\n";
            systemPrompt += "Tag: Speech \r\nText: a paragraph describing the style of speech of the character including their typical register of speech, formality or informality, adherence to standard grammar, degree of verbosity, vocabulary, voice tone, gestures, dialect, and other relevant characteristics of their communication style. \r\n\r\n";


            systemPrompt += $"You will create the full set of attributes for all {goodCharacters} characters. You will not truncate the list. \r\n\r\n";
            systemPrompt += $"You will return a JSON list of the attributes for each of the {goodCharacters} characters, in the following format:\r\n\r\n";

            systemPrompt += "[\r\n\r\n";
            systemPrompt += "{\r\n\r\n";

            systemPrompt += "\"Name\": \"the character's name\",";
            systemPrompt += "\"Age\": \"age of the character in years as a numeric value\",";
            systemPrompt += "\"BackStory\": \"a two-paragraph biography, starting with the character's age and then describing their life prior to the story, including education, socioeconomic background, and important life events\",";
            systemPrompt += "\"Physical\": \"a paragraph providing a physical description of the character at the time of the story in the Story Text. Include descriptions of how the character dresses and how they carry themselves in the world.\",";
            systemPrompt += "\"Personality\": \"a paragraph describing the character's personality, including traits, core beliefs, temperament, and other relevant characteristics.\",";
            systemPrompt += "\"Speech\": \"a paragraph describing the style of speech of the character including their typical register of speech, formality or informality, adherence to standard grammar, degree of verbosity, vocabulary, voice tone, gestures, dialect, and other relevant characteristics of their communication style. \r\n\r\n";
            systemPrompt += "}\r\n\r\n";
            systemPrompt += "]\r\n\r\n";

            systemPrompt += $"Your JSON string will include: Name, Age, BackStory, Physical, Personality, and Speech for every one of the {goodCharacters} characters listed in the user prompt. ";

            string userPrompt = $"Please create details as specified above for all {goodCharacters} character names below:\r\n\r\n";
            userPrompt += $"Do not truncate the list of character details. Return the entire list of {goodCharacters} character details.\r\n\r\n";
            
            string workDescription = "";
            
            foreach (CharacterObj character in myCharacters)
            {
                if (character.age.Trim().Length > 0)
                {
                    if (character.briefDescription.Trim().Length > 0) 
                    {

                        workDescription = $"Description: {character.briefDescription}";
                    }
                    else
                    {
                        workDescription = "";
                    }
                    
                    if (character.age == "10000" || character.age == "10,000")
                    {
                        userPrompt += $"Name: {character.tagName}\r\nAge: immortal\r\n{workDescription}\r\n\r\n";
                    }
                    else
                    {
                        userPrompt += $"Name: {character.tagName}\r\nAge: {character.age}\r\n{workDescription}\r\n\r\n";
                    }

                    
                }

            }

            userPrompt += "\r\n\r\nEND LIST OF CHARACTERS\r\n\r\n";
            if (year.Length > 0)
            {
                userPrompt += $"The story takes place in the year {year}. Be sure to create character attributes appropriate for that time.\r\n\r\n";
            }
            userPrompt += "Return only the JSON string.\r\n\r\n";

            string msg = "Creating Character Details From Character Descriptions";
            string response = await UtilsGPT.doGPT("CreateCharacterDetails", model, 2000, .7, userPrompt, systemPrompt, "", myForm, msg);


            return response;

        }

        public static async Task<string> enrichenMovieText(string model, int phase, int phaseTotal, string movieText, string selectedText, string movieNote, FormApp1 myForm)

        {
            string errorMsg = "";
            string systemPrompt = $" You are a talented assistant helping a screenwriter write a movie script. {Utils.getProfilePrompt(myForm, "MovieText")}We are working on the Movie Text, which is ";
            systemPrompt += "a narrative text describing the movie.\r\n\r\n";


            systemPrompt += "Your task is to rewrite a selected part of the Movie Text. ";

            systemPrompt += "You will be provided in the user prompt with a selected part of the Movie Text, which we will call the \"Selected Text\". ";
            systemPrompt += "This is the Movie Text which includes the Selected Text:\r\n\r\n";
            systemPrompt += movieText;

            systemPrompt += "In the user prompt you will be provided with the Selected Text from the Movie Text. ";
            systemPrompt += "You will also be provided with \'Notes\', which are instructions for rewriting the Selected Text. ";
            systemPrompt += "You will rewrite the Selected Text, taking into consideration the Notes.  To the degree possible, unless instructed otherwise by the \'Note\', ";
            systemPrompt += "you will retain all the details from the original version. You will use a writing style similar to that of the Movie Text";



            string userPrompt = "You will rewrite the Selected Text, taking into consideration the Notes. ";
            
            userPrompt += Utils.getAngleBracketPrompt();
            userPrompt += "Here is the Selected Text:\r\n\r\n";
            userPrompt += selectedText;
            userPrompt += "\r\n\r\nHere are the notes to use for rewriting the Selected Text:\r\n\r\n";
            userPrompt += movieNote;
            userPrompt += "\r\n\r\nPlease rewrite only the Selected Text, considering the Notes.";


            string response = await UtilsGPT.doGPT("EnrichenMovieText", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, $"phase {phase} of {phaseTotal} Enrichen Movie Text");


            return response;

        }

        public static async Task<List<SceneObj>> splitMakeScenesFromMovieText(string gptModel, string movieText, FormApp1 myForm)
        {
            var startTime = DateTime.Now;
            var response = await autoSceneCutter(gptModel, movieText, myForm, "Auto New Make Scenes: ");
            var scenes = await makeSplitScenesParallel(gptModel, response, myForm, "Auto New Make Scenes: ");

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            var statusMessage = $"Auto New Make Scenes";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return scenes;
        }

        public static async Task<string> splitMovieTextIntoScenes(string model, string movieText, string gptMessage,FormApp1 myForm)
        {
            string systemPrompt = @"You are a famous screenwriter of high talent.  

In the user prompt, you will receive a Movie Narrative, which tells a story that you will be using as the basis of a feature-length Movie Screenplay.  

You will take the Movie Narrative and without making any other changes, you will divide the Movie Narrative into Movie Scenes by inserting a new line and then '===' and then another new line as Scene boundaries at the end of each scene.  

 A Scene boundaries must be be inserted whenever a change of physical location occurs.  A Scene boundary must also be inserted when the people involved in the action change. 
You should be liberal in applying a Scene boundaries.  Too many scenes are better than too few.  More often than not, each paragraph should be its own scene.  Also where appropriate you can sinsert a Scene Boundary within a paragraph. If you are not sure, insert a Scene boundary.

It's important that you make no other changes to the text.  Do not include any extra introductory material such as 'Here is the movie screenplay with scene titles:' ";

            

            string userPrompt = movieText;

            string response = await UtilsGPT.doGPT("SplitMovieTextIntoScenesOld", model, 2000, .7, userPrompt, systemPrompt, "", myForm, gptMessage);

            return response;
        }

        public static async Task<string> splitMakeTitlesForScenes(string model, string movieText, FormApp1 myForm)
        {
            string response = "";

            string systemPrompt = "You are a talented screenwriter. "; 

            systemPrompt += "In the user prompt, you will receive a Movie Narrative, which tells a story that you will be using as the basis of a feature-length Movie Screenplay. ";

            systemPrompt += "The Movie Narrative will be divided into Scenes by Scene Boundary markers: \"===\". ";

            systemPrompt += "Provide a brief title for each Scene. "; 
            
            systemPrompt += "You will make no other changes to the Movie Narrative and will return all of the text from the Movie Narrative for each Scene. ";
            systemPrompt += "You will return your response as a JSON list of list of strings.  Each inner list will contain the title and the text of the scene.  Example:\r\n\r\n";
            systemPrompt += "[{\"title\":\"The scene title\",\"text\":\"The scene text\"},{\"title\":\"The scene title\",\"text\":\"The scene text\"},{\"title\" : \"the scene title\",\"text\" : \"The scene text\"}]\r\n\r\n";
            string userPrompt = movieText;

            response = await UtilsGPT.doGPT("SplitMovieTitleForScenesOld", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making Titles For Scenes From Movie Text");
            return response;


        
        }

        public static async Task<string> splitMakeTitlesForScenesCompressedSeed(string model, string movieSceneSeedsJSON, FormApp1 myForm)
        {
            string response = "";
            int numberScenes = Utils.countSubStringOccurances(movieSceneSeedsJSON, "\"text\"");
            string systemPrompt = "You are a talented screenwriter. ";

            systemPrompt += $"In the user prompt, you will receive a Movie Narrative divided into {numberScenes} Scenes.  The Movie Narrative will be divided into {numberScenes} Scenes in the following JSON format: ";
            systemPrompt += "[{\"text\":\"The scene text\"},{\"text\":\"The scene text\"},{\"text\" : \"The scene text\"}] repeating the objects for all " + $"{numberScenes} Scenes.\r\n";
            systemPrompt +=  
            systemPrompt += "You will provide a scene mumber and a title for each Scene. ";

            systemPrompt += $"You will also provide a brief scene summary  You will return the {numberScenes} Scene titles and scene summaries in order as they appear in the Movie Narrative. "; 
            systemPrompt += "You will return your response as a JSON array of objects.  Each object will contain the scene number, scene title, and the scene text summary for the scene.  Example:\r\n\r\n";
            systemPrompt += "[{\"scene_number\":\"the scene number\",\"title\":\"The scene title\",\"text\":\"The scene summary\"},{\"scene_number\":\"the scene number\",\"title\":\"The scene title\",\"text\":\"The scene summary\"},{\"scene_number\":\"the scene number\",\"title\" : \"the scene title\",\"text\" : \"The scene summary\"}]\r\n\r\n";
            systemPrompt += $"You will return {numberScenes} objects in the JSON array.\r\n\r\n";
            systemPrompt += "Only return the JSON array.  Do not include any other text.";
            string userPrompt = movieSceneSeedsJSON;

            response = await UtilsGPT.doGPT("MakeSceneTitlesFromMovieText", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making Titles For Scenes From Movie Text");
            return response;



        }

        public static async Task<string> splitCompressText(string model,string textTitle,FormApp1 myForm)
        {

            string systemPrompt = "You will be provided with a JSON string in the format:\r\n\r\n";
            systemPrompt += "[{\"title\":\"The scene title\",\"text\":\"The scene text\"},{\"title\":\"The scene title\",\"text\":\"The scene text\"},{\"title\" : \"the scene title\",\"text\" : \"The scene text\"}]\r\n\r\n";
            systemPrompt += "\r\n\r\nYou will return a JSON string in the same format with the text fields compressed in length but retaining all names, action, and other semantics. ";
            string userPrompt = textTitle;

            string response = await UtilsGPT.doGPT("CompressMakeScenes", model, 2000, .7, userPrompt, systemPrompt, "", myForm, "Making Scenes From Movie Text Phase 3");
            return response;
        }

        public static async Task<string> autoSceneCutter(string gptModel, string movieText, FormApp1 myForm, string statusPrefix = "")
        {
            var debugFileName = $"debug.autoSceneCutter.txt";

            // first remove all instances of === from the movie text
            movieText = movieText.Replace("===", "");

            // break movie text into list of paragaphs then break the paragraphs into a list of sentences, then combine them into a single list with snetences and paragraph breaks
            var movieTextParagraphs = movieText.Split('\n').ToList();
            var allSentences = new List<string>();
            foreach (var paragraph in movieTextParagraphs)
            {
                var sentences = paragraph.Split('.').ToList();
                // remove any 0 or single character sentences
                sentences.RemoveAll(s => s.Length <= 1);
                if (sentences.Count == 0) continue;
                // trim all
                sentences = sentences.Select(s => s.Trim()).ToList();
                // add period to ends
                sentences = sentences.Select(s => s + ".").ToList();
                // add "(NP) " in front of the first sentence
                sentences[0] = "(NP) " + sentences[0];
                // add the sentences to the list
                allSentences.AddRange(sentences);
            }

            var startTimestamp = DateTime.Now;

            string systemPrompt = Prompts.SystemPrompt_SplitScenes_ChatStyle_V2;

            var userPromptStep1 = Prompts.FillPrompt(Prompts.UserPrompt_SplitScenesStep1_ChatStyle_V1,
                new Dictionary<string, string>()
                {
                    { "sentence", allSentences[0].Replace("(NP) ", "") },
                });

            var messages = new List<ChatMessageModel>
            {
                new ChatMessageModel
                {
                   Role = "system",
                   Content = systemPrompt
                },
                new ChatMessageModel
                {
                   Role = "user",
                   Content = userPromptStep1
                }
            };

            var debugDict = new Dictionary<string, string>()
            {
                 { "SYSTEM_PROMPT", systemPrompt },
                 { "USER_PROMPT_STEP_1", userPromptStep1 },

            };
            writeDebugTxt(debugDict, debugFileName);
           
            var startTime = DateTime.Now;
            var statusMessage = $"{statusPrefix}Adding Scene Breaks '===' to Movie Text Step 1 of {allSentences.Count}";
            myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

            var step1Response = await UtilsGPT.doGPTChat("AutoSceneCutterStep1", gptModel, 0, messages, myForm);
           
            // add response to messages
            messages.Add(new ChatMessageModel
            {
                Role = "assistant",
                Content = step1Response
            });

            debugDict.Add("SENTENCE_1_RESPONSE", step1Response);
            debugDict.Add("STEP_1_TIME_ELAPSED", (DateTime.Now - startTimestamp).TotalSeconds.ToString());
            writeDebugTxt(debugDict, debugFileName);

            string workingText = allSentences[0].Trim().Replace("(NP) ", "");

            // now loop through the rest of the sentences
            for (int i = 1; i < allSentences.Count; i++)
            {
                statusMessage = $"{statusPrefix}Adding Scene Breaks '===' to Movie Text Step {i + 1} of {allSentences.Count}";
                var now = DateTime.Now;
                var elapsed = Math.Round((now - startTime).TotalSeconds);
                myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss") + " (elapsed: " + elapsed + "s)", Color.Red);

                debugDict.Add($"SENTENCE_{i}", allSentences[i].Replace("(NP) ", ""));
                writeDebugTxt(debugDict, debugFileName);

                // add user step 2
                messages.Add(new ChatMessageModel
                {
                    Role = "user",
                    Content = allSentences[i].Replace("(NP) ", "")
                });

                var step3Response = await UtilsGPT.doGPTChat("AutoSceneCutterStep2", gptModel, 0, messages, myForm);

                if (step3Response.Contains("#ErrorCountExceeded")) return "#ErrorCountExceeded";

                if (step3Response.ToLower().Contains("same scene"))
                {
                    workingText += " " + allSentences[i].Trim().Replace("(NP) ", "\r\n\r\n");
                } 
                else if (step3Response.ToLower().Contains("new scene"))
                {
                    workingText += "\r\n\r\n===\r\n\r\n" + allSentences[i].Trim().Replace("(NP) ", "\r\n\r\n");
                }
                else
                {
                    // treat as same scene
                    workingText += " " + allSentences[i].Trim().Replace("(NP) ", "\r\n\r\n");
                }

                // add response to messages
                messages.Add(new ChatMessageModel
                {
                    Role = "assistant",
                    Content = step3Response
                });

                debugDict.Add($"SENTENCE_{i + 1}_RESPONSE", step3Response);
                debugDict.Add($"SENTENCE_{i + 1}_TIME_ELAPSED", (DateTime.Now - startTimestamp).TotalSeconds.ToString());
                writeDebugTxt(debugDict, debugFileName);

            }

            debugDict.Add($"TOTAL_TIME_ELAPSED", (DateTime.Now - startTimestamp).TotalSeconds.ToString());
            debugDict.Add($"ALL_MESSAGES", messages.ToString());
            writeDebugTxt(debugDict, debugFileName);

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            statusMessage = $"{statusPrefix}Adding Scene Breaks '===' to Movie Text";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return workingText.Replace("\r\n\r\n\r\n", "\r\n\r\n").Replace("\r\n\r\n\r\n", "\r\n\r\n");
        }

        public static async Task<string> findCharacterNamesInText(string gptModel, string sectionText, FormApp1 myForm)
        {
            // handle === lines
            if (sectionText.Trim() == "===")
                return "NONE";
            string userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_AngleBracketCharactersNewApproach_V1,
                new Dictionary<string, string>()
                {
                    { "text", sectionText },
                });
            string response = await UtilsGPT.doGPT("FindCharacterNames", gptModel, 4000, 0, userPrompt, Prompts.SystemPrompt_AngleBracketCharactersNewApproach_V1, "", myForm, "");

            return response;
        }

        public static async Task<string> parallelAngleBracketsCharacterText(string gptModel, string text, FormApp1 myForm)
        {
            var movieTextSections = chunkMovieText(text, 3000);

            var startTime = DateTime.Now;
            var statusMessage = $"Enclosing Character Names in Angle Brackets <>";
            myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

            // run calls in parallel with max concurrency of 20
            var tasks = new List<Task<string>>();
            var tasksToWait = new List<Task>();
            int maxConcurrency = 20; // Maximum number of concurrent tasks
            int tasksCompleted = 0;
            foreach (var section in movieTextSections)
            {
                // Make sure we only have maxConcurrency tasks running at a time
                if (tasksToWait.Count >= maxConcurrency)
                {
                    var completedTask = await Task.WhenAny(tasksToWait);
                    tasksToWait.Remove(completedTask);
                }

                var task = findCharacterNamesInText(gptModel, section, myForm);
                _ = task.ContinueWith(t =>
                {
                    // This part runs on the completion of each task
                    tasksCompleted++;
                    var elapsed = Math.Round((DateTime.Now - startTime).TotalSeconds);
                    statusMessage = $"Enclosing Character Names in Angle Brackets <>: {tasksCompleted} of {movieTextSections.Count} completed";
                    myForm.updateGPTStatusFromTask("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss") + " (elapsed: " + elapsed + "s)", Color.Red);
                });

                tasks.Add(task);
                tasksToWait.Add(task);
            }

            var responses = await Task.WhenAll(tasks);

            var joinedResponses = string.Join(",", responses);

            if (joinedResponses.Contains("#ErrorCountExceeded")) return "#ErrorCountExceeded";

            // split the response on , and discard empty entries
            var characterNames = joinedResponses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // trim the character names, remove the word NONE from the list, and remove any duplicates
            characterNames = characterNames.Select(c => c.Trim()).Where(c => c != "NONE").Distinct().ToArray();

            // loop through the chartacter names and enclose them in angle brackets in the text
            foreach (var characterName in characterNames)
            {
                var characterNameTrimmed = characterName.Trim();
                if (characterNameTrimmed.Length == 0) continue;
                var characterNameWithAngleBrackets = "<" + characterNameTrimmed + ">";
                // first replace any instances already in brackets so we don't double up
                text = text.Replace(characterNameWithAngleBrackets, characterNameTrimmed);
                // use regex to replace characterNameTrimmed with characterNameWithAngleBrackets where characterNameTrimmed is surrounded by start of string, end of string, whitespace or punctuation
                text = Regex.Replace(text, @"(^|\s|\p{P})" + characterNameTrimmed + @"($|\s|\p{P})", "$1" + characterNameWithAngleBrackets + "$2");
            }

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            statusMessage = $"Enclosing Character Names in Angle Brackets <>";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);
            // hack to fix not showing last message after tasks
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return text;
        }

        /// <summary>
        /// writes toa debug file in the project root, only works in debug mode
        /// </summary>
        /// <param name="textList">Dictionary - key: section header, value: section text</param>
        /// <param name="filename">name of file to be created at project root, toggle show all files in solution explorer to see file</param>
        public static void writeDebugTxt(Dictionary<string, string> textList, string filename)
        {
            #if DEBUG
            string filepath = filename;
            if (Directory.Exists("../../"))
            {
                filepath = "../../" + filename;
            }

            using (StreamWriter outputFile = new StreamWriter(filepath, false))
            {
                foreach (string key in textList.Keys)
                {
                    outputFile.WriteLine(key + ": \n" + textList[key]);
                    outputFile.WriteLine("--------------------------------------------------\n");
                }
            }
            #endif
        }

        /// <summary>
        /// splits a text on the splitLength, returns a list of sections
        /// - respects === lines and splits on those and adds them to the list
        /// - splits on paragraph breaks near the splitLength
        /// </summary>
        /// <param name="movieText"></param>
        /// <param name="splitLength"></param>
        /// <returns>list of string chunks of movieText</returns>
        public static List<string> chunkMovieText(string movieText, int splitLength = 3000)
        {
            // split on === lines and break apart sections larger than splitLength (if there are no === lines, this will just split on splitLength)
            var sections = new List<string>(movieText.Split(new string[] { "===" }, StringSplitOptions.RemoveEmptyEntries));
            // add a === line to the list after every section and split the section if larger than splitLength
            var sectionsAndBreaks = new List<string>();
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].Length > splitLength)
                {
                    var splitSections = splitTextWithRoughLengthMaintainingParagraphs(sections[i], splitLength);
                    sectionsAndBreaks.AddRange(splitSections);
                } else
                {
                    sectionsAndBreaks.Add(sections[i]);
                }
                if (i < sections.Count - 1)
                    sectionsAndBreaks.Add("===");
            }
            // remove any entries that are empty strings
            sectionsAndBreaks.RemoveAll(s => s.Length == 0);
            return sectionsAndBreaks;
        }

        private static List<string> splitTextWithRoughLengthMaintainingParagraphs(string text, int splitLength)
        {
            List<string> sections = new List<string>();

            // split movieText into paragraphs
            List<string> paragraphs = new List<string>(text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));

            // sections should be around 3000 characters, add paragraphs until we hit that limit
            var section = "";
            foreach (var paragraph in paragraphs)
            {
                if (section.Length + paragraph.Length > splitLength)
                {
                    sections.Add(section);
                    section = "";
                }
                section += paragraph + "\r\n\r\n";
            }

            // remove the trailing newlines from the last section
            section = section.TrimEnd(new char[] { '\r', '\n' });

            // add the last section
            sections.Add(section);

            return sections;
        }

        /// <summary>
        /// uses the create section plan approach to apply notes to movie text
        /// </summary>
        /// <param name="gptModel"></param>
        /// <param name="movieText"></param>
        /// <param name="movieSeed"></param>
        /// <param name="textNote"></param>
        /// <param name="myForm"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<(string, string)> applyNotesForMovieText_PlanStyle(string gptModel, string movieText, string movieSeed, string textNote, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            var startTimestamp = DateTime.Now;
            var debugFile = "debug.applyNotesForMovieText_PlanStyle.txt";

            // split the movie text into sections
            var movieTextSplits = chunkMovieText(movieText, 3000);
            // join the sections with a sections header, e.g. SECTION 1, SECTION 2, etc.
            // also factor out the === sections and save their locations for putting back in
            var movieTextSectionsWithHeaders = new List<string>();
            var movieTextSectionsWithoutHeaders = new List<string>();
            var sceneBreakAfterSectionIndexes = new List<int>();
            var sectionCtr = 1;
            for (int i = 0; i < movieTextSplits.Count; i++)
            {
                // skip the === lines
                if (movieTextSplits[i].Trim() == "===") {
                    sceneBreakAfterSectionIndexes.Add(sectionCtr - 1);
                    continue;
                }
                movieTextSectionsWithoutHeaders.Add(movieTextSplits[i].Trim());
                movieTextSectionsWithHeaders.Add($"SECTION {sectionCtr++}\r\n\r\n{movieTextSplits[i].Trim()}");
            }
            var completeMovieTextWithSections = string.Join("\r\n\r\n", movieTextSectionsWithHeaders);

            // get the movie type from the form
            string movieType = Utils.getProfilePrompt(myForm, "SceneText");
            if (string.IsNullOrEmpty(movieType))
                movieType = "Unspecified";

            // create the step 1 system prompt
            var systemPromptStep1 = Prompts.FillPrompt(Prompts.SystemPrompt_ApplyNotesStep1_PlanStyle_V1,
                new Dictionary<string, string>()
                {
                    { "movieType", movieType },
                    { "movieSeed", movieSeed },
                    { "completeMovieNarrative", completeMovieTextWithSections },
                });

            // create the step 2 user prompt
            var userPromptStep1 = Prompts.FillPrompt(Prompts.UserPrompt_ApplyNotesStep1_PlanStyle_V1,
                new Dictionary<string, string>()
                {
                    { "notes", textNote },
                });

            // write debug file
            var debugDict = new Dictionary<string, string>()
            {
                 { "SYSTEM_PROMPT_STEP_1", systemPromptStep1 }, 
                 { "USER_PROMPT_STEP_1", userPromptStep1 },

            };
            writeDebugTxt(debugDict, debugFile);

            var startTime = DateTime.Now;
            var statusMessage = $"Applying Notes to Movie Text : Generating Plan";
            myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

            // generate the rewrite plan
            string jsonResponse = await UtilsGPT.doGPT("ApplyNotesMovieTextGeneratePlan", gptModel, 4000, .03, systemPromptStep1, userPromptStep1, null, myForm, "", true);

            // clean the json response
            jsonResponse = Utils.TrimOutsideCurlyBrackets(jsonResponse);

            // update debug file
            string prettyPlanJSON = "";
            try
            {
                prettyPlanJSON = JToken.Parse(jsonResponse).ToString(Formatting.Indented);
            }
            catch (Exception e)
            {
                return ("#ErrorCountExceeded", "#ErrorCountExceeded");
            }
            debugDict.Add($"REWRITE_PLAN", prettyPlanJSON);
            writeDebugTxt(debugDict, debugFile);

            // parse the json response
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
            var sections = jsonObject.GetValue("sections");
            if (sections == null)
                throw new Exception("No sections found in JSON response.");

            // loop through the sections
            var workingRewrite = "";
            sectionCtr = 1;
            foreach (var section in sections)
            {
                var elapsed = Math.Round((DateTime.Now - startTime).TotalSeconds);
                statusMessage = $"Applying Notes to Movie Text : Applying to Section {sectionCtr++} of {sections.Count}";
                myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss") + " (elapsed: " + elapsed + "s)", Color.Red);

                // get the section number
                var sectionNumber = section.GetValue("section_number").ToString();
                if (sectionNumber == null)
                    throw new Exception("No section number found in JSON response.");
                var sectionNumberInt = int.Parse(sectionNumber);

                // get the has section changes
                var sectionHasChanges = bool.Parse(section.GetValue("section_has_changes").ToString());

                // if no changes, just add the section to the working rewrite
                if (!sectionHasChanges)
                {
                    workingRewrite += movieTextSectionsWithoutHeaders[sectionNumberInt - 1] + "\r\n\r\n";
                    if (sceneBreakAfterSectionIndexes.Contains(sectionNumberInt))
                        workingRewrite += "===\r\n\r\n";
                    debugDict.Add($"SECTION_{sectionNumberInt}_PLAN", "NO CHANGES");
                    writeDebugTxt(debugDict, debugFile);
                    continue;
                }

                // get the section changes
                var sectionChanges = section.GetValue("changes");
                if (sectionChanges == null)
                    throw new Exception("No section changes found in JSON response.");

                // update debug file
                var prettySectionChanges = JToken.Parse(sectionChanges.ToString()).ToString(Formatting.Indented);
                debugDict.Add($"SECTION_{sectionNumberInt}_PLAN", prettySectionChanges);
                writeDebugTxt(debugDict, debugFile);

                // set movieTextStyleText to the movie text style
                string movieTextStyleText = "";
                if (movieAndSceneTextStyle.label != "-none-")
                {
                    movieTextStyleText = "\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE\r\n\r\n";
                }

                // create the step 2 system prompt
                var systemPromptStep2 = Prompts.FillPrompt(Prompts.SystemPrompt_ApplyNotesStep2_PlanStyle_V2,
                    new Dictionary<string, string>()
                    {
                        { "movieAndSceneTextStyle", movieTextStyleText }
                    });

                // summarize the movie text up to this section
                string movieTextSummaryUpToThisSection = "This is the first section of the narrative, there is no summary of the narrative up to this point.";
                if (sectionNumberInt > 1)
                {
                    var movieTextSectionsUpToThisSection = "";
                    for (int i = 0; i < sectionNumberInt - 1; i++)
                        movieTextSectionsUpToThisSection += movieTextSectionsWithoutHeaders[i] + "\r\n\r\n";
                    // use summarize V3 to summarize preceding sections
                    string userPrompt1 = Prompts.FillPrompt(Prompts.UserPrompt_SummarizeMovieText_V3, new Dictionary<string, string>()
                    {
                        { "movieText", movieTextSectionsUpToThisSection },
                    });
                    movieTextSummaryUpToThisSection = await UtilsGPT.doGPT("ApplyNotesMovieTextSummarize", gptModel, 4000, 0, userPrompt1, Prompts.SystemPrompt_SummarizeMovieText_V3, null, myForm, "");
                }

                if (movieTextSummaryUpToThisSection.Contains("#ErrorCountExceeded")) return ("#ErrorCountExceeded", "#ErrorCountExceeded");

                // update debug file
                debugDict.Add($"SECTION_{sectionNumberInt}_SUMMARY", movieTextSummaryUpToThisSection);
                writeDebugTxt(debugDict, debugFile);

                // create the step 2 user prompt
                var userPromptStep2 = Prompts.FillPrompt(Prompts.UserPrompt_ApplyNotesStep2_PlanStyle_V2,
                    new Dictionary<string, string>()
                    {
                        { "summary", movieTextSummaryUpToThisSection },
                        { "originalDraft", movieTextSectionsWithoutHeaders[sectionNumberInt - 1] },
                        { "plan", prettySectionChanges },
                    });

                // create the step 2 rewrite for this section
                var newDraft = await UtilsGPT.doGPT("ApplyNotesMovieTextRewriteSection", gptModel, 4000, .03, systemPromptStep2, userPromptStep2, null, myForm, "");

                if (newDraft.Contains("#ErrorCountExceeded")) return ("#ErrorCountExceeded", "#ErrorCountExceeded");

                // update working rewrite with the new draft
                workingRewrite += newDraft + "\r\n\r\n";

                if (sceneBreakAfterSectionIndexes.Contains(sectionNumberInt))
                    workingRewrite += "===\r\n\r\n";

                // update the movie text sections with the new draft
                movieTextSectionsWithoutHeaders[sectionNumberInt - 1] = newDraft;

                // update debug file
                debugDict.Add($"SECTION_{sectionNumberInt}_NEW_DRAFT", newDraft);
                debugDict.Add($"STEP_{sectionNumberInt}_TIME_ELAPSED", (DateTime.Now - startTimestamp).TotalSeconds.ToString());
                writeDebugTxt(debugDict, debugFile);
            }

            // put a \r in front of any \n that doesn't aleady have one using regex
            workingRewrite = Regex.Replace(workingRewrite, @"(?<!\r)\n", "\r\n");

            // replace all triple news lines with double new lines recursively until none left
            while (workingRewrite.Contains("\r\n\r\n\r\n"))
                workingRewrite = workingRewrite.Replace("\r\n\r\n\r\n", "\r\n\r\n");

            // remove any trailing \r\n
            workingRewrite = workingRewrite.TrimEnd(new char[] { '\r', '\n' });

            // update debug file
            debugDict.Add($"TOTAL_TIME_ELAPSED", (DateTime.Now - startTimestamp).TotalSeconds.ToString());
            writeDebugTxt(debugDict, debugFile);

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            statusMessage = $"Applying Notes to Movie Text";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return (workingRewrite, prettyPlanJSON);
        }

        /// <summary>
        /// version 3 summarize movie text prompt
        /// </summary>
        /// <param name="workingMovieText"></param>
        /// <param name="myForm"></param>
        /// <param name="statusMessage"></param>
        /// <returns></returns>
        public static async Task<string> summarizeMovieText_V3(string gptModel, string workingMovieText, FormApp1 myForm, string statusMessage = "summarizing movie text")
        {
            string userPrompt1 = Prompts.FillPrompt(Prompts.UserPrompt_SummarizeMovieText_V3, new Dictionary<string, string>()
                {
                    { "movieText", workingMovieText },
                });
            string workingMovieTextSummary = await UtilsGPT.doGPT("SummarizeMovieText", gptModel, 4000, 0, userPrompt1, Prompts.SystemPrompt_SummarizeMovieText_V3, null, myForm, statusMessage);
            return workingMovieTextSummary;
        }

        /// <summary>
        /// run a few analyses on the original text, the new text, and the notes, and return a score
        /// </summary>
        /// <param name="originalText"></param>
        /// <param name="notes"></param>
        /// <param name="newText"></param>
        /// <param name="myForm"></param>
        /// <returns></returns>
        public static async Task<string> analyzeAppliedNotes(string rewritePlan, string originalText, string notes, string newText, FormApp1 myForm)
        {
            string gpt35 = "or/openai/gpt-3.5-turbo-instruct";
            string gpt4 = "or/openai/gpt-4o";

            string analysis = $"Original text length: {originalText.Length}\r\nNew text length: {newText.Length}\r\nNotes: {notes}\r\n\r\n";
            analysis += "\r\n===================================================\r\n";

            // step 1 summarize the original text
            string summarizedOriginal = await summarizeMovieText_V3(gpt4, originalText, myForm, "summarizing original text");
            analysis += "Original Text Summary:\r\n\r\n";
            analysis += summarizedOriginal;
            analysis += "\r\n===================================================\r\n";

            // step 2 summarize the new text
            string summarizedNewText = await summarizeMovieText_V3(gpt4, newText, myForm, "summarizing new text");
            analysis += "New Text Summary:\r\n\r\n";
            analysis += summarizedNewText;
            analysis += "\r\n===================================================\r\n";

            // step 2.5 add the rewrite plan
            analysis += "Rewrite Plan:\r\n\r\n";
            analysis += rewritePlan;
            analysis += "\r\n===================================================\r\n";

            // step 3 compare the two summaries, given the notes, and return a score
            var systemPrompt = Prompts.SystemPrompt_CompareMovieTexts_V1;
            var userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_CompareMovieTexts_V1, new Dictionary<string, string>()
            {
                { "originalText", originalText },
                { "newText", newText },
                { "notes", notes }
            });
            string compareResults = await UtilsGPT.doGPT("CompareMovieTexts", gpt4, 4000, 0, userPrompt, systemPrompt, null, myForm, "comparing new to original");
            analysis += "Comparison Results:\r\n\r\n";
            analysis += compareResults;
            analysis += "\r\n===================================================\r\n";

            // step 3.5 compare the two summaries, see if anything was lost from the original
            systemPrompt = Prompts.SystemPrompt_CompareMovieTextsForLoss_V1;
            userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_CompareMovieTextsForLoss_V1, new Dictionary<string, string>()
            {
                { "originalText", originalText },
                { "newText", newText },
            });
            string compareLossResults = await UtilsGPT.doGPT("CompareMovieTexts", gpt4, 4000, 0, userPrompt, systemPrompt, null, myForm, "comparing new to original");
            analysis += "Lost Points Results:\r\n\r\n";
            analysis += compareLossResults;
            analysis += "\r\n===================================================\r\n";

            // step 4 provide a coherency score for the original text
            systemPrompt = Prompts.SystemPrompt_CoherencyScore_V1;
            userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_CoherencyScore_V1, new Dictionary<string, string>()
            {
                { "movieText", originalText },
            });
            string originalCoherencyScore = await UtilsGPT.doGPT("ScoreCoherency", gpt4, 4000, 0, userPrompt, systemPrompt, null, myForm, "scoring original coherency");
            analysis += "Original Coherency Score:\r\n\r\n";
            analysis += originalCoherencyScore;
            analysis += "\r\n===================================================\r\n";

            // step 5 provide a coherency score for the new text
            systemPrompt = Prompts.SystemPrompt_CoherencyScore_V1;
            userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_CoherencyScore_V1, new Dictionary<string, string>()
            {
                { "movieText", newText },
            });
            string newCoherencyScore = await UtilsGPT.doGPT("ScoreCoherency", gpt4, 4000, 0, userPrompt, systemPrompt, null, myForm, "scoring new coherency");
            analysis += "New Coherency Score:\r\n\r\n";
            analysis += newCoherencyScore;
            analysis += "\r\n===================================================\r\n";

            writeDebugTxt(new Dictionary<string, string>() {
                    { "ORIGINAL VERSION", originalText },
                    { "NOTES", notes },
                    { "NEW VERSION", newText },
                    { "ANALYSIS", analysis },
                }, "debug.analyzeAppliedNotes.txt");

            return analysis;
        }

        public async static Task<List<SceneObj>> makeSplitScenesParallel(string gptModel, string movieText, FormApp1 myForm, string statusPrefix = "")
        {
            var startTime = DateTime.Now;
            var statusMessage = $"{statusPrefix}Make Scenes ===";
            myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

            string[] longSceneSeeds = Utils.splitMovieTextAtEquals(movieText);

            // trim long seeds
            longSceneSeeds = longSceneSeeds.Select(s => s.Trim()).ToArray();

            // run calls in parallel with max concurrency of 20
            var tasks = new List<Task<SceneObj>>();
            var tasksToWait = new List<Task>();
            int maxConcurrency = 20; // Maximum number of concurrent tasks
            int tasksCompleted = 0;
            foreach (var longSeedScene in longSceneSeeds)
            {
                // Make sure we only have maxConcurrency tasks running at a time
                if (tasksToWait.Count >= maxConcurrency)
                {
                    var completedTask = await Task.WhenAny(tasksToWait);
                    tasksToWait.Remove(completedTask);
                }

                var task = makeSceneFromLongSeedText(gptModel, movieText, longSeedScene, myForm);
                _ = task.ContinueWith(t =>
                {
                    tasksCompleted++;
                    var elapsed = Math.Round((DateTime.Now - startTime).TotalSeconds);
                    statusMessage = $"{statusPrefix}Make Scenes ===: {tasksCompleted} of {longSceneSeeds.Length} completed";
                    myForm.updateGPTStatusFromTask("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss") + " (elapsed: " + elapsed + "s)", Color.Red);
                });

                tasks.Add(task);
                tasksToWait.Add(task);
            }

            var scenes = await Task.WhenAll(tasks);

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            statusMessage = $"{statusPrefix}Make Scenes ===";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);
            // hack to fix not showing last message after tasks
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return scenes.ToList();
        }

        private async static Task<SceneObj> makeSceneFromLongSeedText(string gptModel, string movieText, string longSeedScene, FormApp1 myForm)
        {
            string userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_MakeSceneFromLongSeed_V1, new Dictionary<string, string>()
            {
                { "longSeedScene", longSeedScene },
            });
            string jsonResponse = await UtilsGPT.doGPT("MakeSceneFromLongSeedText", gptModel, 4000, 0, userPrompt, Prompts.SystemPrompt_MakeSceneFromLongSeed_V1, "", myForm, "");

            if (jsonResponse == "#ErrorCountExceeded")
            {
               throw new Exception("#ErrorCountExceeded");
            }

            // clean the json response
            jsonResponse = Utils.TrimOutsideCurlyBrackets(jsonResponse);

            // parse the json response
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);

            if (jsonObject == null)
            {
                return new SceneObj()
                {
                    splitSceneMakeFlag = true,
                    compressedHint = longSeedScene,
                    Hint = longSeedScene,
                    Title = "Make Scene Failed",
                };
            }

            var sceneText = jsonObject.GetValue("scene_summary").ToString();
            var sceneTitle = jsonObject.GetValue("scene_title").ToString();

            var scene = new SceneObj()
            {
                splitSceneMakeFlag = true,
                compressedHint = sceneText,
                Hint = longSeedScene,
                Title = sceneTitle,
            };

            return scene;
        }


        public static async Task<string> makeSplitSceneTextWithCharacterGuidance(string model, MovieObj myMovie, List<SceneObj> sceneList, List<CharacterObj> characters, int sceneNum, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {

            var movieTextStylePrompt = "";
            if (movieAndSceneTextStyle.label != "-none-")
            {
                movieTextStylePrompt = "\r\n\r\nHere is the style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE";
            }

            string systemPrompt = Prompts.FillPrompt(Prompts.SystemPrompt_MakeSplitSceneTextWithCharacterGuidance_V1, new Dictionary<string, string>()
            {
                { "movieGuidance", Utils.getProfilePrompt(myForm, "SceneText") },
                { "movieTextStylePrompt", movieTextStylePrompt }
            });

            string userPrompt = "";
            string sceneCharacters = "";
            string introduceCharacters = "";
            string previousScenesSummary = "";
            string nextScenesSummary = "";

            // get everything form the previous scenes
            int sceneCounter = 1;
            HashSet<string> charactersInPreviousScenes = new HashSet<string>();
            while (sceneCounter < sceneNum)
            {
                var sceneText = sceneList[sceneCounter - 1].NarrativeText;
                if (sceneText.Trim().Length < 50)
                    sceneText = sceneList[sceneCounter - 1].Hint;
                var sceneCharactersList = Utils.ExtractTextBetweenAngleBrackets(sceneText);
                foreach (var character in sceneCharactersList)
                    charactersInPreviousScenes.Add(character);
                previousScenesSummary += "SCENE " + sceneCounter + "\r\n\r\n" + sceneText + "\r\n\r\n";
                sceneCounter++;
            }
            previousScenesSummary = previousScenesSummary.Trim();

            // get the next scenes summary
            sceneCounter = sceneNum + 1;
            while (sceneCounter < sceneList.Count)
            {
                var sceneText = sceneList[sceneCounter - 1].NarrativeText;
                if (sceneText.Trim().Length < 50)
                    sceneText = sceneList[sceneCounter - 1].Hint;
                nextScenesSummary += "SCENE " + sceneCounter + "\r\n\r\n" + sceneText + "\r\n\r\n";
                sceneCounter++;
            }

            // get the characters to introduce for this scenes
            var thisSceneCharactersList = Utils.ExtractTextBetweenAngleBrackets(sceneList[sceneNum - 1].Hint);
            foreach (var character in thisSceneCharactersList)
            {
                if (!charactersInPreviousScenes.Contains(character))
                    introduceCharacters += character + ", ";
            }
            // trim trailing ,
            introduceCharacters = introduceCharacters.TrimEnd(new char[] { ',', ' ' });
            if (introduceCharacters.Length > 0)
                introduceCharacters = $"\r\nWhen {introduceCharacters} first appear in the scene, introduce them with their age, physical characteristics, and personality.\r\n";

            // build the scene characters string
            foreach (CharacterObj character in characters)
            {
                if (!thisSceneCharactersList.Contains(character.tagName))
                    continue;

                sceneCharacters += $"\r\n\r\nCharacter Name: <{character.tagName}>\r\n\r\n";

                if (character.age.Trim() == "10000")
                {
                    sceneCharacters += "Age: Immortal\r\n\r\n";
                }
                else
                {
                    sceneCharacters += $"Age: {character.age}\r\n\r\n";
                }

                sceneCharacters += $"Back Story: {character.backStory}\r\n\r\n";
                sceneCharacters += $"Physical Description: {character.physicalDescription}\r\n\r\n";
                sceneCharacters += $"Personality: {character.personality}\r\n\r\n";
                // sceneCharacters += $"Style of Speaking: {character.speechStyle}\r\n\r\n";

                sceneCharacters += $"END OF CHARACTER ATTRIBUTES FOR <{character.tagName}>";
            }

            if (sceneNum == 1)
            {
                userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_MakeSplitSceneTextWithCharacterGuidanceFirstScene_V1, new Dictionary<string, string>()
                {
                    { "sceneHint", sceneList[sceneNum - 1].Hint },
                    { "sceneCharacters", sceneCharacters },
                    { "introduceCharacters", introduceCharacters },
                    { "nextScenesSummary", nextScenesSummary },
                });
            }

            else if ((sceneNum > 1) && (sceneNum != sceneList.Count))
            {
                userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_MakeSplitSceneTextWithCharacterGuidanceMiddleScene_V1, new Dictionary<string, string>()
                {
                    { "sceneHint", sceneList[sceneNum - 1].Hint },
                    { "sceneCharacters", sceneCharacters },
                    { "introduceCharacters", introduceCharacters },
                    { "previousScenesSummary", previousScenesSummary },
                    { "nextScenesSummary", nextScenesSummary },
                    { "sceneNumber", sceneNum.ToString() },
                });
            }

            else if ((sceneNum == sceneList.Count))
            {
                userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_MakeSplitSceneTextWithCharacterGuidanceLastScene_V1, new Dictionary<string, string>()
                {
                    { "sceneHint", sceneList[sceneNum - 1].Hint },
                    { "sceneCharacters", sceneCharacters },
                    { "introduceCharacters", introduceCharacters },
                    { "previousScenesSummary", previousScenesSummary },
                    { "sceneNumber", sceneNum.ToString() },
                });
            }

            // TODO: increase temp
            string response = await UtilsGPT.doGPT("MakeSplitSceneTextWithCharacters", model, 4000, .05, userPrompt, systemPrompt, "", myForm, "making Scene Text");

            return response;
        }
    
    
    
        public static async Task<string> expandHere(string model, string markedMovieText, int cursorLocation, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            string errorMsg = "";
            string response = "";
            string systemPrompt = "";
            string userPrompt = "";

            systemPrompt += $@"You are a visionary screenwriter who specializes in writing compelling and novel narrative turns.{Utils.getProfilePrompt(myForm, "SceneText")}

You will receive a MOVIE NARRATIVE, a detailed synopsis of a movie. In the Movie Narrative, a specific location will be marked with the symbol “XXX”. 

You will write an expansion of the plot designed to be inserted at the XXX location. The expansion text may be as short or as long as you feel the context warrants. The added material should enrich and deepen the plot while retaining continuity between the scenes preceding and following the insertion point. It should not merely elaborate on what already exists, but should add something new that nevertheless retains the integrity of the story. 

Always write in the present tense.";

            if (movieAndSceneTextStyle.label != "-none-")
            {
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE\r\n\r\n";
            }

            systemPrompt += @"Please DO NOT include any disclaimers, e.g. \""Here is the rewritten narrative:\"". ";

            userPrompt += $@"Here is the full Movie Narrative, including the XXX marker which marks the insertion point of the text you will write:

{markedMovieText}

END MOVIE NARRATIVE

Begin now with the first sentence of the expansion text, remembering that it must fit precisely where the XXX is in the MOVIE NARRATIVE above. ";


            response = await UtilsGPT.doGPT("ExpandAtSelection", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, $"expanding at selection");

            return response;
        }


        public static async Task<string> expandHereWithNote(string model, string markedMovieText, string movieTextNote, int cursorLocation, StyleElements movieAndSceneTextStyle, FormApp1 myForm)
        {
            string errorMsg = "";
            string response = "";
            string systemPrompt = "";
            string userPrompt = "";

            systemPrompt += $@"You are a visionary screenwriter who specializes in writing compelling and novel narrative turns.{Utils.getProfilePrompt(myForm, "SceneText")}

You will receive a MOVIE NARRATIVE, a detailed synopsis of a movie. In the Movie Narrative, a specific location will be marked with the symbol “XXX”.

You will also receive a NOTE containing instructions or clarifications.

You will write an expansion of the plot designed to be inserted at the XXX location, taking into account the NOTE. 

The expansion text may be as short or as long as you feel the context warrants. The added material should enrich and deepen the plot while retaining continuity between the scenes preceding and following the insertion point. It should not merely elaborate on what already exists, but should add something new that nevertheless retains the integrity of the story. 

Always write in the present tense.";

            if (movieAndSceneTextStyle.label != "-none-")
            {{
                systemPrompt += "\r\n\r\nUse the following style guide to use for writing the text:\r\n\r\n" + movieAndSceneTextStyle.style + "\r\n\r\nEND STYLE GUIDE\r\n\r\n";
            }}

            systemPrompt += @"Please DO NOT include any disclaimers, e.g. \""Here is the rewritten narrative:\"". ";

            userPrompt += $@"Here is the full MOVIE NARRATIVE, including the XXX marker which marks the insertion point of the text you will write:

{markedMovieText}

END MOVIE NARRATIVE

Here is the NOTE: 

{movieTextNote}

END NOTE

Begin now with the first sentence of the expansion text, taking into account the NOTE, and remembering that the expansion text must fit precisely where the XXX is in the MOVIE NARRATIVE above. ";


            response = await UtilsGPT.doGPT("ExpandAtSelectionWithNote", model, 2000, .7, userPrompt, systemPrompt, errorMsg, myForm, $"expanding at selection");

            return response;
        }

        public static async Task<string> generateOutlineOldStyle(string gptModel, string originalMovieText, FormApp1 formApp1, string statusText)
        {
            var systemPrompt = Prompts.FillPrompt(Prompts.SystemPrompt_GenerateOutlineOldStyle_V1, new Dictionary<string, string>()
            {
                { "profilePrompt", Utils.getProfilePrompt(formApp1, "MovieText") },
            });

            var userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_GenerateOutlineOldStyle_V1, new Dictionary<string, string>()
            {
                { "originalMovieText", originalMovieText },
            });

            var response = await UtilsGPT.doGPT("GenerateOutlineOldStyle", gptModel, 4000, 0, userPrompt, systemPrompt, "", formApp1, statusText);
            return response;

        }

        public static async Task<string> copyEditParallel(string gptModel, string style, string movieText, FormApp1 myForm)
        {
            var movieTextSections = chunkMovieText(movieText, 1000);
            string prettyStyleName;
            string note;
            if (style == "enrichen")
            {
                prettyStyleName = "Enrichen";
                note = "Add more detail.  Make move vivid.   Do not add any commentary or introduction.  Only provide the rewritten text.";
            } 
            else if (style== "depurple")
            {
                prettyStyleName = "Depurple";
                note = "Please simplify.  Divide long and complex sentences into multiple shorter and simpler sentences.    Reduce the number of adjectives and adverbs.   Use forms of the verb \"to be\" sparingly.  In general, apply the principles of Strunk and White.";
            } 
            else
            {
                return "#ErrorCountExceeded";
            }

            var startTime = DateTime.Now;
            var statusMessage = $"Copy Edit - {prettyStyleName}";
            myForm.updateGPTStatus("GPT Status: " + statusMessage + " using " + gptModel + " at " + startTime.ToString("HH:mm:ss"), Color.Red);

            // run calls in parallel with max concurrency of 20
            var tasks = new List<Task<string>>();
            var tasksToWait = new List<Task>();
            int maxConcurrency = 20; // Maximum number of concurrent tasks
            int tasksCompleted = 0;
            foreach (var section in movieTextSections)
            {
                // Make sure we only have maxConcurrency tasks running at a time
                if (tasksToWait.Count >= maxConcurrency)
                {
                    var completedTask = await Task.WhenAny(tasksToWait);
                    tasksToWait.Remove(completedTask);
                }

                var task = runCopyEditStep(gptModel, section, note, myForm);
                _ = task.ContinueWith(t =>
                {
                    // This part runs on the completion of each task
                    tasksCompleted++;
                    var elapsed = Math.Round((DateTime.Now - startTime).TotalSeconds);
                    statusMessage = $"Copy Edit - {prettyStyleName}: {tasksCompleted} of {movieTextSections.Count} completed";
                    myForm.updateGPTStatusFromTask($"GPT Status: {statusMessage} using {gptModel} at {startTime.ToString("HH:mm:ss")} (elapsed: {elapsed}s)", Color.Red);
                });

                tasks.Add(task);
                tasksToWait.Add(task);
            }

            var responses = await Task.WhenAll(tasks);

            var joinedResponses = string.Join("\n\n", responses);

            // recursively replace triple new lines with double new lines until none left
            while (joinedResponses.Contains("\n\n\n"))
                joinedResponses = joinedResponses.Replace("\n\n\n", "\n\n");

            if (joinedResponses.Contains("#ErrorCountExceeded")) return "#ErrorCountExceeded";

            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;
            var seconds = timeSpan.TotalSeconds;
            statusMessage = $"Copy Edit - {prettyStyleName}";
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);
            // hack to fix not showing last message after tasks
            myForm.updateGPTStatus("GPT Status: RETURNED: " + statusMessage + " using " + gptModel + ": duration = " + Math.Round(seconds, 1).ToString() + " seconds", Color.Black);

            return joinedResponses;
        }

        private static async Task<string> runCopyEditStep(string gptModel, string section, string note, FormApp1 myForm)
        {
            if (section.Trim() == "===")
                return "===";
            var systemPrompt = Prompts.FillPrompt(Prompts.SystemPrompt_CopyEdit_V1, new Dictionary<string, string>()
            {
                { "profilePrompt", Utils.getProfilePrompt(myForm, "MovieText") },
            });
            var userPrompt = Prompts.FillPrompt(Prompts.UserPrompt_CopyEdit_V1, new Dictionary<string, string>()
            {
                { "selectedText", section },
                { "note", note }
            });

            return await UtilsGPT.doGPT("CopyEdit", gptModel, 4000, 0.7, userPrompt, systemPrompt, "", myForm, "");
        }

        public static async Task<string> SendChatterMessage(string operationName, string gptModel, List<ChatMessageModel> chatMessages, FormApp1 myForm)
        {
            var repsonse = await UtilsGPT.doGPTChat(operationName, gptModel, 0.7, chatMessages, myForm);
            return repsonse;
        }
    }
}
