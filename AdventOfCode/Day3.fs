module Day3

open System

let day3DataFile = "data/day3_data.txt"
let day3TestDataFile = "data/day3_testdata.txt"

type UniqueNumberGroup =
    {
        Id : Guid
        Number : int
    }

let getNumberGroupValue (row: char array) (numberGroup : int list) =
    numberGroup 
    |> List.map (fun i -> Int32.Parse(row[i].ToString()))
    
let getNumberGroupValues (schematic: char array array) (rowIndex: int) (numberGroup : int list) =
    let row = schematic[rowIndex]
    numberGroup 
    |> List.map (fun i -> Int32.Parse(row[i].ToString()))

    
let convertNumberGroupToNumber (numberGroup : int list) =
    ("", numberGroup)
    ||> List.fold (fun a num -> a + num.ToString())
    |> Int32.Parse

let findNumberGroups (line : char array) =
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

let addNumberGroupToMap (numberGroupMap : Map<int, UniqueNumberGroup>) (currentGroup : int list) number =
    let mutable groupMap = numberGroupMap
    let uniqueNumberGroup = { Id = Guid.NewGuid(); Number = number }
    for group in currentGroup do
        groupMap <- groupMap.Add(group, uniqueNumberGroup)
    groupMap
        
let findNumberGroupMaps (line : char array) =
    let mutable numberGroupMap = Map.empty<int, UniqueNumberGroup>
    let mutable currentGroup : int list = []
    for i in 0 .. (line.Length - 1) do
        let value = line.[i]
        if (Char.IsDigit value) then
            currentGroup <- currentGroup @ [i]
            ()
        else if currentGroup.Length > 0 then
            let number = 
                getNumberGroupValue line currentGroup
                |> convertNumberGroupToNumber
            numberGroupMap <- addNumberGroupToMap numberGroupMap currentGroup number
            currentGroup <- []
            ()
        else
            ()
    if currentGroup.Length > 0 then
        let number = 
            getNumberGroupValue line currentGroup
            |> convertNumberGroupToNumber
        numberGroupMap <- addNumberGroupToMap numberGroupMap currentGroup number
        ()
    else
        ()
    numberGroupMap

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

//let normalizeDigit char =
//    match Char.IsDigit(char) with
//    | true -> 1
//    | false -> 0

//let transformDigits (charArray : char array) =
//    charArray
//    |> Array.map normalizeDigit

let getThreeSpotsNumberGroups (numberGroupMaps : Map<int, UniqueNumberGroup>) (column : int) =
    //get current column
    let currentGroup = numberGroupMaps.TryFind column

    //get previous column
    let previousGroup = numberGroupMaps.TryFind (column - 1)

    //get next column
    let nextGroup = numberGroupMaps.TryFind (column + 1)

    let uniqueGroups = 
        [ currentGroup; previousGroup; nextGroup ]
        |> List.choose id
        |> List.groupBy (fun gr -> gr.Id)
        |> List.map (fun (key, values) -> List.tryHead values)
        |> List.choose id

    uniqueGroups

let getAdjacentNumberGroups (numberGroupMaps : Map<int, UniqueNumberGroup> array) (row : int) (column : int) =
    //get current row
    let mutable uniqueNumberGroups = getThreeSpotsNumberGroups numberGroupMaps[row] column

    //get previous row
    if (row > 0) then
        let previousUniqueRowNumberGroups = getThreeSpotsNumberGroups numberGroupMaps[row - 1] column
        uniqueNumberGroups <- uniqueNumberGroups @ previousUniqueRowNumberGroups
        ()
    else
        ()

    //get next row 
    if (numberGroupMaps.Length - 1> row) then
        let nextUniqueRowNumberGroups = getThreeSpotsNumberGroups numberGroupMaps[row + 1] column
        uniqueNumberGroups <- uniqueNumberGroups @ nextUniqueRowNumberGroups
        ()
    else
        ()
    uniqueNumberGroups

let findAllGearRatios (data : string array) =
    let numberGroupMaps : Map<int, UniqueNumberGroup> array = 
        data
        |> Array.map (fun l -> l.ToCharArray())
        |> Array.map findNumberGroupMaps
    
    let mutable runningGearRatio = 0
    for i in 0 .. (data.Length - 1) do
        let line = data.[i]

        let lineArray = line.ToCharArray()
        for j in 0 .. (lineArray.Length - 1) do
            let charr = lineArray[j]

            if (charr = '*') then
                let adjacentNumberGroups = getAdjacentNumberGroups numberGroupMaps i j
                if adjacentNumberGroups.Length = 2 then
                    let gearRatio = 
                        (1, adjacentNumberGroups)
                        ||> List.fold (fun o n -> o * n.Number)
                    runningGearRatio <- runningGearRatio + gearRatio
                    ()
                else
                    ()
            else
                ()
    runningGearRatio

let SolveDayB () =
    let data = Util.loadDataFile day3DataFile

    
    let allGearRatios = 
        Seq.toArray data
        |> findAllGearRatios

    printfn "Day 3B solution: %d" allGearRatios

let SolveDay () = 
    printfn "Solving day 3's problem"
    
    SolveDayA()
    SolveDayB()