using Battleships.Core;
using Xunit;

namespace Battleships.Tests;

public class UiHelpersTests {
  [Fact]
  public void ParseTargetPosition() {
    Assert.Equal((0, 0), UIHelpers.ParsePosition("A1", 10, 10));
    Assert.Equal((0, 1), UIHelpers.ParsePosition("A2", 10, 10));
    Assert.Equal((1, 1), UIHelpers.ParsePosition("B2", 10, 10));
    Assert.Equal((9, 9), UIHelpers.ParsePosition("J10", 10, 10));
  }

  [Fact]
  public void ParseTargetPositionOutOfBounds() {
    Assert.Null(UIHelpers.ParsePosition("A0", 10, 10));
    Assert.NotNull(UIHelpers.ParsePosition("A10", 10, 10));
    Assert.Null(UIHelpers.ParsePosition("A11", 10, 10));
    Assert.NotNull(UIHelpers.ParsePosition("J3", 10, 10));
    Assert.Null(UIHelpers.ParsePosition("K3", 10, 10));
    Assert.Null(UIHelpers.ParsePosition("X3", 10, 10));
  }

  [Theory]
  [InlineData("")]
  [InlineData(";4")]
  [InlineData("4;;")]
  [InlineData("A'")]
  [InlineData("///")]
  public void ParseTargetPositionWithIncorrectValues(string input) {
    Assert.Null(UIHelpers.ParsePosition(input, 10, 10));
  }

  [Theory]
  [InlineData(10, 10, "A1", "J10")]
  [InlineData(10, 5, "A1", "E10")]
  [InlineData(5, 10, "A1", "J5")]
  [InlineData(5, 5, "A1", "E5")]
  public void CorrectPositionBoundary(int width, int height, string expectedFrom, string expectedTo) {
    Assert.Equal((expectedFrom, expectedTo), UIHelpers.GetPositionBoundaries(width, height));
  }

  [Theory]
  [InlineData(1, 0)]
  [InlineData(0, 1)]
  [InlineData(3, 0)]
  [InlineData(0, 3)]
  [InlineData(3, -1)]
  [InlineData(-1, 3)]
  public void IncorrectPositionBoundary(int width, int height) {
    Assert.Equal(("A1", "A1"), UIHelpers.GetPositionBoundaries(width, height));
  }
}