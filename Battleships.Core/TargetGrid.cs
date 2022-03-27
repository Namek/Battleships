using System.Text;

namespace Battleships.Core;

public class TargetGrid {
  public int Width { get; }
  public int Height { get; }
  
  private readonly CellViewState[] _grid;

  public CellViewState GetState(int row, int col) {
    return _grid[Width * row + col];
  }

  public void SetState(int row, int col, CellViewState state) {
    _grid[Width * row + col] = state;
  }

  public TargetGrid(int width, int height) {
    this.Width = width;
    this.Height = height;
    this._grid = new CellViewState[Width * Height];
  }
  
  public override string ToString() {
    var sb = new StringBuilder();

    for (int i = 0, col = 0; i < _grid.Length; i += 1) {
      var character = _grid[i] switch {
        CellViewState.Unknown => '-',
        CellViewState.Hit => 'x',
        CellViewState.Missed => 'o',
        _ => throw new ArgumentOutOfRangeException()
      };
      sb.Append(character);

      col += 1;
      
      if (col == Width) {
        col = 0;
        sb.Append('\n');
      }
    }

    return sb.ToString();
  }
}

public enum CellViewState {
  Unknown,
  Hit,
  Missed,
}