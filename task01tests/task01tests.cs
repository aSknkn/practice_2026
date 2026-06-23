using Xunit;
using CustomExtention;

public class StringExtensionsTests
{
    [Fact]
    public void IsPalindrome_ValidPalindrome_ReturnsTrue()
    {
        string input = "А роза упала на лапу Азора";
        Assert.True(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_NotPalindrome_ReturnsFalse()
    {
        string input = "Hello, world!";
        Assert.False(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_EmptyString_ReturnsFalse()
    {
        string input = "";
        Assert.False(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_WithPunctuation_IgnoresPunctuation_ReturnsTrue()
    {
        string input = "Was it a car or a cat I saw?";
        Assert.True(input.IsPalindrome());
    }


    [Fact]
    public void IsPalindrome_MultipleSpaces_IgnoresSpaces_ReturnsTrue()
    {
        string input = "ш  а     л аш";
        Assert.True(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_SingleCharacter_ReturnsTrue()
    {
        string input = "а";
        Assert.True(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_ComplexPunctuation_IgnoresPunctuation_ReturnsTrue()
    {
        string input = "Ты, милок, иди яром: у дороги мина, за дорогой  огород, а заним и город у моря; иди, коли мыт";
        Assert.True(input.IsPalindrome());
    }
}
