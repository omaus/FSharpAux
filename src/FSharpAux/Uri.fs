module Uri

open FSharpAux
open System


/// Creates a single URI out of a collection of strings.
let ofStrings (uris : #seq<string>) =
    if String.isNullOrEmpty (Seq.item 0 uris) then raise (UriFormatException())
    let revisedUris =
        uris
        |> Seq.mapi (
            fun i u ->
                if String.isNullOrEmpty u then u
                elif String.last u <> '/' && i < Seq.length uris - 1 then u + "/"
                else u
        )
    let rec loop baseUri i =
        if i < Seq.length uris - 1 then
            let newUri = Uri(baseUri, Seq.item i revisedUris)
            loop newUri (i + 1)
        else Uri(baseUri, Seq.item i revisedUris)
    loop (Uri (Seq.head revisedUris)) 0
    
/// Combines a collection of URIs into one.
let combine (uris : #seq<Uri>) =
    uris
    |> Seq.fold (
        fun acc uri -> Uri(acc, uri)
    ) (Seq.head uris)