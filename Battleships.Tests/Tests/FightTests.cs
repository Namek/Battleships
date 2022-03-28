using Battleships.Core;
using Xunit;

namespace Battleships.Tests;

public class FightTests {
  private GameState state;

  [Fact]
  public void AlreadyShot() {
    state = new GameState();
    state.OpponentGrid.PlaceShip(5, 2, 2, Direction.Horizontal);
    state.OpponentGrid.AssertState(@"
      ----------
      ----------
      --xxxxx---
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ");

    // shooting ship
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 6));
    var AFTER_FIRST_HIT = @"
      ----------
      ----------
      ------x---
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ";
    state.TargetGrid.AssertState(AFTER_FIRST_HIT);
    Assert.Equal(ActionResult.AlreadyShot, state.Shoot(2, 6));
    
    // target grid does not change
    state.TargetGrid.AssertState(AFTER_FIRST_HIT);

    // shooting empty space
    Assert.Equal(ActionResult.Miss, state.Shoot(1, 6));
    var AFTER_FIRST_MISS = @"
      ----------
      ------o---
      ------x---
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ";
    state.TargetGrid.AssertState(AFTER_FIRST_MISS);
    Assert.Equal(ActionResult.AlreadyShot, state.Shoot(1, 6));
    state.TargetGrid.AssertState(AFTER_FIRST_MISS);
  }

  [Fact]
  public void Sink() {
    state = new GameState();
    state.OpponentGrid.PlaceShip(4, 2, 2, Direction.Horizontal);
    state.OpponentGrid.PlaceShip(4, 4, 2, Direction.Horizontal);

    Assert.Equal(ActionResult.Hit, state.Shoot(2, 2));
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 3));
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 4));
    Assert.Equal(ActionResult.Miss, state.Shoot(2, 6));
    Assert.Equal(ActionResult.Sink, state.Shoot(2, 5));
  }

  [Fact]
  public void WinCondition() {
    state = new GameState();
    state.OpponentGrid.PlaceShip(4, 2, 2, Direction.Horizontal);
    state.OpponentGrid.PlaceShip(5, 4, 2, Direction.Horizontal);

    // sink the 1st ship
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 2));
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 3));
    Assert.Equal(ActionResult.Hit, state.Shoot(2, 4));
    Assert.Equal(ActionResult.Miss, state.Shoot(2, 6));
    Assert.Equal(ActionResult.Sink, state.Shoot(2, 5));

    // sink the 2nd ship (the last one)
    Assert.Equal(ActionResult.Hit, state.Shoot(4, 2));
    Assert.Equal(ActionResult.Hit, state.Shoot(4, 3));
    Assert.Equal(ActionResult.Hit, state.Shoot(4, 4));
    Assert.Equal(ActionResult.Hit, state.Shoot(4, 5));
    Assert.Equal(ActionResult.Win, state.Shoot(4, 6));
  }

  [Fact]
  public void TargettingGridHasSeveralStates() {
    state = new GameState();
    state.OpponentGrid.PlaceShip(8, 1, 1, Direction.Vertical);

    // ensure everything is unknown area
    state.TargetGrid.AssertState(@"
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ");
    
    // test a hit
    state.Shoot(1, 1);
    state.TargetGrid.AssertState(@"
      ----------
      -x--------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ");
    
    // test a miss
    state.Shoot(1, 9);
    state.TargetGrid.AssertState(@"
      ----------
      -x-------o
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
      ----------
    ");
  }
}