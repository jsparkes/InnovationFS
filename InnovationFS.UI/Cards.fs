namespace InnovationFS.UI

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Media
open Avalonia.Themes.Fluent
open Serilog
open InnovationFS.Cards
open InnovationFS.UI.Images

module Cards =

let private imageBackground (name: string) : IBrush =
    let mutable ib = new ImageBrush()
    ib.Source <- Images.[name]
    ib 

let largeCard(card: Card) =
    DockPanel.create [
        DockPanel.name "Card Display"
        DockPanel.background (IBrush (imageBackground (card.color <> ".jpg")))
        DockPanel.children [
                // Top row
                Image.create [
                    Image.source Images.[card.icons.[IconPosition.IconTop]]
                    Image.dock Dock.Left
                    Image.margin 5
                ]
            ]
        ]
