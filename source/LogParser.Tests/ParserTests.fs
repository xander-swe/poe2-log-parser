module Tests

open Expecto
open FsCheck
open PoE2.LogParser
open PoE2.LogParser.OutputTypes
open FsCheckGenerators

let private buildDeathLogLine (victim: string) =
    sprintf "2026/09/05 15:38:24 806450593 3ef23348 [INFO Client 7088] : %s has been slain." victim

[<Tests>]
let tests =
    testList "parserTests" [
        testProperty "death log lines parse with any victim name" (
            FsCheck.FSharp.Prop.forAll latinUnicodeStr1To50Arb <|
                fun victim -> 
                    let logLine = buildDeathLogLine victim
                    let result = Parser.Parse logLine
                    match result with
                    | Ok (Some (LogEvent.Death event)) -> 
                        Expecto.Expect.equal event.Victim victim "The parsed victim name should match the original name"
                    | other -> 
                        failtestf "Expected a Death event, got %A" other
        )
    ]