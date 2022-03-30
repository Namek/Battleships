using Battleships.Core;

const int BOARD_WIDTH = 10;
const int BOARD_HEIGHT = 10;
const int SIZE_BATTLESHIP = 5;
const int SIZE_DESTROYER = 4;

var state = new GameState(BOARD_WIDTH, BOARD_HEIGHT);
state.OpponentGrid.PlaceShip(SIZE_BATTLESHIP, 2, 2, Direction.Horizontal);
state.OpponentGrid.PlaceShip(SIZE_DESTROYER, 3, 1, Direction.Horizontal);
state.OpponentGrid.PlaceShip(SIZE_DESTROYER, 4, 2, Direction.Vertical);

int shootCount = 0;
while (true) {
  var input = Console.ReadLine()?.Trim().ToUpper();

  if (input == null) {
    return;
  }

  var pos = UIHelpers.ParsePosition(input, state.TargetGrid.Width, state.TargetGrid.Height);
  if (pos is null) {
    var (from, to) = UIHelpers.GetPositionBoundaries(state.TargetGrid.Width, state.TargetGrid.Height); 
    Console.WriteLine($"Incorrect position. Please provide a position between {from} and {to}");
    continue;
  }

  shootCount += 1;

  var (row, col) = pos.Value;
  switch (state.Shoot(row, col)) {
    case ActionResult.AlreadyShot:
      Console.WriteLine("You have already shot that position. Pick another one.");
      break;
    case ActionResult.Miss:
      Console.WriteLine($"{shootCount}. shoot {input}: miss!");
      Console.Write(state.TargetGrid.ToString());
      break;
    case ActionResult.Hit:
      Console.WriteLine($"{shootCount}. shoot {input}: hit!");
      Console.Write(state.TargetGrid.ToString());
      break;
    case ActionResult.Sink:
      Console.WriteLine($"{shootCount}. shoot {input}: ship sink!");
      Console.Write(state.TargetGrid.ToString());
      break;
    case ActionResult.Win:
      Console.WriteLine();
      Console.WriteLine($"Congratulations. You have destroyed all the enemy ships with {shootCount} shots.");
      Console.Write(state.TargetGrid.ToString());
      Console.WriteLine("Press any key to finish the program.");
      Console.ReadKey();
      return;
    default:
      throw new ArgumentOutOfRangeException();
  }
}