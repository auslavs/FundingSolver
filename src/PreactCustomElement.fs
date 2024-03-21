[<RequireQualifiedAccess>]
module PreactCustomElement
  open Fable.Core

  [<ImportDefault("preact-custom-element")>]
  let private registerInternal (input: obj * string * obj * {| shadow: bool |}): unit = jsNative

  let register reactComponent tagName = registerInternal (reactComponent, tagName, None, {| shadow = false |})

