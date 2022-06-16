module InnovaationFS.Player

open InnovationFS.Cards

// Per player tableau
// Per color stacks of cards, and the score and achievement files
type Tableau() =
    member val stacks = Map.empty<CardColor, Pile>


type Hand() =
    member val cards = Set.empty<Card> with get, set
    member x.Add(card: Card) : unit = x.cards <- Set.add card x.cards
    member x.Remove(card: Card) : unit = x.cards <- Set.remove card x.cards


type Player(name: string) =
    member val hand = new Hand() with get, set
    member val Name = name with get

    


