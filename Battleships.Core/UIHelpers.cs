namespace Battleships.Core;

public static class UIHelpers {
  /// <summary>
  /// Parse a string position like "A5" or "G3" to a pair of row and column, 0-based indices.
  /// </summary>
  /// <param name="input">the string should be already trimmed from whitespace</param>
  /// <param name="width"></param>
  /// <param name="height"></param>
  /// <returns></returns>
  public static (int, int)? ParsePosition(string input, int width, int height) {
    if (input.Length == 0) {
      return null;
    }

    int row = input[0] - 'A';

    if (row < 0 || row >= height) {
      return null;
    }

    if (int.TryParse(input.Substring(1), out int col) && col is > 0 && col <= width) {
      return (row, col - 1);
    }

    return null;
  }

  /// <summary>
  /// Returns 'from' and 'to' boundaries for a user input.
  /// For example, for width=10, height=5 it is (A1,E10).
  ///
  /// <returns>For incorrect size (smaller than 1) it returns (A1,A1)</returns>
  /// </summary>
  public static (string, string) GetPositionBoundaries(int width, int height) {
    string from = "A1";
    string to;
    if (width > 0 && height > 1) {
      char row = (char) ('A' + height - 1);
      int col = Math.Max(1, width);
      to = $"{row}{col}";
    } else {
      to = from;
    }

    return (from, to);
  }
}