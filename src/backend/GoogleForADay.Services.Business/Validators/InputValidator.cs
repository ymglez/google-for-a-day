using System;
using System.Text.RegularExpressions;

namespace GoogleForADay.Services.Business.Validators
{
    internal static class InputValidator
    {
        public static bool ValidUrl(string url)
        {
            try
            {
                var unused = new Uri(url);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool ValidWord(string word)
        {
            return !string.IsNullOrEmpty(word) && 
                   new Regex(@"^[a-zA-Z0-9]*$").IsMatch(word);
        }

    }
}
