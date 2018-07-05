using System;

namespace EasySendler.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string inputString, int symbolCount)
        {
            return $"{inputString.Substring(0, Math.Min(inputString.Length, symbolCount))}...";
        }
    }
}