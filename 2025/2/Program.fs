open System
open System.IO

type Id =
  abstract member isFake: bool
  abstract member value: Int64

type PartOneId (text: string, value: Int64) =
  interface Id with
    member this.isFake =
      if Int32.IsEvenInteger text.Length then
        let middle = text.Length / 2
        let left = text.AsSpan(0, middle)
        let right = text.AsSpan middle
        left.SequenceEqual right
      else
        false
    member this.value: Int64 = value

type PartTwoId (text: String, value: Int64) =
  let chunks (size: int) =
    let length = text.Length
    [ for i in 0 .. size .. (length - 1) do
        yield text.Substring(i, min size (length - i)) ]

  let allMatch (items: List<string>) =
    if items.Length = 1 then
      items.Head
        |> Seq.forall
          (fun ch -> ch = items.Head.Chars 0)
    else
      let item =
        items
        |> Seq.tryFind
          (fun item -> item <> items[0])
      match item with
      | Some _ -> false
      | None -> true

  let isChunkable size = text.Length % size = 0

  let allChunksMatch size =
    if isChunkable size then
      let chunkSize = text.Length / size
      let chunks = chunks chunkSize
      allMatch (Seq.toList chunks)
    else
      false

  interface Id with
    member this.isFake =
      let mutable loop = true
      let mutable i = text.Length - 1
      let mutable fake = false
      while loop && i > 0 do
        if allChunksMatch i then
          fake <- true
          loop <- false
        else
          i <- i - 1
      fake

    member this.value = value

let toRange (s: string) =
  let parts = s.Split("-")
  let start = Int64.Parse parts[0]
  let last= Int64.Parse parts[1]
  [start..last]

type Part = One | Two

let toId (id: Int64, part: Part): Id =
  match part with
  | One -> PartOneId (string id, id)
  | Two -> PartTwoId (string id, id)

let total part =
  File.ReadAllText "./input.txt"
    |> _.Split(",")
    |> Array.toSeq
    |> Seq.map toRange
    |> Seq.collect id
    |> Seq.map
      (fun id -> toId (id, part))
    |> Seq.filter _.isFake
    |> Seq.map _.value
    |> Seq.sum

printfn "%d" (total One)
printfn "%d" (total Two)
