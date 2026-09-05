open PoE2.LogParser.Parser

//**************************************************************************************************************
// LOGS
//**************************************************************************************************************

let logText = """2026/08/20 18:02:11 123456123 1a2 [INFO Client 8412] ***** LOG FILE OPENING *****
2026/08/20 18:02:11 123456145 1a3 [INFO Client 8412] Build: 2.1.0.4 (x64)
2026/08/20 18:02:11 123456201 1a4 [INFO Client 8412] Started with PID 8412
2026/08/20 18:02:12 123457002 1b0 [INFO Client 8412] Async connecting to 12.34.56.78:20481
2026/08/20 18:02:12 123457340 1b1 [INFO Client 8412] Connecting to instance server at 12.34.56.78:6112
2026/08/20 18:02:13 123458110 1c9 [INFO Client 8412] Tricky (Warrior) is now level 1
2026/08/20 18:02:14 123458900 1d0 [INFO Client 8412] [SCENE] Set Source [Clearfell Encampment]
2026/08/20 18:02:14 123459005 1d1 [INFO Client 8412] Generating level 1 area "G1_town" with seed 1928374651
2026/08/20 18:04:02 123566880 2a0 [INFO Client 8412] [SCENE] Set Source [The Riverbank]
2026/08/20 18:04:02 123566912 2a1 [INFO Client 8412] Generating level 2 area "G1_1" with seed 837465102
2026/08/20 18:07:45 123790110 3b7 [INFO Client 8412] : Tricky has been slain.
2026/08/20 18:07:48 123793201 3b9 [INFO Client 8412] : Tricky has been slain by Rustic Sentry.
2026/08/20 18:09:20 123885330 3f2 [INFO Client 8412] : Tricky (Warrior) is now level 2
2026/08/20 18:12:10 124055440 410 [INFO Client 8412] @From Xyrella_HC: Hi, I'd like to buy your 1x Chaos Orb for my 3x Orb of Alchemy in Standard
2026/08/20 18:12:44 124089200 415 [INFO Client 8412] @To Xyrella_HC: sure, meet me at hideout
2026/08/20 18:13:02 124107001 41a [INFO Client 8412] Xyrella_HC has joined the area.
2026/08/20 18:14:55 124220050 44c [INFO Client 8412] Xyrella_HC has left the area.
2026/08/20 18:15:10 124235100 452 [INFO Client 8412] #Tricky: anyone want to duo the Trial of Chaos?
2026/08/20 18:15:33 124258700 459 [INFO Client 8412] %Grouper99: invite sent
2026/08/20 18:15:40 124265900 45c [INFO Client 8412] &LeagueGuild: gg on the boss kill everyone
2026/08/20 18:16:02 124288100 461 [INFO Client 8412] $Trader_Joe: WTS Headhunter 45ex or best offer
2026/08/20 18:16:10 124296300 463 [INFO Client 8412] Grouper99 has joined the area.
2026/08/20 18:18:00 124406100 4a1 [INFO Client 8412] [SCENE] Set Source [The Mud Flats]
2026/08/20 18:18:00 124406132 4a2 [INFO Client 8412] Generating level 4 area "G1_2" with seed 293847561
2026/08/20 18:22:15 124661400 500 [INFO Client 8412] : Tricky has been slain by Kabala, Constrictor Queen.
2026/08/20 18:22:16 124662001 501 [INFO Client 8412] : Grouper99 has been slain by Kabala, Constrictor Queen.
2026/08/20 18:25:00 124826000 540 [INFO Client 8412] : Tricky (Warrior) is now level 5
2026/08/20 18:30:44 125150200 5a2 [INFO Client 8412] Connecting to instance server at 12.34.56.78:6112
2026/08/20 18:30:45 125151100 5a3 [INFO Client 8412] [SCENE] Set Source [Ogham Farmlands]
2026/08/20 18:30:45 125151140 5a4 [INFO Client 8412] Generating level 6 area "G2_1" with seed 918273645
2026/08/20 18:34:12 125358900 5f0 [INFO Client 8412] Lost connection: Connection lost with game server. (Timeout)
2026/08/20 18:34:13 125359700 5f1 [INFO Client 8412] [DEBUG Client 8412] Reconnecting...
2026/08/20 18:34:20 125366900 5f5 [INFO Client 8412] Connecting to instance server at 12.34.56.78:6112
2026/08/20 18:34:21 125367800 5f6 [INFO Client 8412] [SCENE] Set Source [Ogham Farmlands]
2026/08/20 18:40:03 125709000 640 [INFO Client 8412] : Tricky has completed Trial of the Sekhemas
2026/08/20 18:42:19 125844100 660 [INFO Client 8412] @From Vendor_Bot: Thanks for the trade!
2026/08/20 18:45:00 126005000 690 [INFO Client 8412] Chat away team joined.
2026/08/20 18:45:02 126006900 691 [INFO Client 8412] %Tricky: ready
2026/08/20 18:48:33 126218100 6c1 [INFO Client 8412] [SCENE] Set Source [The Halani Gates]
2026/08/20 18:48:33 126218144 6c2 [INFO Client 8412] Generating level 8 area "G2_town" with seed 564738291
2026/08/20 18:51:19 126384000 700 [INFO Client 8412] : Tricky (Warrior) is now level 8
2026/08/20 18:55:00 126605000 740 [INFO Client 8412] : Grouper99 has left the game.
2026/08/20 19:01:12 126977000 7a0 [INFO Client 8412] : Tricky has been slain by Count Geonor.
2026/08/20 19:01:12 126977040 7a1 [INFO Client 8412] : Tricky has been slain by Count Geonor.
2026/08/20 19:01:13 126978100 7a2 [INFO Client 8412] : Tricky has been slain by Count Geonor.
2026/08/20 19:05:00 127217000 800 [INFO Client 8412] [SCENE] Set Source [Freythorn]
2026/08/20 19:05:00 127217033 801 [INFO Client 8412] Generating level 9 area "G2_2" with seed 102938475
2026/08/20 19:10:44 127542100 850 [INFO Client 8412] $CurrencySeller: buying all fragments, add me
2026/08/20 19:12:00 127618000 860 [INFO Client 8412] Async connecting to 12.34.56.78:20481
2026/08/20 19:12:00 127618100 861 [INFO Client 8412] ***** LOG FILE OPENING *****
"""

let altLogText = """2025/12/24 04:58:30 100000 abc [INFO Client 12345] ***** LOG FILE OPENING *****
2025/12/24 04:58:45 123456 abc [INFO Client 12345] [SCENE] Set Source [The Mud Flats]
2025/12/24 04:58:45 123456 abc [INFO Client 12345] Connecting to instance server at 123.45.67.89:6112
2025/12/24 05:15:33 123456 abc [INFO Client 12345] : MyCharacter (Sorceress) is now level 22
2025/12/24 05:22:41 123456 abc [INFO Client 12345] : MyCharacter has been slain.
"""

let testLine = "2026/08/20 18:07:48 123793201 3b9 [INFO Client 8412] : Tricky has been slain by Rustic Sentry."

    
// parse
// split -> extractTimeStamp -> match to event type 
// string -> LogLine -> TimeStamp -> LogEvent
//split testLine |> Option.map parse |> printfn "%A"
//split testLine |> Option.map |> fun line -> (extractTimeStamp line.Common, parse line.Unique)
//toLogLine testLine |> Option.map parseHeader |> printfn "%A"