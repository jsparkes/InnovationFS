module InnovationFS.Images

open Xamarin.Forms

let private loadImage(filename: string) : string * Image =
    // TODO add error handling here
    (System.IO.Path.GetFileName filename, 
     new Image(Source = ImageSource.FromFile(filename)))

let Images =
    System.IO.Directory.GetFiles("images", "*.jpg")
    |> Array.map loadImage
    |> Map.ofArray
