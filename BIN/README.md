# Compiled binaries
Compiled to naitive win x64 architecture.

## How to use
Make a \*.csproj file containing:
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <PublishAot>true</PublishAot>
    <Configuration>Release</Configuration>
  </PropertyGroup>

</Project>
```

Compile: 
```dotnet publish -r win10-x64```

Run: 
```bin\Release\net7.0\win10-x64\native\*.exe```