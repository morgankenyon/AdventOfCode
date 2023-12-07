module Day4

open System


let dayDataFile = "data/day4_data.txt"
let dayTestDataFile = "data/day4_testdata.txt"

type Scratchoff =
    {
        Title : string
        WinningNumbers : int Set
        YourNumbers : int array
    }

let extractNumbers (numberLine: string) =
    numberLine.Split(" ")
    |> Array.filter (fun num -> not (String.IsNullOrWhiteSpace num))
    |> Array.map Int32.Parse

let parseCardLine (line: string) =
    let parts = line.Split(":")
    let card = parts[0]
    let numbers = parts[1].Split("|")
    let winningNumbers = 
        extractNumbers numbers[0]
        |> Set.ofArray

    let yourNumbers = extractNumbers numbers[1]
    {
        Title = card; WinningNumbers = winningNumbers; YourNumbers = yourNumbers
    }

let scoreGame scratchoff =
    let yourWinningNumbers =
        scratchoff.YourNumbers
        |> Array.filter (fun num -> scratchoff.WinningNumbers.Contains num)
    let power = yourWinningNumbers.Length - 1
    let score = Math.Pow(2,power)
    (int)score

let parseAndScoreGame line =
    line
    |> parseCardLine
    |> scoreGame


let SolveDayA () =
    let scorecardScore =
        Util.loadAllData dayDataFile
        |> Array.map parseAndScoreGame
        |> Array.sum
    printfn "Day 4A solution: %d" scorecardScore

let SolveDayB () =
    //let data = Util.loadDataFile day3TestDataFile

    printfn "Day 4B solution: %d" 22

let SolveDay () = 
    printfn "Solving day 3's problem"
    
    SolveDayA()
    SolveDayB()