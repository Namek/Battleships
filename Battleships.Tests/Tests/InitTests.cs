using Battleships.Core;
using Xunit;


namespace Battleships.Tests;

public class InitTests {
  private OceanGrid ocean;

  [Fact]
  public void ShipPlacement() {
    ocean = new OceanGrid(8, 5);

    Assert.True(ocean.PlaceShip(4, 1, 2, Direction.Horizontal));
    ocean.AssertState(@"
      --------
      --xxxx--
      --------
      --------
      --------
    ");

    Assert.True(ocean.PlaceShip(5, 0, 6, Direction.Vertical));
    ocean.AssertState(@"
      ------x-
      --xxxxx-
      ------x-
      ------x-
      ------x-
    ");
  }

  [Fact]
  public void FailingShipPlacementDoesNotChangeOcean() {
    ocean = new OceanGrid(8, 5);

    Assert.True(ocean.PlaceShip(4, 1, 1, Direction.Horizontal));
    ocean.AssertState(@"
      --------
      -xxxx---
      --------
      --------
      --------
    ");

    Assert.False(ocean.PlaceShip(5, 0, 1, Direction.Vertical));
    ocean.AssertState(@"
      --------
      -xxxx---
      --------
      --------
      --------
    ");
  }

  [Fact]
  public void ShipsCantOverlap() {
    ocean = new OceanGrid(10, 8);

    Assert.True(ocean.PlaceShip(4, 3, 2, Direction.Horizontal));

    foreach (var size in new int[] {4, 5}) {
      // the same spot 
      Assert.False(ocean.PlaceShip(size, 3, 2, Direction.Horizontal));

      // 1 column to the left
      Assert.False(ocean.PlaceShip(size, 3, 1, Direction.Horizontal));

      // 1 column to the right
      Assert.False(ocean.PlaceShip(size, 3, 3, Direction.Horizontal));

      // vertical overlap: staring 1 row above, column +1
      Assert.False(ocean.PlaceShip(size, 2, 3, Direction.Vertical));
    }
  }
}