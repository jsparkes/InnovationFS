module Cards

//open Android.App
//open Android.Content
//open Android.OS
//open Android.Runtime
//open Android.Views
//open Android.Widget
open System
open FSharp.Data

type IconPosition =
    | IconTop = 0
    | IconLeft = 1
    | IconMiddle = 2
    | IconRight = 3

[<CustomComparison; CustomEquality>]
type Card =
    { id : int32
      age : int32
      color : string
      title : string
      icons : Map<IconPosition, string>
      hexagon : string
      dogmaIcon : string
      dogmaCondition1 : string
      dogmaCondition2 : string
      dogmaCondition3 : string }

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

type CardData = CsvProvider<"Innovation.txt", Separators="\t">

let cardData = CardData.Load("Innovation.txt")

let Cards =
    cardData.Rows
    |> Array.ofSeq
    |> Array.map (fun row ->
           let icons =
               [ (IconPosition.IconTop, row.Top)
                 (IconPosition.IconLeft, row.Left)
                 (IconPosition.IconMiddle, row.Middle)
                 (IconPosition.IconRight, row.Right) ]
               |> Map.ofList
           { id = row.ID
             age = row.Age
             color = row.Color
             title = row.Title
             icons = icons
             hexagon = row.``Hexagon (info only)``
             dogmaIcon = row.``Dogma Icon``
             dogmaCondition1 = row.``Dogma Condition 1``
             dogmaCondition2 = row.``Dogma Condition 2``
             dogmaCondition3 = row.``Dogma Condition 3`` })

let CardsList = List.ofArray Cards

let getIdByName (name : string) : Option<Card> =
    Cards
    |> Array.where (fun s -> s.title = name)
    |> Array.tryHead

let getCardById (id : int32) : Card =
    if (id < 0 || id > 105) then
        invalidArg (nameof id) "card ID is out of range"
    // Stupid starting array at zero.
    // Maybe I should use a Dictionary.
    Cards.[id - 1]

let getHighestCard (cards : List<Card>) : Option<Card> =
    match cards with
    | [] -> None
    | _ ->
        cards
        |> List.max
        |> Some

let isHighestCard (id : int32) (cards : List<Card>) : bool =
    let max = getHighestCard cards
    match max with
    | None -> true // XXX is the card supposed to be in the list?
    | Some c -> c.age = (getCardById id).age

let getLowestCard (cards : List<Card>) : Option<Card> =
    match cards with
    | [] -> None
    | _ ->
        cards
        |> List.min
        |> Some

let isLowestCard (id : int32) (cards : List<Card>) : bool =
    let min = getLowestCard cards
    match min with
    | None -> true // XXX is the card supposed to be in the list?
    | Some c -> c.age = (getCardById id).age
