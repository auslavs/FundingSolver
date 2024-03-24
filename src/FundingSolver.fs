namespace FundingSolver.Components

open Feliz
open Elmish
open Feliz.UseElmish

module FundingSolver =

  type LineItemResult = {
    Id: string
    Qty: int
  }

  type SolverResult =
    | Success of (LineItemResult * LineItemResult * LineItemResult) list
    | InProgress
    | NoSolutionsFound
    | Error of string
    | NotStarted

  type State = {
    TotalCost: string
    ``11_022``: LineItem
    ``11_023``: LineItem
    ``11_024``: LineItem
    Result: SolverResult
  }
  and LineItem = {
    Id: string
    Price: float
  }

  type Msg =
    | UpdateLineItemCost of string * float
    | UpdateTotalCost of string
    | Solve
    | Solved of SolverResult

  module State =

    let [<Literal>] MaxQty = 500
    
    let init () =
      { TotalCost = ""
        ``11_022`` = { Id = "11_022"; Price = 214.41 }
        ``11_023`` = { Id = "11_023"; Price = 193.99 }
        ``11_024`` = { Id = "11_024"; Price = 74.63 }
        Result = NotStarted
      }, Cmd.none


    let solve (state: State) = async {
      printfn "Solving..."

      let _022 = state.``11_022``
      let _023 = state.``11_023``
      let _024 = state.``11_024``

      match System.Double.TryParse(state.TotalCost) with
      | (true, totalCost) ->
          return
            [ for x in [0..MaxQty] do
              for y in [0..MaxQty] do
              for z in [0..MaxQty] do
                if (_022.Price * (float x)) + (_023.Price * (float y)) + (_024.Price * (float z)) = totalCost then
                  yield ( 
                    { Id = "11_022"; Qty = x },
                    { Id = "11_023"; Qty = y },
                    { Id = "11_024"; Qty = z }
                  )
            ] |> function
            | [] -> NoSolutionsFound
            | x -> Success x
      | (false, _) -> return Error "Total cost is not a valid number"
    }


    let update : Msg -> State -> State * Cmd<Msg> =
      fun (msg: Msg) (state: State) ->
        match msg with
        | UpdateLineItemCost (id, price) ->
            let newState = 
              match id with
              | "11_022" -> { state with ``11_022`` = { state.``11_022`` with Price = price } }
              | "11_023" -> { state with ``11_023`` = { state.``11_023`` with Price = price } }
              | "11_024" -> { state with ``11_024`` = { state.``11_024`` with Price = price } }
              | _ -> failwith "Invalid line item id"
            newState, Cmd.none
        | UpdateTotalCost tc ->
            { state with TotalCost = tc }, Cmd.none
        | Solve ->
            let solveAsync () = state |> solve
            { state with Result = InProgress }, Cmd.OfAsync.perform solveAsync () Solved
        | Solved result ->
            { state with Result = result }, Cmd.none

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

  let priceInput dispatch itemId price =
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
          prop.onChange (fun x -> UpdateLineItemCost (itemId, x) |> dispatch)
        ]
        Html.div [
          prop.classes [ "pointer-events-none"; "absolute"; "inset-y-0"; "right-0"; "flex"; "items-center"; "pr-3" ]
          prop.children [ Html.span [ prop.classes [ "text-gray-500"; "sm:text-sm" ]; prop.id "price-currency"; prop.text "AUD" ] ]
        ]
      ]
    ]

  let lineItemInput dispatch { Id=itemId; Price=itemPrice } =
    Html.div [
      prop.className "mt-2"
      prop.children [
        Html.label [ prop.className "block text-sm font-medium leading-6 text-gray-900"; prop.for' "total"; prop.text itemId ]
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
              prop.onTextChange (fun newCost -> (itemId, float newCost) |> UpdateLineItemCost |> dispatch)
              prop.value itemPrice
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
    

  let button (label: string) isPrimary isWaiting onClick =
    let spinner = Html.span [ prop.text "Working..." ]

    Html.button [
      prop.className [
        "flex w-full mt-4 justify-center rounded-md px-3 py-1.5 text-base font-semibold"
        "leading-6 shadow-sm focus-visible:outline focus-visible:outline-2"
        "focus-visible:outline-offset-2 select-none"
        if isPrimary
        then "bg-sky-600 text-white hover:bg-sky-500 focus-visible:outline-sky-600"
        else "bg-gray-200 text-gray-800 hover:bg-gray-300 focus-visible:outline-gray-200" ]
      prop.text label
      prop.onClick <| if isWaiting then (fun _ -> ()) else onClick
      prop.disabled isWaiting
      prop.children [
        if isWaiting then spinner else Html.span [ prop.text label]
      ]
    ]

  let result (result: SolverResult) =

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

    match result with
    | InProgress -> Html.none
    | NoSolutionsFound -> Html.div [ prop.className "mt-6"; prop.text "No solutions found" ]
    | Error msg -> Html.div [ prop.className "mt-6"; prop.text msg ]
    | NotStarted -> Html.none
    | Success resultLst ->
        Html.ul [ 
          prop.className "grid grid-cols-3 gap-6 mt-6 border-1 border-blue-100 rounded-lg bg-gray-50 shadow"
          prop.role "list"
          prop.children (resultLst |> List.collect createRow)
        ]


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
              prop.className "mx-auto max-w-md sm:w-96 "
              prop.children [
                Html.h1 [ prop.className "mb-8 text-3xl font-extrabold text-gray-900"; prop.text "Funding Solver" ]
                Html.div [
                  prop.children [
                    Html.div [
                      lineItemInput dispatch state.``11_022``
                      lineItemInput dispatch state.``11_023``
                      lineItemInput dispatch state.``11_024``
                    ]
                    totalInput dispatch state.TotalCost
                    button "Example" false false (fun _ -> UpdateTotalCost "17153.28" |> dispatch)
                    button "Solve" true (state.Result = InProgress) (fun _ -> Solve |> dispatch)
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
