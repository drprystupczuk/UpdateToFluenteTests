using System;
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
            "Assert.That(response, Is.Not.Null);",
            "Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));",
            "Assert.That(responseContent.Referrals.IsReferralActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Allowance.IsAllowanceActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Autosave.IsAutosaveActive, Is.EqualTo(true));",
            "Assert.That(responseContent.CasholaCard.KidEnabled, Is.EqualTo(true));",
            "Assert.That(responseContent.Quizzes.IsQuizzesActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Shared.IsNotificationCenterActive, Is.EqualTo(false));",
            "Assert.That(responseContent.CasholaCard.ParentEnabled, Is.EqualTo(false));",
            "Assert.That(responseContent.Investment.IsInvestmentActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Shared.IsP2pActive, Is.EqualTo(false));",
            "Assert.That(responseContent.Goalcards.IsGoalcardActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Shared.IsRoundUpActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Games.IsMoneylingoActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Games.IsGamesActive, Is.EqualTo(true));",
            "Assert.That(responseContent.Allowance.AllowancePaymentSources.First(), Is.EqualTo(EAllowancePaymentSource.Wallet));"
        };

        foreach (string nunitAssertion in nunitAssertions)
        {
            string fluentAssertion = ConvertAssertion(nunitAssertion);
            Console.WriteLine(fluentAssertion);
        }
    }
}