namespace FundingSolver

/// Shared types for Web Worker communication
/// Both main app and worker reference this for type safety
module WorkerTypes =

    type WorkerMessageData = {
        price022: float
        price023: float
        price024: float
        totalCost: float
        maxQty: int
    }

    type WorkerResponseData = {
        ``type``: string
        solutions: (int * int * int) array option
    }
