using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number.Models
{
    public interface IModel
    {
        string ModelTypeName { get; }

        List<ModelResult> Parse(string query);
    }

    public class ModelResult
    {
        public string Text { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string TypeName { get; set; }
        //Resolutions field
        public SortedDictionary<string, string> Resolutions { get; set; }
        //the GeneralResolutions are for the entities can have several different resolutions
        //e.g. ResolveToPast and ResolveToFuture in DateTime
        public Dictionary<string, Object> GeneralResolutions { get; set; }
    }
}