open System
open System.IO

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

let evenIds (ids: seq<int64>) =
  ids
    |> Seq.map
      (fun id -> string id)
    |> Seq.filter
      (fun id -> Int32.IsEvenInteger id.Length)

let matches (ids: seq<string>) =
  ids
    |> Seq.filter
      (fun id ->
        let middle = id.Length / 2 // 0 based middle
        let left = id.Substring(0, middle)
        let right = id.Substring middle
        left = right)
    |> Seq.map
      (fun id -> Int64.Parse id)

let total =
  matches (evenIds ids)
   |> Seq.sum

printfn "%d" total
