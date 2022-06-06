module Main

open Fabulous.XamarinForms
open InnovationFS.App


type FabulousApp() as this = 
    inherit Xamarin.Forms.Application ()

    let runner =
        program
#if DEBUG
        // |> Program.withTrace
#endif
        |> Program.startApplication
    
    do this.MainPage <- runner.MainPage