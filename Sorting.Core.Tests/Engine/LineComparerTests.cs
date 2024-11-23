using Sorting.Core.Engine;

namespace Sorting.Core.Tests.Engine;

[TestFixture]
public class LineComparerTests
{
    private LineComparer _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new LineComparer();
    }
    
    [TestCase("2. abc", "1. abc", 1)]
    [TestCase("1. abc", "2. abc", -1)]
    [TestCase("1. abcd", "1. abce", -1)]
    [TestCase("1. abce", "1. abcd", 1)]
    [TestCase("1. abc", "1. abc", 0)]
    public void Compare_ShouldReturnExpectedResult(string x, string y, int expectedResult)
    {
        // Act
        var result = _sut.Compare(x, y);
        
        // Assert
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [TestCase("1 abc", "1. abc")]
    [TestCase("1. abc", "1 abc")]
    public void Compare_IncorrectLine_ShouldThrowFormatException(string x, string y)
    {
        // Act & Assert
        var ex = Assert.Throws<FormatException>(() => _sut.Compare(x, y));
        Assert.That(ex.Message, Does.Contain("Invalid format: line should contain number and string"));
    }
    
    [TestCase("abc. abc", "1. abc")]
    [TestCase("1. abc", "abc. abc")]
    public void Compare_LineWithoutNumber_ShouldThrowFormatException(string x, string y)
    {
        // Act & Assert
        var ex = Assert.Throws<FormatException>(() => _sut.Compare(x, y));
        Assert.That(ex.Message, Does.Contain("Invalid format: line does not start with a number"));
    }
}