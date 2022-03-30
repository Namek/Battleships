using System.Collections.Generic;
using Battleships.Core;
using Xunit;

namespace Battleships.Tests;

public class AiTests {
  /// <summary>
  /// Note: the tested function can be flaky to test. Thus, we test it only with very marginal values:
  /// 1. sure to succeed
  /// 2. sure to fail (too many ships)
  /// </summary>
  [Theory]
  [InlineData(true, 5, 0, 0, 1)]
  [InlineData(true, 13, 0, 2, 1)]
  [InlineData(true, 8, 0, 2, 0)]
  [InlineData(true, 14, 0, 1, 2)]
  [InlineData(true, 12, 1, 1, 1)]
  [InlineData(false, -1, 0, 1, 20)]
  [InlineData(false, -1, 20, 0, 0)]
  public void PlacesShipsCorrectly(bool expectedSuccess, int expectedSum, int size3Count, int size4Count, int size5Count) {
    var ocean = new OceanGrid(10, 10);
    
    // We could improve the test by providing more randomization seed examples.
    bool succeed = ocean.PlaceRandomShips(312454,new Dictionary<int, int> {
      [3] = size3Count,
      [4] = size4Count,
      [5] = size5Count
    });
    Assert.Equal(expectedSuccess, succeed);

    if (expectedSuccess) {
      // Only check the filled spots on success.
      // Otherwise this is unpredictable due to function resulting in half success modification.
      int filledCount = 0;
      for (int row = 0; row < ocean.Width; row += 1) {
        for (int col = 0; col < ocean.Height; col += 1) {
          if (!ocean.IsEmptyCell(row, col)) {
            filledCount += 1;
          }
        }
      }

      Assert.Equal(expectedSum, filledCount);
    }
  }

  [Theory]
  [InlineData(true, 312454, 312454)]
  [InlineData(false, 64367, 312454)]
  public void ShipsPlacementIsDeterministicallyRandom(bool expectingEqual, int seed1, int seed2) {
    var ocean1 = new OceanGrid(10, 10);
    bool succeed = ocean1.PlaceRandomShips(seed1,new Dictionary<int, int> {
      [4] = 2,
      [5] = 1
    });
    Assert.True(succeed);

    var ocean2 = new OceanGrid(10, 10);
    succeed = ocean2.PlaceRandomShips(seed2,new Dictionary<int, int> {
      [4] = 2,
      [5] = 1
    });
    Assert.True(succeed);

    Assert.Equal(expectingEqual, ocean1.Equals(ocean2));
  }
}