using System;

namespace CustomExtention
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (input.Length == 0) return false;
            else {
            input = input.ToLower();
            string output="";
            for (int i=0; i<input.Length; i++)
            {
                if (!char.IsPunctuation(input[i]) && !char.IsWhiteSpace(input[i])) output+=input[i];
            }
            
            return output.SequenceEqual(output.Reverse());
            }
        }
    }
}