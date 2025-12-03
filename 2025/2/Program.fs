open System
open System.IO

type Id(text: string, value: Int64) =
  member this.isFake =
    if Int32.IsEvenInteger text.Length then
      let middle = text.Length / 2 // 0 based middle
      let left = text.AsSpan(0, middle)
      let right = text.AsSpan middle
      left.SequenceEqual right
    else
      false
  member this.value: Int64 = value

let toRange (s: string) =
  let parts = s.Split("-")
  let start = Int64.Parse parts[0]
  let last= Int64.Parse parts[1]
  [start..last]

let total =
  File.ReadAllText "./input.txt"
    |> _.Split(",")
    |> Array.toSeq
    |> Seq.map toRange
    |> Seq.collect id
    |> Seq.map
      (fun id -> Id (string id, id))
    |> Seq.filter _.isFake
    |> Seq.map _.value
    |> Seq.sum

printfn "%d" total
