using System.Reflection;

namespace HITS.Demo.WeakEvents.Shared
{
    public class IoHelper
    {
        public static string ExecutingDirectory { get; set; }

        public static DirectoryInfo GetExecutingDirectory()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new System.IO.FileInfo(location.AbsolutePath).Directory;
        }

        public static string GetFileHref(string folderName, string file)
        {
            return $"/{folderName}/{Path.GetFileName(file)}";
        }

        public static string GetFilePath(string file)
        {
            return $@"{IoHelper.ExecutingDirectory}\{file}";
        }
    }
}
