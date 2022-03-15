module rec SdkgenGenerated
open Sdkgen.Runtime
open Sdkgen.Helpers
open Sdkgen.Context
open System.Threading.Tasks
open System.Text.Json
open System
open System.Globalization
#if NET5_0
// To use tasks as computation expression in .NET 5 install https://www.nuget.org/packages/TaskBuilder.fs/
open FSharp.Control.Tasks.V2.ContextInsensitive
#endif

type FatalException =
  inherit SdkgenException

  new(message: string) = {
    inherit SdkgenException("Fatal", message, null)
  }

  new(message: string, inner: Exception) = {
    inherit SdkgenException("Fatal", message, inner)
  }


type PostAuthor = {
  Name: string
} with
  static member Create (name: string): PostAuthor =
    { Name = name }

let DecodePostAuthor (json_: JsonElement) (path_: string): PostAuthor =
  if (json_.ValueKind <> JsonValueKind.Object) then raise (FatalException($"'{path_}' must be an object."))
  
  let nameJson_ = decodeJsonElementStrict "name" json_ $"{path_}.name"
  let name =
    decodeString nameJson_ $"{path_}.name"
  
  { Name = name }

let EncodePostAuthor (obj_: PostAuthor) (resultWriter_: Utf8JsonWriter) (path_: string) =
  resultWriter_.WriteStartObject()
  
  resultWriter_.WritePropertyName("name")
  resultWriter_.WriteStringValue(obj_.Name)
  resultWriter_.WriteEndObject()


type Post = {
  Id: Guid;
  Title: string;
  Body: string;
  CreatedAt: DateTime;
  Author: PostAuthor
} with
  static member Create (id': Guid, title: string, body: string, createdAt: DateTime, author: PostAuthor): Post =
    { Id = id'; Title = title; Body = body; CreatedAt = createdAt; Author = author }

let DecodePost (json_: JsonElement) (path_: string): Post =
  if (json_.ValueKind <> JsonValueKind.Object) then raise (FatalException($"'{path_}' must be an object."))
  
  let idJson_ = decodeJsonElementStrict "id" json_ $"{path_}.id"
  let id' =
    decodeUuid idJson_ $"{path_}.id"
  
  let titleJson_ = decodeJsonElementStrict "title" json_ $"{path_}.title"
  let title =
    decodeString titleJson_ $"{path_}.title"
  
  let bodyJson_ = decodeJsonElementStrict "body" json_ $"{path_}.body"
  let body =
    decodeString bodyJson_ $"{path_}.body"
  
  let createdAtJson_ = decodeJsonElementStrict "createdAt" json_ $"{path_}.createdAt"
  let createdAt =
    decodeDateTime createdAtJson_ $"{path_}.createdAt"
  
  let authorJson_ = decodeJsonElementStrict "author" json_ $"{path_}.author"
  let author =
    DecodePostAuthor authorJson_ $"{path_}.author"
  
  { Id = id'; Title = title; Body = body; CreatedAt = createdAt; Author = author }

let EncodePost (obj_: Post) (resultWriter_: Utf8JsonWriter) (path_: string) =
  resultWriter_.WriteStartObject()
  
  resultWriter_.WritePropertyName("id")
  resultWriter_.WriteStringValue(obj_.Id)

  resultWriter_.WritePropertyName("title")
  resultWriter_.WriteStringValue(obj_.Title)

  resultWriter_.WritePropertyName("body")
  resultWriter_.WriteStringValue(obj_.Body)

  resultWriter_.WritePropertyName("createdAt")
  resultWriter_.WriteStringValue(obj_.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFF'Z'"))

  resultWriter_.WritePropertyName("author")
  EncodePostAuthor obj_.Author resultWriter_ $"{path_}.author"
  resultWriter_.WriteEndObject()

type Api() =
  member val GetPost: Context -> {| id: Guid; |} -> Task<Post option> =
    fun _ -> (raise (FatalException("Function 'getPost' not implemented.")))
    with get, set
    
  interface BaseApi with
    member __.ExecuteFunction(context_: Context, resultWriter_: Utf8JsonWriter) : Task =
        task {
          match context_.Name with
          | "getPost" ->
            let idJson_ =
              match context_.Args.TryGetValue("id") with
              | true, v -> v
              | _ -> raise (FatalException("'getPost().args.id' must be set to a value of type uuid."))

            let id' =
              decodeUuid idJson_ "getPost().args.id"

            let! result_ = (__.GetPost context_ {| id = id' |})
            if (result_.IsNone) then
              resultWriter_.WriteNullValue()
            else
              EncodePost result_.Value resultWriter_ "getPost().ret"

          | _ -> raise (FatalException($"Unknown function '{context_.Name}'."))
        } :> Task
    member __.GetAstJson() = """{
            "annotations": {},
            "errors": [
                "Fatal"
            ],
            "functionTable": {
                "getPost": {
                    "args": {
                        "id": "uuid"
                    },
                    "ret": "Post?"
                }
            },
            "typeTable": {
                "PostAuthor": {
                    "name": "string"
                },
                "Post": {
                    "id": "uuid",
                    "title": "string",
                    "body": "string",
                    "createdAt": "datetime",
                    "author": "PostAuthor"
                }
            }
        }""";