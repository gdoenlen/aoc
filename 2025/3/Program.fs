open System
open System.IO

let toInt ch = int (Char.GetNumericValue ch)

type Battery (value: string) =
  let maxL =
    let mutable i= 1
    let mutable l = toInt value[0]
    let mutable lindex = 0
    while i < value.Length - 2 do
      let v = toInt value[i]
      if v > l then
        l <- v
        lindex <- i
      i <- i + 1
    lindex

  let maxR start =
    printfn "  starting r search at %d" start
    printfn "  maxL = %c" value[start - 1]
    let mutable r = toInt value[start]
    let mutable rindex = start
    let mutable i = start + 1
    while i < value.Length - 1 do
      let v = toInt value[i]
      if v > r then
        r <- v
        rindex <- i
      i <- i + 1
    rindex

  member this.maxJolts: int =
    let lindex = maxL
    let l = string value[lindex]
    let rindex = maxR (lindex + 1)
    let r = string value[rindex]
    printfn "lr = %s" (l + r)
    Int32.Parse (l + r)

let total =
  File.ReadAllLines "./input.txt"
    |> Array.toSeq
    |> Seq.map
      (fun batt ->
        printfn "batt %s" batt
        Battery batt)
    |> Seq.map _.maxJolts
    |> Seq.sum

printfn "%d" total
