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
    /// Used to create voice files for all the clue audio voice lines for all writing prompts
    /// </summary>
    internal class AudioFileCreatorManager : IAudioFileCreatorManager
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public async Task StartExportingAudioFilesAsync(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            try
            {
                await Task.Run(() => CreateAudioFiles(filePaths, ttsVoice, questionType, logger, cancellationToken));
                logger.LogNewLine("Operation Completed Successfully");
            }
            catch (OperationCanceledException)
            {
                logger.LogNewLine("Operation Cancelled");
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }
        }

        public void CancelAudioExport()
        {
            cancellationTokenSource.Cancel();
        }

        public void CreateAudioFiles(FilePaths filePaths, ITTSVoice ttsVoice, QuestionType questionType, ILogger logger, CancellationToken token)
        {
            switch (questionType)
            {
                case QuestionType.TappingList:
                    AudioFileCreatorGeneral tapAFC = new AudioFileCreatorGeneral();
                    tapAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.NumberTarget:
                    AudioFileCreatorGeneral numAFC = new AudioFileCreatorGeneral();
                    numAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.Matching:
                    AudioFileCreatorGeneral matAFC = new AudioFileCreatorGeneral();
                    matAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.RapidFire:
                    AudioFileCreatorGeneral rapAFC = new AudioFileCreatorGeneral();
                    rapAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.Guessing:
                    //AudioFileCreatorGeneral gueAFC = new AudioFileCreatorGeneral();
                    //gueAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    AudioFileCreatorGuessingWithComments gueAFCG = new AudioFileCreatorGuessingWithComments();
                    gueAFCG.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.TypingList:
                    AudioFileCreatorGeneral typAFC = new AudioFileCreatorGeneral();
                    typAFC.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    AudioFileCreatorWriting typAFCW = new AudioFileCreatorWriting();
                    typAFCW.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.MiscLines:
                    AudioFileCreatorMiscLines AFCML = new AudioFileCreatorMiscLines();
                    AFCML.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                case QuestionType.BankLines:
                    AudioFileCreatorBankFiles AFCBF = new AudioFileCreatorBankFiles();
                    AFCBF.CreateAudioFiles(filePaths, ttsVoice, questionType, logger, token);
                    break;
                default:
                    break;
            }
        }
    }
}
