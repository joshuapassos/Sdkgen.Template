open SdkgenGenerated
open Sdkgen.Runtime
open System

let api = Api()

api.GetPost <- fun _ctx args ->
  task {
    let author = PostAuthor.Create("Bob")
    let post = Post.Create(args.id, "First Post", "This is the first post", DateTime.Now, author)
    return Some(post)
}

let server = SdkgenHttpServer(api)
server.Listen(8000)