module Day1

open System
open System.IO

let day1aDataFile = "data/day1a_data.txt"
let day1bTestDataFile = "data/day1b_testdata.txt"

let reverseString (x: string) =
    let charArray = x.ToCharArray()
    Array.Reverse(charArray)
    System.String charArray

let findDigit findFunc (x: string) =
    let firstDigit =
        Seq.toList x
        |> findFunc(fun x -> Char.IsDigit x)
    firstDigit

let findIndexMatch (orig: string) (search: string) =
    //let mutable index = -2
    //while index <> -1 do
    orig.IndexOf(search)

let findIndexMatches (orig: string) (search: string) =
    let mutable mutableOrig = orig
    let mutable indexes: int list = []
    let mutable index = -2
    let mutable offset = 0
    while index <> -1 do
        index <- mutableOrig.IndexOf(search, offset)
        if (index >= 0) then 
            indexes <- indexes @ [index]
            offset <- index + 1
        else ()
    indexes
    
let tryFindFirstDigit = findDigit Seq.tryFind
let findFirstDigit = findDigit Seq.find
let findFirstMatchIndex = findDigit Seq.findIndex
let findLastDigit = findDigit Seq.findBack
let findLastMatchIndex = findDigit Seq.findIndexBack

let parseFirstAndLastNum firstDigit lastDigit =
    let number = firstDigit.ToString() + "" + lastDigit.ToString()
    
    Int32.Parse number

let extractNumber (x: string) =
    let firstDigit = findFirstDigit x
    let lastDigit = findLastDigit x
    
    parseFirstAndLastNum firstDigit lastDigit

let findAllIndexesOfDigitWords (x: string) =
    let searchWords = [ "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"]
    let mutable matchingIndexes: (int * string) list = []
    for word in searchWords do
        let indexes = findIndexMatches x word

        match indexes with
        | [] -> ()
        | _ ->
            
            for index in indexes do
                matchingIndexes <- matchingIndexes @ [(index, word)]

    matchingIndexes
        |> List.sortBy (fun tup -> fst(tup))

let findDigitRepresentation str =
    match str with
    | "one" | "eno" -> "1"
    | "two" | "owt" -> "2"
    | "three" | "eerht" -> "3"
    | "four" | "ruof" -> "4"
    | "five" | "evif" -> "5"
    | "six" | "xis" -> "6"
    | "seven" | "neves" -> "7"
    | "eight" | "thgie" -> "8"
    | "nine" | "enin" -> "9"
    | _ -> ""

let replaceWordWithDigit (x: string) word =
    let mutable str = x
    let digit = findDigitRepresentation word
    let pos = str.IndexOf(word);
    if pos < 0 then
        str
    else
        str <- str.Substring(0, pos) + digit + str.Substring(pos + word.Length)
        str

let extractNumFromString (x: string) =
    let mutable str = x

    let allIndexes = findAllIndexesOfDigitWords x

    match allIndexes with
    | [] -> 
        extractNumber str
    | [ _ ] ->
        let indexPair = List.head(allIndexes)
        let word = snd indexPair
        str <- replaceWordWithDigit str word
        extractNumber str
    | _ ->
        let firstIndexPair = List.head allIndexes
        let firstWord = snd firstIndexPair
        let originalFirstDigit = tryFindFirstDigit str
        let tempStr = replaceWordWithDigit str firstWord
        let replacedFirstDigit = findFirstDigit tempStr
        let firstDigit = 
            if originalFirstDigit.IsNone then
                str <- tempStr
                replacedFirstDigit
            else if originalFirstDigit.Value = replacedFirstDigit then
                originalFirstDigit.Value
            else 
                str <- tempStr
                replacedFirstDigit
        
        str <- reverseString str
        let lastIndexPair = List.last allIndexes
        let lastWord = 
            snd lastIndexPair
            |> reverseString
        str <- replaceWordWithDigit str lastWord
        let newLastString = reverseString str
        let lastDigit = findLastDigit newLastString

        parseFirstAndLastNum firstDigit lastDigit

let find1ANum data =
    
    let sum = 
        data
        |> Seq.map(fun x -> extractNumber x)
        |> Seq.sum

    sum

let SolveDayA () =     
    let data = Util.loadDataFile day1aDataFile

    let sum = find1ANum data

    printfn "Day 1A solution: %d" sum

let find1BNum data =
    let sum =
        data
        |> Seq.map(fun x -> extractNumFromString x)
        |> Seq.sum
    sum
    
let SolveDayB () =
    let dataFile = Util.loadDataFile day1aDataFile

    let sum = find1BNum dataFile

    printfn "Day 1B solution: %d" sum

let SolveDay () = 
    printfn "Solving day 1's problem"
    
    SolveDayA()
    SolveDayB()