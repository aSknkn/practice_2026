using System;
using System.Linq;

namespace CustomExtention
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            input = input.ToLower();
            string output="";
            for (int i=0; i<input.Length; i++)
            {
                if (!char.IsPunctuation(input[i]) && !char.IsWhiteSpace(input[i])) output+=input[i];
            }
            
            if (output.Length == 0) return false;
            else return output.SequenceEqual(output.Reverse());
        }
    }
}
