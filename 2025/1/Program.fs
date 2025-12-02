open System
open System.IO

let parseLine (line: string) =
  let amount = Int32.Parse line[1..]
  match line[0] with
  | 'L' -> amount
  | 'R' -> -amount
  | _ -> failwith "Not Left or Right?"

let parseFile =
  File.ReadLines "./input.txt"
   |> Seq.map parseLine

type Dial() =
  member val index = 50 with get, private set
  member this.rotate(amount: int) =
    let actual = amount % 100
    this.index <- this.index + actual
    if this.index < 0 then
      this.index <- this.index + 100
    else if this.index >= 100 then
      this.index <- this.index - 100

let partTwo (moves: seq<int>) =
  let dial = Dial()
  moves
    |> Seq.fold
      (fun (count) move ->
        let current = dial.index
        let rotations = abs (move / 100)
        dial.rotate move

        let mutable next = count + rotations
        if dial.index = 0 then
          next <- next + 1
        if move < 0 && dial.index > current then
          next <- next + 1
        if move > 0 && dial.index < current then
          next <- next + 1
        next
      )
      0

let partOne (moves: seq<int>) =
  let dial = Dial()
  moves
    |> Seq.fold
      (fun (count) move ->
        dial.rotate move
        if dial.index = 0 then
          count + 1
        else
          count
      )
      0

let moves = parseFile
printfn "part one %d" (partOne moves)
printfn "part two %d" (partTwo moves)
