using System;
using Battleships.Core;
using Xunit;

namespace Battleships.Tests;

static class TestHelpers {
  /// <summary>
  /// Ensure the ocean is in the described state.
  /// The expected state is in format of string, for example:
  ///    @"
  ///     --------
  ///     -xxxx---
  ///     --------
  ///     --------
  ///     --------
  ///    "
  /// 
  /// where:
  ///   - 'x' is a part of a ship
  ///   ' '-' is an empty cell
  /// </summary>
  public static void AssertState(this OceanGrid ocean, string expectedGridAsText) {
    int i = 0;
    foreach (char ch in expectedGridAsText) {
      int row = i / ocean.Width;
      int col = i % ocean.Width;

      if (ch == '-') {
        Assert.True(ocean.IsEmptyCell(row, col));
        i += 1;
      } else if (ch == 'x') {
        Assert.False(ocean.IsEmptyCell(row, col));
        i += 1;
      } else if (char.IsWhiteSpace(ch)) {
        if (col != 0) {
          Assert.Equal(ocean.Width, col);
        }
      } else {
        throw new ArgumentException($"the string should not contain the '{ch}' character");
      }
    }

    Assert.Equal(ocean.Width * ocean.Height, i);
  }

  /// <summary>
  /// Ensure the targetting grid is in the described state.
  /// The expected state is in format of string, for example:
  ///    @"
  ///     ---o----
  ///     -xxxx---
  ///     --o-----
  ///     ----o---
  ///     --------
  ///    "
  ///
  /// where:
  ///   - 'x' is a part of a ship and
  ///   - 'o' is a missed shot
  ///   - '-' is a undiscovered cell
  /// </summary>
  public static void AssertState(this TargetGrid targetGrid, string expectedStateAsString) {
    int i = 0;
    foreach (char ch in expectedStateAsString) {
      int row = i / targetGrid.Width;
      int col = i % targetGrid.Width;
      
      if (char.IsWhiteSpace(ch)) {
        if (col != 0) {
          Assert.Equal(targetGrid.Width, col);
        }
        continue;
      }

      var cell = targetGrid.GetState(row, col);

      if (ch == '-') {
        Assert.Equal(CellViewState.Unknown, cell);
        i += 1;
      } else if (ch == 'o') {
        Assert.Equal(CellViewState.Missed, cell);
        i += 1;
      } else if (ch == 'x') {
        Assert.Equal(CellViewState.Hit, cell);
        i += 1;
      } else  {
        throw new ArgumentException($"the string should not contain the '{ch}' character");
      }
    }

    Assert.Equal(targetGrid.Width * targetGrid.Height, i);
  }
}