using Battleships.Core;

const int BOARD_WIDTH = 10;
const int BOARD_HEIGHT = 10;
const int SIZE_BATTLESHIP = 5;
const int SIZE_DESTROYER = 4;

const int RADAR_X = 67;
const int RADAR_Y = 1;

var state = new GameState(BOARD_WIDTH, BOARD_HEIGHT);
state.OpponentGrid.PlaceRandomShips(new Random().Next(), new Dictionary<int, int>() {
  [SIZE_BATTLESHIP] = 1,
  [SIZE_DESTROYER] = 2,
});

Console.Clear();
Console.SetCursorPosition(0, 0);
Console.Write(" ".PadLeft(Console.BufferWidth, ' '));
Console.WriteLine(@"
                                    )___(
                             _______/__/_
                    ___     /===========|   ___
   ____       __   [\\\]___/____________|__[///]   __
   \   \_____[\\]__/___________________________\__[//]___
    \                 Battleships v0.5                   |
     \                (""a half of it"")                 /
  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
");

var (boundaryFrom, boundaryTo) = UIHelpers.GetPositionBoundaries(state.TargetGrid.Width, state.TargetGrid.Height);
var (radarWidth, radarHeight) = state.TargetGrid.RenderRadar(RADAR_X, RADAR_Y);
Console.SetCursorPosition(0, RADAR_Y + radarHeight);
Console.WriteLine($"Captain, we should target positions from {boundaryFrom} to {boundaryTo}!\n");

int shootCount = 0;
while (true) {
  Console.Write($"{shootCount + 1}. ");
  Console.Write(shootCount == 0 ? "What is our next target position? " : "Next? ");
  var posBeforeInput = Console.GetCursorPosition();
  var input = Console.ReadLine()?.Trim().ToUpper();

  if (input == null) {
    return;
  }

  var targetPos = UIHelpers.ParsePosition(input, state.TargetGrid.Width, state.TargetGrid.Height);
  if (targetPos is null) {
    Console.WriteLine($"`{input}` is an incorrect position, sir.");
    continue;
  }

  // move back to the previous line
  Console.SetCursorPosition(posBeforeInput.Left + input.Length + 2, posBeforeInput.Top);

  shootCount += 1;

  var (row, col) = targetPos.Value;
  var shootResult = state.Shoot(row, col);
  state.TargetGrid.RenderRadar(RADAR_X, RADAR_Y);

  switch (shootResult) {
    case ActionResult.AlreadyShot:
      Console.WriteLine("\nWe have already shot that position. Please pick another one.");
      break;
    case ActionResult.Miss:
      Console.WriteLine($"Miss!");
      break;
    case ActionResult.Hit:
      Console.WriteLine($"We have a hit!");
      break;
    case ActionResult.Sink:
      Console.WriteLine($"A ship sunk!");
      break;
    case ActionResult.Win:
      Console.WriteLine();
      Console.WriteLine($"Congratulations. You have destroyed all the enemy ships with {shootCount} shots.\n");
      Console.WriteLine("Press any key to finish the program.");
      Console.ReadKey();
      return;
    default:
      throw new ArgumentOutOfRangeException();
  }
}