
let print s = 
    printfn s

let runDaysProblems day =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    match day with
    | "1" -> Day1.SolveDay()
    | "1a" -> Day1.SolveDayA()
    | "1b" -> Day1.SolveDayB()
    | "2" -> Day2.SolveDay()
    | "2a" -> Day2.SolveDayA()
    | "2b" -> Day2.SolveDayB()
    | "3" -> Day3.SolveDay()
    | "3a" -> Day3.SolveDayA()
    | "3b" -> Day3.SolveDayB()
    | _ -> printfn "%s" "Not a currently solved problem"
    stopWatch.Stop()
    printfn "Milliseconds to solve: %f" stopWatch.Elapsed.TotalMilliseconds

let validateArgs (args : string array) =
    match args.Length with
    | 1 -> Ok args.[0]
    //| 0 -> Error "Please specify which days problem you want to solve"
    | 0 -> Ok "3a"
    | _ -> Error "Cannot pass multiple flags to this program"

[<EntryPoint>]
let main args =
    printfn "Arguments passed to function : %A" args

    let validationResult = validateArgs args

    match validationResult with
    | Ok problem -> runDaysProblems problem
    | Error msg -> printfn "%s" msg
    // Return 0. This indicates success.
    0