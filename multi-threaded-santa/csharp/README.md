# Multi-Threaded Santa — C# Reference Implementation

- .NET 8+
- xUnit for the test runner
- FluentAssertions for readable assertions
- `Channel<T>` for bounded queues
- `Task` for concurrent workers
- `SemaphoreSlim` for sleigh exclusion

## Build & Run

```bash
cd multi-threaded-santa/csharp
dotnet restore
dotnet test
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
