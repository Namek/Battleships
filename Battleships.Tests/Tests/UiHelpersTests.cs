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
}