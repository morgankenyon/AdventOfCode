module Day3Tests

open System
open Xunit

let generateSchematic () =
    let inputs = [| "467..114.."; "...*......"; "..35..633." |]
    Day3.transformInput inputs
let generateTestSchematic () =
    let inputs = [| "467..114.."; "...*......"; "..35..633."; "*........*" |]
    Day3.transformInput inputs
[<Fact>]
let ``Can transform input into 2d array`` () =
//467..114..
//...*......
//..35..633.
    let inputs = [| "467..114.."; "...*......"; "..35..633." |]
    
    let transformation = Day3.transformInput inputs

    Assert.Equal(3, transformation.Length)
    Assert.Equal(10, transformation.[0].Length)
    Assert.Equal(10, transformation.[1].Length)
    Assert.Equal(10, transformation.[2].Length)

    
[<Fact>]
let ``Can find number groups`` () =
    let input = "467..114.."
    let line = input.ToCharArray()

    let numberGroups = Day3.findNumberGroups line

    Assert.Equal(2, numberGroups.Length)
    Assert.Equal(3, numberGroups[0].Length)
    Assert.Equal<list<int>>([0;1;2], numberGroups[0])
    Assert.Equal(3, numberGroups[1].Length)
    Assert.Equal<list<int>>([5;6;7], numberGroups[1])

[<Fact>]
let ``Can find more number groups`` () =
    let input = "467..1.213"
    let line = input.ToCharArray()

    let numberGroups = Day3.findNumberGroups line

    Assert.Equal(3, numberGroups.Length)
    Assert.Equal(3, numberGroups[0].Length)
    Assert.Equal<list<int>>([0;1;2], numberGroups[0])
    Assert.Equal(1, numberGroups[1].Length)
    Assert.Equal<list<int>>([5], numberGroups[1])
    Assert.Equal(3, numberGroups[2].Length)
    Assert.Equal<list<int>>([7;8;9], numberGroups[2])

[<Theory>]
[<InlineData(0, 0, false)>]
[<InlineData(0, 1, false)>]
[<InlineData(0, 2, true)>] //next row, next column
[<InlineData(0, 3, true)>] //next row, current column
[<InlineData(0, 4, true)>] //next row, previous column
[<InlineData(0, 5, false)>]
[<InlineData(0, 6, false)>]
[<InlineData(0, 7, false)>]
[<InlineData(0, 8, false)>]
[<InlineData(0, 9, false)>]
[<InlineData(1, 0, false)>]
[<InlineData(1, 1, false)>]
[<InlineData(1, 2, true)>] //current row, next column
[<InlineData(1, 3, true)>] //current row, current column
[<InlineData(1, 4, true)>] //current row, previous column
[<InlineData(1, 5, false)>]
[<InlineData(1, 6, false)>]
[<InlineData(1, 7, false)>]
[<InlineData(1, 8, false)>]
[<InlineData(1, 9, false)>]
[<InlineData(2, 0, true)>]
[<InlineData(2, 1, true)>]
[<InlineData(2, 2, true)>] //previous row, next column
[<InlineData(2, 3, true)>] //previous row, current column
[<InlineData(2, 4, true)>] //previous row, previous column
[<InlineData(2, 5, false)>]
[<InlineData(2, 6, false)>]
[<InlineData(2, 7, false)>]
[<InlineData(2, 8, true)>]
[<InlineData(2, 9, true)>]
let ``Can validate if by symbol`` (row, column, expected) =
    let schematic = generateTestSchematic()
    let actual = Day3.isBySymbol schematic row column

    Assert.Equal(expected, actual)

[<Theory>]
[<InlineData('.', false)>]
[<InlineData('9', false)>]
[<InlineData('2', false)>]
[<InlineData('#', true)>]
[<InlineData('/', true)>]
[<InlineData('~', true)>]
[<InlineData('?', true)>]
let ``Can test if symbol`` (testChar, expected) =
    let actual = Day3.isSymbol testChar

    Assert.Equal(expected, actual)

[<Fact>]
let ``Can find if number group is by symbol`` () =
    let schematic = generateSchematic()
    let actual = Day3.isNumberGroupBySymbol schematic 0 [0;1;2]

    Assert.Equal(true, actual)

[<Fact>]
let ``Can find numbers associated with number group``() =
    let schematic = generateSchematic()
    let numberGroup = [0;1;2]

    let numbers = Day3.getNumberGroupValues schematic 0 numberGroup
    let num = Day3.convertNumberGroupToNumber numbers

    Assert.Equal(467, num)

[<Fact>]
let ``Can turn number group into number``() =
    let numberGroup = [4;6;7]

    let num = Day3.convertNumberGroupToNumber numberGroup

    Assert.Equal(467, num)