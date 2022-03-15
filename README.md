
## Installation

```
dotnet new -i "Sdkgen.Template::*"
```

## Updating the template

You can also explicitly set the version when installing the template:

```
dotnet new -i "Sdkgen.Template::0.0.1"
```

## Basics

After installing the template you can create a new Sdkgen Project using:

```
dotnet new sdkgen
```

If you wish to use a custom ProjectName you can use the `--name` parameter when creating a new application:

```
dotnet new sdkgen --name packageName
```