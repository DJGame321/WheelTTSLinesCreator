using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelTTSLinesCreatorLibrary
{
    internal static class DirectoryManager
    {
        /// <summary>
        /// creates the required directories needed
        /// </summary>
        /// <param name="filePaths">
        /// needed to get the output directory
        /// </param>
        /// <param name="questionTypeToMakePathFor">
        /// question type to make directory for
        /// </param>
        public static void CreateOutputDirectory(FilePaths filePaths, QuestionType questionTypeToMakePathFor)
        {
            if (questionTypeToMakePathFor == QuestionType.MiscLines)
            {
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\TalkshowExport");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\TalkshowExport\project");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\TalkshowExport\project\media");
            }
            else if (questionTypeToMakePathFor == QuestionType.BankLines)
            {
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\TheWheelHost");
            }
            else
            {
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content");
                Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en");

                switch (questionTypeToMakePathFor)
                {
                    case QuestionType.Guessing:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelGuessing");
                        break;
                    case QuestionType.Matching:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelMatching");
                        break;
                    case QuestionType.NumberTarget:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelNumberTarget");
                        break;
                    case QuestionType.RapidFire:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelRapidFire");
                        break;
                    case QuestionType.TappingList:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelTappingList");
                        break;
                    case QuestionType.TypingList:
                        Directory.CreateDirectory($@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelTypingList");
                        break;
                    default:
                        break;
                }

            }
        }

        /// <summary>
        /// gets the working game directory for a specific question type
        /// </summary>
        /// <param name="filePaths">
        /// needed to get the game directory
        /// </param>
        /// <param name="questionTypeToWorkWith">
        /// question type to fetch working directory for
        /// </param>
        /// <returns>
        /// The working game directory path for the specific question type
        /// </returns>
        public static string FetchWorkingGameDirectory(FilePaths filePaths, QuestionType questionTypeToWorkWith)
        {
            if (questionTypeToWorkWith == QuestionType.MiscLines)
            {
                return $@"{ filePaths.GamePath }\The Jackbox Party Pack 8\games\TheWheel\TalkshowExport\project\media";
            }
            else if (questionTypeToWorkWith == QuestionType.BankLines)
            {
                return null;
            }
            else
            {
                switch (questionTypeToWorkWith)
                {
                    case QuestionType.Guessing:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelGuessing";
                    case QuestionType.Matching:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelMatching";
                    case QuestionType.NumberTarget:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelNumberTarget";
                    case QuestionType.RapidFire:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelRapidFire";
                    case QuestionType.TappingList:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelTappingList";
                    case QuestionType.TypingList:
                        return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelTypingList";
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// gets the working game directory for a specific question type
        /// </summary>
        /// <param name="filePaths">
        /// needed to get the output directory
        /// </param>
        /// <param name="questionTypeToOutput">
        /// question type to fetch working directory for
        /// </param>
        /// <returns>
        /// The working output directory path for the specific question type
        /// </returns>
        public static string FetchWorkingOutputDirectory(FilePaths filePaths, QuestionType questionTypeToOutput)
        {
            if (questionTypeToOutput == QuestionType.MiscLines)
            {
                return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\TalkshowExport\project\media";
            }
            else if (questionTypeToOutput == QuestionType.BankLines)
            {
                return $@"{ filePaths.OutputPath }\TheWheelHost";
            }
            else
            {
                switch (questionTypeToOutput)
                {
                    case QuestionType.Guessing:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelGuessing";
                    case QuestionType.Matching:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelMatching";
                    case QuestionType.NumberTarget:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelNumberTarget";
                    case QuestionType.RapidFire:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelRapidFire";
                    case QuestionType.TappingList:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelTappingList";
                    case QuestionType.TypingList:
                        return $@"{ filePaths.OutputPath }\The Jackbox Party Pack 8\games\TheWheel\content\en\TheWheelTypingList";
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Returns the path of the main .jet file for a specific question type
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="questionType"></param>
        /// <returns></returns>
        public static string FetchWorkingJetFile(FilePaths filePaths, QuestionType questionType)
        {
            switch (questionType)
            {
                case QuestionType.Guessing:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelGuessing.jet";
                case QuestionType.Matching:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelMatching.jet";
                case QuestionType.NumberTarget:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelNumberTarget.jet";
                case QuestionType.RapidFire:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelRapidFire.jet";
                case QuestionType.TappingList:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelTappingList.jet";
                case QuestionType.TypingList:
                    return $@"{ filePaths.GamePath }\games\TheWheel\content\en\TheWheelTypingList.jet";
                default:
                    return null;
            }
        }
    }
}
