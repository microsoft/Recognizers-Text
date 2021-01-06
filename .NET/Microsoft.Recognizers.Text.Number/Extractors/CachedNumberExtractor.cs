using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class CachedNumberExtractor : BaseNumberExtractor
    {

        protected CachedNumberExtractor(NumberOptions options = NumberOptions.None)
            : base(options)
        {
        }

        public override List<ExtractResult> Extract(string source)
        {

            List<ExtractResult> results;

            if ((this.Options & NumberOptions.NoProtoCache) != 0)
            {
                results = base.Extract(source);
            }
            else
            {
                var key = GenKey(source);

                results = ResultsCache.GetOrCreate(key, () => base.Extract(source));
            }

            return results;
        }

        protected abstract object GenKey(string input);

    }
}
