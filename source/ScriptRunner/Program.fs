open PoE2.LogParser.Parser
open System.IO

let path = Path.Combine(__SOURCE_DIRECTORY__, "..", "..", "test_data", "Client.txt")
let fullPath = Path.GetFullPath(path)

for line in File.ReadLines fullPath do
    let parsed  = Parse line
    match parsed with
    | Ok (Some value) -> printf "%A\n" value
    | Ok None -> printf ""
    | Error err -> printf "%A\n" err