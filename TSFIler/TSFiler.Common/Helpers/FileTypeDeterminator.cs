using Microsoft.VisualBasic.FileIO;
using TSFiler.Common.Enums;
using TSFiler.Common.Exceptions;

namespace TSFiler.Common.Helpers
{
    public static class FileTypeDeterminator
    {
        public static FileType GetFileType(string fileName)
        {
            var dotIndex = fileName.IndexOf('.');
            var fileExtension = fileName.Substring(dotIndex + 1);
            var fileTipe = fileExtension switch
            {
                "txt" => FileType.Txt,
                "json" => FileType.Json,
                "yaml" => FileType.Yaml,
                "xml" => FileType.Xml,
                _ => FileType.None
            };

            return fileTipe;
        }
    }
}
