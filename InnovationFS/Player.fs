module InnovationFS.Player

open InnovationFS.Cards
open System

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

type Hand() =
    member val cards = Set.empty<Card> with get, set
    member x.Add(card: Card) : unit = x.cards <- Set.add card x.cards
    member x.Remove(card: Card) : unit = x.cards <- Set.remove card x.cards

    member x.Score() =
        x.cards
        |> Set.toList
        |> List.sumBy (fun card -> card.age)

type Player(name: string) =
    member val hand = new Hand() with get, set
    member val name = name
    member val scorePile = new Pile() with get, set
    member val tableau = new Tableau() with get, set
    member val achievements = List.empty<Achievement> with get, set

    member x.Achieve(ach: Achievement) =
        x.achievements <-
            x.achievements
            @ List.singleton { ach with by = Some x }

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
      card: Option<Card> }
