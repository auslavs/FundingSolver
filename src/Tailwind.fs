namespace FundingSolver

[<RequireQualifiedAccess>]
module Tailwind =

    let css: string = Fable.Core.JsInterop.importDefault "./../main.css?inline" |> string