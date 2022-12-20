module UriTests

open FSharpAux
open Expecto
open System

let testUri1 = Uri("localhost:1000")
let testUri1' = Uri("localhost:1000/")
let resultUri1a = Uri(testUri1', "ping")
let resultUri2a = Uri(Uri(testUri1', "ping/"), "pong")

let resultUri1b = Uri.combine [|"localhost:1000"; |]

[<Tests>]
let uriTests =
    testList "UriTests" [
        testList "Uri.combine" [
            testCase "concatinates Uris correctly" (fun _ ->
                Expect.isTrue
            )
        ]
    ]