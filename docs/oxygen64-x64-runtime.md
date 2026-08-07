# oxygen64.dll x64 runtime issue  

Official OxygenBasic ships both `oxygen.dll` (x86) and `oxygen64.dll` (x64). The managed wrapper resolves the correct file at runtime via `DllImport` / `NativeLibrary` resolver.

On current Windows 11 hosts, **`oxygen64.dll` cannot complete a normal load**. Prefer **x86 (`oxygen.dll`)** for execution until upstream provides a fixed binary.

## Symptom  

`LoadLibrary` / `NativeLibrary.Load` of `oxygen64.dll` raises:

**ACCESS_VIOLATION (0xC0000005)** inside `DllMain` during `DLL_PROCESS_ATTACH`.

## Reproduced with  

- .NET `NativeLibrary` / P/Invoke (`OxygenBasic.NET`)
- Native MSVC `LoadLibraryW` (no .NET involved)

So this is not a managed marshaling bug.

## PE Traits (inspected)  

| Field | Value |
|-------|--------|
| Machine | `IMAGE_FILE_MACHINE_AMD64` (PE32+) |
| DLL flag | Yes |
| ImageBase | `0x10000000` |
| BaseReloc directory | **Absent** |
| Imports | `KERNEL32.DLL`, `USER32.DLL` |

## Further Probes  

| Step | Result |
|------|--------|
| `LoadLibraryEx(..., DONT_RESOLVE_DLL_REFERENCES)` | Succeeds (maps at preferred base) |
| Manual IAT bind for KERNEL32/USER32 | Succeeds |
| Call `o2_mode` / `o2_errno` without `DllMain` | Works |
| Call `o2_basic` / `o2_version` without `DllMain` | AV |
| Call `DllMain(PROCESS_ATTACH)` manually | AV |

Skipping `DllMain` is not a viable workaround: the compiler entry points still need initialization that only `DllMain` would perform, and that path itself faults.

## Impact on this Project  

- **x86**: full build, tests (net8/9/10), hosted example — supported
- **x64**: managed build and NuGet packaging of `oxygen64.dll` — ready; **native tests skipped** in CI until the DLL loads cleanly
- Resolver remains in place so a fixed upstream `oxygen64.dll` works without managed API changes

## TODO  

Waiting for OxygenBasic / Charles Pegge:

1. `oxygen64.dll` `DllMain(DLL_PROCESS_ATTACH)` → ACCESS_VIOLATION on Windows 11  
2. PE has `ImageBase=0x10000000` and no relocation table  
3. Reproducible with a minimal native `LoadLibraryW` host
