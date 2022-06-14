module Images

open Elmish
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open System.Diagnostics
open System.Runtime.InteropServices
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.FuncUI.DSL
open Avalonia.Media.Imaging


let private loadImage(filename: string) : string * IView<Image> =
    // TODO add error handling here
    use bm = new Bitmap(filename)
    (System.IO.Path.GetFileName filename,
     Image.create [
         Image.source bm
     ])

let Images =
    System.IO.Directory.GetFiles("images", "*.jpg")
    |> Array.map loadImage
    |> Map.ofArray


