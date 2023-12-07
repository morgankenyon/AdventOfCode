module Day4Tests

open System
open Xunit

let dayDataFile = "data/day4_data.txt"
let dayTestDataFile = "data/day4_testdata.txt"

[<Fact>]
let ``Can extract numbers`` () =
    let line = "83 86  6 31 17  9 48 53"

    let numbers = Day4.extractNumbers(line)

    Assert.Equal([83; 86; 6; 31; 17; 9; 48; 53], numbers)

[<Fact>]
let ``Can parse card line`` () =
    let line = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"
    let expectedWinningNumbers = [17; 41; 48; 83; 86]

    let scratchoff = Day4.parseCardLine(line)

    let actualWinningNumbers = Set.toList scratchoff.WinningNumbers
    Assert.Equal("Card 1", scratchoff.Title)
    Assert.Equal<list<int>>(expectedWinningNumbers, actualWinningNumbers)
    Assert.Equal([83; 86; 6; 31; 17; 9; 48; 53], scratchoff.YourNumbers)

[<Fact>]
//[<InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", 8)>]
let ``Can score scratchoff`` () =
    let expectedWinningNumbers = [17; 41; 48; 83; 86]
    let scratchoff: Day4.Scratchoff = 
        {
            Title = "Card 1"
            WinningNumbers = Set.ofList expectedWinningNumbers
            YourNumbers = Array.ofList [83; 86; 6; 31; 17; 9; 48; 53]
        }
    
    let score = Day4.scoreGame scratchoff

    Assert.Equal(8, score)


[<Theory>]
[<InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", 8)>]
[<InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", 2)>]
[<InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", 2)>]
[<InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", 1)>]
[<InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", 0)>]
[<InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 0)>]
let ``Can score game`` (line, expected) =
    let score = Day4.parseAndScoreGame line

    Assert.Equal(expected, score)
    