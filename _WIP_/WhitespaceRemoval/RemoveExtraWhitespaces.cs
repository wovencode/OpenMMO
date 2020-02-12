//FROM https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
/* PERFORMANCE RESULTS
NormalizeWhiteSpaceForLoop: 156 ms(by Me - From my answer on removing all whitespace)
NormalizeWhiteSpace: 267 ms(by Alex K.)
RegexCompiled: 1950 ms(by SLaks)
Regex: 2261 ms(by SLaks)
Compact: ???
Simple: ???
*/
using System.Text.RegularExpressions;
using System.Text;
using System;

public class RemoveExtraWhitespaces
{
    //??? ms TODO: UNTESTED
    #region Simple
    public static string Simple(string text)
    {
        return string.Join(" ", text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
    }
    #endregion

    //??? ms TODO: UNTESTED
    #region CompactWhiteSpaces
    //BY Sergey Povalyaev
    public static string CompactWhitespaces(String s)
    {
        StringBuilder sb = new StringBuilder(s);

        CompactWhitespaces(sb);

        return sb.ToString();
    }
    
    public static void CompactWhitespaces(StringBuilder sb)
    {
        if (sb.Length == 0)
            return;

        // set [start] to first not-whitespace char or to sb.Length

        int start = 0;

        while (start < sb.Length)
        {
            if (char.IsWhiteSpace(sb[start]))
                start++;
            else
                break;
        }

        // if [sb] has only whitespaces, then return empty string

        if (start == sb.Length)
        {
            sb.Length = 0;
            return;
        }

        // set [end] to last not-whitespace char

        int end = sb.Length - 1;

        while (end >= 0)
        {
            if (char.IsWhiteSpace(sb[end]))
                end--;
            else
                break;
        }

        // compact string

        int dest = 0;
        bool previousIsWhitespace = false;

        for (int i = start; i <= end; i++)
        {
            if (char.IsWhiteSpace(sb[i]))
            {
                if (!previousIsWhitespace)
                {
                    previousIsWhitespace = true;
                    sb[dest] = ' ';
                    dest++;
                }
            }
            else
            {
                previousIsWhitespace = false;
                sb[dest] = sb[i];
                dest++;
            }
        }

        sb.Length = dest;
    }
    #endregion

    //157 ms
    #region NormalizeWhiteSpaceForLoop
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
    #endregion

    //267
    #region NormalizeWhitespace
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
    #endregion

    //1850 ms
    #region RegxCompiled
    public static string WithRegexCompiled(Regex compiledRegex, string text)
    {
        return compiledRegex.Replace(text, " ");
    }
    #endregion

    //2261 ms
    #region Regex
    public static string WithRegex(string text)
    {
        return Regex.Replace(text, @"\s+", " ");
    }
    #endregion

}