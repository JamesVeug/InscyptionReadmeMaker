using System.Collections.Generic;
using System.Text;
using JamesGames.ReadmeMaker.Sections;

namespace JamesGames.ReadmeMaker
{
    public static class ReadmeTableMaker
    {
        public static string Dump(List<ASection> sections)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("### Includes:\n");
            for (int i = 0; i < sections.Count; i++)
            {
                ASection section = sections[i];
                section.GetTableDump(out List<ASection.TableHeader> headers, out List<Dictionary<string, string>> rows);
                if (rows == null || rows.Count == 0)
                {
                    continue;
                }
                
                using (new HeaderScope($"{rows.Count} {section.SectionName}:\n", stringBuilder, true))
                {
                    BuildTable(stringBuilder, headers, rows);
                }
            }
            
            return stringBuilder.ToString();
        }
        
        private static void BuildTable(StringBuilder builder, List<ASection.TableHeader> headers, List<Dictionary<string, string>> rows)
        {
            // Headers
            //|Left columns|Right columns|
            for (int i = 0; i < headers.Count; i++)
            {
                builder.Append("|" + headers[i].HeaderName);
                if (i == headers.Count - 1)
                {
                    builder.Append("|\n");
                }
            }

            // Sorting types
            //|-------------|:-------------:|
            for (int i = 0; i < headers.Count; i++)
            {
                switch (headers[i].Alignment)
                {
                    case ASection.Alignment.Left:
                        builder.Append("|:-");
                        break;
                    case ASection.Alignment.Middle:
                        builder.Append("|:-:");
                        break;
                    case ASection.Alignment.Right:
                        builder.Append("|-:");
                        break;
                }
                if (i == headers.Count - 1)
                {
                    builder.Append("|\n");
                }
            }

            // Cards
            //|alien|thingy|
            //|baby|other thing|
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    Dictionary<string, string> cardData = rows[i];
                    cardData.TryGetValue(headers[j].HeaderName, out string value);
                    string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                    builder.Append("|" + parsedValue);

                    if (j == headers.Count - 1)
                    {
                        builder.Append("|\n");
                    }
                }
            }
        }
    }
}