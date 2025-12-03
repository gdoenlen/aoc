open System
open System.IO


type Id(value: string) =
  member this.isFake =
    if Int32.IsEvenInteger value.Length then
      let middle = value.Length / 2 // 0 based middle
      let left = value.AsSpan(0, middle)
      let right = value.AsSpan middle
      left.SequenceEqual right
    else
      false
  member this.value: Int64 =
    Int64.Parse value

let ids =
  let text = File.ReadAllText "./input.txt"
  text.Split(",")
    |> Array.toSeq
    |> Seq.map
      (fun id ->
        let parts = id.Split("-")
        let start = Int64.Parse parts[0]
        let last= Int64.Parse parts[1]
        [start..last])
    |> Seq.collect
      (fun lst -> lst)
    |> Seq.map
      (fun id -> Id (string id))

let total =
  ids
    |> Seq.filter
      (fun id -> id.isFake)
    |> Seq.map
      (fun id -> id.value)
    |> Seq.sum

printfn "%d" total
