using System;
using System.Collections.Generic;
using System.Linq;

namespace KP_KAZLOVSKIY
{
    public static class Processing
    {
        // 1. Удалить повторы
        public static IEnumerable<string> RemoveDuplicates(IEnumerable<string> lines)
        {
            return lines.Distinct();
        }

        // 2. Удаление строк по ключевому слову (KEYWORD)
        public static IEnumerable<string> RemoveByKeyword(IEnumerable<string> lines)
        {
            if (FormKeyWordDelLines.DelLinesKeyword == null || !FormKeyWordDelLines.DelLinesKeyword.Any())
                return lines;

            return lines.Where(line => !FormKeyWordDelLines.DelLinesKeyword
                .Any(keyword => line.Contains(keyword)));
        }

        // 3. Удаление строк без ключевого слова (KEYWORD)
        public static IEnumerable<string> KeepOnlyByKeyword(IEnumerable<string> lines)
        {
            if (FormWithoutKeyWordDelLines.DelLinesWithoutKeyword == null || !FormWithoutKeyWordDelLines.DelLinesWithoutKeyword.Any())
                return lines;
            return lines.Where(line => FormWithoutKeyWordDelLines.DelLinesWithoutKeyword
            .Any(keyword => line.ToLower().Contains(keyword.ToLower())));
        }

        // Удаление содержимого до ключевого слова
        public static IEnumerable<string> RemoveBeforeKeyword(IEnumerable<string> lines)
        {
            if (string.IsNullOrWhiteSpace(FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword)) return lines;

            return lines.Select(line =>
            {
                int index = line.IndexOf(FormKeyWordDelLineContentBefore.DelLineContentBeforeKeyword, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    return line.Substring(index);
                }
                return line;
            });
        }

        // Удаление содержимого после ключевого слова
        public static IEnumerable<string> RemoveAfterKeyword(IEnumerable<string> lines)
        {
            if (string.IsNullOrWhiteSpace(FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword)) return lines;
            string keyword = FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword;
            Console.WriteLine($"Используемое ключевое слово: '{keyword}' (длина: {keyword?.Length ?? 0})");
            return lines.Select(line =>
            {
                int index = line.IndexOf(FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    int endIndex = index + FormKeyWordDelLineContentAfter.DelLineContentAfterKeyword.Length;
                    return line.Substring(0, endIndex);
                }
                return line;
            });
        }
    }
}
