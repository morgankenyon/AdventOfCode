module Day3

open System

let day3DataFile = "data/day3_data.txt"
let day3TestDataFile = "data/day3_testdata.txt"

let findNumberGroups (line : char array) =
    let mutable inNumber = false
    let mutable numberGroups : int list list = []
    let mutable currentGroup : int list = []
    for i in 0 .. (line.Length - 1) do
        let value = line.[i]
        if (Char.IsDigit value) then
            currentGroup <- currentGroup @ [i]
            ()
        else if currentGroup.Length > 0 then
            numberGroups <- numberGroups @ [currentGroup]
            currentGroup <- []
            ()
        else
            ()
    if currentGroup.Length > 0 then
        numberGroups <- numberGroups @ [currentGroup]
        ()
    else
        ()
    numberGroups

let isSymbol testChar =
    let isDigit = Char.IsDigit testChar
    let isPeriod = testChar = '.'
    not (isDigit || isPeriod)

let isSymbolInThreePlaces (schematic : char array array) (row : int) (column : int) =
    
    //test current column
    let symbolInCurrentColumn = isSymbol (schematic[row][column])

    //test previous column

    let symbolInPreviousColumn = 
        if ((column - 1) >= 0) then
            isSymbol (schematic[row][column-1])
        else
            false

    //test next column
    let symbolInNextColumn =
        if (schematic[row].Length > column + 1) then
            isSymbol (schematic[row][column + 1])
        else
            false

    symbolInCurrentColumn || symbolInPreviousColumn || symbolInNextColumn

let isBySymbol (schematic : char array array) (row : int) (column : int) =
    let mutable bySymbol = false
    //previous row
    if (row - 1 >= 0) then
        let previousRow = row - 1
        bySymbol <- bySymbol || isSymbolInThreePlaces schematic previousRow column
        ()
    else
        ()

    //current row
    bySymbol <- bySymbol || isSymbolInThreePlaces schematic row column

    //next row
    if (schematic.Length - 1) > row then
        let nextRow = row + 1
        bySymbol <- bySymbol || isSymbolInThreePlaces schematic nextRow column
        ()
    else
        ()

    bySymbol

let isNumberGroupBySymbol (schematic : char array array) (row : int) (numberGroup : int list)  =
    let mutable numberGroupBySymbol = false
    for i in numberGroup do
        numberGroupBySymbol <- numberGroupBySymbol || isBySymbol schematic row i
    numberGroupBySymbol

let transformInput (data : string array) =
    data
    |> Array.map (fun i -> i.ToCharArray())

let getNumberGroupValues (schematic: char array array) (rowIndex: int) (numberGroup : int list) =
    let row = schematic[rowIndex]
    numberGroup 
    |> List.map (fun i -> Int32.Parse(row[i].ToString()))
    
let convertNumberGroupToNumber (numberGroup : int list) =
    ("", numberGroup)
    ||> List.fold (fun a num -> a + num.ToString())
    |> Int32.Parse
    

let SolveDayA () =
    let data = Util.loadAllData day3DataFile

    let schematic = transformInput data

    let mutable sum = 0
    
    for i in 0 .. (schematic.Length - 1) do
        let line = schematic.[i]
        let numberGroups = findNumberGroups line

        for numberGroup in numberGroups do
            let bySymbol = isNumberGroupBySymbol schematic i numberGroup
            if bySymbol then
                let numberGroupValues = getNumberGroupValues schematic i numberGroup
                let num = convertNumberGroupToNumber numberGroupValues
                sum <- sum + num
                ()
            else
                ()

    printfn "Day 3A solution: %d" sum

let SolveDayB () =
    let data = Util.loadDataFile day3TestDataFile

    printfn "Day 3B solution: %d" 22

let SolveDay () = 
    printfn "Solving day 3's problem"
    
    SolveDayA()
    SolveDayB()