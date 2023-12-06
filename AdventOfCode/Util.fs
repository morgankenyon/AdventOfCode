module Util

open System.IO


let loadDataFile path = 
    File.ReadLines(path)

let loadAllData path =
    File.ReadAllLines path