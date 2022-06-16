module Board

open System
open InnovationFS.Cards

type Board() =
    member val highlightedCard = Cards.[1]
