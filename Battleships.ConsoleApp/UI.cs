using Battleships.Core;

public static class UI {
  /// <summary>
  /// Returns size of the rendered frame.
  /// </summary>
  /// <param name="grid"></param>
  /// <param name="renderX"></param>
  /// <param name="renderY"></param>
  /// <returns></returns>
  public static (int, int) RenderRadar(this TargetGrid grid, int renderX, int renderY) {
    const int CELL_WIDTH = 2;

    var prevConsolePos = Console.GetCursorPosition();

    int curY = renderY;
    // column names: numbers
    for (int col = 1; col <= grid.Width; col += 1) {
      Console.SetCursorPosition(renderX + 3 + (col-1) * CELL_WIDTH, curY);
      Console.Write(col);
    }
    curY += 1;

    // top frame
    Console.SetCursorPosition(renderX + 1, curY);
    Console.Write('┌');
    Console.Write("".PadLeft(CELL_WIDTH * grid.Width + 1, '─'));
    Console.Write('┐');
    curY += 1;
    
    // rows
    for (int row = 0; row < grid.Height; row += 1, curY += 1) {
      Console.SetCursorPosition(renderX, curY);
      Console.Write((char)('A' + row));
      Console.Write('│');

      for (int col = 0; col < grid.Width; col += 1) {
        Console.Write(' ');
        
        char character = grid.GetState(row, col) switch {
          CellViewState.Unknown => '-',
          CellViewState.Hit => 'x',
          CellViewState.Missed => 'o',
          _ => throw new ArgumentOutOfRangeException()
        };
        Console.Write(character);
      }
      
      Console.Write(" │");
    }
    
    // bottom frame
    Console.SetCursorPosition(renderX + 1, curY);
    Console.Write('└');
    Console.Write("".PadLeft(CELL_WIDTH * grid.Width + 1, '─'));
    Console.Write('┘');
    
    Console.SetCursorPosition(prevConsolePos.Left, prevConsolePos.Top);

    return (grid.Width * CELL_WIDTH + 3, curY - renderY);
  }
}