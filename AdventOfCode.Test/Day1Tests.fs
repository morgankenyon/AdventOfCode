module Day1Tests

open System
open Xunit

[<Theory>]
[<InlineData("irst2ir9rin2", 22)>]
[<InlineData("1abc2", 12)>]
[<InlineData("pqr3stu8vwx", 38)>]
[<InlineData("a1b2c3d4e5f", 15)>]
[<InlineData("treb7uchet", 77)>]
let ``Can extract 1a number`` (str, expected) =
    let num = Day1.extractNumber str

    Assert.Equal(expected, num)

[<Fact>]
let ``Can solve 1a example problem`` () =
    let strs = [| "1abc2"; "pqr3stu8vwx"; "a1b2c3d4e5f"; "treb7uchet" |]

    let sum = 
        Array.toSeq strs
        |> Day1.find1ANum 

    Assert.Equal(142, sum)

[<Theory>]
[<InlineData("eightwothree", "eight", 0)>]
[<InlineData("eightwothree", "five", -1)>]
let ``Can find index of substring`` (str, search, expectedIndex) =
    let index = Day1.findIndexMatch str search

    Assert.Equal(expectedIndex, index)

[<Theory>]
[<InlineData("eighttwothree", "eight", "0")>]
[<InlineData("eighteight", "eight", "0;5")>]
[<InlineData("xgjjmnlvznf2nineltmsevenine", "nine", "12;23")>]
//[<InlineData("xgjjmnlvznf2nineltmsevenine", "nine")>]
let ``Can find multiple indexes of substring`` (str: string, search: string, expectedIndexexStr: string) =
    let expectedIndexes = 
        expectedIndexexStr.Split(";") 
        |> Array.toList
        |> List.map (fun num -> Int32.Parse num)

    let indexes = Day1.findIndexMatches str search

    Assert.Equal<List<int>>(expectedIndexes, indexes)

[<Fact>]
let ``Can find multiple indexes return empty list for no match`` () =
    let expected: int list = []

    let indexes = Day1.findIndexMatches "eight" "two"

    Assert.Equal<List<int>>(expected, indexes)

[<Fact>]
let ``Can find all indexes of words in str`` () =
    let expectedIndexes = [(0, "eight"); (4, "two"); (7, "three") ]

    let str = "eightwothree"

    let indexes = Day1.findAllIndexesOfDigitWords str

    let expectedSeq = List.toSeq expectedIndexes
    let actualSeq = List.toSeq indexes

    Assert.Equal<seq<int * string>>(expectedSeq, actualSeq)

[<Fact>]
let ``Can find all indexes of duplicated word in str`` () =
    let expectedIndexes = [(0, "eight"); (5, "eight") ]

    let str = "eighteight"

    let indexes = Day1.findAllIndexesOfDigitWords str

    let expectedSeq = List.toSeq expectedIndexes
    let actualSeq = List.toSeq indexes

    Assert.Equal<seq<int * string>>(expectedSeq, actualSeq)

[<Fact>]
let ``Can find all indexes in tricky string`` () =
    let expectedIndexes = [(12, "nine"); (19, "seven"); (23, "nine")]

    let str = "xgjjmnlvznf2nineltmsevenine"

    let indexes = Day1.findAllIndexesOfDigitWords str

    Assert.Equal<List<int * string>>(expectedIndexes, indexes)

[<Theory>]
[<InlineData("xgjjmnlvznf2nineltmsevenine", "nine", "xgjjmnlvznf29ltmsevenine")>]
let ``Can replace word with digit`` (str, word, expected) =
    let actual = Day1.replaceWordWithDigit str word 

    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData("eightwothree", 83)>]
[<InlineData("abc2x3oneight", 28)>]
[<InlineData("1eightwo", 12)>]
[<InlineData("nine9ninesix6xmgbsgfmpgxkzgpzlxqnjsqhr", 96)>]
[<InlineData("fivezvmqbczkgclsxfour3eightthreethree", 53)>]
[<InlineData("436", 46)>]
[<InlineData("m5ffive", 55)>]
[<InlineData("mffive", 55)>]
[<InlineData("zstrmphtxdvdpsnhpnq4threenbjznsb", 43)>]
[<InlineData("zstrmphtxdvdpsnhpnqthreenbjznsb", 33)>]
[<InlineData("zstrmphtxdvdpsnhpnq4threnbjznsb", 44)>]
[<InlineData("9g", 99)>]
[<InlineData("eightwo", 88)>]
//[<InlineData("twone", 21)>]
[<InlineData("xgjjmnlvznf2nineltmsevenine", 29)>] //reddit
[<InlineData("36twonine", 39)>] //reddit
[<InlineData("3nqqgfone", 31)>] //reddit
[<InlineData("mkfone4ninefour", 14)>] //reddit
[<InlineData("kgnprzeight7nine", 89)>] //reddit
let ``Can turn words into digits`` (str, expected) =
    let digit = Day1.extractNumFromString str

    Assert.Equal(expected, digit)

[<Theory>]
[<InlineData("xgjjmnlvznf2nineltmsevenine", 29)>] //reddit
[<InlineData("36twonine", 39)>] //reddit
[<InlineData("3nqqgfone", 31)>] //reddit
[<InlineData("mkfone4ninefour", 14)>] //reddit
[<InlineData("kgnprzeight7nine", 89)>] //reddit
let ``Can turn words into digits:reddit test cases`` (str, expected) =
    let digit = Day1.extractNumFromString str

    Assert.Equal(expected, digit)

[<Theory>]
[<InlineData("one", "1")>]
[<InlineData("eno", "1")>]
[<InlineData("two", "2")>]
[<InlineData("owt", "2")>]
[<InlineData("three", "3")>]
[<InlineData("eerht", "3")>]
[<InlineData("four", "4")>]
[<InlineData("ruof", "4")>]
[<InlineData("five", "5")>]
[<InlineData("evif", "5")>]
[<InlineData("six", "6")>]
[<InlineData("xis", "6")>]
[<InlineData("seven", "7")>]
[<InlineData("neves", "7")>]
[<InlineData("eight", "8")>]
[<InlineData("thgie", "8")>]
[<InlineData("nine", "9")>]
[<InlineData("enin", "9")>]
let ``Can findDigitRepresentation`` str expected =
    let digit = Day1.findDigitRepresentation str

    Assert.Equal(expected, digit)

[<Fact>]
let ``Can solve 1b example problem`` () =
    let strs = [| "two1nine" ;"eightwothree" ;"abcone2threexyz" ;"xtwone3four" ;"4nineeightseven2" ;"zoneight234" ;"7pqrstsixteen" |]

    let sum = Day1.find1BNum strs

    Assert.Equal(281, sum)