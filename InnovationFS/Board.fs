module Board

open Cards
open System

type SplayDirection =
    | Unsplayed
    | Left
    | Right
    | Up

let rng = new Random()

let shuffle (org: _[]) =
    let arr = Array.copy org
    let max = (arr.Length - 1)

    let randomSwap (arr: _[]) i =
        let pos = rng.Next(max)
        let tmp = arr.[pos]
        arr.[pos] <- arr.[i]
        arr.[i] <- tmp
        arr

    [| 0..max |] |> Array.fold randomSwap arr

// A stack of cards.  Thw word "Stack" is overloaded...
// Stored in a list top to bottom.
type Pile() =
    member val cards: List<Card> = List.empty with get, set
    member val splayed = SplayDirection.Unsplayed with get, set

    member this.Top() : Option<Card> =
        match this.cards with
        | [] -> None
        | x :: xs -> Some x

    member x.Add(card: Card) = x.cards <- card :: x.cards

    member this.RemoveTop() : Option<Card> =
        match this.cards with
        | [] -> None
        | x :: xs ->
            this.cards <- xs
            Some x

    member x.Shuffle() : unit =
        x.cards <- x.cards |> List.toArray |> shuffle |> Array.toList

    member x.Tuck(card: Card) =
        x.cards <- x.cards @ (List.singleton card)

    member x.Splay(dir: SplayDirection) = x.splayed <- dir
    member x.SplayDirection() : SplayDirection = x.splayed
    member x.CanSplay() : bool = (List.length x.cards) > 1

type ScorePile() =
    member val cards = List.empty<Card> with get, set

    member x.Add(card: Card) : Unit =
        x.cards <- x.cards @ (List.singleton card)

// RemoveByAge
// RemoveLowest
// RemoveHighest
type Hand() =
    member val cards = Set.empty<Card> with get, set
    member x.Add(card: Card) : unit = x.cards <- Set.add card x.cards
    member x.Remove(card: Card) : unit = x.cards <- Set.remove card x.cards

// Per player tableau
// Per color stacks of cards, and the score and acheivement files
type Tableau() =
    member val stacks = Map.empty<CardColor, Pile>

type Board() =
    member val highlightedCard = Cards.[1]
