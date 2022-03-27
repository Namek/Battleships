using System.Text;

namespace Battleships.Core;

public class OceanGrid {
  public const int DEFAULT_WIDTH = 10;
  public const int DEFAULT_HEIGHT = 10;

  private const int EMPTY_CELL_ID = 0;

  public int Width { get; }
  public int Height { get; }

  /// <summary>
  /// Full grid with IDs of ships on each cell. If cell equals zero (0) then there is no ship.
  /// </summary>
  private readonly int[] _grid;

  private int _lastShipId = 0;
  private readonly Dictionary<int, int> _shipIdToLeftHits = new Dictionary<int, int>();

  public OceanGrid(int width, int height) {
    Width = width;
    Height = height;
    _grid = new int[width * height];
  }

  public OceanGrid() : this(DEFAULT_WIDTH, DEFAULT_HEIGHT) {
  }

  public OceanShootResult Shoot(int row, int col) {
    int id = _grid[Width * row + col];
    if (id == EMPTY_CELL_ID) {
      return OceanShootResult.Miss;
    }

    var life = _shipIdToLeftHits[id] - 1;
    if (life > 0) {
      _shipIdToLeftHits[id] = life;
      return OceanShootResult.Hit;
    }

    // Remove the dead ship from the list so for the `LastSink` case
    // we don't have to check all the life amounts of the left ships.
    _shipIdToLeftHits.Remove(id);
    return _shipIdToLeftHits.Count > 0 ? OceanShootResult.Sink : OceanShootResult.LastSink;
  }

  public bool IsEmptyCell(int row, int col) {
    int id = _grid[Width * row + col];
    return id == EMPTY_CELL_ID;
  }

  /// <returns>true if placement succeeded, otherwise false</returns>
  public bool PlaceShip(int shipSize, int startRow, int startCol, Direction direction) {
    bool isHorz = direction == Direction.Horizontal;
    int lastRow, lastCol;
    int dirCol, dirRow;

    if (isHorz) {
      lastRow = startRow;
      lastCol = startCol + shipSize - 1;
      dirRow = 0;
      dirCol = 1;
    } else {
      lastRow = startRow + shipSize - 1;
      lastCol = startCol;
      dirRow = 1;
      dirCol = 0;
    }

    if (lastRow > Height - 1 || lastCol > Width - 1) {
      return false;
    }

    for (int i = 0; i < shipSize; i += 1) {
      int row = startRow + dirRow * i;
      int col = startCol + dirCol * i;
      if (GetCellShipId(row, col).HasValue) {
        return false;
      }
    }

    int shipId = ++_lastShipId;

    for (int i = 0; i < shipSize; i += 1) {
      int row = startRow + dirRow * i;
      int col = startCol + dirCol * i;
      SetCellShipId(row, col, shipId);
    }

    _shipIdToLeftHits[shipId] = shipSize;

    return true;
  }

  private int? GetCellShipId(int row, int col) {
    int id = _grid[Width * row + col];
    return id == EMPTY_CELL_ID ? null : id;
  }

  private void SetCellShipId(int row, int col, int? shipId) {
    _grid[Width * row + col] = shipId ?? EMPTY_CELL_ID;
  }

  // for debugging purposes
  public override string ToString() {
    var sb = new StringBuilder();

    for (int i = 0, col = 0; i < _grid.Length; i += 1) {
      sb.Append(_grid[i] == EMPTY_CELL_ID ? '-' : 'x');

      col += 1;

      if (col == Width) {
        col = 0;
        sb.Append('\n');
      }
    }

    return sb.ToString();
  }
}

public enum OceanShootResult {
  Hit,
  Miss,
  Sink,
  LastSink
}