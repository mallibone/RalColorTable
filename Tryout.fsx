#I "./packages"
#r "FSharp.Data/lib/netstandard2.0/FSharp.Data.dll"

open FSharp.Data
open System.IO

type wikipedia = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_RAL_colors">

type ralColor = { ral: string; hex: string; name: string}

let ralColorSection = wikipedia.Load("https://en.wikipedia.org/wiki/List_of_RAL_colors").Tables.``All RAL Colours in a single listing``

let ralColors = ralColorSection.Rows
                    |> Seq.map((fun r -> {ral = r.``RAL Number``; hex = r.``HEX Triplet``; name = r.``Colour name``}))

let writeToCsvFile ralColors =
    let filePath = "/./Work/ral_colour_map.csv"
    let csvRalColors = ralColors |> Seq.map((fun r -> (sprintf "%s;%s;%s" r.ral r.hex r.name)))
    if File.Exists filePath then File.Delete filePath |> ignore
    File.WriteAllText(filePath, "RAL;HEX;NAME\r\n") |> ignore
    File.AppendAllLines(filePath, csvRalColors) |> ignore
 
 writeToCsvFile ralColors
