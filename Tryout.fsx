#I "./packages"
#r "FSharp.Data/lib/netstandard2.0/FSharp.Data.dll"
#r "Newtonsoft.Json/lib/netstandard2.0/Newtonsoft.Json.dll"

open FSharp.Data
open Newtonsoft.Json
open System.IO
open System.Text

type wikipedia = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_RAL_colors">

type ralColor = { ral: string; hex: string; name: string}

let ralColorSection = wikipedia.Load("https://en.wikipedia.org/wiki/List_of_RAL_colors").Tables.``All RAL Colours in a single listing``

let ralColors = ralColorSection.Rows
                    |> Seq.map((fun r -> {ral = r.``RAL Number``; hex = r.``HEX Triplet``; name = r.``Colour name``}))

let writeToJsonFile ralColors =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "ral_colour_map.json")
    let jsonString = JsonConvert.SerializeObject(ralColors)
    File.WriteAllText(filePath, jsonString, Encoding.UTF8) |> ignore

let writeToCsvFile ralColors =
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, "ral_colour_map.csv")
    let csvRalColors = ralColors |> Seq.map((fun r -> (sprintf "%s;%s;%s" r.ral r.hex r.name)))
    if File.Exists filePath then File.Delete filePath |> ignore
    File.WriteAllText(filePath, "RAL;HEX;NAME\r\n") |> ignore
    File.AppendAllLines(filePath, csvRalColors) |> ignore
 
 writeToJsonFile ralColors
