using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    internal class AudioFileCreatorMiscLines : IAudioFileCreator
    {
        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            //returns if the question type is a question type this method cannot handle
            if (questionType != QuestionType.MiscLines)
            {
                return;
            }

            DirectoryManager.CreateOutputDirectory(filePaths, questionType);
            string outputDirectory = DirectoryManager.FetchWorkingOutputDirectory(filePaths, questionType);

            string[] lines = File.ReadAllLines(filePaths.MiscellaneousLinesPath);

            for (int i = 0; i < lines.Length; i++)
            {
                //stops exporting audio files when task cancellation requested
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                string fileName = lines[i].Substring(0, lines[i].IndexOf(":"));
                string voiceLine = lines[i].Substring(lines[i].IndexOf(":") + 1);

                if (voiceLine.Contains("(ignore)"))
                {
                    logger?.LogNewLine($"({ i + 1 }/{ lines.Length }) Voice Line Ignored");
                    continue;
                }

                logger?.LogNewLine($"({ i + 1 }/{ lines.Length }) Creating File { outputDirectory }{ fileName }.ogg");

                ttsVoice.SpeakToWavFile(voiceLine, outputDirectory, fileName);

                WavToOggConverter.EncodeWavToOgg(outputDirectory, fileName);
            }
        }
    }
}
