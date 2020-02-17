//FROM https://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
/*NOTE: You must import the NUnit testing library to use this script*/
//#define TEST // <-- ENABLE HERE

#if TEST
using System.Text.RegularExpressions;


[TestFixture]
public class RemoveExtraWhitespacesTest
{
    private const string _text = "foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo foo                  bar                  foobar                     moo ";
    private const string _expected = "foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo foo bar foobar moo ";

    private const int _iterations = 10000;

    [Test]
    public void Regex()
    {
        var result = TimeAction("Regex", () => RemoveExtraWhitespaces.WithRegex(_text));
        Assert.AreEqual(_expected, result);
    }

    [Test]
    public void RegexCompiled()
    {
        var compiledRegex = new Regex(@"\s+", RegexOptions.Compiled);
        var result = TimeAction("RegexCompiled", () => RemoveExtraWhitespaces.WithRegexCompiled(compiledRegex, _text));
        Assert.AreEqual(_expected, result);
    }

    [Test]
    public void NormalizeWhiteSpace()
    {
        var result = TimeAction("NormalizeWhiteSpace", () => RemoveExtraWhitespaces.NormalizeWhiteSpace(_text));
        Assert.AreEqual(_expected, result);
    }

    [Test]
    public void NormalizeWhiteSpaceForLoop()
    {
        var result = TimeAction("NormalizeWhiteSpaceForLoop", () => RemoveExtraWhitespaces.NormalizeWhiteSpaceForLoop(_text));
        Assert.AreEqual(_expected, result);
    }

    public string TimeAction(string name, Func<string> func)
    {
        var timer = Stopwatch.StartNew();
        string result = string.Empty; ;
        for (int i = 0; i < _iterations; i++)
        {
            result = func();
        }

        timer.Stop();
        Console.WriteLine(string.Format("{0}: {1} ms", name, timer.ElapsedMilliseconds));
        return result;
    }
}
#endif