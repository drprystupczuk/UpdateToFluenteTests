﻿using System.Text.RegularExpressions;

public class TestConverter
{
    public static string ConvertAssertion(string nunitAssertion)
    {
        // Patterns for specific replacements
        var specificReplacements = new (string pattern, string replacement)[]
        {
            (@"Assert\.That\((.*?),\s*Is\.Empty\);", "$1.Should().BeEmpty();"),
            (@"Assert\.That\((.*?),\s*Is\.Null\);", "$1.Should().BeNull();"),
            (@"Assert\.That\((.*?),\s*Is\.Not\.Null\);", "$1.Should().NotBeNull();"),
            (@"Assert\.That\((.*?),\s*Is\.Not\.Empty\);", "$1.Should().NotBeEmpty();"),
            (@"Assert\.That\((.*?),\s*Is\.False\);", "$1.Should().BeFalse();"),
            (@"Assert\.That\((.*?),\s*Is\.True\);", "$1.Should().BeTrue();"),
            (@"Assert\.That\((.*?),\s*Is\.EqualTo\(true\)\);", "$1.Should().BeTrue();"),
            (@"Assert\.That\((.*?),\s*Is\.EqualTo\(false\)\);", "$1.Should().BeFalse();"),
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

        // Default pattern for general cases
        string pattern = @"Assert\.That\((.*?),\s*Is\.(.*?)\((.*?)\)\);";
        string replacement = "$1.Should().$2($3);";

        return Regex.Replace(nunitAssertion, pattern, replacement);
    }

    public static void Main()
    {
        string[] nunitAssertions = new string[]
        {
            "Assert.That(response, Is.Not.Null);",
            "Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));",
            "Assert.That(responseContent.Something, Is.EqualTo(true));",
            "Assert.That(responseContent.PropertyA, Is.EqualTo(10));",
            "Assert.That(someVariable, Has.Count.EqualTo(2));"
        };

        string filePath = "FluentAssertions.txt";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (string nunitAssertion in nunitAssertions)
            {
                string fluentAssertion = ConvertAssertion(nunitAssertion);
                writer.WriteLine(fluentAssertion);
            }
        }

        Console.WriteLine($"Converted assertions saved to {filePath}");
    }
}