﻿module TestTasks

open BlackFox.Fake
open Fake.DotNet

open ProjectInfo
open BasicTasks

let runTestsDotnet = BuildTask.create "runTestsDotnet" [clean; build] {
    testProjects
    |> Seq.iter (fun testProject ->
        Fake.DotNet.DotNet.test(fun testParams ->
            {
                testParams with
                    Logger = Some "console;verbosity=detailed"
                    Configuration = DotNet.BuildConfiguration.fromString configuration
                    NoBuild = true
            }
        ) testProject
    )
}

module private FableHelper =

    let FableTestPath = "tests/FSharpAux.Mocha.Tests/fable"

    open Fake.Core

    let createProcess exe arg dir =
        CreateProcess.fromRawCommandLine exe arg
        |> CreateProcess.withWorkingDirectory dir
        |> CreateProcess.ensureExitCode

    let dotnet = createProcess "dotnet"

    let npm =
        let npmPath =
            match ProcessUtils.tryFindFileOnPath "npm" with
            | Some path -> path
            | None ->
                "npm was not found in path. Please install it and make sure it's available from your path. " +
                "See https://safe-stack.github.io/docs/quickstart/#install-pre-requisites for more info"
                |> failwith

        createProcess npmPath

    let run proc arg dir =
        proc arg dir
        |> Proc.run
        |> ignore
    
let cleanFable = BuildTask.create "cleanFable" [clean; build] {
    System.IO.Directory.CreateDirectory FableHelper.FableTestPath |> ignore
    FableHelper.run FableHelper.dotnet "fable clean --yes" FableHelper.FableTestPath
}

/// runs `npm test` in root. 
/// npm test consists of `test` and `pretest`
/// check package.json in root for behavior
let runTestsFable = BuildTask.create "RunTestsFable" [clean; cleanFable; build] {
    FableHelper.run FableHelper.npm "test" ""
}

let runTests = BuildTask.create "RunTests" [clean; build; runTestsFable; runTestsDotnet] { 
    ()
}

// to do: use this once we have actual tests
let runTestsWithCodeCov = BuildTask.create "RunTestsWithCodeCov" [clean; build] {
    let standardParams = Fake.DotNet.MSBuild.CliArguments.Create ()
    testProjects
    |> Seq.iter(fun testProject -> 
        Fake.DotNet.DotNet.test(fun testParams ->
            {
                testParams with
                    MSBuildParams = {
                        standardParams with
                            Properties = [
                                "AltCover","true"
                                "AltCoverCobertura","../../codeCov.xml"
                                "AltCoverForce","true"
                            ]
                    };
                    Logger = Some "console;verbosity=detailed"
            }
        ) testProject
    )
}