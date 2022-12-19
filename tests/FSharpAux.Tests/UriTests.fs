module UriTests

open FSharpAux
open Expecto
open System

let testUri1 = Uri("localhost:1000")
let testUri1' = Uri("localhost:1000/")
let resultUri1 = Uri(testUri1', "ping")