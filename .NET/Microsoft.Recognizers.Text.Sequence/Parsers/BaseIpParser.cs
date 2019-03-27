namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseIpParser : BaseSequenceParser
    {
        public override ParseResult Parse(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
                ResolutionStr = DropLeadingZeros(extResult.Text),
                Data = extResult.Data,
            };

            return result;
        }

        private static string DropLeadingZeros(string text)
        {
            var result = string.Empty;
            var number = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c == '.' || c == ':')
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        number = number == "0" ? number : number.TrimStart('0');
                        number = string.IsNullOrEmpty(number) ? "0" : number;
                        result += number;
                    }

                    result += text[i];
                    number = string.Empty;
                }
                else
                {
                    number += c.ToString();
                    if (i == text.Length - 1)
                    {
                        number = number == "0" ? number : number.TrimStart('0');
                        number = string.IsNullOrEmpty(number) ? "0" : number;
                        result += number;
                    }
                }
            }

            return result;
        }
    }
}
