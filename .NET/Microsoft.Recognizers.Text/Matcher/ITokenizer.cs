using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Matcher
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string input);
    }
}
