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
//let private skipIcons n =
//    [ 1..n ]
//    |> List.map (fun i ->
//        Image.create
//            [ Image.source Images.["transparent.jpg"]
//              Image.dock Dock.Left
//              Image.margin 5 ])

let private imageFor (name: string) : Imaging.Bitmap = Images[name.ToLowerInvariant()]

let backgroundColor =
    seq {
        (CardColor.Yellow, Color.FromRgb(227uy, 224uy, 145uy))
        (CardColor.Red, Color.FromRgb(216uy, 146uy, 144uy))
        (CardColor.Purple, Color.FromRgb(161uy, 171uy, 180uy))
        (CardColor.Blue, Color.FromRgb(132uy, 173uy, 217uy))
        (CardColor.Green, Color.FromRgb(129uy, 187uy, 126uy))
    }
    |> Map.ofSeq

let private block = 38.0

let largeTitle (card: Card) =
    TextBlock.create
        [ TextBlock.dock Dock.Left
          TextBlock.foreground "black"
          TextBlock.fontSize 14.0
          TextBlock.fontWeight FontWeight.Bold
          TextBlock.textWrapping TextWrapping.Wrap
          TextBlock.verticalAlignment VerticalAlignment.Center
          TextBlock.horizontalAlignment HorizontalAlignment.Center
          TextBlock.text card.title ]

let largeDogma (card: Card) =
    TextBlock.create
        [ TextBlock.dock Dock.Left
          TextBlock.foreground "black"
          TextBlock.fontSize 12.0
          TextBlock.textWrapping TextWrapping.Wrap
          TextBlock.verticalAlignment VerticalAlignment.Center
          TextBlock.horizontalAlignment HorizontalAlignment.Center
          TextBlock.text (
              card.dogmaCondition1
              + "\n"
              + card.dogmaCondition2
              + "\n"
              + card.dogmaCondition3
          ) ]

let largeCard (card: Card) =
    StackPanel.create
        [ StackPanel.name "Card Display"
          // Each column is 9 icons wide, plus the margin of 5 between them.
          // That is 38 pixels wide.

          StackPanel.background (backgroundColor[ card.color ].ToString())

          StackPanel.children
              [ DockPanel.create
                    [

                      DockPanel.dock Dock.Left

                      DockPanel.children
                          [ Image.create
                                [
                                  // [ Image.source [Bitmap "https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_272x92dp.png" ]]
                                  Image.source (imageFor card.icons[IconPosition.IconTop])
                                  Image.dock Dock.Left
                                  Image.margin 5
                                  Image.stretch Stretch.None
                                  // Image.HorizontalAlignment HorizontalAlignment.left
                                  ]
                            largeTitle card ] ]
                largeDogma card

                // Until I figure out out how to loop in here, cut and paste!
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]
                //Image.create
                //    [ Image.source Images["transparent.jpg"]
                //      Image.dock Dock.Left
                //      Image.margin 5 ]

                StackPanel.create
                    [ StackPanel.orientation Orientation.Horizontal
                      StackPanel.children
                          [ Image.create
                                [ Image.source (imageFor card.icons[IconPosition.IconLeft])
                                  Image.dock Dock.Bottom
                                  Image.margin 5
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor card.icons[IconPosition.IconMiddle])
                                  Image.dock Dock.Bottom
                                  Image.margin 5
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor "transparent")
                                  Image.stretch Stretch.None ]
                            Image.create
                                [ Image.source (imageFor card.icons[IconPosition.IconRight])
                                  Image.dock Dock.Bottom
                                  Image.margin 5
                                  Image.stretch Stretch.None ] ] ] ] ]

let smallTitle (card: Card) =
    TextBlock.create
        [ TextBlock.fontSize 20.0
          // TextBlock.width
          TextBlock.foreground "black"
          TextBlock.verticalAlignment VerticalAlignment.Center
          TextBlock.horizontalAlignment HorizontalAlignment.Center
          TextBlock.textAlignment TextAlignment.Center
          TextBlock.width (4.0 * block)
          TextBlock.text card.title ]

let smallCard (card: Card) =
    StackPanel.create
        [ StackPanel.background (backgroundColor[ card.color ].ToString())
          StackPanel.orientation Orientation.Vertical
          StackPanel.children
              [ StackPanel.create
                    [ StackPanel.orientation Orientation.Horizontal
                      // StackPanel.horizontalAlignment left
                      StackPanel.children
                          [ Image.create
                                [ Image.source (imageFor card.icons[IconTop])
                                  Image.stretch Stretch.None ]
                            smallTitle card
                          ]]]]