namespace FundingSolver.Components

open Feliz
open Elmish
open Feliz.UseElmish

module FundingSolver =

  type State = {
    TotalCost: string
    ``11_022``: LineItem
    ``11_023``: LineItem
    ``11_024``: LineItem
    Result: (LineItem * LineItem * LineItem) list option
  }
  and LineItem = {
    Id: string
    Price: float
    Qty: int 
  }

  type Msg =
    | UpdateLineItem of string * float * int
    | UpdateTotalCost of string
    | Solve

  module State =
    
    let init () =
      { 
        TotalCost = "17153.28"
        ``11_022`` = { Id = "11_022"; Price = 214.41; Qty = 60  }
        ``11_023`` = { Id = "11_023"; Price = 193.99; Qty = 40  }
        ``11_024`` = { Id = "11_024"; Price = 74.63;  Qty = 200 }
        Result = None
      }, Cmd.none


    let solve (state: State) =
      printfn "Solving..."
      try
        let _022 = state.``11_022``
        let _023 = state.``11_023``
        let _024 = state.``11_024``
        let totalCost = state.TotalCost |> float

        [ 
          for x in [0.._022.Qty] do
          for y in [0.._023.Qty] do
          for z in [0.._024.Qty] do
            if (_022.Price * (float x)) + (_023.Price * (float y)) + (_024.Price * (float z)) = totalCost then
              yield ( 
                { Id = "11_022"; Price = _022.Price; Qty = x },
                { Id = "11_023"; Price = _023.Price; Qty = y },
                { Id = "11_024"; Price = _024.Price; Qty = z }
              )
        ] |> (fun x -> printfn "Result: %A" x; x)

      with ex ->
        printfn "Error: %s" ex.Message
        []

    let update : Msg -> State -> State * Cmd<'a> =
      fun (msg: Msg) (state: State) ->
        match msg with
        | UpdateLineItem (id, price, qty) ->
            let newState = 
              match id with
              | "11_022" -> { state with ``11_022`` = { state.``11_022`` with Price = price; Qty = qty } }
              | "11_023" -> { state with ``11_023`` = { state.``11_023`` with Price = price; Qty = qty } }
              | "11_024" -> { state with ``11_024`` = { state.``11_024`` with Price = price; Qty = qty } }
              | _ -> failwith "Invalid line item id"
            newState, Cmd.none
        | UpdateTotalCost tc ->
            { state with TotalCost = tc }, Cmd.none
        | Solve ->
            let result = solve state
            { state with Result = Some result }, Cmd.none

  let logo =
    Html.img [ 
      prop.src "http://play.tailwindcss.com/img/beams.jpg"
      prop.alt ""
      prop.className "absolute left-1/2 top-1/2 max-w-none -translate-x-1/2 -translate-y-1/2"
      prop.width 1308
    ]

  let background =
    Html.div [
      prop.className "absolute inset-0 bg-[url(http://play.tailwindcss.com/img/grid.svg)] bg-center [mask-image:linear-gradient(180deg,white,rgba(255,255,255,0))]"
    ]

  let priceInput price =
    Html.div [
      prop.classes [ "relative"; "col-span-5"; "col-start-1"; "mt-2"; "rounded-md"; "shadow-sm" ]
      prop.children [
        Html.div [
          prop.classes [ "pointer-events-none"; "absolute"; "inset-y-0"; "left-0"; "flex"; "items-center"; "pl-3" ]
          prop.children [ Html.span [ prop.classes [ "text-gray-500"; "sm:text-sm" ]; prop.text "$" ] ]
        ]
        Html.input [
          prop.type' "number"
          prop.name "price"
          prop.id "price"
          prop.className "block w-full rounded-md border-0 py-1.5 pl-7 pr-12 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          prop.placeholder "200"
          prop.min 0.01
          prop.step 0.01
          prop.max 999.99
          prop.maxLength 7
          prop.value (string price)
        ]
        Html.div [
          prop.classes [ "pointer-events-none"; "absolute"; "inset-y-0"; "right-0"; "flex"; "items-center"; "pr-3" ]
          prop.children [ Html.span [ prop.classes [ "text-gray-500"; "sm:text-sm" ]; prop.id "price-currency"; prop.text "AUD" ] ]
        ]
      ]
    ]

  let qtyInput (item: LineItem) =
    Html.div [
      prop.className "relative col-span-3 col-start-6 mt-2 rounded-md shadow-sm"
      prop.children [
        Html.input [
          prop.type' "number"
          prop.name "qty"
          prop.id "qty"
          prop.className "block w-full rounded-md border-0 py-1.5 pl-3 pr-12 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          //prop.placeholder "100"
          prop.min 0
          prop.max 999
          prop.maxLength 3
          prop.value (string item.Qty)
        ]
        Html.div [
          prop.className "pointer-events-none absolute inset-y-0 right-0 flex items-center pr-3"
          prop.children [ Html.span [ prop.className "text-gray-500 sm:text-sm"; prop.id "price-currency"; prop.text "QTY" ] ]
        ]
      ]
    ]

  let lineItem (item: LineItem) =
    Html.div [
      prop.className "mt-2"
      prop.children [
        Html.label [ prop.className "block text-sm font-medium leading-6 text-gray-900"; prop.for' "price"; prop.text item.Id ]
        Html.div [
          prop.className "grid grid-cols-8 gap-x-4"
          prop.children [
            priceInput item.Price
            qtyInput item
          ]
        ]
      ]
    ]

  let totalInput dispatch (totalCost: string) =
    Html.div [
      prop.className "mt-2"
      prop.children [
        Html.label [ prop.className "block text-sm font-medium leading-6 text-gray-900"; prop.for' "total"; prop.text "Total Cost" ]
        Html.div [
          prop.className "relative rounded-md shadow-sm"
          prop.children [
            Html.div [
              prop.classes [ "pointer-events-none"; "absolute"; "inset-y-0"; "left-0"; "flex"; "items-center"; "pl-3" ]
              prop.children [ Html.span [ prop.classes [ "text-gray-500"; "sm:text-sm" ]; prop.text "$" ] ]
            ]
            Html.input [
              prop.type' "number"
              prop.name "total"
              prop.id "total"
              prop.className "block w-full rounded-md border-0 py-1.5 pl-7 pr-12 text-gray-900 ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
              prop.placeholder "Total cost"
              prop.onTextChange (UpdateTotalCost >> dispatch)
              prop.value totalCost
              prop.min 0.01
              prop.step 0.01
              prop.max 99999.99
              prop.maxLength 8
            ]
            Html.div [
              prop.classes [ "pointer-events-none"; "absolute"; "inset-y-0"; "right-0"; "flex"; "items-center"; "pr-3" ]
              prop.children [ Html.span [ prop.classes [ "text-gray-500"; "sm:text-sm" ]; prop.id "price-currency"; prop.text "AUD" ] ]
            ]
          ]
        ]
      ]
    ]
    

  let button (label: string) onClick =
    Html.button [
      prop.className [
        "flex w-full mt-8 justify-center rounded-md bg-sky-600 px-3 py-1.5 text-base font-semibold"
        "leading-6 text-white shadow-sm hover:bg-sky-500 focus-visible:outline focus-visible:outline-2"
        "focus-visible:outline-offset-2 focus-visible:outline-sky-600 select-none" ]
      prop.text label
      prop.onClick onClick
    ]

  let result (resultOpt:(LineItem * LineItem * LineItem) list option) =

    let listItem (name: string) (value: string) =
      Html.li [
        prop.className "col-span-1 divide-y divide-gray-200 rounded-lg"
        prop.children [ 
          Html.div [ 
            prop.className "flex w-full items-center justify-between space-x-6 py-6"
            prop.children [ 
              Html.div [ 
                prop.className "flex-1 truncate"
                prop.children [ 
                  Html.div [ 
                    prop.className "flex"
                    prop.children [ 
                      Html.h3 [ prop.className "truncate text-sm font-medium text-gray-900 w-full text-center"; prop.text name ] 
                    ] 
                  ]
                  Html.p [ prop.className [ "mt-1 truncate text-6xl text-gray-500 w-full text-center" ]; prop.text value ] 
                ] 
              ] 
            ] 
          ] 
        ] 
      ]

    let createRow (x, y, z) = [
      listItem "11_022" $"%d{x.Qty}"
      listItem "11_023" $"%d{y.Qty}"
      listItem "11_024" $"%d{z.Qty}"
    ]

    match resultOpt with
    | Some result ->
        Html.ul [ 
          prop.className "grid grid-cols-3 gap-6 mt-6 border-1 border-blue-100 rounded-lg bg-gray-50 shadow"
          prop.role "list"
          prop.children (result |> List.collect createRow)
        ]
    | None -> Html.div []


  let page dispatch (state: State) =
    Html.div [
      prop.className "relative flex min-h-screen flex-col justify-center overflow-hidden bg-gray-50 py-6 sm:py-12"
      prop.children [
        Html.style FundingSolver.Tailwind.css
        logo
        background
        Html.div [
          prop.classes [ "relative"; "bg-white"; "px-6"; "pb-8"; "pt-10"; "shadow-xl"; "ring-1"; "ring-gray-900/5"; "sm:mx-auto"; "sm:max-w-lg"; "sm:rounded-lg"; "sm:px-10" ]
          prop.children [
            Html.div [
              prop.className "mx-auto max-w-md"
              prop.children [
                Html.h1 [ prop.className "mb-8 text-3xl font-extrabold text-gray-900"; prop.text "Funding Solver" ]
                Html.div [
                  prop.children [
                    Html.div [
                      lineItem state.``11_022``
                      lineItem state.``11_023``
                      lineItem state.``11_024``
                    ]
                    totalInput dispatch state.TotalCost
                    button "Solve" (fun _ -> Solve |> dispatch)
                    result state.Result
                  ]
                ]
              ]
            ]
          ]
        ]
      ]
    ]

  [<ReactComponent>]
  let Render () : ReactElement =

    let state, dispatch = React.useElmish(State.init, State.update, [| |])
    page dispatch state
