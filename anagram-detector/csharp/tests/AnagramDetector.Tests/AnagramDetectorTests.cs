using FluentAssertions;
using Xunit;

namespace AnagramDetector.Tests;

public class AnagramDetectorTests
{
    [Fact]
    public void Listen_and_silent_are_anagrams()
    {
        AnagramDetector.AreAnagrams("listen", "silent").Should().BeTrue();
    }

    [Fact]
    public void Hello_and_world_are_not_anagrams()
    {
        AnagramDetector.AreAnagrams("hello", "world").Should().BeFalse();
    }

    [Fact]
    public void Cat_and_tac_are_anagrams()
    {
        AnagramDetector.AreAnagrams("cat", "tac").Should().BeTrue();
    }

    [Fact]
    public void A_word_is_not_an_anagram_of_itself()
    {
        AnagramDetector.AreAnagrams("cat", "cat").Should().BeFalse();
    }

    [Fact]
    public void Comparison_is_case_insensitive()
    {
        AnagramDetector.AreAnagrams("Cat", "tac").Should().BeTrue();
    }

    [Fact]
    public void Empty_strings_are_not_anagrams()
    {
        AnagramDetector.AreAnagrams("", "").Should().BeFalse();
    }

    [Fact]
    public void Single_identical_letters_are_not_anagrams()
    {
        AnagramDetector.AreAnagrams("a", "a").Should().BeFalse();
    }

    [Fact]
    public void Ab_and_ba_are_anagrams()
    {
        AnagramDetector.AreAnagrams("ab", "ba").Should().BeTrue();
    }

    [Fact]
    public void Aab_and_abb_are_not_anagrams_because_letter_counts_differ()
    {
        AnagramDetector.AreAnagrams("aab", "abb").Should().BeFalse();
    }

    [Fact]
    public void Astronomer_and_Moon_starer_are_anagrams_ignoring_case_and_whitespace()
    {
        AnagramDetector.AreAnagrams("Astronomer", "Moon starer").Should().BeTrue();
    }

    [Fact]
    public void Rail_safety_and_fairy_tales_are_anagrams_across_multi_word_phrases()
    {
        AnagramDetector.AreAnagrams("rail safety", "fairy tales").Should().BeTrue();
    }

    [Fact]
    public void Find_anagrams_returns_all_matching_candidates()
    {
        AnagramDetector.FindAnagrams("listen", new[] { "silent", "tinsel" })
            .Should().Equal("silent", "tinsel");
    }

    [Fact]
    public void Find_anagrams_returns_empty_when_no_candidate_matches()
    {
        AnagramDetector.FindAnagrams("listen", new[] { "hello", "world" })
            .Should().BeEmpty();
    }

    [Fact]
    public void Find_anagrams_preserves_input_order_for_mixed_matches()
    {
        AnagramDetector.FindAnagrams("master", new[] { "stream", "maters", "pigeon" })
            .Should().Equal("stream", "maters");
    }

    [Fact]
    public void Group_anagrams_collects_words_sharing_one_anagram_key()
    {
        AnagramDetector.GroupAnagrams(new[] { "eat", "tea", "ate" })
            .Should().BeEquivalentTo(new[] { new[] { "eat", "tea", "ate" } });
    }

    [Fact]
    public void Group_anagrams_returns_singletons_when_no_keys_are_shared()
    {
        AnagramDetector.GroupAnagrams(new[] { "abc", "def" })
            .Should().BeEquivalentTo(new[] { new[] { "abc" }, new[] { "def" } });
    }

    [Fact]
    public void Group_anagrams_returns_an_empty_list_for_no_words()
    {
        AnagramDetector.GroupAnagrams(Array.Empty<string>()).Should().BeEmpty();
    }

    [Fact]
    public void Group_anagrams_preserves_first_occurrence_order_of_groups_and_members()
    {
        var result = AnagramDetector.GroupAnagrams(new[] { "eat", "tea", "tan", "ate", "nat", "bat" });

        result.Should().HaveCount(3);
        result[0].Should().Equal("eat", "tea", "ate");
        result[1].Should().Equal("tan", "nat");
        result[2].Should().Equal("bat");
    }
}
