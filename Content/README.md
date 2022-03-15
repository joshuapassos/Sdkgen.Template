## How generate types

```
npx sdkgen src/Schemas/Api.sdkgen -o src/Api.fs -t fsharp_server
```

## How Run Server

```
dotnet tool restore
dotnet build
dotnet run --project src/SdkgenServer.fsproj
```
