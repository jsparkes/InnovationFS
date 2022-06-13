namespace InnovationFS

module Images =

open Xamarin.Forms

let private loadImage(filename: string) =
    // TODO add error handling here
    ImageSource.FromFile(filename)

let Images =
    System.IO.Directory.GetFiles("c:\\tmp", "*.jpg")
    |> Array.map loadImage
