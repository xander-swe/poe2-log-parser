namespace PoE2.LogParser

open System
open System.Text.RegularExpressions

module ValueParser =

    type StringParser<'a> = string -> 'a option

    let string (s: string) = Some s

    let int (s: string) =
        match Int32.TryParse s with
        | true, parsed -> Some parsed
        | false, _ -> None

    let dateOnly (s: string) =
        match System.DateOnly.TryParse s with
        | true, value -> Some value
        | false, _ -> None

    let timeOnly (s: string) =
        match System.TimeOnly.TryParse s with
        | true, value -> Some value
        | false, _ -> None

module GroupParser =

    let parseGroup (group: Group) (tryParse: ValueParser.StringParser<'a>) : 'a option =
        match group.Success with
        | true -> tryParse group.Value
        | false -> None