﻿module MathTests

open FSharpAux
#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

let mathTests =
    testList "MathTests" [
        testList "Math.nthRoot" [
            testCase "returns correct float" (fun _ ->
                Expect.equal (Math.nthRoot 3 27.) 3. "Math.nthRoot did return correct float"
            )
            testCase "does not return incorrect float" (fun _ ->
                Expect.notEqual (Math.nthRoot 3 27.) 9. "Math.nthRoot did not return incorrect float"
            )
        ]
    ]