using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KP_KAZLOVSKIY
{
    internal class WorkWithFiles
    {
        public static IEnumerable<string> DifferenceTxt(IEnumerable<string> lines)
        {
            if (string.IsNullOrWhiteSpace(TxtDifference.CombineTxtDifference))
            {
                return lines;
            }

            var keywords = TxtDifference.CombineTxtDifference.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            var keywordsSet = new HashSet<string>(keywords, StringComparer.OrdinalIgnoreCase);

            return lines.Where(line => !keywordsSet.Contains(line));
        }

        public static IEnumerable<string> FilterFileLinesByKeyword(List<string> selectedFiles)
        {
            if (selectedFiles == null || string.IsNullOrWhiteSpace(FileNameCombine.combineByFileName))
                return Enumerable.Empty<string>();

            var result = new List<string>();

            foreach (var filePath in selectedFiles)
            {
                if (!File.Exists(filePath))
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName.IndexOf(FileNameCombine.combineByFileName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    try
                    {
                        var lines = File.ReadLines(filePath);
                        result.AddRange(lines);
                    }
                    catch (IOException)
                    {
                        continue;
                    }
                }
            }

            return result;
        }
    }
}
