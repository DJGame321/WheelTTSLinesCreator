using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    public interface IAudioFileCreator
    {
        /// <summary>
        /// Creates the audio files for a specific set of voice lines
        /// </summary>
        /// <param name="filePaths">
        /// Contains all the file paths required
        /// </param>
        /// <param name="ttsVoice">
        /// Text to speech voice to use
        /// </param>
        void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token);
    }
}
