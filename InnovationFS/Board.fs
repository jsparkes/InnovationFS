module InnovationFS.Board

open System
open InnovationFS.Cards
open InnovationFS.Player
open InnovationFS.Achievements

type Board(players: List<Player>) =
    // Upper left corner display card
    member val highlightedCard = Cards.[1] with get, set
    member val DrawPiles = Map.empty<int, Pile> with get, set
    member val Achievements = new Achievements()
    member val PlayerCount = List.length players
    member val Players = players

    // Do we try keep the icon counts up to date, or do we
    // recalculate after each round?  The first is more complicated
    // but but uses less CPU.  Picking the second for now, maybe
    // we can optimize into the first later, if necessary.  I
    // doubt this performance wil be a problem.
    // Maybe we cache the result between moves though.
    member x.GetIconCounts() : Map<Player, Map<string, int>> =
        players
        |> List.map (fun player -> player, player.GetIconCounts())
        |> Map.ofList

    member x.FillDrawPile (i: int) (cards: seq<Card>) =
        x.Achievements.AddRegular i (Seq.head cards)
        let pile = new Pile()
        pile.Add(Seq.tail cards)
        pile.Shuffle()
        x.DrawPiles <- Map.add i pile x.DrawPiles

    /// Return the cards for a specific age
    member x.CardsOfAge(age: int) : seq<Card> =
        Cards.Values
        |> Seq.filter (fun card -> card.age = age)

    member x.FillDrawPiles() =
        [ 1..10 ]
        |> Seq.iter (fun age -> x.FillDrawPile age (x.CardsOfAge age))

    // Can we serialize the game state with JSON?
    member x.SerializeGame() : string =
        "JSON string"

    static member DeserializeGame(json: string) : Board =
        new Board(List.empty)

    static member LoadGame() = 
        // Read save file
        let contents = "{}"
        Board.DeserializeGame(contents)

    member x.SaveGame() =
        let contents = x.SerializeGame()
        System.IO.File.WriteAllText("save.json", contents)
