using System.Text.RegularExpressions;

public class TestConverter
{
    public static string ConvertAssertion(string nunitAssertion)
    {
        string pattern = @"Assert\.That\((.*?),\s*Is\.(.*?)\((.*?)\)\);";
        string replacement = "$1.Should().$2($3);";

        // Patterns for specific replacements
        var specificReplacements = new (string pattern, string replacement)[]
        {
            (@"Assert\.That\((.*?),\s*Is\.Empty\);", "$1.Should().BeEmpty();"),
            (@"Assert\.That\((.*?),\s*Is\.Null\);", "$1.Should().BeNull();"),
            (@"Assert\.That\((.*?),\s*Is\.Not\.Empty\);", "$1.Should().NotBeEmpty();"),
            (@"Assert\.That\((.*?),\s*Is\.False\);", "$1.Should().BeFalse();"),
            (@"Assert\.That\((.*?),\s*Is\.True\);", "$1.Should().BeTrue();"),
            (@"Assert\.That\((.*?),\s*Is\.EqualTo\((.*?)\)\);", "$1.Should().Be($2);"),
            (@"Assert\.That\((.*?),\s*Has\.Count\.EqualTo\((.*?)\)\);", "$1.Should().HaveCount($2);")
        };

        // Apply specific replacements
        foreach (var (specPattern, specReplacement) in specificReplacements)
        {
            if (Regex.IsMatch(nunitAssertion, specPattern))
            {
                return Regex.Replace(nunitAssertion, specPattern, specReplacement);
            }
        }

        // Default replacement for general cases
        return Regex.Replace(nunitAssertion, pattern, replacement);
    }

    public static void Main()
    {
        string[] nunitAssertions = new string[]
        {
            //input here
        };

        foreach (string nunitAssertion in nunitAssertions)
        {
            string fluentAssertion = ConvertAssertion(nunitAssertion);
            Console.WriteLine(fluentAssertion);
        }
    }
}