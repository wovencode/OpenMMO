//FROM https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
/* PERFORMANCE RESULTS
NormalizeWhiteSpaceForLoop: 156 ms(by Me - From my answer on removing all whitespace)
NormalizeWhiteSpace: 267 ms(by Alex K.)
RegexCompiled: 1950 ms(by SLaks)
Regex: 2261 ms(by SLaks)
*/
using System.Text.RegularExpressions;

public class RemoveExtraWhitespaces
{
    public static string WithRegex(string text)
    {
        return Regex.Replace(text, @"\s+", " ");
    }

    public static string WithRegexCompiled(Regex compiledRegex, string text)
    {
        return compiledRegex.Replace(text, " ");
    }

    public static string NormalizeWhiteSpace(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        int current = 0;
        char[] output = new char[input.Length];
        bool skipped = false;

        foreach (char c in input.ToCharArray())
        {
            if (char.IsWhiteSpace(c))
            {
                if (!skipped)
                {
                    if (current > 0)
                        output[current++] = ' ';

                    skipped = true;
                }
            }
            else
            {
                skipped = false;
                output[current++] = c;
            }
        }

        return new string(output, 0, current);
    }

    public static string NormalizeWhiteSpaceForLoop(string input)
    {
        int len = input.Length,
            index = 0,
            i = 0;
        var src = input.ToCharArray();
        bool skip = false;
        char ch;
        for (; i < len; i++)
        {
            ch = src[i];
            switch (ch)
            {
                case '\u0020':
                case '\u00A0':
                case '\u1680':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                case '\u202F':
                case '\u205F':
                case '\u3000':
                case '\u2028':
                case '\u2029':
                case '\u0009':
                case '\u000A':
                case '\u000B':
                case '\u000C':
                case '\u000D':
                case '\u0085':
                    if (skip) continue;
                    src[index++] = ch;
                    skip = true;
                    continue;
                default:
                    skip = false;
                    src[index++] = ch;
                    continue;
            }
        }

        return new string(src, 0, index);
    }
}