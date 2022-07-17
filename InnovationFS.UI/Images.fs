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
    if System.IO.File.Exists filename then
        use bm = new Bitmap(filename)
        let s = bm.Size
        (System.IO.Path.GetFileName filename, bm)
    else
        failwith $"File {filename} does not exist"

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
