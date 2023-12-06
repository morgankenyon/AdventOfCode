module Day2

open System

let day2aDataFile = "data/day2a_data.txt"
let day2bTestDataFile = "data/day2a_testdata.txt"

type GameRound =
    {
        Blue : int
        Red : int
        Green : int
    }
type MinimumCubes = GameRound
type Game =
    {
        GameNumber : int;
        GameRounds : GameRound list
    }

let parseGameRound (gameRound : string) =
    let numColors = gameRound.Split(",")
    let mutable red = 0
    let mutable blue = 0
    let mutable green = 0

    for numColor in numColors do
        let ncs = numColor.Trim().Split(" ")
        let num = Int32.Parse(ncs[0])
        let color = ncs[1]
        if color = "blue" then
            blue <- num
        else if color = "red" then
            red <- num
        else
            green <- num
    let round = 
        {
            Blue = blue; Red = red; Green = green
        }
    round

let parseGameRounds (gameRound :string) =
    let mutable parsedRounds: GameRound list = []
    let gameRounds = gameRound.Trim().Split(";")
    for gameRound in gameRounds do
        let round = parseGameRound gameRound
        parsedRounds <- parsedRounds @ [round]
    parsedRounds

let parseGameLine (gameStr : string) =
    let parts = gameStr.Split(":")
    let game = parts[0]
    let rounds = parts[1]

    let gameNumber = 
        game.Split(" ")[1]
        |> Int32.Parse

    let gameRounds = parseGameRounds rounds

    let game : Game =
        {
            GameNumber = gameNumber;
            GameRounds = gameRounds
        }
    game

let findPossibleGames (games : seq<Game>) loadedRound =
    let mutable possibleSum = 0
    for game in games do
        let mutable gameImpossible = false
        for round in game.GameRounds do
            if round.Blue <= loadedRound.Blue && round.Red <= loadedRound.Red && round.Green <= loadedRound.Green then
                ()
            else
                gameImpossible <- true
                ()
        if gameImpossible then
            ()
        else
            possibleSum <- possibleSum + game.GameNumber
    possibleSum

let findMinimumCubes (game : Game) =
    let mutable minRed = 0
    let mutable minBlue = 0
    let mutable minGreen = 0

    for round in game.GameRounds do
        minRed <- Math.Max(minRed, round.Red)
        minBlue <- Math.Max(minBlue, round.Blue)
        minGreen <- Math.Max(minGreen, round.Green)

    let minimumCubes : MinimumCubes = { Red = minRed; Blue = minBlue; Green = minGreen }

    minimumCubes

let findPowerOfCubes (cubes : MinimumCubes) =
    cubes.Blue * cubes.Red * cubes.Green

let parseGames data =
    
    let games = 
        data
        |> Seq.map(fun x -> parseGameLine x)
    games

let SolveDayA () =
    let data = Util.loadDataFile day2aDataFile

    let games = parseGames data

    let loadedRound = 
        { 
            Red = 12; Green = 13; Blue = 14
        }

    let possibleGameSum = findPossibleGames games loadedRound

    printfn "Day 2A solution: %d" possibleGameSum

let SolveDayB () =
    let data = Util.loadDataFile day2aDataFile

    let games = parseGames data

    let powerSum =
        games
        |> Seq.map findMinimumCubes
        |> Seq.map findPowerOfCubes
        |> Seq.sum

    printfn "Day 2B solution: %d" powerSum

let SolveDay () = 
    printfn "Solving day 2's problem"
    
    SolveDayA()
    SolveDayB()