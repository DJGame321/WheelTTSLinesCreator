using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WheelTTSLinesCreatorLibrary
{
    internal class AudioFileCreatorGuessingWithComments : IAudioFileCreator
    {
        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            if (questionType != QuestionType.Guessing) return;

            DirectoryManager.CreateOutputDirectory(filePaths, questionType);

            string inputdirectory = DirectoryManager.FetchWorkingGameDirectory(filePaths, questionType);
            string outputDirectory = DirectoryManager.FetchWorkingOutputDirectory(filePaths, questionType);

            string mainJetFilePath = DirectoryManager.FetchWorkingJetFile(filePaths, questionType);

            //read json file contents
            GuessingQuestions guessingQuestions = JsonConvert.DeserializeObject<GuessingQuestions>(File.ReadAllText(mainJetFilePath));

            //get all question-containing folders
            string[] folderNames = Directory.GetDirectories(inputdirectory);

            //cut containing paths leaving just the folder names
            for (int i = 0; i < folderNames.Length; i++)
            {
                folderNames[i] = folderNames[i].Substring(inputdirectory.Length);
            }

            //setup dictionary for guessing questions based on id
            Dictionary<string, GuessingQuestion> idGuessingQuestionDictionary = new Dictionary<string, GuessingQuestion>();

            foreach (GuessingQuestion guessingQuestion in guessingQuestions.content)
            {
                idGuessingQuestionDictionary.Add(guessingQuestion.id, guessingQuestion);
            }    

            //cycle through each folder one by one
            for (int i = 0; i < folderNames.Length; i++)
            {
                string currentId = folderNames[i].Replace("\\", "");

                //fetch the question clues
                List<Clue> clues = idGuessingQuestionDictionary[currentId].clues;

                //loops through each clue
                for (int j = 0; j < clues.Count; j++)
                {
                    //stops exporting audio files when task cancellation requested
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    string spokenString;

                    if (clues[j].clueComment != string.Empty && clues[j].clueComment != null) spokenString = clues[j].clueComment;
                    else spokenString = clues[j].clue;

                    //removes any formatting from the voiceline
                    spokenString = spokenString.Replace("[i]", ""); //italic formatting
                    spokenString = spokenString.Replace("[/i]", "");
                    spokenString = spokenString.Replace("\\", ""); //the \" formatting for quotes
                    spokenString = spokenString.Replace("*", ""); //asterisk symbols
                    spokenString = spokenString.Replace("!", ","); //exclamation points -> commas
                    spokenString = spokenString.Replace(". ", ", "); //periods -> commas

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
