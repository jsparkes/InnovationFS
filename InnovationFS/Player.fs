module InnovationFS.Player

open InnovationFS.Cards
open System
open System.Text.Json
open System.Text.Json.Serialization

// Per player tableau
// Per color stacks of cards, and the score and achievement files
type Tableau() =
    let mutable stacks = Map.empty<CardColor, Pile>

    do
        stacks <-
            CardColor.AllColors
            |> List.map (fun color -> (color, new Pile()))
            |> Map.ofList

    member x.Stacks = stacks

    // Return the highest card in the entire tableau.
    // This may return EmptyCard!
    member x.GetHighestCard() =
        stacks
        |> Map.map (fun color pile -> pile.Top())
        |> Map.values
        |> Seq.sortBy (fun card -> card.age)
        |> Seq.head

    member x.Add(card: Card) =
        stacks.[card.color].Add(card)
        // Remove the empty card marker, if necessary
        stacks.[card.color].Remove(EmptyCard)

type Hand(player: Player) =
    member val cards = Set.empty<Card> with get, set
    member val tuckedCount = 0 with get, set

    member x.Add(card: Card) : unit = x.cards <- Set.add card x.cards
    member x.Remove(card: Card) : unit = x.cards <- Set.remove card x.cards
    member x.ResetTuckCount() = x.tuckedCount <- 0
    member x.CountTuck() = x.tuckedCount <- x.tuckedCount + 1

    member x.Score() =
        x.cards
        |> Set.toList
        |> List.sumBy (fun card -> card.age)

    member x.Tuck (card: Card) (pile: Pile) =
        x.Remove card
        pile.Tuck card
        x.CountTuck()

    //if x.tuckedCount > 5 then
    //    player.Achieve(player.Board.Achievements.GetSpecialByName("Monument"))

    member x.Return(card: Card, board: Board) =
        x.Remove card
        board.Return card

and Player(board: Board, name: string) =
    member val Board = board
    member x.hand = Hand(x)
    member val name = name
    member val scorePile = new Pile() with get, set
    member val tableau = new Tableau() with get, set
    member val achievements = List.empty<Achievement> with get, set

    member x.Achieve(ach: Achievement) =
        x.achievements <-
            x.achievements
            @ List.singleton { ach with by = Some x }

    member x.UpdateIconCounts() =
        // For when we switch to maintaining the icons counts
        // Maybe it should be event driven?
        // Currently compue icons counts per turn
        ()

    member x.Tuck(card: Card, pile: Pile) =
        x.hand.Tuck card pile
        x.UpdateIconCounts()

    member x.CountIcon (icon: string) (counts: Map<string, int>) =
        if icon = "" || icon = "transparent.jpg" then
            // Skip empty icon
            counts
        else
            match Map.tryFind icon counts with
            | None -> Map.add icon 1 counts
            | Some x -> Map.add icon (counts[icon] + 1) counts

    member x.CountIcons (counts: Map<string, int>) (card: Card) =
        let mutable map = counts

        for pos in IconPosition.IconPositions do
            map <- x.CountIcon card.icons[pos] map

        map

    member x.GetIconCountsForPile (pile: Pile) (counts: Map<string, int>) : Map<string, int> =
        let mutable map = counts

        for card in pile.cards do
            map <- x.CountIcons map card

        map

    member x.GetIconCounts() : Map<string, int> =
        // Get icon counts per tableau pile and merge them
        // Map of icon name -> card, then count by icon?
        // Map of icon name -> count
        let mutable iconCounts = Map.empty<string, int>

        for pile in x.tableau.Stacks.Values do
            iconCounts <- x.GetIconCountsForPile pile iconCounts

        iconCounts


    interface IComparable with
        member this.CompareTo other =
            match other with
            | :? Player as p -> (this :> IComparable<_>).CompareTo p
            | _ -> -1

    interface IComparable<Player> with
        member this.CompareTo other = other.name.CompareTo this.name

    interface IEquatable<Player> with
        member this.Equals other = other.name.Equals this.name

    override this.Equals other =
        match other with
        | :? Player as p -> (this :> IEquatable<_>).Equals p
        | _ -> false

    override this.GetHashCode() = this.name.GetHashCode()

and Achievement =
    { index: int
      title: string
      by: Option<Player>
      // Card used as a marker, it is removed from play.
      card: Option<Card> }

and Achievements() =
    // They are mutable to contain the players who achieve them
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

and Board(players: List<Player>) =
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

    member x.Return(card: Card) =
        let drawPile = x.DrawPiles[card.age]
        drawPile.Tuck card
        // Not sure if this update is required, draw pile is internally mutable
        x.DrawPiles <- Map.add card.age drawPile x.DrawPiles

    // Can we serialize the game state with JSON?
    member x.SerializeGame() : string =
        let options =
            new JsonSerializerOptions(
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            )

        JsonSerializer.Serialize(x, options)

    static member DeserializeGame(json: string) : Board = JsonSerializer.Deserialize<Board>(json)

    static member LoadGame() : Option<Board> =
        // Read save file
        if System.IO.File.Exists("save.json") then
            let contents = System.IO.File.ReadAllText("save.json")
            Some(Board.DeserializeGame(contents))
        else
            None

    member x.SaveGame() =
        let contents = x.SerializeGame()
        System.IO.File.WriteAllText("save.json", contents)
