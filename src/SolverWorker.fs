module FundingSolver.SolverWorker

open Fable.Core
open Fable.Core.JsInterop
open Browser.Types
open FundingSolver.WorkerTypes

// Helper to post message back to main thread
[<Emit("self.postMessage($0)")>]
let postMessage (msg: obj) : unit = jsNative

// Helper to add event listener on self
[<Emit("self.addEventListener($0, $1)")>]
let addEventListener (eventType: string) (handler: MessageEvent -> unit) : unit = jsNative

let solve (data: WorkerMessageData) =
    printfn "Worker: Starting calculation..."
    
    let solutions = ResizeArray<int * int * int>()
    
    for x in 0 .. data.maxQty do
        for y in 0 .. data.maxQty do
            for z in 0 .. data.maxQty do
                let total = (data.price022 * float x) + (data.price023 * float y) + (data.price024 * float z)
                if total = data.totalCost then
                    solutions.Add((x, y, z))
    
    printfn $"Worker: Found {solutions.Count} solutions"
    
    if solutions.Count = 0 then
        postMessage {| ``type`` = "NoSolutions" |}
    else
        postMessage {| ``type`` = "Solutions"; solutions = solutions.ToArray() |}

// Initialize worker - listen for messages
addEventListener "message" (fun (e: MessageEvent) ->
    let data = e.data :?> WorkerMessageData
    solve data
)
