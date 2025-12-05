open System
open System.Collections.Generic
open System.IO

let toInt ch = int (Char.GetNumericValue ch)

type Battery (value: string, numSwitches: int) =
  let findMax start stop =
    let mutable v = toInt value[start]
    let mutable vindex = start
    let mutable i = start + 1
    while i < stop do
      let t = toInt value[i]
      if t > v then
        v <- t
        vindex <- i
      i <- i + 1
    vindex

  member this.maxJolts: Int64 =
    let digits = List<String>()
    let mutable previousIndex = 0
    let switches = List.rev [0..(numSwitches - 1)]
    for i in switches do
      let index = findMax previousIndex (value.Length - i)
      digits.Add (string value[index])
      previousIndex <- (index + 1)
    Int64.Parse (String.Join ("", digits))

let total numSwitches =
  File.ReadAllLines "./input.txt"
    |> Array.toSeq
    |> Seq.map
      (fun batt -> Battery (batt, numSwitches))
    |> Seq.map _.maxJolts
    |> Seq.sum

printfn "%d" (total 2)
printfn "%d" (total 12)
