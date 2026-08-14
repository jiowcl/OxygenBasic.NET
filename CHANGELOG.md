# Changelog

## 1.0.8

**Breaking** for callers upgrading from 1.0.7 and earlier.

### Breaking changes

Native pointers are no longer truncated to 32-bit `uint`. These methods now return `IntPtr` so x64 addresses stay intact:

| Method | 1.0.7 | 1.0.8 |
|--------|-------|-------|
| `O2Basic` | `uint` | `IntPtr` |
| `Exec()` / `Exec(...)` | `uint` | `IntPtr` |
| `Buf` | `uint` | `IntPtr` |
| `Lib` | `uint` | `IntPtr` |
| `Link` | `uint` | `IntPtr` |
| `Eval` | `uint` (alias of `Link`) | `IntPtr` |

`Pathcall` / `Varcall` pointer arguments are `IntPtr` (the old `uint` overloads remain for x86).

**Migration:**

```csharp
// 1.0.7
uint code = Oxygenbasic.O2Basic(script);
if (code > 0 && Oxygenbasic.Errno() == 0)
{
    uint ran = Oxygenbasic.Exec();
}

// 1.0.8
IntPtr code = Oxygenbasic.O2Basic(script);
if (code != IntPtr.Zero && Oxygenbasic.Errno() == 0)
{
    IntPtr ran = Oxygenbasic.Exec();
}

// or use the hosted runner
OxygenRunResult result = Oxygenbasic.Run(script);
```

A 64-bit process (including **AnyCPU** on 64-bit Windows) no longer loads `oxygen64.dll` (that path AVs in `DllMain`). The first native call throws `PlatformNotSupportedException`. Prefer **x86**. See [docs/oxygen64-x64-runtime.md](docs/oxygen64-x64-runtime.md).

### Other

- Manual UTF-8 BSTR marshaling (replaces obsolete `UnmanagedType.AnsiBStr`)
- Hosted `Run` / `RunFile` with `OxygenHostOptions` / `OxygenRunResult`
- `InitHost`, `Pathcall` / `Varcall` managed resolvers, `Eval` alias
- Runtime resolver for `oxygen.dll` (x86) / `oxygen64.dll` (x64)
- Windows GitHub Actions CI (x86 test, x64 build)
