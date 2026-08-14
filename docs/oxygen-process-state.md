# oxygen.dll process-wide state

`oxygen.dll` / `oxygen64.dll` keep **one compiler engine per process**. This wrapper cannot isolate sessions inside the same process: there is no native reset API.

## Rules

- **One thread at a time.** Public APIs share a lock so concurrent calls are serialized, but they still mutate the same native engine. Do not compile from thread-pool workers in parallel and expect independent results.
- **`Abst()` is sticky.** `o2_abst` switches the engine into abstract/assembler view. Later `O2Basic` / `Run` / `Exec` / `Link` fail or produce assembler errors. After `Abst()`, those APIs throw `InvalidOperationException`. Check `Oxygenbasic.IsAbstractMode`.
- **No in-process reset.** To compile again after `Abst()`, **start a new process**. Unloading `oxygen.dll` from a running .NET process is not supported.

## Suggested patterns

Hosted scripts (typical):

```csharp
Oxygenbasic.Run(source); // compile + exec in this process
```

Abstract listing (tooling): run `Abst` in a **short-lived helper process**, or only after you are done compiling.

Out-of-process isolation: host Oxygen in a child `dotnet` process (x86) and talk over stdin/stdout or a named pipe if you need many independent compiles.
