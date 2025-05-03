using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp222
{
    public class Sorting
    {
        // Сортировка по домену
        public static IEnumerable<string> SortByDomain(IEnumerable<string> lines)
        {
            // KeywordSort получение инфы
            string[] priorityDomains = string.IsNullOrEmpty(FormSortPriority.SortKeyword)
                ? Array.Empty<string>()
                : FormSortPriority.SortKeyword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Фильтруем строки, чтобы обрабатывать только те, где можно извлечь домен
            var validLines = lines.Where(line => line.Contains("Country: ")).ToList();

            // Группируем по домену
            var groupedLines = validLines
                .GroupBy(line => GetDomain(line))
                .OrderBy(group =>
                {
                    string domain = group.Key;
                    int priorityIndex = Array.IndexOf(priorityDomains, domain);
                    return (priorityIndex == -1) ? int.MaxValue : priorityIndex;
                })
                .ThenBy(group => group.Key);

            // Формируем результат
            var result = new List<string>();
            foreach (var group in groupedLines)
            {
                result.AddRange(group);
                result.Add("");
            }

            // Удаляем последнюю пустую строку, если она есть
            if (result.Count > 0 && result[result.Count - 1] == "")
            {
                result.RemoveAt(result.Count - 1);
            }

            return result;
        }

        // Получение домена для сортировки по домену
        private static string GetDomain(string line)
        {
            int index = line.IndexOf("Country: ");
            if (index == -1) return "Unknown";

            return line.Substring(index + 9).Trim();
        }

        // Сортировка по числу (Total или после —)
        public static IEnumerable<string> SortByNumber(IEnumerable<string> lines)
        {
            return lines
                .Where(line => line.Contains("Total: ") || line.Contains("—"))
                .OrderByDescending(line => GetNumber(line))
                .ToList();
        }

        // Получение числового значения из строки
        private static int GetNumber(string line)
        {
            int number = 0;
            if (line.Contains("Total"))
            {
                int index = line.IndexOf("Total: ");
                string totalPart = line.Substring(index + 7).Split('|')[0].Trim(); // "Total: " → 7 символов
                int.TryParse(totalPart, out number);
            }
            else if (line.Contains("—"))
            {
                int index = line.IndexOf("—");
                string numberPart = line.Substring(index + 1).Trim();
                int.TryParse(numberPart, out number);
            }
            return number;
        }

        // Комбинированная сортировка по домену и тоталу для строк с "Total" и по числу для строк с "—"
        public static IEnumerable<string> CombineSortDomainTotalAndNumber(IEnumerable<string> lines)
        {
            string[] priorityDomains = string.IsNullOrWhiteSpace(FormSortPriority.SortKeyword)
                ? Array.Empty<string>()
                : FormSortPriority.SortKeyword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Разделяем строки на два списка
            var linesWithTotal = lines.Where(line => line.Contains("Total")).ToList();
            var linesWithDash = lines.Where(line => line.Contains("—")).ToList();

            // Обрабатываем строки с "Total"
            var grouped = linesWithTotal
                .GroupBy(line => GetDomain(line))
                .OrderBy(group =>
                {
                    int index = Array.IndexOf(priorityDomains, group.Key);
                    return (index == -1) ? int.MaxValue : index;
                })
                .ThenBy(group => group.Key);

            var result = new List<string>();

            foreach (var group in grouped)
            {
                var sortedGroup = group
                    .OrderByDescending(line => GetNumber(line));

                result.AddRange(sortedGroup);
                result.Add("");
            }

            // Обрабатываем строки с "—"
            var sortedLinesWithDash = linesWithDash
                .OrderByDescending(line => GetNumber(line));

            // Объединяем результаты
            result.AddRange(sortedLinesWithDash);

            return result;
        }

        // Обратная сортировка
        public static IEnumerable<string> ReverseSorting(IEnumerable<string> lines)
        {
            var blocks = new List<List<string>>();
            var currentBlock = new List<string>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentBlock.Count > 0)
                    {
                        blocks.Add(new List<string>(currentBlock));
                        currentBlock.Clear();
                    }
                }
                else
                {
                    currentBlock.Add(line);
                }
            }

            if (currentBlock.Count > 0)
            {
                blocks.Add(currentBlock);
            }

            blocks.Reverse();

            var result = new List<string>();
            foreach (var block in blocks)
            {
                result.AddRange(block);
                result.Add("");
            }

            return result;
        }
    }
}
