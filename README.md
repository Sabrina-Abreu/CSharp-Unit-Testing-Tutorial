# 🧪 C# Unit Testing Tutorial — .NET 10 + xUnit
## By Daniel Lim-Apo

> **172 tests · 20 examples · 0 failures**  
> A hands-on guide to unit testing in C# from zero to advanced patterns.

---

## 📋 Table of Contents

1. [What is Unit Testing?](#what-is-unit-testing)
2. [Project Structure](#project-structure)
3. [Quick Start](#quick-start)
4. [The 20 Examples](#the-20-examples)
5. [Key Libraries](#key-libraries)
6. [Core Concepts](#core-concepts)
7. [Best Practices](#best-practices)

---

## What is Unit Testing?

A **unit test** verifies one small piece of logic (a "unit") in complete isolation from the rest of the system. A good unit test:

- Runs **fast** (milliseconds, not seconds)
- Is **deterministic** — same input always produces the same result
- Is **isolated** — no database, no network, no filesystem (or fakes them)
- Has a single, clear reason to fail

The most popular pattern for structuring tests is **Arrange / Act / Assert**:

```csharp
[Fact]
public void Add_TwoPositiveNumbers_ReturnsSum()
{
    // Arrange — set up inputs and the object under test
    var calc = new Calculator();

    // Act — call the method being tested
    int result = calc.Add(3, 5);

    // Assert — verify the outcome
    Assert.Equal(8, result);
}
```

---

## Project Structure

```
src/
├── UnitTestingTutorial.slnx          ← solution file
├── Library/                          ← System Under Test (production code)
│   ├── Calculator.cs
│   ├── StringHelper.cs
│   ├── MathHelper.cs
│   ├── ShoppingCart.cs
│   ├── BankAccount.cs
│   ├── Validators.cs
│   ├── UserService.cs
│   ├── EventScheduler.cs
│   ├── OrderService.cs
│   ├── WeatherService.cs
│   ├── FileProcessor.cs
│   ├── ExchangeRateService.cs
│   ├── DiscountCalculator.cs         ← internal class
│   └── ProductCatalog.cs
└── Tests/                            ← all 20 test examples
    ├── 01_BasicAssertionsTests.cs
    ├── 02_StringTests.cs
    ├── 03_NumberTests.cs
    ├── 04_CollectionTests.cs
    ├── 05_ExceptionTests.cs
    ├── 06_BooleanTests.cs
    ├── 07_NullTests.cs
    ├── 08_DateTimeTests.cs
    ├── 09_TheoryTests.cs
    ├── 10_MockingWithMoqTests.cs
    ├── 11_DependencyInjectionTests.cs
    ├── 12_AsyncTests.cs
    ├── 13_FloatingPointTests.cs
    ├── 14_CustomAssertionsTests.cs
    ├── 15_SetupAndTeardownTests.cs
    ├── 16_InternalsVisibleToTests.cs
    ├── 17_FileIOTests.cs
    ├── 18_HttpClientTests.cs
    ├── 19_FluentAssertionsTests.cs
    └── 20_IntegrationStyleTests.cs
```

---

## Quick Start

```bash
# Build everything
dotnet build src/UnitTestingTutorial.slnx

# Run all 172 tests
dotnet test src/UnitTestingTutorial.slnx

# Run tests from a specific example class (xUnit uses FullyQualifiedName~)
dotnet test src/UnitTestingTutorial.slnx --filter "FullyQualifiedName~BasicAssertionsTests"
dotnet test src/UnitTestingTutorial.slnx --filter "FullyQualifiedName~TheoryTests"
dotnet test src/UnitTestingTutorial.slnx --filter "FullyQualifiedName~MockingWithMoqTests"

# Run a single test by exact method name
dotnet test src/UnitTestingTutorial.slnx --filter "FullyQualifiedName=Tests.BasicAssertionsTests.Add_TwoPositiveNumbers_ReturnsSum"

# Run with detailed output
dotnet test src/UnitTestingTutorial.slnx --logger "console;verbosity=detailed"

# Run with code coverage
dotnet test src/UnitTestingTutorial.slnx --collect:"XPlat Code Coverage"
```

> **⚠️ xUnit Filter Note:**  
> xUnit does **not** support the `ClassName=` filter property (that's MSTest/NUnit).  
> Always use `FullyQualifiedName~ClassName` (the `~` means *contains*) to filter by class.

| Goal | Filter expression |
|---|---|
| Run one class | `FullyQualifiedName~TheoryTests` |
| Run one method | `FullyQualifiedName=Tests.TheoryTests.IsPrime_VariousInputs` |
| Run by partial name | `FullyQualifiedName~Async` |
| Run by trait/category | `Trait=Category,Integration` |

---

## The 20 Examples

### 01 — Basic Assertions
**File:** `Tests/01_BasicAssertionsTests.cs` · **SUT:** `Library/Calculator.cs`

The most fundamental xUnit assertions. Every test method is decorated with `[Fact]`.

| Assertion | Purpose |
|---|---|
| `Assert.Equal(expected, actual)` | Values are equal |
| `Assert.NotEqual(expected, actual)` | Values are different |
| `Assert.True(condition)` | Condition is true |
| `Assert.False(condition)` | Condition is false |
| `Assert.Same(obj1, obj2)` | Same reference |
| `Assert.NotSame(obj1, obj2)` | Different references |

```csharp
[Fact]
public void Add_TwoPositiveNumbers_ReturnsSum()
{
    var result = _calc.Add(3, 5);
    Assert.Equal(8, result);
}
```

---

### 02 — String Assertions
**File:** `Tests/02_StringTests.cs` · **SUT:** `Library/StringHelper.cs`

xUnit has dedicated helpers for string comparisons.

```csharp
Assert.Contains("@example.com", result);
Assert.StartsWith("j", result);
Assert.EndsWith("...", result);
Assert.Equal("cba", result, ignoreCase: true);
```

---

### 03 — Number / Math Tests
**File:** `Tests/03_NumberTests.cs` · **SUT:** `Library/MathHelper.cs`

Tests numeric edge cases — zero, negatives, boundary values — and uses `Assert.InRange` to check that a value is within acceptable bounds.

```csharp
Assert.InRange(result, 0, int.MaxValue);
Assert.Equal([0, 1, 1, 2, 3, 5, 8], fib);
```

---

### 04 — Collection Assertions
**File:** `Tests/04_CollectionTests.cs` · **SUT:** `Library/ShoppingCart.cs`

| Assertion | Purpose |
|---|---|
| `Assert.Empty(col)` | Collection has zero elements |
| `Assert.Single(col)` | Collection has exactly one element |
| `Assert.Contains(col, predicate)` | At least one element matches |
| `Assert.DoesNotContain(col, predicate)` | No element matches |
| `Assert.All(col, action)` | Every element satisfies the action |

```csharp
Assert.All(cart.Items, item => Assert.True(item.Price > 0));
Assert.Contains(cart.Items, i => i.Name == "Bread");
```

---

### 05 — Exception Tests
**File:** `Tests/05_ExceptionTests.cs` · **SUT:** `Library/BankAccount.cs`

Use `Assert.Throws<T>` to verify that the right exception is thrown. You can also inspect the exception's properties.

```csharp
var ex = Assert.Throws<InsufficientFundsException>(() => account.Withdraw(200m));
Assert.Equal(200m, ex.Amount);
Assert.Contains("200", ex.Message);
```

> **Tip:** Always check that the state is unchanged after a failed operation.

---

### 06 — Boolean / Guard Tests
**File:** `Tests/06_BooleanTests.cs` · **SUT:** `Library/Validators.cs`

Testing business rules using boundary value analysis:

```csharp
Assert.True(_ageValidator.IsAdult(18));   // boundary: inclusive
Assert.False(_ageValidator.IsAdult(17));  // one below the boundary
```

---

### 07 — Null Tests
**File:** `Tests/07_NullTests.cs` · **SUT:** `Library/UserService.cs`

```csharp
Assert.Null(user);      // value must be null
Assert.NotNull(users);  // value must not be null
```

Combined with Moq to control what a repository returns.

---

### 08 — DateTime Tests
**File:** `Tests/08_DateTimeTests.cs` · **SUT:** `Library/EventScheduler.cs`

**Problem:** `DateTime.Now` makes tests non-deterministic.  
**Solution:** Inject a clock as `Func<DateTime>` so tests can freeze time.

```csharp
// Production: uses real clock
var scheduler = new EventScheduler();

// Tests: clock is frozen at a known instant
var scheduler = new EventScheduler(clock: () => new DateTime(2025, 6, 15, 12, 0, 0));
```

This pattern is also known as the **Ambient Context** pattern and applies to any external dependency that varies over time.

---

### 09 — Parameterized Tests (Theory)
**File:** `Tests/09_TheoryTests.cs` · **SUT:** `Library/MathHelper.cs`

`[Theory]` runs the same test logic with multiple data sets, eliminating copy-paste.

**`[InlineData]`** — data inline in the attribute:
```csharp
[Theory]
[InlineData(2,  true)]
[InlineData(4,  false)]
public void IsPrime_VariousInputs(int n, bool expected) { ... }
```

**`[MemberData]`** — data from a static property:
```csharp
public static IEnumerable<object[]> GcdTestCases => [
    [12, 8, 4],
    [7, 13, 1],
];

[Theory]
[MemberData(nameof(GcdTestCases))]
public void Gcd_MemberData(int a, int b, int expected) { ... }
```

**`[ClassData]`** — data from a dedicated class:
```csharp
public class EvenOddData : TheoryData<int, bool>
{
    public EvenOddData() { Add(0, true); Add(1, false); }
}

[Theory]
[ClassData(typeof(EvenOddData))]
public void IsEven_ClassData(int n, bool expected) { ... }
```

---

### 10 — Mocking with Moq
**File:** `Tests/10_MockingWithMoqTests.cs` · **SUT:** `Library/OrderService.cs`

A **mock** replaces a real dependency with a controllable fake. Moq lets you:

1. **Setup** — define what the mock returns:
```csharp
_repoMock.Setup(r => r.GetById(1)).Returns(order);
```

2. **Verify** — assert a method was called:
```csharp
_repoMock.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);
_notifMock.Verify(n => n.SendEmail(...), Times.Never);
```

3. **It.IsAny\<T\>** — wildcard matcher for any value of type T.

---

### 11 — Dependency Injection Tests
**File:** `Tests/11_DependencyInjectionTests.cs`

Tests that use `Microsoft.Extensions.DependencyInjection` to wire services exactly as production does, but with mock dependencies registered in the container.

```csharp
var services = new ServiceCollection();
services.AddSingleton(repoMock.Object);   // fake dependency
services.AddTransient<OrderService>();    // real service under test
var provider = services.BuildServiceProvider();

var service = provider.GetRequiredService<OrderService>(); // resolves!
```

Also verifies lifetime behaviours: `Transient` creates a new instance each time, `Singleton` returns the same instance.

---

### 12 — Async / Await Tests
**File:** `Tests/12_AsyncTests.cs` · **SUT:** `Library/WeatherService.cs`

xUnit natively awaits `async Task` test methods. Use `ReturnsAsync` in Moq for async setups:

```csharp
_apiMock.Setup(a => a.GetWeatherAsync("London"))
        .ReturnsAsync(new WeatherData("London", 18.5, "Cloudy"));

// xUnit awaits this automatically
string summary = await _service.GetWeatherSummaryAsync("London");
Assert.Contains("London", summary);

// Async exception test
await Assert.ThrowsAsync<ArgumentException>(
    () => _service.GetWeatherSummaryAsync(""));
```

---

### 13 — Floating-Point Tests
**File:** `Tests/13_FloatingPointTests.cs` · **SUT:** `Library/Calculator.cs`

> ⚠️ **Never use `==` to compare `double` or `float` values.** Floating-point arithmetic is inexact.

| Assertion | Use case |
|---|---|
| `Assert.Equal(expected, actual, precision: N)` | Round both to N decimal places |
| `Assert.Equal(expected, actual, tolerance: d)` | Allow absolute difference ≤ d |
| `Assert.InRange(actual, low, high)` | Value within a range |

```csharp
// π rounded to 5 decimal places
Assert.Equal(Math.PI, _calc.CircleArea(1.0), precision: 5);

// Within ±0.01 of 98.6°F
Assert.InRange(fahrenheit, 98.59, 98.61);
```

---

### 14 — Custom Assertions
**File:** `Tests/14_CustomAssertionsTests.cs`

When the same multi-step assertion appears across many tests, extract it into a named helper. This makes tests more readable and easier to maintain:

```csharp
// Before — raw assertions scattered everywhere
Assert.Equal(3, cart.Count);
Assert.Equal(7.50m, cart.Total);
Assert.All(cart.Items, i => Assert.True(i.Price > 0));

// After — one readable call
CartAssert.HasItemCount(cart, 3);
CartAssert.TotalEquals(cart, 7.50m);
CartAssert.AllItemsHavePositivePrice(cart);
```

---

### 15 — Setup and Teardown
**File:** `Tests/15_SetupAndTeardownTests.cs`

xUnit uses standard C# idioms instead of `[SetUp]`/`[TearDown]` attributes:

| Pattern | Runs |
|---|---|
| **Constructor** | Before **each** test |
| **`IDisposable.Dispose()`** | After **each** test |
| **`IClassFixture<T>`** | Setup/teardown shared **once** for the class |

```csharp
public class MyTests : IClassFixture<TempDirectoryFixture>, IDisposable
{
    public MyTests(TempDirectoryFixture fixture) { /* per-test setup */ }
    public void Dispose() { /* per-test cleanup */ }
}
```

---

### 16 — Testing Internal Methods (InternalsVisibleTo)
**File:** `Tests/16_InternalsVisibleToTests.cs` · **SUT:** `Library/DiscountCalculator.cs`

Internal classes are implementation details — don't make them public just to test them. Instead:

**Step 1** — Add to the Library project (in any `.cs` file):
```csharp
[assembly: InternalsVisibleTo("Tests")]
```

**Step 2** — The test project now has full access to `internal` members:
```csharp
// DiscountCalculator is internal — accessible because of InternalsVisibleTo
var calc = new DiscountCalculator();
decimal discount = calc.CalculateDiscount(100m, tier: 2);
Assert.Equal(10m, discount);
```

---

### 17 — File I/O Tests
**File:** `Tests/17_FileIOTests.cs` · **SUT:** `Library/FileProcessor.cs`

Rules for testing file system interactions:

1. **Use temp paths** — `Path.GetTempPath()` + `Guid.NewGuid()` for unique names
2. **Always clean up** — delete files in `Dispose()` or a finally block
3. **Test both paths** — happy path (file exists) and error path (file missing)

```csharp
public class FileIOTests : IDisposable
{
    private readonly string _tempFile =
        Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid():N}.txt");

    public void Dispose() => File.Delete(_tempFile); // always runs
}
```

---

### 18 — HttpClient Tests
**File:** `Tests/18_HttpClientTests.cs` · **SUT:** `Library/ExchangeRateService.cs`

Never make real HTTP calls in unit tests. Instead, inject a custom `HttpMessageHandler`:

```csharp
internal sealed class FakeHttpMessageHandler(HttpResponseMessage response)
    : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken ct) =>
        Task.FromResult(response);
}

// In test:
var handler    = new FakeHttpMessageHandler(fakeResponse);
var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://...") };
var service    = new ExchangeRateService(httpClient);
```

You can also inspect `LastRequest` on the handler to verify the correct URL was called.

---

### 19 — Fluent Assertions
**File:** `Tests/19_FluentAssertionsTests.cs`

[FluentAssertions](https://fluentassertions.com/) replaces `Assert.X()` with English-like syntax that produces richer failure messages:

```csharp
// xUnit style
Assert.Equal("olleh", result);
Assert.True(result.Length == 5);

// FluentAssertions style
result.Should().Be("olleh")
      .And.HaveLength(5)
      .And.StartWith("o");
```

Common patterns:
```csharp
value.Should().Be(42);
value.Should().BeGreaterThan(0).And.BeApproximately(3.333, 0.001);
collection.Should().HaveCount(3).And.Contain(x => x.IsActive);
action.Should().Throw<DivideByZeroException>().WithMessage("*zero*");
obj.Should().BeNull();
obj.Should().NotBeNull().And.Match<Product>(p => p.Price > 0);
```

---

### 20 — Integration-Style Tests
**File:** `Tests/20_IntegrationStyleTests.cs` · **SUT:** `Library/ProductCatalog.cs`

Integration tests wire **real components** together (no mocks) to verify end-to-end collaboration. Here, `InMemoryProductRepository` is a genuine in-memory implementation — not a mock:

```csharp
var repo    = new InMemoryProductRepository(); // real implementation
var service = new ProductCatalogService(repo); // real service

var product = service.AddProduct("Laptop", "Electronics", 999m, 10);
service.Purchase(product.Id, 3);

Assert.Equal(7, repo.GetById(product.Id)!.Stock); // verifies end-to-end
```

> **Unit vs Integration:** Unit tests isolate with mocks; integration tests use real collaborators. Both are important.

---

## Key Libraries

| Package | Version | Purpose |
|---|---|---|
| `xunit` | 2.9.x | Core testing framework |
| `xunit.runner.visualstudio` | 2.8.x | VS Test Explorer integration |
| `Microsoft.NET.Test.Sdk` | 17.x | .NET test host |
| `Moq` | 4.20.x | Mocking framework |
| `FluentAssertions` | 8.x | Readable assertion library |
| `Microsoft.Extensions.DependencyInjection` | 9.0 | DI container |
| `coverlet.collector` | 6.x | Code coverage |

---

## Core Concepts

### Naming Convention
Test names follow the pattern: **`MethodName_Scenario_ExpectedResult`**

```
Add_TwoPositiveNumbers_ReturnsSum
Withdraw_MoreThanBalance_ThrowsInsufficientFundsException
GetUser_NonExistingId_ReturnsNull
```

### Test Doubles
| Type | Description | Library |
|---|---|---|
| **Stub** | Returns canned data | Moq `.Setup().Returns()` |
| **Mock** | Verifies interactions | Moq `.Verify()` |
| **Fake** | Real implementation (in-memory) | Example 20 |
| **Spy** | Records calls for later verification | Moq `.Verify()` |

### F.I.R.S.T. Principles
- **F**ast — run in milliseconds
- **I**solated — no shared state between tests
- **R**epeatable — same result every run
- **S**elf-validating — pass or fail, no manual inspection
- **T**imely — written alongside (or before) production code

---

## Best Practices

| ✅ Do | ❌ Avoid |
|---|---|
| Test one thing per test | Multiple assertions on unrelated things |
| Use `IDisposable` for cleanup | Leaving files / state behind |
| Inject clock / random for determinism | `DateTime.Now` directly in SUT |
| Name tests descriptively | `Test1`, `Test2` |
| Use `[Theory]` for multiple cases | Copy-pasting the same test |
| Keep tests independent | Relying on test execution order |
| Mock at the boundary (interface) | Mocking concrete classes |

---

## Running Code Coverage

```bash
dotnet test src/UnitTestingTutorial.slnx --collect:"XPlat Code Coverage"
```

Install `reportgenerator` to view HTML reports:
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage" -reporttypes:Html
```

---

*Built with .NET 9 · xUnit · Moq · FluentAssertions*
