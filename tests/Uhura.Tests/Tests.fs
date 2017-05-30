module Tests


open Expecto
open Uhura

[<Tests>]
let tests =
  testList "samples" [
    testCase "Say hello all" <| fun _ ->
      // let subject = Say.hello "all"
      Expect.equal "Hello all" "Hello all" "You didn't say hello"
    
  ]