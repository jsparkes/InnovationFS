module InnovationFS.UI.Cards

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Media
open Avalonia.Themes.Fluent
open Serilog
open InnovationFS.Cards
open InnovationFS.UI.Images
open Avalonia.Layout

let private imageBackground (name: string) : IBrush =
    let mutable ib = new ImageBrush()
    ib.Source <- Images.[name]
    ib

// Fill with n transparent icons
let private skipIcons n =
    for i = 1 to n do
        Image.create
            [ Image.source Images.["transparent.jpg"]
              Image.dock Dock.Left
              Image.margin 5 ]

let backgroundColor =
    seq {
        (CardColor.Yellow, Color.FromRgb(129uy, 187uy, 126uy))
        (CardColor.Red, Color.FromRgb(216uy, 146uy, 144uy))
        (CardColor.Purple, Color.FromRgb(161uy, 171uy, 180uy))
        (CardColor.Blue, Color.FromRgb(132uy, 173uy, 217uy))
        (CardColor.Green, Color.FromRgb(129uy, 187uy, 126uy))
    }
    |> Map.ofSeq

let largeCard (card: Card) =
    DockPanel.create
        [ DockPanel.name "Card Display"
          // Each row is 9 icons wide, plus the margin of 5 between them.
          // That is 38 pixels wide.

          // DockPanel.background ((backgroundColor card.color) :: Color)
          DockPanel.children
              // TextBlock.create
                  //    [ TextBlock.dock Dock.Top
                  //      TextBlock.fontSize 48.0
                  //      TextBlock.verticalAlignment VerticalAlignment.Center
                  //      TextBlock.horizontalAlignment HorizontalAlignment.Center
                  //      TextBlock.text "Hello world"]

                // Top row
                [Image.create
                    //[ Image.source (Images.[card.icons.[IconPosition.IconTop] + ".jpg"])
                    [ Image.source (Images.[card.icons.[IconPosition.IconTop] + ".jpg"])
                      Image.dock Dock.Left
                      Image.margin 5 ]]
                //for i = 1 to 8 do
                //    Image.create
                //        [ Image.source Images.["transparent.jpg"]
                //          Image.dock Dock.Left
                //          Image.margin 5 ]
                // Middle
                // Bottom

