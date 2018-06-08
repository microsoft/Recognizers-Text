using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Recognizers.Text.Matcher
{
    public interface ITokenizer
    {
        List<Token> Tokenize(string input);
    }
}
