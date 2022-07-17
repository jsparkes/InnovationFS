module InnovationFS.Achievements

open InnovationFS.Cards
open InnovationFS.Player

type Achievements() =
    let mutable regular = List.empty<Achievement>
    let mutable special = List.empty<Achievement>

    do
        regular <-
            [ { index = 1
                title = "Prehistory (1)"
                by = None
                card = None }
              { index = 2
                title = "Classical (2)"
                by = None
                card = None }
              { index = 3
                title = "Medieval (3)"
                by = None
                card = None }
              { index = 4
                title = "Renaissance (4)"
                by = None
                card = None }
              { index = 5
                title = "Exploration (5)"
                by = None
                card = None }
              { index = 6
                title = "Enlightenment (6)"
                by = None
                card = None }
              { index = 7
                title = "Romance (7)"
                by = None
                card = None }
              { index = 8
                title = "Modern (8)"
                by = None
                card = None }
              { index = 9
                title = "Postmodern (9)"
                by = None
                card = None }
              { index = 10
                title = "Information (10)"
                by = None
                card = None } ]

        special <-
            [ { index = 11
                title = "Monument"
                by = None
                card = None }
              { index = 12
                title = "Empire"
                by = None
                card = None }
              { index = 13
                title = "World"
                by = None
                card = None }
              { index = 14
                title = "Universe"
                by = None
                card = None }
              { index = 15
                title = "Wonder"
                by = None
                card = None } ]

    member x.Regular = regular
    member x.Special = special

    member x.Count = regular.Length + special.Length

    member x.GetSpecialByName(name: string) : Option<Achievement> =
        special
        |> List.tryFind (fun card -> card.title = name)

    //member x.AddAchievement(card: Card) =
    //    regular <-
    //        regular
    //        |> List.map (fun ach ->
    //            if ach.index = card.age then
    //                { ach with card = Some card }
    //            else
    //                ach)

    member x.AddRegular (index: int) (card: Card) =
        regular <-
            regular
            |> List.map (fun ach ->
                if ach.index = index then
                    { ach with card = Some card }
                else
                    ach)

    member x.AchieveRegular(index: int, player: Player) =
        regular <-
            regular
            |> List.map (fun ach ->
                if ach.index = index then
                    { ach with by = Some player }
                else
                    ach)

    member x.AchieveSpecial(index: int, player: Player) =
        special <-
            special
            |> List.map (fun ach ->
                if ach.index = index then
                    { ach with by = Some player }
                else
                    ach)
