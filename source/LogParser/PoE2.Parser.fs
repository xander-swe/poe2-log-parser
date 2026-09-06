namespace PoE2.LogParser

open System.Text.RegularExpressions

open PoE2.LogParser.OutputTypes
open PoE2.LogParser.InputTypes
open PoE2.LogParser.IntermdiateTypes
open PoE2.LogParser.Errors

module Parser =

    [<Literal>]
    let dateAndTimeRegex = @"^(?<date>[0-9]{4}/[0-9]{2}/[0-9]{2}) (?<time>[0-9]{2}:[0-9]{2}:[0-9]{2})"
    [<Literal>]
    let deathRegex = @"^: (?<victim>.+?) has been slain\."
    [<Literal>]
    let sceneChangeRegex = @"^\[SCENE\] Set Source \[(?<sceneName>.+?)\]"
    [<Literal>]
    let levelUpRegex= @"^: (?<characterName>.+?) \((?<class>.+?)\) is now level (?<level>.+?)"
    [<Literal>]
    let logOpenRegex = @"^(?<open>\*\*\*\*\* LOG FILE OPENING \*\*\*\*\*)"

    let (|Death|_|) (line: LogLineHeaderParsed) = 
        let m = Regex.Match(line.Message, deathRegex)
        
        let victim = GroupParser.parseGroup m.Groups["victim"] ValueParser.string

        match victim with
        | Some victim -> Some { TimeStamp=line.TimeStamp; Victim=victim }
        | None -> None

    let (|SceneChange|_|) (line: LogLineHeaderParsed) =
        let m = Regex.Match(line.Message, sceneChangeRegex)

        let sceneName = GroupParser.parseGroup m.Groups["sceneName"] ValueParser.string

        match sceneName with
        | Some sceneName -> Some { TimeStamp=line.TimeStamp; SceneName=sceneName }
        | None -> None

    let (|LevelUp|_|) (line: LogLineHeaderParsed) =
        let m = Regex.Match(line.Message, levelUpRegex)

        let characterName = GroupParser.parseGroup m.Groups["characterName"] ValueParser.string
        let class' = GroupParser.parseGroup m.Groups["class"] ValueParser.string
        let level = GroupParser.parseGroup m.Groups["level"] ValueParser.int

        match characterName, class', level with
        | Some character, Some class', Some level -> 
            Some { 
                TimeStamp=line.TimeStamp
                CharacterName=character
                Level=level 
                Class=class' 
            }
        | _, _, _ -> None
        
    let (|LogOpen|_|) (line: LogLineHeaderParsed) =
        let m = Regex.Match(line.Message, logOpenRegex)

        let open' = GroupParser.parseGroup m.Groups["open"] ValueParser.string

        match open' with
        | Some open' -> Some { TimeStamp=line.TimeStamp; }
        | None -> None

    let parseMessage (line: LogLineHeaderParsed) : LogEvent option =
        match line with
        | SceneChange event -> Some (LogEvent.SceneChange event)
        | Death event -> Some (LogEvent.Death event)
        | LevelUp event -> Some (LogEvent.LevelUp event)
        | LogOpen event -> Some (LogEvent.LogOpen event)
        | _ -> None
            
    let parseHeader (line: LogLine) =
        let m = Regex.Match(line.Header, dateAndTimeRegex)
        let date = GroupParser.parseGroup m.Groups["date"] ValueParser.dateOnly
        let time = GroupParser.parseGroup m.Groups["time"] ValueParser.timeOnly

        match date, time with
        | Some date, Some time -> Ok { TimeStamp={ Date=date; Time=time }; Message=line.Message }
        | None, _ -> Error (InvalidDate m.Groups["date"].Value)
        | _, None -> Error (InvalidTime m.Groups["time"].Value)

    let toLogLine (line: string) = 
        let rightBracketIndex = line.IndexOf(']')

        if rightBracketIndex = -1 then
            Error (InvalidLogLine line)
        else
            let splitIndex = rightBracketIndex + 2  // Index of first closing square bracket. Plus 1 to include the bracket in the left result. Plus 1 more to include the following whitespace in the left result.
            let left = line.Substring(0, splitIndex)
            let right = line.Substring(splitIndex)
            Ok { Header=left; Message=right }

    let Parse (line: string) : Result<LogEvent option,ParserError> =
        toLogLine line
        |> Result.bind parseHeader
        |> Result.map parseMessage
