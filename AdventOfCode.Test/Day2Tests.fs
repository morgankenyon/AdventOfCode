module Day2Tests

open System
open Xunit

let getTestGame () =
    let round1: Day2.GameRound = 
        {
            Blue = 3; Red = 4; Green = 0
        }
    let round2: Day2.GameRound =
        {
            Red = 1; Green = 2; Blue = 6
        }
    let round3: Day2.GameRound =
        {
            Green = 2; Blue = 0; Red = 0
        }

    let game : Day2.Game =
        {
            GameNumber = 1;
            GameRounds = [ round1; round2; round3 ]
        }
    game
[<Fact>]
let ``Can parse game line`` () =
    let gameLine = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"

    let expectedGame = getTestGame()

    let actualGame = Day2.parseGameLine gameLine

    Assert.Equal(expectedGame.GameNumber, actualGame.GameNumber)
    Assert.Equal(expectedGame.GameRounds.Length, actualGame.GameRounds.Length)

[<Theory>]
[<InlineData("  3 blue, 4 red ", 3, 4, 0)>]
[<InlineData(" 1 red, 2 green, 6 blue ", 6, 1, 2)>]
[<InlineData(" 2 green", 0, 0, 2)>]
let ``Can parse game round`` (str, exBlue, exRed, exGreen) =
    let gameRound = Day2.parseGameRound str
    
    Assert.Equal(exBlue, gameRound.Blue)
    Assert.Equal(exRed, gameRound.Red)
    Assert.Equal(exGreen, gameRound.Green)

[<Fact>]
let ``Can find minimum cubes needed`` () =
    let game = getTestGame()

    let minimumCubes = Day2.findMinimumCubes game
    
    Assert.Equal(6, minimumCubes.Blue)
    Assert.Equal(4, minimumCubes.Red)
    Assert.Equal(2, minimumCubes.Green)

[<Fact>]
let ``Can find power of minimum cubes`` () =
    let minimumCubes: Day2.MinimumCubes = 
        {
            Red = 6; Blue = 4; Green = 2
        }

    let power = Day2.findPowerOfCubes minimumCubes

    Assert.Equal(48, power)