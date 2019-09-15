# OxygenBasic.NET

.Net Wrapper for OxygenBasic Programming Language.

![GitHub](https://img.shields.io/github/license/jiowcl/MQL-Quotes.svg)
![Nuget](https://img.shields.io/nuget/v/OxygenBasic.NET)
![Travis-Ci](https://travis-ci.com/jiowcl/OxygenBasic.NET.svg?branch=master)

## Environment

- Windows 7 above (recommend)
- OxygenBasic 0.23

## NuGet Installation

```powershell
PM> Install-Package OxygenBasic.NET
```

## How to Build

Building requires [Visual Studio 2019 Community](https://visualstudio.microsoft.com/vs/community/) and test under Windows 10.

## Example

```csharp
string scriptPath = @"Sample\test_fib.txt";
string scriptBuffer = File.ReadAllText(scriptPath, Encoding.UTF8);

Oxygenbasic.O2Basic(scriptBuffer);
Oxygenbasic.Exec();

Console.ReadKey();
```

## License

Copyright (c) 2017-2019 Ji-Feng Tsai.  

Code released under the MIT license.

## TODO

- More examples

## Donation

If this application help you reduce time to trading, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/TW/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3RNMD6Q3B495N&source=url)

[Paypal Me](https://paypal.me/jiowcl?locale.x=zh_TW)
