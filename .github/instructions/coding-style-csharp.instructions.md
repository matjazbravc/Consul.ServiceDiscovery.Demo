---
Description: 
"This file provides guidelines for writing clean, maintainable, and idiomatic C# code with
a focus on functional patterns and proper abstraction."
---

## References
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- [.NET 8 What's New](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [C# 12 Language Features](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12)
 -[GitHub Custom instructions](https://docs.github.com/en/copilot/tutorials/customization-library/custom-instructions/your-first-custom-instructions)

## General Code Style Guidelines
- Preferred language is C#
- Use C# 12 syntax and .NET 8 features where appropriate.
- Follow [Microsoft's C# coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions).
- Prefer modern language features (e.g., records, pattern matching, file-scoped namespaces).
- Focus on writing clean, maintainable, and idiomatic C# code.
- Emphasize functional programming patterns and proper abstraction.
- Avoid anti-patterns and code smells.
- Ensure code is well-documented with XML comments for public types and members.
- Organize code logically into folders and namespaces.
- Keep classes small and focused on a single responsibility.
- Use regions to group related methods within a class when necessary, but avoid excessive use.

  ```csharp
  public class CustomerService
  {
      #region Public Methods

      public void AddCustomer(Customer customer) { }

      public Customer GetCustomer(int id) { }

      #endregion
  }
  ```
---

### Using Statements
- Place using directives at the top of the file.
- Group using statements by their purpose: system, third-party, and local namespaces.
  ```csharp
  using System;
  using System.Collections.Generic;
  using MyProject.Models;
  ```

---

### File Organization
- Use one public type per file.
- Place `using` directives at the top of the file, sorted alphabetically, with system namespaces first.
- Namespace declarations use file-scoped syntax:
  ```csharp
  namespace MyApp.Models;
  ```
---

### Naming Conventions
- **Classes, Interfaces, Enums:** PascalCase (e.g., `CustomerOrder`)
- **Methods, Properties, Events:** PascalCase (e.g., `GetCustomer`, `OrderDate`)
- **Parameters, Local Variables:** camelCase (e.g., `orderId`)
- **Interfaces:** Prefix with `I` (e.g., `IRepository`)
- **Constants & Static Readonly:** PascalCase (e.g., `MaxItems`)
- **Private Fields:** `_camelCase` (e.g., `_orderList`)
- **Async Methods:** End with `Async` (e.g., `SaveOrderAsync`)
---

### Formatting & Layout
- Use **2-space indentation**. No tabs.
- Use expression-bodied members where appropriate.
- Use object and collection initializers.
- Use `var` for local variables when the type is obvious.
- Place **braces on the next line**
- Prefer pattern matching and switch expressions in .NET 8.
- Omit `this.` unless necessary.
- Maximum line length: 120 characters.
---

### Brace Style
- All braces on new lines (Allman style) for all control flow statements, even single-line bodies:
  ```csharp
  // Good
  if (condition)
  {
      DoSomething();
  }
    
  // Avoid
  if (condition)
      DoSomething();
  ```
---

### Nullability and Types
- Enable nullable reference types (`<Nullable>enable</Nullable>` in `.csproj` or `#nullable enable`).
- Use records for immutable data models.
- Use tuples and deconstruction for simple groupings.
- Prefer records for data types:
  ```csharp
  // Good: Immutable data type with value semantics
  public sealed record CustomerDto(string Name, Email Email);
    
  // Avoid: Class with mutable properties
  public class Customer
  {
      public string Name { get; set; }
      public string Email { get; set; }
  }
  ```

- Mark nullable fields explicitly:
  ```csharp
  // Good: Explicit nullability
  public class OrderProcessor
  {
      private readonly ILogger<OrderProcessor>? _logger;
      private string? _lastError;
        
      public OrderProcessor(ILogger<OrderProcessor>? logger = null)
      {
          _logger = logger;
      }
  }
    
  // Avoid: Implicit nullability
  public class OrderProcessor
  {
      private readonly ILogger<OrderProcessor> _logger; // Warning: Could be null
      private string _lastError; // Warning: Could be null
  }
  ```

- Use null checks only when necessary for reference types and public methods:
  ```csharp
  // Good: Proper null checking
  public void ProcessOrder(Order order)
  {
      ArgumentNullException.ThrowIfNull(order); // Appropriate for reference types

      _logger?.LogInformation("Processing order {Id}", order.Id);
  }
    
  // Good: Using pattern matching for null checks
  public decimal CalculateTotal(Order? order) =>
      order switch
      {
          null => throw new ArgumentNullException(nameof(order)),
          { Lines: null } => throw new ArgumentException("Order lines cannot be null", nameof(order)),
          _ => order.Lines.Sum(l => l.Total)
      };

  // Avoid null checks for value types
  public void ProcessOrder(int orderId)
  {
      ArgumentNullException.ThrowIfNull(order); // DON'T USE Null checks are unnecessary for value types
  }

  // Avoid: null checks for non-public methods
  private void ProcessOrder(Order order)
  {
      ArgumentNullException.ThrowIfNull(order); // DON'T USE, ProcessOrder is private
  }
  ```

- Use null-forgiving operator when appropriate:
  ```csharp
  public class OrderValidator
  {
      private readonly IValidator<Order> _validator;
        
      public OrderValidator(IValidator<Order> validator)
      {
          _validator = validator ?? throw new ArgumentNullException(nameof(validator));
      }
        
      public ValidationResult Validate(Order order)
      {
          // We know _validator can't be null due to constructor check
          return _validator!.Validate(order);
      }
  }
  ```

- Use nullability attributes:
  ```csharp
  public class StringUtilities
  {
      // Output is non-null if input is non-null
      [return: NotNullIfNotNull(nameof(input))]
      public static string? ToUpperCase(string? input) =>
          input?.ToUpperInvariant();
        
      // Method never returns null
      [return: NotNull]
      public static string EnsureNotNull(string? input) =>
          input ?? string.Empty;
        
      // Parameter must not be null when method returns true
      public static bool TryParse(string? input, [NotNullWhen(true)] out string? result)
      {
          result = null;
          if (string.IsNullOrEmpty(input))
              return false;
                
          result = input;
          return true;
      }
  }
  ```

- Use init-only properties with non-null validation:
  ```csharp
  // Good: Non-null validation in constructor
  public sealed record Order
  {
      public required OrderId Id { get; init; }
      public required ImmutableList<OrderLine> Lines { get; init; }
        
      public Order()
      {
          Id = null!; // Will be set by required property
          Lines = null!; // Will be set by required property
      }
        
      private Order(OrderId id, ImmutableList<OrderLine> lines)
      {
          Id = id;
          Lines = lines;
      }
        
      public static Order Create(OrderId id, IEnumerable<OrderLine> lines) =>
          new(id, lines.ToImmutableList());
  }
  ```

- Document nullability in interfaces:
  ```csharp
  public interface IOrderRepository
  {
      // Explicitly shows that null is a valid return value
      Task<Order?> FindByIdAsync(OrderId id, CancellationToken ct = default);
        
      // Method will never return null
      [return: NotNull]
      Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default);
        
      // Parameter cannot be null
      Task SaveAsync([NotNull] Order order, CancellationToken ct = default);
  }
  ```
---

### String Interpolation
- Prefer string interpolation over concatenation:
  ```csharp
  var message = $"Hello, {userName}!";
  ```
---

### LINQ & Collections
- Use LINQ query or method syntax as appropriate, favoring readability.
- Prefer `foreach` over `for` unless index is required.
- Prefer range indexers over LINQ:
  ```csharp
  // Good: Using range indexers with clear comments
  var lastItem = items[^1];  // ^1 means "1 from the end"
  var firstThree = items[..3];  // ..3 means "take first 3 items"
  var slice = items[2..5];  // take items from index 2 to 4 (5 exclusive)
    
  // Avoid: Using LINQ when range indexers are clearer
  var lastItem = items.LastOrDefault();
  var firstThree = items.Take(3).ToList();
  var slice = items.Skip(2).Take(3).ToList();
  ```
---

### Exceptions & Error Handling
- Throw specific exceptions; avoid catching general `Exception`.
- Do not use exceptions for flow control.

---

### Control Flow:
- Prefer collection initializers:
  ```csharp
  // Good: Using collection initializers
  string[] fruits = ["Apple", "Banana", "Cherry"];
    
  // Avoid: Using explicit initialization when type is clear
  var fruits = new List<int>() {
      "Apple",
      "Banana",
      "Cherry"
  };
  ```

- Use pattern matching effectively:
  ```csharp
  // Good: Clear pattern matching
  public decimal CalculateDiscount(Customer customer) =>
      customer switch
      {
          { Tier: CustomerTier.Premium } => 0.2m,
          { OrderCount: > 10 } => 0.1m,
          _ => 0m
      };
    
  // Avoid: Nested if statements
  public decimal CalculateDiscount(Customer customer)
  {
      if (customer.Tier == CustomerTier.Premium)
          return 0.2m;
      if (customer.OrderCount > 10)
          return 0.1m;
      return 0m;
  }
  ```
---

### Safe Operations:
- Use Try methods for safer operations:
  ```csharp
  // Good: Using TryGetValue for dictionary access
  if (dictionary.TryGetValue(key, out var value))
  {
      // Use value safely here
  }
  else
  {
      // Handle missing key case
  }
  ```
  ```csharp
  // Avoid: Direct indexing which can throw
  var value = dictionary[key];  // Throws if key doesn't exist

  // Good: Using Uri.TryCreate for URL parsing
  if (Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
  {
      // Use uri safely here
  }
  else
  {
      // Handle invalid URL case
  }
  ```
  ```csharp
  // Avoid: Direct Uri creation which can throw
  var uri = new Uri(urlString);  // Throws on invalid URL

  // Good: Using int.TryParse for number parsing
  if (int.TryParse(input, out var number))
  {
      // Use number safely here
  }
  else
  {
      // Handle invalid number case
  }
  ```
  ```csharp
  // Good: Combining Try methods with null coalescing
  var value = dictionary.TryGetValue(key, out var result)
      ? result
      : defaultValue;

  // Good: Using Try methods in LINQ with pattern matching
  var validNumbers = strings
      .Select(s => (Success: int.TryParse(s, out var num), Value: num))
      .Where(x => x.Success)
      .Select(x => x.Value);
  ```

- Prefer Try methods over exception handling:
  ```csharp
  // Good: Using Try method
  if (decimal.TryParse(priceString, out var price))
  {
      // Process price
  }

  // Avoid: Exception handling for expected cases
  try
  {
      var price = decimal.Parse(priceString);
      // Process price
  }
  catch (FormatException)
  {
      // Handle invalid format
  }
  ```
---

### Asynchronous Programming:
- Use Task.FromResult for pre-computed values:
  ```csharp
  // Good: Return pre-computed value
  public Task<int> GetDefaultQuantityAsync() =>
      Task.FromResult(1);
    
  // Better: Use ValueTask for zero allocations
  public ValueTask<int> GetDefaultQuantityAsync() =>
      new ValueTask<int>(1);
    
  // Avoid: Unnecessary thread pool usage
  public Task<int> GetDefaultQuantityAsync() =>
      Task.Run(() => 1);
  ```

- Always flow CancellationToken:
  ```csharp
  // Good: Propagate cancellation
  public async Task<Order> ProcessOrderAsync(
      OrderRequest request,
      CancellationToken cancellationToken)
  {
      var order = await _repository.GetAsync(
          request.OrderId, 
          cancellationToken);
            
      await _processor.ProcessAsync(
          order, 
          cancellationToken);
            
      return order;
  }
  ```

- Prefer await:
  ```csharp
  // Good: Using await
  public async Task<Order> ProcessOrderAsync(OrderId id)
  {
      var order = await _repository.GetAsync(id);
      await _validator.ValidateAsync(order);
      return order;
  }
  ```

- Never use Task.Result or Task.Wait:
  ```csharp
  // Good: Async all the way
  public async Task<Order> GetOrderAsync(OrderId id)
  {
      return await _repository.GetAsync(id);
  }
    
  // Avoid: Blocking on async code
  public Order GetOrder(OrderId id)
  {
      return _repository.GetAsync(id).Result; // Can deadlock
  }
  ```

- Use TaskCompletionSource correctly:
  ```csharp
  // Good: Using RunContinuationsAsynchronously
  private readonly TaskCompletionSource<Order> _tcs = 
      new(TaskCreationOptions.RunContinuationsAsynchronously);
    
  // Avoid: Default TaskCompletionSource can cause deadlocks
  private readonly TaskCompletionSource<Order> _tcs = new();
  ```

- Always dispose CancellationTokenSources:
  ```csharp
  // Good: Proper disposal of CancellationTokenSource
  public async Task<Order> GetOrderWithTimeout(OrderId id)
  {
      using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
      return await _repository.GetAsync(id, cts.Token);
  }
  ```

- Prefer async/await over direct Task return:
  ```csharp
  // Good: Using async/await
  public async Task<Order> ProcessOrderAsync(OrderRequest request)
  {
      await _validator.ValidateAsync(request);
      var order = await _factory.CreateAsync(request);
      return order;
  }
    
  // Avoid: Manual task composition
  public Task<Order> ProcessOrderAsync(OrderRequest request)
  {
      return _validator.ValidateAsync(request)
          .ContinueWith(t => _factory.CreateAsync(request))
          .Unwrap();
  }
  ```
---

### Symbol References:
- Always use nameof operator:
  ```csharp
  // Good: Using nameof in attributes
  public class OrderProcessor
  {
      [Required(ErrorMessage = "The {0} field is required")]
      [Display(Name = nameof(OrderId))]
      public string OrderId { get; init; }
        
      [MemberNotNull(nameof(_repository))]
      private void InitializeRepository()
      {
          _repository = new OrderRepository();
      }
        
      [NotifyPropertyChangedFor(nameof(FullName))]
      public string FirstName
      {
          get => _firstName;
          set => SetProperty(ref _firstName, value);
      }
  }
  ```

- Use nameof with exceptions:
  ```csharp
  public class OrderService
  {
      public async Task<Order> GetOrderAsync(OrderId id, CancellationToken ct)
      {
          var order = await _repository.FindAsync(id, ct);
            
          if (order is null)
              throw new OrderNotFoundException(
                  $"Order with {nameof(id)} '{id}' not found");
                    
          if (!order.Lines.Any())
              throw new InvalidOperationException(
                  $"{nameof(order.Lines)} cannot be empty");
                    
          return order;
      }
        
      public void ValidateOrder(Order order)
      {
          if (order.Lines.Count == 0)
              throw new ArgumentException(
                  "Order must have at least one line",
                  nameof(order));
      }
  }
  ```
---

### Usings and Namespaces:
- Use implicit usings:
  ```csharp
  // Good: Implicit
  namespace MyNamespace
  {
      public class MyClass
      {
          // Implementation
      }
  }
  // Avoid:
  using System; // DON'T USE
  using System.Collections.Generic; // DON'T USE
  using System.IO; // DON'T USE
  using System.Linq; // DON'T USE
  using System.Net.Http; // DON'T USE
  using System.Threading; // DON'T USE
  using System.Threading.Tasks;// DON'T USE
  using System.Net.Http.Json; // DON'T USE
  using Microsoft.AspNetCore.Builder; // DON'T USE
  using Microsoft.AspNetCore.Hosting; // DON'T USE
  using Microsoft.AspNetCore.Http; // DON'T USE
  using Microsoft.AspNetCore.Routing; // DON'T USE
  using Microsoft.Extensions.Configuration; // DON'T USE
  using Microsoft.Extensions.DependencyInjection; // DON'T USE
  using Microsoft.Extensions.Hosting; // DON'T USE
  using Microsoft.Extensions.Logging; // DON'T USE
  using Good: Explicit usings; // DON'T USE
    
  namespace MyNamespace
  {
      public class MyClass
      {
          // Implementation
      }
  }
  ```

- Use file-scoped namespaces:
  ```csharp
  // Good: File-scoped namespace
  namespace MyNamespace;
    
  public class MyClass
  {
      // Implementation
  }
    
  // Avoid: Block-scoped namespace
  namespace MyNamespace
  {
      public class MyClass
      {
          // Implementation
      }
  }
  ```
---

### Modern C# Features (C# 12 / .NET 8)
- Use required members in object initializers:
  ```csharp
  public class Person
  {
      public required string Name { get; set; }
  }
  ```
- Use primary constructors for classes where applicable.
- Use collection expressions:
  ```csharp
  var numbers = [1, 2, 3];
  ```
- Use `readonly struct` and `readonly record struct` when immutability is required.
---

### Documentation Guidelines for Classes
- **All public classes must have XML documentation summary comments.**  
  Place the summary directly above the class declaration if not existing.

  Example:
  ```csharp
  /// <summary>
  /// Represents a customer order in the sales system.
  /// </summary>
  public class CustomerOrder
  {
      // Class members...
  }
  ```

- **Summaries should concisely describe the purpose and responsibilities of the class.**
- **Use complete sentences in summaries.**
- **Avoid repeating the class name or stating the obvious.**
- **Include remarks or examples if the class usage is non-trivial.**

---

### Documentation Guidelines for public methods
- **All public methods must have XML documentation summary comments.**  
  Place the summary directly above the method declaration if not existing.
- Use inline comments sparingly to explain complex logic, but avoid obvious comments.

  Example:
  ```csharp
  /// <summary>
  /// Calculates the total price of an order.
  /// </summary>
  /// <param name="order">The order to calculate.</param>
  /// <returns>Total price.</returns>
  public decimal CalculateTotal(Order order)
  {
    // Sum up line items.
  }
  ```

## Logging guidelines
- All logging statements (except Error and Warning level) is wrapped in the appropriate IsEnabled check (depends on logging framework).
- Don't break the logging into multiple lines.
- Don't modify or break existing logging statements.
- Don't apply any other code style changes.
- Convert existing string interpolation in logging statements to use Format.
  ```csharp
  // Good example 1
  private void TestMethod(int parama1, string param2)
  {
    if (_log.IsDebugEnabled)
    {
      _log.DebugFormat("Executing with {0}, {1}", parama1, param2);
    }
  }

  // Avoid example
  private void TestMethod(int parama1, string param2)
  {
    _log.Debug($"Executing with {parama1}, {param2}");
  }
  ```

  ```csharp
  // Good example 2
  private void TestMethod(int parama1, string param2)
  {
    if (_log.IsEnabled(LogLevel.Information))
    {
      _log.LogInformation("Executing with {0}, {1}", parama1, param2);
    }  
  }

  // Avoid example
  private void TestMethod(int parama1, string param2)
  {
    _log.LogInformation("Executing with {0}, {1}", parama1, param2);
  }
  ```

### Debug logging
- Add debug logging with 'Entering' and 'Exiting' messages to indicate method entry and exit points.
- Don't add any comments before logging statements.
- Don't add debug logging inside constructors.
- Don't add debug logging inside loops or conditional statements.
- Use `nameof()` as the first parameter in logging statements to indicate the method name, instead of hardcoded method names.
- Debug logging statements should be added only at the start and end of methods.
  ```csharp
  // Debug logging convention example 1
  public class OrderProcessor
  {
    public void ProcessOrder(Order order)
    {
      if (_log.IsDebugEnabled)
      {
        _log.DebugFormat("Entering {0} with orderId={1}", nameof(ProcessOrder), order.Id);
      }

      var result = new Result();
      // ... method logic ...

      if (_log.IsDebugEnabled)
      {
        _log.DebugFormat("Exiting {0} with result={1}", nameof(ProcessOrder), result);
      }

      return result;
    }
  }
  ```

  ```csharp
  // Debug logging convention example 2 (ILogger<T> syntax)
  public class OrderProcessor
  {
    public void ProcessOrder(Order order)
    {
      if (_log.IsEnabled(LogLevel.Debug))
      {
        _log.LogDebug("Entering {Method} with orderId={OrderId}", nameof(ProcessOrder), order.Id);
      }

      var result = new Result();
      // ... method logic ...

      if (_log.IsEnabled(LogLevel.Debug))
      {
        _log.LogDebug("Exiting {Method} with result={Result}", nameof(ProcessOrder), result);
      }

      return result;
    }
  }
  ```

### Info Logging
  ```csharp
  // Info logging convention example 1
  public class OrderProcessor
  {
    public void ProcessOrder(Order order)
    {
      if (_log.IsInfoEnabled)
      {
        _log.InfoFormat("Order details: {0} ...", order, ...);
      }
    }
  }
  ```

  ```csharp
  // Info logging convention example 2 (ILogger<T> syntax)
  public class OrderProcessor
  {
    public void ProcessOrder(Order order)
    {
       if (_logger.IsEnabled(LogLevel.Information))
      {
        _logger.LogInformation("Order details: {Order} ...", order);
      }
    }
  }
  ```
