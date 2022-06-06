module Cards

//open Android.App
//open Android.Content
//open Android.OS
//open Android.Runtime
//open Android.Views
//open Android.Widget

open FSharp.Data

type Card =
    { id : int32
      age : int32
      color : string
      title : string
      top : string
      left : string
      middle : string
      right : string
      hexagon : string
      dogmaIcon : string
      dogmaCondition1 : string
      dogmaCondition2 : string
      dogmaCondition3 : string }

type CardData = CsvProvider<"Innovation.txt", Separators="\t">

let cardData = CardData.Load("Innovation.txt")

let Cards =
    cardData.Rows
    |> Array.ofSeq
    |> Array.map (fun row ->
           { id = row.ID
             age = row.Age
             color = row.Color
             title = row.Title
             top = row.Top
             left = row.Left
             middle = row.Middle
             right = row.Right
             hexagon = row.``Hexagon (info only)``
             dogmaIcon = row.``Dogma Icon``
             dogmaCondition1 = row.``Dogma Condition 1``
             dogmaCondition2 = row.``Dogma Condition 2``
             dogmaCondition3 = row.``Dogma Condition 3`` })
