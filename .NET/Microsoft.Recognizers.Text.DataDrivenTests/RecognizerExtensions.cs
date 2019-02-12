using System;
using System.Collections.Concurrent;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    public static class RecognizerExtensions
    {
        public static ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel> GetInternalCache<TRecognizerOptions>(this Recognizer<TRecognizerOptions> source)
            where TRecognizerOptions : struct
        {
            var modelFactoryProp = typeof(Recognizer<TRecognizerOptions>).GetField("factory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var modelFactory = modelFactoryProp.GetValue(source);
            var cacheProp = modelFactory.GetType().GetField("cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return cacheProp.GetValue(modelFactory) as ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel>;
        }
    }
}
