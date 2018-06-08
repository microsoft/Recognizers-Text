namespace Microsoft.Recognizers.Text.Matcher
{
    public class Token
    {
        public string Text { get; set; }

        public Token(int s, int l)
        {
            Start = s;
            Length = l;
        }

        public Token(int s, int l, string t) : this(s, l)
        {
            Text = t;
        }

        public int Start { get; private set; }
        public int Length { get; private set; }
        public int End
        {
            get
            {
                return Start + Length;
            }
        }

    }
}
