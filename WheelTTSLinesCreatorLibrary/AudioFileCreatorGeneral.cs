using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    /// <summary>
    /// Used to create voice line files for all question types since they all have the
    /// intro, prompt, followup, outro format (reveal for guessing questions)
    /// </summary>
    internal class AudioFileCreatorGeneral : IAudioFileCreator
    {
        private enum VoicelineType
        {
            intro,
            prompt,
            followup,
            outro,
            reveal,
        }

        //in the jet file, each voice line is declared in this format:
        // "n": "[voice-line-type]Audio",
        // "s": "[voice-line]"
        private static string[] keys =
        {
            "\"IntroAudio\",\n   \"s\": \"",
            "\"PromptAudio\",\n   \"s\": \"",
            "\"FollowupAudio\",\n   \"s\": \"",
            "\"OutroAudio\",\n   \"s\": \"",
            "\"RevealAudio\",\n   \"s\": \""
        };

        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            //returns if the question type is a question type this method cannot handle
            if (questionType == QuestionType.MiscLines || questionType == QuestionType.BankLines)
            {
                return;
            }

            DirectoryManager.CreateOutputDirectory(filePaths, questionType);

            string inputdirectory = DirectoryManager.FetchWorkingGameDirectory(filePaths, questionType);
            string outputDirectory = DirectoryManager.FetchWorkingOutputDirectory(filePaths, questionType);

            //get all question-containing folders
            string[] folderNames = Directory.GetDirectories(inputdirectory);

            //cut containing paths leaving just the folder names
            for (int i = 0; i < folderNames.Length; i++)
            {
                folderNames[i] = folderNames[i].Substring(inputdirectory.Length);
            }

            //cycle through each folder one by one
            for (int i = 0; i < folderNames.Length; i++)
            {
                //fetch the jet file inside the folder
                string fileContents = File.ReadAllText($@"{ inputdirectory }{ folderNames[i] }\data.jet");

                //cycle through each voice line type
                for (int j = 0; j < keys.Length; j++)
                {
                    //stops exporting audio files when task cancellation requested
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    //searches for a specific set of characters in the jet file to see if a voice line of that type exists
                    //and if not, moves on to the next one
                    if (fileContents.IndexOf(keys[j]) == -1) continue;

                    //fetches the voice line text from the jet file if the key exists
                    //spoken string is of format: "[voiceline]...rest of file..."
                    string spokenString = fileContents.Substring(fileContents.IndexOf(keys[j]) + keys[j].Length);

                    //cuts the ending part off
                    //spoken string is now of format: "[voiceline]"
                    spokenString = spokenString.Substring(0, spokenString.IndexOf("\"\n"));

                    //removes any formatting from the voiceline
                    spokenString = TextFilter.FilterString(spokenString);

                    logger?.LogNewLine($"{ questionType } Question { i + 1 }/{ folderNames.Length }: Saving {(VoicelineType)j} TTS Audio To {outputDirectory}{folderNames[i]}\\{ (VoicelineType)j }.ogg");

                    //output folder path
                    string pathToFolder = $@"{outputDirectory}{folderNames[i] }";
                    Directory.CreateDirectory(pathToFolder);

                    //gives the TTS voice the prompt and lets it speak to a wav file
                    ttsVoice.SpeakToWavFile(spokenString, pathToFolder, ((VoicelineType)j).ToString());

                    //takes the output .wav file and converts it to the .ogg format used in the game
                    WavToOggConverter.EncodeWavToOgg(pathToFolder, $"{ (VoicelineType)j }");
                }
            }
        }
    }
}
