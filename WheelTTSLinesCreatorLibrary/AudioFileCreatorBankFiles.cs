using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    internal class AudioFileCreatorBankFiles : IAudioFileCreator
    {
        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            //returns if the question type is a question type this method cannot handle
            if (questionType != QuestionType.BankLines)
            {
                return;
            }

            DirectoryManager.CreateOutputDirectory(filePaths, questionType);
            string outputDirectory = DirectoryManager.FetchWorkingOutputDirectory(filePaths, questionType);

            string[] lines = File.ReadAllLines(filePaths.BankLinesPath);

            for (int i = 0; i < lines.Length; i++)
            {
                //stops exporting audio files when task cancellation requested
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                string fileName = lines[i].Substring(0, lines[i].IndexOf(".wav:"));
                string voiceLine = lines[i].Substring(lines[i].IndexOf(".wav:") + ".wav:".Length);

                logger?.LogNewLine($"({ i + 1 }/{ lines.Length }) Creating Wav File { outputDirectory }\\{ fileName }.wav");

                ttsVoice.SpeakToWavFile(voiceLine, outputDirectory, fileName);
            }

            logger?.LogNewLine($"Creating TheWheelHost.txt File");

            //creates TheWheelHost.txt file
            StreamWriter writer = File.CreateText($@"{ outputDirectory }\TheWheelHost.txt");

            //writes to it each of the file names in it
            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i].Substring(0, lines[i].IndexOf(":")));
            }

            writer.Close();
        }
    }
}
