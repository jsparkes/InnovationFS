module InnovationFS.Cards

//open Android.App
//open Android.Content
//open Android.OS
//open Android.Runtime
//open Android.Views
//open Android.Widget

open System
open FSharp.Data

type IconPosition =
    | IconTop
    | IconLeft
    | IconMiddle
    | IconRight

    static member IconPositions =
        [ IconTop
          IconLeft
          IconMiddle
          IconRight ]

type CardColor =
    | Green
    | Red
    | Blue
    | Yellow
    | Purple

    static member Parse(str: string) : CardColor =
        match str.ToLowerInvariant() with
        | "green" -> CardColor.Green
        | "red" -> CardColor.Red
        | "blue" -> CardColor.Blue
        | "yellow" -> CardColor.Yellow
        | "purple" -> CardColor.Purple
        | _ -> invalidArg (nameof str) (sprintf "Invalid color: %s" str)

    static member ToString(color: CardColor) : string =
        match color with
        | CardColor.Green -> "Green"
        | CardColor.Red -> "Red"
        | CardColor.Blue -> "Blue"
        | CardColor.Yellow -> "Yellow"
        | CardColor.Purple -> "Purple"

    static member AllColors = [ Green; Red; Blue; Yellow; Purple ]

let rng = new Random()

let shuffle (org: _[]) =
    let arr = Array.copy org
    let max = (arr.Length - 1)

    let inline randomSwap (arr: _[]) i =
        let pos = rng.Next(max)
        let tmp = arr.[pos]
        arr.[pos] <- arr.[i]
        arr.[i] <- tmp
        arr

    [| 0..max |] |> Array.fold randomSwap arr

let CardBackgrounds =
    [| CardColor.Green
       CardColor.Red
       CardColor.Blue
       CardColor.Yellow
       CardColor.Purple |]
    |> Array.map (fun color ->
        let name = CardColor.ToString(color)
        (color, $"imagers/{name.ToLowerInvariant()}.jpg"))
    |> Map.ofArray

[<CustomComparison; CustomEquality>]
type Card =
    { id: int32 // unique id
      age: int32
      color: CardColor
      title: string
      icons: Map<IconPosition, string> // up to four icons
      hexagon: string // ignored in play
      dogmaIcon: string
      dogmaCondition1: string
      dogmaCondition2: string
      dogmaCondition3: string }

    interface IComparable with
        member this.CompareTo other =
            match other with
            | :? Card as c -> (this :> IComparable<_>).CompareTo c
            | _ -> -1

    interface IComparable<Card> with
        member this.CompareTo other = other.id.CompareTo this.id

    interface IEquatable<Card> with
        member this.Equals other = other.id.Equals this.id

    override this.Equals other =
        match other with
        | :? Card as c -> (this :> IEquatable<_>).Equals c
        | _ -> false

    override this.GetHashCode() = this.id.GetHashCode()

    member x.HasSymbol(symbol: string) : bool = Seq.contains symbol x.icons.Values

type CardData = CsvProvider<"Innovation.txt", Separators="\t">

let cardData = CardData.Load("Innovation.txt")


let Cards =

    // Convert missing icons into transparent ones
    let convertEmptyIcons (icons: Map<IconPosition, string>) : Map<IconPosition, string> =
        icons
        |> Map.map (fun k v -> if v = "" then "transparent.jpg" else v)
    
    cardData.Rows
    |> Seq.map (fun row ->
        let icons =
            [ (IconPosition.IconTop, row.Top)
              (IconPosition.IconLeft, row.Left)
              (IconPosition.IconMiddle, row.Middle)
              (IconPosition.IconRight, row.Right) ]
            |> Map.ofList
            |> convertEmptyIcons

        let card =
            { id = row.ID
              age = row.Age
              color = CardColor.Parse row.Color
              title = row.Title
              icons = icons
              hexagon = row.``Hexagon (info only)``
              dogmaIcon = row.``Dogma Icon``
              dogmaCondition1 = row.``Dogma Condition 1``
              dogmaCondition2 = row.``Dogma Condition 2``
              dogmaCondition3 = row.``Dogma Condition 3`` }

        (row.ID, card))
    |> Map.ofSeq

let CardsByName =
    Map.values Cards
    |> Seq.map (fun card -> (card.title, card))
    |> Map.ofSeq

// Placeholder to avoid use of Option<Card>.
let EmptyCard =
    { id = 0
      age = 0
      color = CardColor.Yellow
      title = "Empty"
      icons =
        [ (IconPosition.IconTop, "transparent.jpg")
          (IconPosition.IconLeft, "transparent.jpg")
          (IconPosition.IconMiddle, "transparent.jpg")
          (IconPosition.IconRight, "transparent.jpg") ]
        |> Map.ofList
      hexagon = "Empty"
      dogmaIcon = "transparent.jpg"
      dogmaCondition1 = "Empty"
      dogmaCondition2 = "Empty"
      dogmaCondition3 = "Empty" }

let isEmpty (card: Card) = card.id = 0

let getCardByName (name: string) : Option<Card> = CardsByName |> Map.tryFind name

let getCardById (id: int) : Option<Card> = Cards |> Map.tryFind id

let getHighestCard (cards: List<Card>) : Option<Card> =
    match cards with
    | [] -> None
    | _ -> cards |> List.max |> Some

let isHighestCard (id: int32) (cards: List<Card>) : bool =
    let max = getHighestCard cards

    match max with
    | None -> true // XXX is the card supposed to be in the list?
    | Some c -> c.age = (Cards.[id]).age

let getLowestCard (cards: List<Card>) : Option<Card> =
    match cards with
    | [] -> None
    | _ -> cards |> List.min |> Some

let isLowestCard (id: int32) (cards: List<Card>) : bool =
    let min = getLowestCard cards

    match min with
    | None -> true // XXX is the card supposed to be in the list?
    | Some c -> c.age = Cards.[id].age

type SplayDirection =
    // Value is score
    | Unsplayed
    | Left
    | Right
    | Up

    static member ToString(dir: SplayDirection) : string =
        match dir with
        | Unsplayed -> "Unsplayed"
        | Left -> "left"
        | Right -> "Right"
        | Up -> "Up"

    static member Score(dir: SplayDirection) : int =
        match dir with
        | Unsplayed -> 0
        | Left -> 10
        | Right -> 25
        | Up -> 40

    static member SplayDirections = [ Unsplayed; Left; Right; Up ]

// A stack of cards.  Thw word "Stack" is overloaded...
// Stored in a list top to bottom.
// This represents a user's played cards and the draw piles of cards.
type Pile() =
    member val cards: List<Card> = List.empty with get, set
    member val splayed = SplayDirection.Unsplayed with get, set

    member this.Top() : Card =
        match this.cards with
        | [] -> EmptyCard
        | x :: xs -> x

    member x.Add(card: Card) = x.cards <- card :: x.cards

    member x.Add(cards: seq<Card>) =
        cards |> Seq.iter (fun card -> x.Add card)

    member this.RemoveTop() : Option<Card> =
        match this.cards with
        | [] -> None
        | x :: xs ->
            this.cards <- xs
            Some x

    member x.Remove(card: Card) =
        x.cards <- List.filter (fun c -> c <> card) x.cards
        if x.cards.Length <= 1 then
            x.Splay Unsplayed

    member x.Shuffle() : unit =
        x.cards <- x.cards |> List.toArray |> shuffle |> Array.toList

    member x.Tuck(card: Card) =
        x.cards <- x.cards @ (List.singleton card)

    member x.Splay(dir: SplayDirection) = x.splayed <- dir
    member x.SplayDirection() : SplayDirection = x.splayed
    member x.CanSplay() : bool = (List.length x.cards) > 1

    member x.TotalScore() : int =
        x.cards |> List.sumBy (fun card -> card.age)

type ScorePile() =
    member val cards = List.empty<Card> with get, set

    member x.Add(card: Card) : Unit =
        x.cards <- x.cards @ (List.singleton card)

    member x.Remove(card: Card) =
        x.cards <- List.filter (fun c -> c <> card) x.cards

    member x.GetHighestAge() =
        match x.cards.Length with
        | 0 -> 0
        | _ ->
            x.cards
            |> List.map (fun card -> card.age)
            |> List.max

    member x.GetLowestAge() = 
        match x.cards.Length with
        | 0 -> 0
        | _ ->
            x.cards
            |> List.map (fun card -> card.age)
            |> List.min

    // Any card of highest age will do
    member x.RemoveHighestAge() =
        let card =
            match x.cards.Length with
            | 0 -> EmptyCard
            | _ -> 
                x.cards
                |> List.maxBy (fun c -> c.age)
        x.Remove card
        card

    member x.RemoveLowestAge() =
        let card =
            match x.cards.Length with
            | 0 -> EmptyCard
            | _ -> 
                x.cards
                |> List.minBy (fun c -> c.age)
        x.Remove card
        card

    member x.RemoveByAge(age: int) =
        let card =
            match x.cards.Length with
            | 0 -> Some EmptyCard
            | _ -> 
                x.cards
                |> List.filter (fun c -> c.age = age)
                |> List.tryHead
        match card with
        | None -> EmptyCard
        | Some c ->
            x.Remove c
            c
