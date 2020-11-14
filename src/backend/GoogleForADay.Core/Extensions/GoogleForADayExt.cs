using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleForADay.Core.Model.Crawler;
using GoogleForADay.Core.Model.Store;

namespace GoogleForADay.Core.Extensions
{
    public static class GoogleForADayExt
    {

        public static string[] SplitIntoWords(this string text)
        {
            var str = text.Append('.');
            var sb = new StringBuilder(); // for better performance
            var words = new List<string>();
            foreach (var c in str)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
                else if (sb.Length > 1)
                {
                    words.Add(sb.ToString().ToLowerInvariant());
                    sb.Clear();
                }
                else
                {
                    sb.Clear();
                }
            }

            return words.ToArray();
        }



        


    }
}
