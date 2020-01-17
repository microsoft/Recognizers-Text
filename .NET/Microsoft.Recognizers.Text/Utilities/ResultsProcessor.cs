using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class ResultsProcessor
    {

        public static void UpdateUnicodeOffsets(string query, ref List<ModelResult> results)
        {

            var origin = query;

            // Save UTF-16 code unit index to Unicode Text Element index
            //
            // Example : "nai\u0308ve X" will generate a lookup array of [0,1,2,-1,3,4,5,6].
            //           When we try to find the word "X", we know the UTF-16 offset of "X" is 7,
            //           and the lookup[7] is 6, so the text element index of "X" is 6.
            var textElementIndex = new int[origin.Length];
            for (int i = 0; i < textElementIndex.Length; i++)
            {
                textElementIndex[i] = -1;
            }

            var enumerator = StringInfo.GetTextElementEnumerator(origin);
            int index = 0;
            while (enumerator.MoveNext())
            {
                textElementIndex[enumerator.ElementIndex] = index;
                index++;
            }

            foreach (var result in results)
            {
                var utf16Offset = result.Start;
                var utf16End = result.End;

                result.Start = textElementIndex[utf16Offset];
                result.End = result.Start + new StringInfo(result.Text).LengthInTextElements - 1;
            }

            Console.WriteLine();
        }
    }
}
