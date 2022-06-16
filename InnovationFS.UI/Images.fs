module InnovationFS.UI.Images

open Elmish
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open System.Diagnostics
open System.Runtime.InteropServices
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.FuncUI.DSL
open Avalonia.Media.Imaging
open FsLibLog
open FsLibLog.Types

let private logger = LogProvider.getLoggerByName "InnovationFS.UI.Images"

let private loadImage (filename: string) : string * Bitmap =
    // TODO add error handling here
    use bm = new Bitmap(filename)
    (System.IO.Path.GetFileName filename, bm)

let Images : Map<string, Bitmap> =
    let map =
        System.IO.Directory.GetFiles("images", "*.jpg")
        |> Array.map loadImage
        |> Map.ofArray

    logger.debug (
        Log.setMessage "Image map {map}"
        >> Log.addParameter map
    )

    map
