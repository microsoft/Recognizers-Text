using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Recognizers.Text
{
    public class TestTextWriterTraceListener : TextWriterTraceListener
    {
        private bool isAppend = false;
        public TestTextWriterTraceListener(string language, string recognizer, string model) : base(string.Join("-", "results", language, recognizer, model) + ".json", string.Join("-", language, recognizer, model))
        {
            var path = Path.Combine("..", "..", "..", "..", "Specs", recognizer, language);
            Directory.CreateDirectory(path);
            base.Writer = new StreamWriter(Path.Combine(path, model + ".json"), false);
            base.Filter = new TestFilter(language, recognizer, model);
            base.Write("[");
        }
        
        public override void WriteLine(string message)
        {
            if (Filter.ShouldTrace(null, "", TraceEventType.Verbose, 0, message, null, null, null))
            {
                var model = JsonConvert.DeserializeObject<SingleTestModel>(message);
                if (isAppend)
                {
                    base.WriteLine(",");
                }
                else
                {
                    isAppend = true;
                }

                base.WriteLine(TestWriter.GetJson(model, Formatting.Indented));
                //base.Flush();
            }
        }

        public override void Close()
        {
            base.Close();
            this.Writer.Close();
        }
    }
    
    public class TestFilter : TraceFilter
    {
        private readonly string language;
        private readonly string recognizer;
        private readonly string model;

        public TestFilter(string language, string recognizer, string model)
        {
            this.language = language;
            this.recognizer = recognizer;
            this.model = model;
        }

        public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            var testCase = JsonConvert.DeserializeObject<TestModel>(formatOrMessage);
            return testCase.Language.Equals(language) && testCase.Recognizer.Equals(recognizer) && testCase.Model.Equals(model);
        }
    }
}
