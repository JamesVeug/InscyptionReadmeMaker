using System;
using System.Text;

namespace JamesGames.ReadmeMaker
{
    public class HeaderScope : IDisposable
    {
        private readonly StringBuilder stringBuilder;

        public HeaderScope(string text, StringBuilder stringBuilder, bool appendNewLinePrefix=false)
        {
            this.stringBuilder = stringBuilder;
            switch (ReadmeConfig.Instance.GeneralHeaderType)
            {
                case ReadmeConfig.HeaderType.Foldout:
                    // <details>
                    // <summary>See older changes</summary>
                    stringBuilder.Append(GetPrefix(appendNewLinePrefix) + "<details>\n");
                    stringBuilder.Append("<summary>" + text + "</summary>\n\n");
                    break;
                case ReadmeConfig.HeaderType.Label:
                    // ## New Cards:
                    string header = HeaderSizeToPrefix(ReadmeConfig.Instance.GeneralHeaderSize, appendNewLinePrefix) + text;
                    stringBuilder.Append(GetPrefix(appendNewLinePrefix) + header);
                    break;
            }
        }

        private string GetPrefix(bool appendNewLinePrefix)
        {
            return appendNewLinePrefix ? "\n" : "";
        }
        
        public void Dispose()
        {
            if (ReadmeConfig.Instance.GeneralHeaderType == ReadmeConfig.HeaderType.Foldout)
            {
                if (stringBuilder[stringBuilder.Length - 1] != '\n')
                {
                    stringBuilder.AppendLine();
                }
                else
                {
                    stringBuilder.Append("</details>\n");
                }
            }
        }

        private string HeaderSizeToPrefix(ReadmeConfig.HeaderSize size, bool addNewLinePrefix)
        {
            switch (size)
            {
                case ReadmeConfig.HeaderSize.Biggest:
                    return "# ";
                case ReadmeConfig.HeaderSize.Bigger:
                    return "### ";
                case ReadmeConfig.HeaderSize.Big:
                    return "#### ";
                case ReadmeConfig.HeaderSize.Small:
                    return "##### ";
                case ReadmeConfig.HeaderSize.Smaller:
                    return "##### ";
                case ReadmeConfig.HeaderSize.Smallest:
                    return "###### ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(size), size, null);
            }
        }
    }
}