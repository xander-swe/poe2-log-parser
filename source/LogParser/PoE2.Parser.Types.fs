namespace PoE2.LogParser

open System

module OutputTypes =

    type TimeStamp = {
        Date : DateOnly
        Time : TimeOnly
    }

    type LogOpen = {
        TimeStamp : TimeStamp
    }

    type LevelUp = {
        TimeStamp : TimeStamp
        CharacterName : string
        Level : int
    }

    type Death = {
        TimeStamp : TimeStamp
        Victim : string
        Killer : string option
    }

    type SceneChange = {
        TimeStamp : TimeStamp
        SceneName : string
    }

    type LogEvent =
        | LogOpen of LogOpen
        | LevelUp of LevelUp
        | Death of Death
        | SceneChange of SceneChange


module InputTypes =

    type LogLine = {
        Header : string
        Message : string
    }


module IntermdiateTypes =

    type LogLineHeaderParsed = {
        TimeStamp : OutputTypes.TimeStamp
        Message : string
    }


module Errors =

    type ParserError =
    | MissingTimestamp
    | InvalidDate of string
    | InvalidTime of string
    | InvalidLogLine of string