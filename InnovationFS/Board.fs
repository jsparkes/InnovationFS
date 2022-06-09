module Board

open Cards

type SplayDirection =
    | Unsplayed
    | Left
    | Right
    | Up

// A stack of cards.  Thw word "Stack" is overloaded...
// Stored in a list top to bottom.
type Pile() =
    member val cards: List<Card> = List.empty with get, set
    member val splayed = SplayDirection.Unsplayed with get, set

    member this.Top() : Option<Card> =
        match this.cards with
        | [] -> None
        | x::xs -> Some x

    member x.Add(card: Card) =
        x.cards <- card :: x.cards

    member this.Remove() : Option<Card> =
        match this.cards with
        | [] -> None
        | x::xs ->
            this.cards <- xs
            Some x

    member x.Tuck (card: Card) =
        x.cards <- x.cards @ (List.singleton card)

    member x.Splay (dir: SplayDirection) =
        x.splayed <- dir

    member x.CanSplay () : bool =
        (List.length x.cards) > 1

type ScorePile() = 
    member val cards = List.empty<Card>

    // RemoveByAge
    // RemoveLowest
    // RemoveHighest

type Hand() =
    member val cards = Set.empty<Card> with get, set

    member x.Add(card: Card) : unit =
        x.cards <- Set.add card x.cards

    member x.Remove(card: Card) : unit =
        x.cards <- Set.remove card x.cards



// Per player tableau
// Per color stacks of cards, and the score and acheivement files
type Tableau() =
    member val stacks = Map.empty<CardColor, Pile>



//type Board =
//    member val 

