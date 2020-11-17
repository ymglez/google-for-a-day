using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GoogleForADay.Services.Business.Validators
{
    static class InputValidator
    {
        public static bool ValidUrl(string url)
        {
            try
            {
                new Uri(url);
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
