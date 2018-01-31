using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text
{
    public class ExtendedTypesModelResult : ModelResult
    {
        public string ParentText { get; set; }

        public ExtendedTypesModelResult() { }

        public ExtendedTypesModelResult(ModelResult modelResult)
        {
            Start = modelResult.Start;
            End = modelResult.End;
            TypeName = modelResult.TypeName;
            Resolution = modelResult.Resolution;
            Text = modelResult.Text;
        }
    }
}
