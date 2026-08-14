# OxygenBasic.NET

.Net Wrapper for OxygenBasic Programming Language.

![GitHub](https://img.shields.io/github/license/jiowcl/OxygenBasic.NET)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![OxygenBasic](https://img.shields.io/badge/Language-OxygenBasic-00A896?style=flat-square)
![Nuget](https://img.shields.io/nuget/v/OxygenBasic.NET)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/06f3d62e7abb41c290f9feeb44bd4827)](https://www.codacy.com/gh/jiowcl/OxygenBasic.NET/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=jiowcl/OxygenBasic.NET&amp;utm_campaign=Badge_Grade)

## Environment

- Windows 10 above (recommend)  
- OxygenBasic 0.90 
- .NET 8 / .NET 9 / .NET 10 (Windows, **x86 and x64**)  
- Native: `oxygen.dll` (x86) / `oxygen64.dll` (x64), selected at runtime via `DllImport` resolver  

> **x64 runtime:** Prefer x86 for execution today. See [docs/oxygen64-x64-runtime.md](docs/oxygen64-x64-runtime.md) for the `oxygen64.dll` `DllMain` AV investigation.

## CI

GitHub Actions runs on `windows-latest`:

- **x86**: build + test (net8/9/10) + hosted example  
- **x64**: build only (native tests skipped; see [docs/oxygen64-x64-runtime.md](docs/oxygen64-x64-runtime.md))

## NuGet Installation

```powershell
PM> Install-Package OxygenBasic.NET
```

## How to Build

Building requires [Visual Studio 2026 Community](https://visualstudio.microsoft.com/vs/community/) and test under Windows 11.

## Example

Hosted script with include-path callback and shared .NET memory (see `OxygenBasic.Example`):

```csharp
OxygenRunResult result = Oxygenbasic.Run(script, new OxygenHostOptions
{
    IncludeRoot = @"Sample\inc",
    VarResolver = name => /* optional host variable lookup */
});
```

`Run` performs `InitHost` → Pathcall (`%app_includepath%`) → `O2Basic` → `Exec`, and throws `OxygenException` on compile/runtime errors (`ThrowOnError = false` to inspect `OxygenRunResult` instead).

See `OxygenBasic.Example` for include-path + shared .NET memory (`Sample\hosted_demo.txt`, `Sample\inc\math_helpers.inc`).  
A simpler Fibonacci-only script remains at `Sample\test_fib.txt`.

## License

Copyright (c) 2017-2026 Ji-Feng Tsai.  
OxygenBasic Copyright (c) Charles Pegge [OxygenBasic Compiler](https://github.com/Charles-Pegge/OxygenBasic).  
Code released under the MIT license.  

## Donation

If this application help you reduce time to coding, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/TW/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3RNMD6Q3B495N&source=url)

[Paypal Me](https://paypal.me/jiowcl?locale.x=zh_TW)
