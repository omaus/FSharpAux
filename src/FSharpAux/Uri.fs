module Uri

open FSharpAux
open System

/// Combines an array of URIs into one.
let combine (uris : string []) =
    if String.isNullOrEmpty uris[0] then raise (UriFormatException())
    let revisedUris =
        uris
        |> Array.mapi (
            fun i u ->
                if String.isNullOrEmpty u then u
                elif String.last u <> '/' && i < uris.Length - 1 then u + "/"
                else u
        )
    let rec loop baseUri i =
        if i < uris.Length - 1 then
            let newUri = Uri(baseUri, revisedUris[i])
            loop newUri (i + 1)
        else Uri(baseUri, revisedUris[i])
    loop (Uri revisedUris[0]) 0