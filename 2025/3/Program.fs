open System
open System.IO

let toInt ch = int (Char.GetNumericValue ch)

type Battery (value: string) =
  let findMax start stop =
    let mutable v = toInt value[start]
    let mutable vindex = start
    let mutable i = start + 1
    while i <= stop do
      let t = toInt value[i]
      if t > v then
        v <- t
        vindex <- i
      i <- i + 1
    vindex

  member this.maxJolts: int =
    let lindex = findMax 0 (value.Length - 2)
    let l = string value[lindex]
    let rindex = findMax (lindex + 1) (value.Length - 1)
    let r = string value[rindex]
    Int32.Parse (l + r)

let total =
  File.ReadAllLines "./input.txt"
    |> Array.toSeq
    |> Seq.map
      (fun batt -> Battery batt)
    |> Seq.map _.maxJolts
    |> Seq.sum

printfn "%d" total
