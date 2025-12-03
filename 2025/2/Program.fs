open System
open System.IO

type Id =
  abstract member isFake: bool
  abstract member value: Int64

type PartOneId (text: string, value: Int64) =
  interface Id with
    member this.isFake =
      if Int32.IsEvenInteger text.Length then
        let middle = text.Length / 2 // 0 based middle
        let left = text.AsSpan(0, middle)
        let right = text.AsSpan middle
        left.SequenceEqual right
      else
        false
    member this.value: Int64 = value

type PartTwoId (text: String, value: Int64) =
  let fake (text: string) =
    match text.Length with
    | 2 -> text.Chars 0 = text.Chars 1
    | 3 -> text.Chars 0 = text.Chars 1 && text.Chars 1 = text.Chars 2
    | 4 -> text.Chars 0 = text.Chars 1 && text.Chars 2 = text.Chars 3
    | _ -> failwith "todo"

  interface Id with
    member this.isFake = fake text

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
