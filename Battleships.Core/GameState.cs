namespace Battleships.Core;

public class GameState {
  public OceanGrid OpponentGrid { get; }
  public TargetGrid TargetGrid { get; }


  public GameState(int width, int height) {
    OpponentGrid = new OceanGrid(width, height);
    TargetGrid = new TargetGrid(width, height);
  }

  public GameState() : this(OceanGrid.DEFAULT_WIDTH, OceanGrid.DEFAULT_HEIGHT) {
  }

  /// <summary>
  /// Shoot opponent grid.
  /// </summary>
  public ActionResult Shoot(int row, int col) {
    if (TargetGrid.GetState(row, col) != CellViewState.Unknown) {
      return ActionResult.AlreadyShot;
    }

    var oceanShoot = OpponentGrid.Shoot(row, col);
    TargetGrid.SetState(row, col, oceanShoot == OceanShootResult.Miss ? CellViewState.Missed : CellViewState.Hit);

    return oceanShoot switch {
      OceanShootResult.Miss => ActionResult.Miss,
      OceanShootResult.Hit => ActionResult.Hit,
      OceanShootResult.Sink => ActionResult.Sink,
      OceanShootResult.LastSink => ActionResult.Win,
      _ => ActionResult.Miss
    };
  }
}

public enum ActionResult {
  AlreadyShot,
  Miss,
  Hit,
  Sink,
  Win,
}