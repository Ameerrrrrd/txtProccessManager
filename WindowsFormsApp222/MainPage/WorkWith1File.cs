using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_KAZLOVSKIY
{
    internal class WorkWith1File
    {
        public static void SplitAndSaveFiles(IEnumerable<string> lines, string saveDirectory)
        {
            int linesPerFile = int.Parse(DivIntoPartsForm.txtLinesCountFinal);
            int filesCount = int.Parse(DivIntoPartsForm.txtFilesCountFinal);
            string baseFileName = FormSavingName.savingName;

            var inputLines = lines.ToList();
            int totalExpectedLines = linesPerFile * filesCount;

            while (inputLines.Count < totalExpectedLines)
            {
                inputLines.Add("Не хватило строк");
            }

            for (int i = 0; i < filesCount; i++)
            {
                var part = inputLines
                    .Skip(i * linesPerFile)
                    .Take(linesPerFile)
                    .ToList();

                string fileName = Path.Combine(saveDirectory, $"{baseFileName}_{i + 1}.txt");
                File.WriteAllLines(fileName, part);
            }
        }

        public static IEnumerable<string> GenerateCaseMutations(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                int atIndex = line.IndexOf('@');
                if (atIndex == -1)
                {
                    yield return line;
                    yield return "";
                    continue;
                }

                string prefix = line.Substring(0, atIndex);
                string suffix = line.Substring(atIndex);

                var letterIndices = new List<int>();
                for (int i = 0; i < prefix.Length; i++)
                {
                    if (char.IsLetter(prefix[i]))
                        letterIndices.Add(i);
                }

                int combinations = 1 << letterIndices.Count;

                for (int mask = 0; mask < combinations; mask++)
                {
                    var chars = prefix.ToCharArray();
                    for (int bit = 0; bit < letterIndices.Count; bit++)
                    {
                        int index = letterIndices[bit];
                        bool toUpper = ((mask >> bit) & 1) == 1;
                        chars[index] = toUpper
                            ? char.ToUpper(chars[index])
                            : char.ToLower(chars[index]);
                    }

                    yield return new string(chars) + suffix;
                }

                yield return "";
            }
        }
    }
}
