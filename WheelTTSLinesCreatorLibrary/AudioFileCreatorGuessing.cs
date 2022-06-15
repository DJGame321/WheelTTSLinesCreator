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
    /// Used to create voice files for all the clue audio voice lines for all guessing prompts
    /// </summary>
    internal class AudioFileCreatorGuessing : IAudioFileCreator
    {
        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            //returns if the question type is a question type this method cannot handle
            if (questionType != QuestionType.Guessing)
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

                //loops up to 10 times (a safe bet for the max amount of guessing prompts)
                for (int j = 0; j < 10; j++)
                {
                    //stops exporting audio files when task cancellation requested
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    //specific key that occurs before each clue audio voice line
                    string hintKey = $"\"ClueAudio{ j }\",\n   \"s\": \"";

                    //skips to the next number if the key is not found
                    if (fileContents.IndexOf(hintKey) == -1) continue;

                    //fetches the voice line text from the jet file if the key exists
                    //spoken string is of format: "[voiceline]...rest of file..."
                    string spokenString = fileContents.Substring(fileContents.IndexOf(hintKey) + hintKey.Length);

                    //cuts the ending part off
                    //spoken string is now of format: "[voiceline]"
                    spokenString = spokenString.Substring(0, spokenString.IndexOf("\"\n"));

                    //removes any formatting from the voiceline
                    spokenString = TextFilter.FilterString(spokenString);

                    logger?.LogNewLine($"{ questionType } Question { i + 1 }/{ folderNames.Length }: Saving Hint Audio { j } TTS Audio To { outputDirectory }{ folderNames[i] }\\clues_{ j }_clue.ogg");

                    //output folder path
                    string pathToFolder = $@"{ outputDirectory}{ folderNames[i] }";
                    Directory.CreateDirectory(pathToFolder);

                    //gives the TTS voice the prompt and lets it speak to a wav file
                    ttsVoice.SpeakToWavFile(spokenString, pathToFolder, $"clues_{ j }_clue");

                    //takes the output .wav file and converts it to the .ogg format used in the game
                    WavToOggConverter.EncodeWavToOgg(pathToFolder, $"clues_{ j }_clue");
                }
            }
        }
    }
}
