using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    public interface IAudioFileCreatorManager : IAudioFileCreator
    {
        /// <summary>
        /// Starts exporting audio files asyncronously
        /// </summary>
        /// <returns></returns>
        Task StartExportingAudioFilesAsync(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger);

        void CancelAudioExport();
    }
}
