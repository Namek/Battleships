namespace Battleships.Core;

public static class UIHelpers {
  /// <summary>
  /// Parse a string position like "A5" or "G3" to a pair of row and column, 0-based indices.
  /// </summary>
  /// <param name="input">the string should be already trimmed from whitespace</param>
  /// <param name="maxWidth"></param>
  /// <param name="maxHeight"></param>
  /// <returns></returns>
  public static (int, int)? ParsePosition(string input, int maxWidth, int maxHeight) {
    int row = input[0] - 'A';

    if (row < 0 || row >= maxHeight) {
      return null;
    }

    if (int.TryParse(input.Substring(1), out int col) && col is > 0 && col <= maxWidth) {
      return (row, col - 1);
    }

    return null;
  }
}