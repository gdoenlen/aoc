open System
open System.IO

let parseLine (line: string) =
  let amount = Int32.Parse line[1..]
  match line[0] with
  | 'L' -> -amount
  | 'R' -> amount
  | _ -> failwith "Not Left or Right?"

let parseFile =
  File.ReadLines("./input.txt")
   |> Seq.map parseLine

type State = {
  current: int
  next: int
  count: int
  move: int
} with
  member this.numRotations = abs this.move / 100

let count moves (fn: State -> int) =
  let _, count =
    moves
      |> Seq.fold
        (fun (index, count) move ->
          let next = (index + move) % 100
          let mutable count = fn {
            current = index
            next = next
            count = if next = 0 then count + 1 else count
            move = move
          }
          (next, count))
        (50, 0)
  count

let partTwo (state: State) =
  let mutable count = state.count + state.numRotations
  if state.next < 0 && state.current > 0 then
    count <- count + 1
  if state.next > 0 && state.current < 0 then
    count <- count + 1
  count


let moves = parseFile
printfn "part one %d" (count moves (fun state -> state.count))
printfn "part two %d" (count moves partTwo)
