namespace PoE2.LogParser

open System
open System.Text.RegularExpressions

open PoE2.LogParser.OutputTypes
open PoE2.LogParser.InputTypes
open PoE2.LogParser.IntermdiateTypes
open PoE2.LogParser.Errors

module Parser =
    // Some guidelines:
    // 1. Make sure that we are examining the pattern from the start of the line using the '^' symbol. That way if 
    //    chat messages contain key patterns, the parser doesn't read the comment as a different type of log line.
    // 2. 

    // split -> extractTimeStamp -> fitToEventType

    //[<Literal>]
    //let dateStampRegex = @"[0-9]{4}/[0-9]{2}/[0-9]{2}"

    [<Literal>]
    let dateAndTimeRegex = @"([0-9]{4}/[0-9]{2}/[0-9]{2}) ([0-9]{2}:[0-9]{2}:[0-9]{2})" // first capture group is date, second is time

    //[<Literal>]
    //let characterSlainNameRegex = @"(?<=^: ).*?(?= has been slain)"
    //
    //[<Literal>]
    //let characterSlainKillerNameRegex = @"(?<= by).*?(?=\.)" // use this once we know we are looking at a character slain line.

    [<Literal>]
    let deathRegex = @"^: (.*)has been slain(?: by )?(.*)." // first capture group is killed, second is killer. Second may not exist.

    let split (line: string) = 
        let rightBracketIndex = line.IndexOf(']')
        if rightBracketIndex <> -1 then
            let splitIndex = rightBracketIndex + 2  // index of first closing square bracket -- plus 1 to include the bracket in the left result -- plus 1 more to include the following whitespace in the left result
            let left = line.Substring(0, splitIndex)
            let right = line.Substring(splitIndex)
            Some { Header=left; Message=right }
        else
            None
            
    let extractTimeStamp (line: LogLine) =
        let m = Regex.Match(line.Header, dateAndTimeRegex)
        
        if (not m.Success || m.Groups.Count < 3) then
            Error MissingTimestamp
        else
            let dateStr = m.Groups[1].Value
            let timeStr = m.Groups[2].Value
            let dateOk, date = DateOnly.TryParse(dateStr)
            let timeOk, time = TimeOnly.TryParse(timeStr)

            match dateOk, timeOk with
            | true, true -> Ok { Date=date; Time=time }
            | false, _ -> Error (InvalidDate dateStr)
            | _, false -> Error (InvalidTime timeStr)

    let (|Death|_|) (line: LogLine) = 
        let m = Regex.Match(line.Message, deathRegex)
        if (m.Success) then
            Some m.Groups[0].Value
        else
            None

    let parse (line: LogLine) =
        match line with
        | Death parsed -> 
            Some 1
        | _ -> None