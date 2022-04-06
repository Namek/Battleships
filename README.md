# Battleships v0.5 (version "a half of it")

A simplified version of the paper Battleships game:
1. computer player places ships on the map of size 10x10
2. human player shoots them down by guessing ship positions
3. when everything is shot down, the human player sees a congratulations message with the shot count. 

In other words, the simplification means this is a one-sided game. The computer player does not shoot in response to the human player's moves. Also, the human player does not own ships. Shoots from the cosmos, probably.

![Battleships](/docs/screenshot.png "How it looks like")

## Tech

Console application written in current C# with .NET 6. Focuses on tests for logic.

Tests are written mainly for the game logic, less for the "UI".


## Launch

Can be started with `dotnet run` from the `Battleships.ConsoleApp` subfolder.


## Potential improvements

1. UI without scrolling so the radar could be visible all the time.
2. Correct rendering after window gets resized.
3. Ensure ship placement is as random as possible.
4. A better algorithm for tight ship placement (the Knapsack problem).
